using AppTest.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.DBCLib
{
    public class DBCReadHelper
    {
        /// <summary>
        /// 加载文本文件到字符串
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stringOut"></param>
        /// <returns></returns>
        public static int Load(string path,ref string stringOut)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs,System.Text.Encoding.Default);

                stringOut = sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                LogHelper.Warn(ex.ToString());
                if(sr != null)
                {
                    sr.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
                return 1;
            }

            if (sr != null)
            {
                sr.Close();
            }
            if (fs != null)
            {
                fs.Close();
            }

            return 0;
        }

        string path;
        string fileBuffer;

        public int Parse(string _path)
        {
            int err = 0;

            path = _path;

            err = Load(path,ref fileBuffer);

            err = StrToDbeFile();

            return err;
        }

        public DbcFile dbcFile;

        private int StrToDbeFile()
        {
            int err = 0;
            string[] bufferAry = null;

            dbcFile = new DbcFile();
            if(fileBuffer == null)
            {
                if(fileBuffer == "")
                {
                    System.Windows.Forms.MessageBox.Show("Dbc文件为空");
                }
            }
            /**
             BU_: ABS ACU BCM BMS CCU DCDC EPS GW ICU MCU MMI PTC TPMS VCU IVI OBC
             BO_ 1707 RMCU_0x6AB: 8 MCU
                SG_ DBC_MCU_idc : 48|16@1+ (0.1,0) [0|6553.5] "" Vector__XXX
                SG_ DBC_MCU_F16_resver1 : 32|16@1+ (0.1,-1000) [-1000|5553.5] "" Vector__XXX
                SG_ DBC_MCU_Tcalc : 16|16@1+ (0.1,-1000) [-1000|5553.5] "" Vector__XXX
                SG_ DBC_MCU_Pinv : 0|16@1+ (0.1,-1000) [-1000|5553.5] "" Vector__XXX
            **/
            bufferAry = fileBuffer.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if(bufferAry.Length < 3)
            {
                System.Windows.Forms.MessageBox.Show("Dbc文件格式有误");
                return 0;
            }

            int lineNum = bufferAry.Length;
            bool isMessageValid = false;

            for (int i = 0; i < lineNum; i++)
            {
                string[] lineAry = bufferAry[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(lineAry.Length < 1)
                {
                    continue;
                }
                //每行的开头
                switch (lineAry[0])
                {
                    case "VAL_":
                        break;
                    case "CM_":
                        Comment cmt = new Comment();
                        lineAry = bufferAry[i].Split(new char[] { ' ', '"', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                        if(lineAry.Length == 6)
                        {
                            cmt.messageID = lineAry[2];
                            cmt.signalName = lineAry[3];
                            cmt.comment = lineAry[4];
                        }
                        else
                        {
                            cmt.comment = bufferAry[i];
                        }
                        dbcFile.comments.Add(cmt);
                        break;
                    case "BU_":
                        for (int j = 1; j < lineAry.Length; j++)
                        {
                            dbcFile.nodes.Add(lineAry[j]);
                        }
                        break;
                    case "BO_":
                  
                        Message message = new Message();
                        uint id = Convert.ToUInt32(lineAry[1]);
                        //跳过默认信息
                        if(id == 0xC0000000)
                        {
                            isMessageValid = false;
                            break;
                        }
                        else
                        {
                            isMessageValid = true;
                        }
                        //最高位为1的为扩展帧
                        if ((id & 0x80000000) != 0)
                        {
                            id &= 0x7Fffffff;
                            message.isExternID = true;
                        }
                        else
                        {
                            message.isExternID = false;
                        }
                        message.messageID = id;
                        message.messageName = lineAry[2].Substring(0, lineAry[2].Length - 1);
                        message.messageSize = Convert.ToUInt32(lineAry[3]);
                        message.transmitter = lineAry[4];

                        dbcFile.messages.Add(message);
                        break;
                    case "SG_":
                        if (isMessageValid)
                        {
                            uint byteOffset = 0;
                            Signal signal = new Signal();
                            signal.signalName = lineAry[1];

                            if(lineAry[2] == ":")
                            {
                                signal.multiplexerIndicator = -2;
                                byteOffset = 0;
                            }
                            else
                            {
                                byteOffset = 1;
                                if(lineAry[2][0] == 'M')
                                {
                                    signal.multiplexerIndicator = -1;
                                }
                                else if(lineAry[2][0] == 'm')
                                {
                                    signal.multiplexerIndicator = Convert.ToInt32(lineAry[2].Substring(1, lineAry[2].Length - 1));
                                }
                                else
                                {
                                    throw new Exception("Dbc信号格式错误");
                                }
                            }

                            string[] sp = lineAry[3 + byteOffset].Split(new char[] { '|', '@' }, StringSplitOptions.RemoveEmptyEntries);

                            signal.startBit = Convert.ToUInt32(sp[0]);
                            signal.signalSize = Convert.ToUInt32(sp[1]);

                            if(sp[2][0] == '0')
                            {
                                signal.byteOrder = 0;
                            }else if(sp[2][0] == '1')
                            {
                                signal.byteOrder = 1;
                            }

                            if(sp[2][1] == '+')
                            {
                                signal.valueType = 0;
                            }
                            else if (sp[2][1] == '-')
                            {
                                signal.valueType = 1;
                            }

                            string[] sp1 = lineAry[4 + byteOffset].Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                            signal.factor = Convert.ToDouble(sp1[0]);
                            signal.offset = Convert.ToDouble(sp1[1]);

                            sp1 = lineAry[5 + byteOffset].Split(new char[] { '[', '|', ']' }, StringSplitOptions.RemoveEmptyEntries);
                            signal.minimum = Convert.ToDouble(sp1[0]);
                            signal.maximum = Convert.ToDouble(sp1[1]);

                            signal.uintStr = lineAry[6 + byteOffset];

                            signal.receivers = lineAry[7 + byteOffset].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                            dbcFile.messages[dbcFile.messages.Count - 1].signals.Add(signal);
                        }
                        break;
                    case "BA_":
                        if (lineAry.Length < 3)
                            break;
                        if (lineAry[1].Contains("CycleTime") && lineAry[2]=="BO_")
                        {
                            if (uint.TryParse(lineAry[3], out uint messageID))
                            {
                                dbcFile.messages.Find(x => x.messageID == messageID).cycleTime = int.Parse(lineAry[4].Replace(';',' ')); 
                            }
                        }
                        break;
                }
            }

            return err;
        }
    }

    public class DbcFile
    {
        public List<string> nodes = new List<string>();
        public List<Message> messages = new List<Message>();
        public List<Comment> comments = new List<Comment>();
    }

    public class Message
    {
        public uint messageID = 0;
        /// <summary>
        /// 是否是扩展帧
        /// </summary>
        public bool isExternID = false;
        public string messageName = "";
        public uint messageSize = 0;
        public string transmitter = "";
        /// <summary>
        /// 周期 ，默认为10ms
        /// </summary>
        public int cycleTime = 10;
        public List<Signal> signals = new List<Signal>();
    }

    public class Signal
    {
        public string signalName = "";
        /// <summary>
        /// -2:普通信号；-1：复用选择信号；0-N：复用信号
        /// </summary>
        public int multiplexerIndicator = -2;
        /// <summary>
        /// 起始位
        /// </summary>
        public uint startBit = 0;
        /// <summary>
        /// 数据长度
        /// </summary>
        public uint signalSize = 0;
        /// <summary>
        /// 0:Motorola;1:Intel
        /// </summary>
        public uint byteOrder = 0;
        /// <summary>
        /// 0:unsigned;1:signed
        /// </summary>
        public uint valueType = 0;
        public double factor = 0;
        /// <summary>
        /// 偏移量
        /// </summary>
        public double offset = 0;

        public double minimum = 0;
        public double maximum = 0;
        /// <summary>
        /// 单位
        /// </summary>
        public string uintStr = "";

        public string[] receivers;
    }

    public class Comment
    {
        public string signalName;
        public string messageID;
        public string comment = "";
    }
}
