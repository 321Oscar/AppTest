using AppTest.DBCLib;
using AppTest.FormType;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProtocolLib
{
    public class DBCProtocol : BaseProtocol,IProtocol
    {
        public DBCProtocol()
        {
        }

        public DBCProtocol(string protocolName)
        {
            this.protocolName = protocolName;
            AllSignalAndValue = new Dictionary<DBCSignal, string>();
            try
            {
                var protocols = protocolName.Split(';');
                for (int i = 0; i < protocols.Length; i++)
                {
                    foreach (var item in ProtocolFile(protocols[i]))
                    {
                        if (item is DBCSignal signal)
                        {
                            if (!AllSignalAndValue.ContainsKey(signal))
                                AllSignalAndValue.Add(signal, "0");
                            //AllSignalAndValue.Add(signal, "0");
                        }
                       
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.Error("协议文档解析错误",ex);
            }
            
        }
        /// <summary>
        /// 从协议文档中读取的所有信号值
        /// </summary>
        private Dictionary<DBCSignal,string> AllSignalAndValue;

        private string protocolName;
        public override string ProtocolName { get { return protocolName; } set { protocolName = value; } }

        #region IProtocol
        /// <summary>
        /// 解析报文中的信号
        /// </summary>
        /// <param name="can_msg"></param>
        /// <param name="singals"></param>
        /// <returns></returns>
        public override Dictionary<BaseSignal, string> Multip(CANReceiveFrame[] can_msg, List<BaseSignal> singals)
        {
            Dictionary<BaseSignal, string> signalValue = new Dictionary<BaseSignal, string>();
            //解析can msg
            #region DBC数据解释处理
            for (int i = 0; i < singals.Count; i++)
            {
                //不能在解析的时候赋值0，若只发了一条数据，则autoget会将其覆盖，显示结果全是0
                //singals.Signal[i].StrValue = "0";
                DBCSignal signal = singals[i] as DBCSignal;
                for (int j = 0; j < can_msg.Length; j++)
                {
                    if (can_msg[j].cid == int.Parse(signal.MessageID,System.Globalization.NumberStyles.HexNumber))//   /相当于整数除法中的除号,%相当于余号
                    {
                        int len_rem1 = (int)(8 - (signal.StartBit % 8));
                        int byte_start = (int)(signal.StartBit / 8);
                        int len_rem2 = (int)((signal.StartBit + signal.Length) % 8);
                        int byte_end = (int)((signal.StartBit + signal.Length) / 8);
                        long tmp = 0;
                        if ((byte_start + 1) <= byte_end)
                        {
                            for (int k = byte_start + 1; k < byte_end; k++)
                            {
                                tmp = tmp * 256;
                                tmp += can_msg[j].b[k];
                            }
                            tmp = tmp * (long)Math.Pow(2, len_rem1) + (long)(can_msg[j].b[byte_start] >> (8 - len_rem1));

                            long tmp2 = 0;
                            if (byte_end >= 8)
                                tmp2 = 0;
                            else
                                tmp2 = can_msg[j].b[byte_end] % (long)Math.Pow(2, len_rem2);
                            tmp2 = tmp2 * (long)Math.Pow(2, (signal.Length - len_rem2));

                            tmp = tmp + tmp2;
                        }
                        else
                        {
                            tmp = (can_msg[j].b[byte_start] % (int)Math.Pow(2, len_rem2)) >> (8 - len_rem1);
                        }

                        double tmp_value = (double)((double)tmp * (double)signal.Factor  + signal.Offset);
                        if (signalValue.ContainsKey(signal))
                        {
                            signalValue[signal] = tmp_value.ToString();
                        }
                        else
                        {
                            signalValue.Add(signal, tmp_value.ToString());
                        }
                        signal.StrValue = tmp_value.ToString();
                        //yield return singals.Signal[i];
                    }
                }
            }

            #endregion
            return signalValue;
        }

        public override IEnumerable<BaseSignal> MultipYield(CANReceiveFrame[] can_msg, List<BaseSignal> singals)
        {
            //Dictionary<SignalItem, string> signalValue = new Dictionary<SignalItem, string>();
            //解析can msg
            #region DBC数据解释处理
            // 为每一个 signal 执行以下逻辑：
            for (int i = 0; i < singals.Count; i++)
            {
                DBCSignal signal = singals[i] as DBCSignal;
                // 若信号无需收发，则跳过此次循环。
                if (!signal.WhetherSendOrGet)
                    continue;
                // 获取 can_msg 中 messageID 与当前信号相符的 CANReceiveFrame 元素集合，若不存在则直接返回当前 signal（yield return）。
                var canThisID = can_msg.Where(x => x.cid == int.Parse(signal.MessageID, System.Globalization.NumberStyles.HexNumber));
                if (canThisID != null && canThisID.Count() != 0) 
                {
                    // 对于查找到匹配 CAN 消息中包含当前 Signal Item 的情况进行如下处理：
                    foreach (var item in canThisID)
                    {
                        int len_rem1 = (int)(8 - (signal.StartBit % 8));// 当前 Signal 在其位宽所在字节中占用比特数不足一字节时的左侧剩余比特位数。
                        int byte_start = (int)(signal.StartBit / 8);// 当前 Signal 数据存放开始位置所在的字节序列号。
                        int len_rem2 = (int)((signal.StartBit + signal.Length) % 8);// 当前 Signal 在其位宽最后个别比特处结束时右侧占用的比特位数。 
                        int byte_end = (int)((signal.StartBit + signal.Length) / 8);// 结束位置所在字节序列编号。
                        long tmp = 0;// 存储信号数据的中间值。
                         // 情况一：当前 Signal 数据所在位置不足覆盖某个完整字节。
                        if ((byte_start + 1) <= byte_end)
                        {
                            for (int k = byte_start + 1; k < byte_end; k++)
                            {
                                tmp = tmp * 256;
                                tmp += item.b[k];
                            }
                            tmp = tmp * (long)Math.Pow(2, len_rem1) + (long)(item.b[byte_start] >> (8 - len_rem1));

                            long tmp2 = 0;
                            // 若结束时在下一个字节之前，则直接取出对应实际比特位数，
                            // 否则，计算并取出最后一个部分包含该Signal数据的比特位的值
                            if (byte_end >= 8)
                                tmp2 = 0;
                            else
                                tmp2 = item.b[byte_end] % (long)Math.Pow(2, len_rem2);
                            tmp2 = tmp2 * (long)Math.Pow(2, (signal.Length - len_rem2));

                            tmp = tmp + tmp2;
                        }
                        // 当前 Signal 数据占用比特位宽度跨越单个字节，此时只需从开始位置所在字节序列中取值即可。	
                        else
                        {
                            tmp = (item.b[byte_start] % (int)Math.Pow(2, len_rem2)) >> (8 - len_rem1);
                        }

                        double tmp_value = (double)((double)tmp * (double)signal.Factor + signal.Offset);
                        signal.StrValue = tmp_value.ToString("f2");
                        yield return signal;
                    }
                }
                else
                {
                    yield return signal;
                }
            }

            #endregion
            //return signalValue;
        }

        public override List<BaseSignal> ProtocolFile(string fileName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + ProtocolType.DBC.ToString() + "\\" + fileName;
            if (!File.Exists(filePath))
            {
                FormType.Helper.LeapMessageBox.Instance.ShowInfo("协议文档丢失");
                return null;
            }
            List<BaseSignal> singals = new List<BaseSignal>();
            DBCReadHelper dbcHelper = new DBCReadHelper();
            dbcHelper.Parse(filePath);

            for (int i = 0; i < dbcHelper.dbcFile.messages.Count; i++)
            {
                for (int j = 0; j < dbcHelper.dbcFile.messages[i].signals.Count; j++)
                {
                    DBCSignal signalItem = new DBCSignal();

                    signalItem.MessageID = dbcHelper.dbcFile.messages[i].messageID.ToString("X");

                    string commentStr = string.Empty;
                    Comment comment = dbcHelper.dbcFile.comments.Find(x => x.signalName == dbcHelper.dbcFile.messages[i].signals[j].signalName
                        && x.messageID == dbcHelper.dbcFile.messages[i].messageID.ToString());
                    if (comment != null)
                    {
                        commentStr = comment.comment;
                    }
                    signalItem.Comment = commentStr;

                    signalItem.SignalName = dbcHelper.dbcFile.messages[i].signals[j].signalName;
                    signalItem.Unit = dbcHelper.dbcFile.messages[i].signals[j].uintStr;
                    signalItem.StartBit = Convert.ToInt32(dbcHelper.dbcFile.messages[i].signals[j].startBit);
                    signalItem.Length = Convert.ToInt32(dbcHelper.dbcFile.messages[i].signals[j].signalSize);
                    signalItem.Factor = dbcHelper.dbcFile.messages[i].signals[j].factor;
                    signalItem.Offset = dbcHelper.dbcFile.messages[i].signals[j].offset;
                    signalItem.Minimum = dbcHelper.dbcFile.messages[i].signals[j].minimum;
                    signalItem.Maximum = dbcHelper.dbcFile.messages[i].signals[j].maximum;
                    signalItem.ByteOrder = Convert.ToInt32(dbcHelper.dbcFile.messages[i].signals[j].byteOrder);
                    signalItem.ValueType = Convert.ToInt32(dbcHelper.dbcFile.messages[i].signals[j].valueType);
                    signalItem.CycleTime = dbcHelper.dbcFile.messages[i].cycleTime;

                    singals.Add(signalItem);
                }
            }
            return singals;
        }

        public override string Single(CANReceiveFrame[] can_msg, BaseSignal signalItem)
        {
            throw new NotImplementedException();
        }

        public override CANSendFrame[] BuildFrames(Dictionary<BaseSignal, string> signalValue)
        {
            List<CANSendFrame> cAN_Msg_BytesList = new List<CANSendFrame>();

            Dictionary<string, UInt64> keySum = new Dictionary<string, ulong>();
            Dictionary<string, int> messageIDByteOrder = new Dictionary<string, int>();
            foreach (var item in signalValue)
            {
                if (item.Key is DBCSignal dBCSignal)
                {
                    if (!messageIDByteOrder.ContainsKey(dBCSignal.MessageID))
                        messageIDByteOrder.Add(dBCSignal.MessageID, dBCSignal.ByteOrder);
                }
            }
            //int index = 0;
            //相同MessageID的值相加
            foreach (var oldSignal in AllSignalAndValue)
            {
                if (!messageIDByteOrder.ContainsKey(oldSignal.Key.MessageID))
                    continue;
                //foreach (var item in signalValue)
                //{
                //if (!messageIDByteOrder.ContainsKey(item.Key.MessageID))
                //    messageIDByteOrder.Add(item.Key.MessageID, item.Key.ByteOrder);

                decimal tmp;
                //new list exit value
                if (signalValue.ContainsKey(oldSignal.Key))
                {
                    if (string.IsNullOrEmpty(signalValue[oldSignal.Key]))
                    {
                        tmp = 0;
                    }
                    else
                    {
                        tmp = Convert.ToDecimal(signalValue[oldSignal.Key]);

                        //sum += (UInt64)((tmp + item.Key.Offset) * 1000 / item.Key.Factor * (UInt64)Math.Pow(2, item.Key.Length));
                    }
                }
                else
                {
                     tmp = Convert.ToDecimal(oldSignal.Value);
                } 

                //ulong value = (UInt64)((tmp - oldSignal.Key.Offset) * 1000 / oldSignal.Key.Factor * (UInt64)Math.Pow(2, oldSignal.Key.StartBit));
                
                //小数舍弃小数点后的数据，左移StartBit位

                ulong value = (UInt64)((tmp - (decimal)oldSignal.Key.Offset)  / (decimal)oldSignal.Key.Factor) * (UInt64)Math.Pow(2, oldSignal.Key.StartBit);
                if (!keySum.ContainsKey(oldSignal.Key.MessageID))
                {
                    keySum.Add(oldSignal.Key.MessageID, value);
                }
                else
                {
                    keySum[oldSignal.Key.MessageID] += value;
                }
                //cAN_Msg_Bytes[index] = new CAN_msg_byte(int.Parse(item.Key.MessageID), data);
                //index++;
                //}

            }

            if (keySum.Count == 0)
            {
                foreach (var item in signalValue)
                {
                    float tmp;
                    //new list exit value
                    if (item.Key is DBCSignal dbcsignal)
                    {
                        if (keySum.ContainsKey(dbcsignal.MessageID))
                        {
                            if (string.IsNullOrEmpty(signalValue[item.Key]))
                            {
                                tmp = 0;
                            }
                            else
                            {
                                tmp = (float)Convert.ToDecimal(signalValue[item.Key]);

                                //sum += (UInt64)((tmp + item.Key.Offset) * 1000 / item.Key.Factor * (UInt64)Math.Pow(2, item.Key.Length));
                            }
                        }
                        else
                        {
                            tmp = Convert.ToSingle(item.Value);
                        }

                        //ulong value = (UInt64)((tmp - oldSignal.Key.Offset) * 1000 / oldSignal.Key.Factor * (UInt64)Math.Pow(2, oldSignal.Key.StartBit));

                        //小数舍弃小数点后的数据，左移StartBit位

                        ulong value = (UInt64)((tmp - dbcsignal.Offset) / dbcsignal.Factor) * (UInt64)Math.Pow(2, dbcsignal.StartBit);
                        if (!keySum.ContainsKey(dbcsignal.MessageID))
                        {
                            keySum.Add(dbcsignal.MessageID, value);
                        }
                        else
                        {
                            keySum[dbcsignal.MessageID] += value;
                        }
                    }
                }
            }

            foreach (var item in keySum)
            {
                byte[] data = new byte[8];
                ulong value = item.Value;

                if (messageIDByteOrder[item.Key] == (int)Global.ByteOrder.Intel)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        data[i] = (Byte)(value % 256);
                        value /= 256;
                    }
                }
                else
                {
                    for (int i = 7; i > -1; i--)
                    {
                        data[i] = (Byte)(value % 256);
                        value /= 256;
                    }
                }
                 cAN_Msg_BytesList.Add(new CANSendFrame(int.Parse(item.Key,System.Globalization.NumberStyles.HexNumber), data));
            }

            return cAN_Msg_BytesList.ToArray();
        }

        public override CANSendFrame BuildFrame(BaseSignal signal, string value)
        {
            CANSendFrame cAN_Msg_Byte;// = new CAN_msg_byte(int.Parse(signal.MessageID),new byte[8] { 0, 0, 0, 0 , 0, 0, 0, 0 });
            UInt64 sum = 0;
            float tmp;
            if (signal is DBCSignal dBCSignal)
            {
                if (string.IsNullOrEmpty(value))
                {
                    tmp = 0;
                }
                else
                {
                    tmp = (float)Convert.ToDecimal(value);
                    sum += (UInt64)((tmp + dBCSignal.Offset) * 1000 / dBCSignal.Factor * (UInt64)Math.Pow(2, dBCSignal.Length));
                }

                byte[] data = new byte[8];
                if (dBCSignal.ByteOrder == 0)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        data[i] = (Byte)(sum % 256);
                        sum /= 256;
                    }
                }
                else
                {
                    for (int i = 7; i > -1; i--)
                    {
                        data[i] = (Byte)(sum % 256);
                        sum /= 256;
                    }
                }
                cAN_Msg_Byte = new CANSendFrame(int.Parse(dBCSignal.MessageID), data);
                return cAN_Msg_Byte;
            }
            else
            {
                throw new Exception("不是DBC信号，无法组帧。");
            }
        }

        public override Task<List<BaseSignal>> ProtocolFileTask(string FilePath)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
