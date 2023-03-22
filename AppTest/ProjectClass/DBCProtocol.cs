using AppTest.DBCLib;
using AppTest.FormType;
using AppTest.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProjectClass
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
                    foreach (var item in ProtocolFile(protocols[i]).Signal)
                    {
                        if (!AllSignalAndValue.ContainsKey(item))
                            AllSignalAndValue.Add(item, "0");
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
        public override Dictionary<DBCSignal, string> Multip(CAN_msg[] can_msg, Singals singals)
        {
            Dictionary<DBCSignal, string> signalValue = new Dictionary<DBCSignal, string>();
            //解析can msg
            #region DBC数据解释处理
            for (int i = 0; i < singals.Signal.Count; i++)
            {
                //不能在解析的时候赋值0，若只发了一条数据，则autoget会将其覆盖，显示结果全是0
                //singals.Signal[i].StrValue = "0";
               
                for (int j = 0; j < can_msg.Length; j++)
                {
                    if (can_msg[j].cid == int.Parse(singals.Signal[i].MessageID,System.Globalization.NumberStyles.HexNumber))//   /相当于整数除法中的除号,%相当于余号
                    {
                        int len_rem1 = (int)(8 - (singals.Signal[i].StartBit % 8));
                        int byte_start = (int)(singals.Signal[i].StartBit / 8);
                        int len_rem2 = (int)((singals.Signal[i].StartBit + singals.Signal[i].Length) % 8);
                        int byte_end = (int)((singals.Signal[i].StartBit + singals.Signal[i].Length) / 8);
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
                            tmp2 = tmp2 * (long)Math.Pow(2, (singals.Signal[i].Length - len_rem2));

                            tmp = tmp + tmp2;
                        }
                        else
                        {
                            tmp = (can_msg[j].b[byte_start] % (int)Math.Pow(2, len_rem2)) >> (8 - len_rem1);
                        }

                        double tmp_value = (double)((double)tmp * (double)singals.Signal[i].Factor  + singals.Signal[i].Offset);
                        if (signalValue.ContainsKey(singals.Signal[i]))
                        {
                            signalValue[singals.Signal[i]] = tmp_value.ToString();
                        }
                        else
                        {
                            signalValue.Add(singals.Signal[i], tmp_value.ToString());
                        }
                        singals.Signal[i].StrValue = tmp_value.ToString();
                        //yield return singals.Signal[i];
                    }
                }
            }

            #endregion
            return signalValue;
        }

        public override IEnumerable<DBCSignal> MultipYeild(CAN_msg[] can_msg, Singals singals)
        {
            //Dictionary<SignalItem, string> signalValue = new Dictionary<SignalItem, string>();
            //解析can msg
            #region DBC数据解释处理
            for (int i = 0; i < singals.Signal.Count; i++)
            {
                var canThisID = can_msg.Where(x => x.cid == int.Parse(singals.Signal[i].MessageID, System.Globalization.NumberStyles.HexNumber));
                if (canThisID != null || canThisID.Count() != 0) 
                {
                    foreach (var item in canThisID)
                    {
                        int len_rem1 = (int)(8 - (singals.Signal[i].StartBit % 8));
                        int byte_start = (int)(singals.Signal[i].StartBit / 8);
                        int len_rem2 = (int)((singals.Signal[i].StartBit + singals.Signal[i].Length) % 8);
                        int byte_end = (int)((singals.Signal[i].StartBit + singals.Signal[i].Length) / 8);
                        long tmp = 0;
                        if ((byte_start + 1) <= byte_end)
                        {
                            for (int k = byte_start + 1; k < byte_end; k++)
                            {
                                tmp = tmp * 256;
                                tmp += item.b[k];
                            }
                            tmp = tmp * (long)Math.Pow(2, len_rem1) + (long)(item.b[byte_start] >> (8 - len_rem1));

                            long tmp2 = 0;
                            if (byte_end >= 8)
                                tmp2 = 0;
                            else
                                tmp2 = item.b[byte_end] % (long)Math.Pow(2, len_rem2);
                            tmp2 = tmp2 * (long)Math.Pow(2, (singals.Signal[i].Length - len_rem2));

                            tmp = tmp + tmp2;
                        }
                        else
                        {
                            tmp = (item.b[byte_start] % (int)Math.Pow(2, len_rem2)) >> (8 - len_rem1);
                        }

                        double tmp_value = (double)((double)tmp * (double)singals.Signal[i].Factor + singals.Signal[i].Offset);
                        singals.Signal[i].StrValue = tmp_value.ToString("f2");
                        yield return singals.Signal[i];
                    }
                }
                else
                {
                    yield return singals.Signal[i];
                }
            }

            #endregion
            //return signalValue;
        }

        public override Singals ProtocolFile(string fileName)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + ProtocolType.DBC.ToString() + "\\" + fileName;
            if (!File.Exists(filePath))
            {
                FormType.Helper.LeapMessageBox.Instance.ShowInfo("协议文档丢失");
                return null;
            }
            Singals singals = new Singals();
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

                    singals.Signal.Add(signalItem);
                }
            }
            return singals;
        }

        public override string Single(CAN_msg[] can_msg, DBCSignal signalItem)
        {
            throw new NotImplementedException();
        }

        public override CAN_msg_byte[] BuildFrames(Dictionary<DBCSignal, string> signalValue)
        {
            List<CAN_msg_byte> cAN_Msg_BytesList = new List<CAN_msg_byte>();

            Dictionary<string, UInt64> keySum = new Dictionary<string, ulong>();
            Dictionary<string, int> messageIDByteOrder = new Dictionary<string, int>();
            foreach (var item in signalValue)
            {
                if (!messageIDByteOrder.ContainsKey(item.Key.MessageID))
                    messageIDByteOrder.Add(item.Key.MessageID, item.Key.ByteOrder);
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
                    if (keySum.ContainsKey(item.Key.MessageID))
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

                    ulong value = (UInt64)((tmp - item.Key.Offset) / item.Key.Factor) * (UInt64)Math.Pow(2, item.Key.StartBit);
                    if (!keySum.ContainsKey(item.Key.MessageID))
                    {
                        keySum.Add(item.Key.MessageID, value);
                    }
                    else
                    {
                        keySum[item.Key.MessageID] += value;
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
                 cAN_Msg_BytesList.Add(new CAN_msg_byte(int.Parse(item.Key,System.Globalization.NumberStyles.HexNumber), data));
            }

            return cAN_Msg_BytesList.ToArray();
        }

        public override CAN_msg_byte BuildFrame(DBCSignal signal, string value)
        {
            CAN_msg_byte cAN_Msg_Byte;// = new CAN_msg_byte(int.Parse(signal.MessageID),new byte[8] { 0, 0, 0, 0 , 0, 0, 0, 0 });
            UInt64 sum = 0;
            float tmp;
            if (string.IsNullOrEmpty(value))
            {
                tmp = 0;
            }
            else
            {
                tmp = (float)Convert.ToDecimal(value);
                sum += (UInt64)((tmp + signal.Offset) * 1000 / signal.Factor * (UInt64)Math.Pow(2, signal.Length));
            }

            byte[] data = new byte[8];
            if (signal.ByteOrder == 0)
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
            cAN_Msg_Byte = new CAN_msg_byte(int.Parse(signal.MessageID), data);
            return cAN_Msg_Byte;
        }
        #endregion

    }
}
