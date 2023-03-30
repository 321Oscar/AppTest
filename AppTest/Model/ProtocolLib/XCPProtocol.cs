using AppTest.FormType;
using AppTest.Helper;
using AppTest.Model;
using LPCanControl.CANInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTest.ProtocolLib
{
    public class XCPProtocol : BaseProtocol
    {
        public override string ProtocolName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private List<XCPSignal> A2LFileparsing(object filePath)
        {
            List<XCPSignal> xcpSignals = new List<XCPSignal>();

            CCP_CHARACTERISTIC[] cc = new CCP_CHARACTERISTIC[120];

            CCP_CHARACTERISTIC[] A2lParsedChart = new CCP_CHARACTERISTIC[1024];

            CCP_MEASUREMENT[] A2lParsedMeasurement = new CCP_MEASUREMENT[1024];

            CCP_COMPU_METHOD[] COMPU_METHODs = new CCP_COMPU_METHOD[1024];

            CCP_AXIS_PTS[] AXIS_PTSs = new CCP_AXIS_PTS[1024];

            string A2Lpath = (string)filePath;
            Global.ByteOrder byteOrder = Global.ByteOrder.Intel;
            // int kk = 1;

            if (File.Exists(A2Lpath))
            {
                /*暂存List*/
                UInt32 i = 0;


                /*解析A2l文件*/
                FileStream fs = new FileStream(A2Lpath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.Default);

                #region 提取文本
                string line = string.Empty;
                string[,] A2Lcharts = new string[1024, 50];
                string[,] A2LMeasurement = new string[1024, 50];
                string[,] A2LCompuMethod = new string[1024, 50];
                string[,] A2LAXISPTS = new string[1024, 50];
                /*提取CHARACTERISTIC*/
                i = 0;
                int j = 0;
                int flag = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string xx = line.Trim();

                    try
                    {
                        if (line.Trim().CompareTo("/begin CHARACTERISTIC") == 0)
                        {
                            A2Lcharts[i, j] = line;
                            flag = 1;
                            j++;
                            continue;
                        }
                        if (line.Trim().CompareTo("/end CHARACTERISTIC") == 0)
                        {
                            A2Lcharts[i, j] = line;
                            flag = 0;
                            i++;
                            j = 0;
                            continue;
                        }
                        if (flag == 1)
                        {
                            A2Lcharts[i, j] = line;
                            j++;
                        }
                    }
                    catch
                    {
                    }
                }

                /*
                 *读取 byteorder 
                 */
                i = 0;
                j = 0;
                flag = 0;
                sr.DiscardBufferedData();
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                //sr.ReadLine().
                while ((line = sr.ReadLine()) != null)
                {
                    string xx = line.Trim();
                    try
                    {
                        if (xx.Contains("/begin MOD_COMMON"))
                        {
                            //A2LMeasurement[i, j] = line;
                            flag = 1;
                            j++;
                            continue;
                        }
                        if (xx.Contains("/end MOD_COMMON"))
                        {
                            //A2LMeasurement[i, j] = line;
                            flag = 0;
                            i++;
                            j = 0;
                            continue;
                        }
                        if (flag == 1)
                        {
                            //A2LMeasurement[i, j] = line;
                            if (xx.Contains("BYTE_ORDER"))
                            {
                                var byteorderStr = System.Text.RegularExpressions.Regex.Split(xx, "\\s+", System.Text.RegularExpressions.RegexOptions.IgnoreCase);//line.Split(' ');
                                byteOrder = byteorderStr[1].Trim().ToLower().Contains("msb") ? Global.ByteOrder.Intel : Global.ByteOrder.Motorola;
                                flag = 0;
                                break;
                            }
                            j++;
                        }
                    }
                    catch
                    {
                    }
                }



                /*提取MEASUREMENT*/
                i = 0;
                j = 0;
                flag = 0;
                sr.DiscardBufferedData();
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                //sr.ReadLine().
                while ((line = sr.ReadLine()) != null)
                {
                    string xx = line.Trim();
                    try
                    {
                        if (line.Trim().CompareTo("/begin MEASUREMENT") == 0)
                        {
                            A2LMeasurement[i, j] = line;
                            flag = 1;
                            j++;
                            continue;
                        }
                        if (line.Trim().CompareTo("/end MEASUREMENT") == 0)
                        {
                            A2LMeasurement[i, j] = line;
                            flag = 0;
                            i++;
                            j = 0;
                            continue;
                        }
                        if (flag == 1)
                        {
                            A2LMeasurement[i, j] = line;
                            j++;
                        }
                    }
                    catch
                    {
                    }
                }



                /*提取COMPU_METHOD*/
                i = 0;
                j = 0;
                flag = 0;
                sr.DiscardBufferedData();
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                //sr.ReadLine().
                while ((line = sr.ReadLine()) != null)
                {
                    string xx = line.Trim();
                    try
                    {
                        if (line.Trim().CompareTo("/begin COMPU_METHOD") == 0)
                        {
                            A2LCompuMethod[i, j] = line;
                            flag = 1;
                            j++;
                            continue;
                        }
                        if (line.Trim().CompareTo("/end COMPU_METHOD") == 0)
                        {
                            A2LCompuMethod[i, j] = line;
                            flag = 0;
                            i++;
                            j = 0;
                            continue;
                        }
                        if (flag == 1)
                        {
                            A2LCompuMethod[i, j] = line;
                            j++;
                        }
                    }
                    catch
                    {
                    }
                }



                /*提取AXIS_PTS*/
                i = 0;
                j = 0;
                flag = 0;
                sr.DiscardBufferedData();
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                //sr.ReadLine().
                while ((line = sr.ReadLine()) != null)
                {
                    string xx = line.Trim();
                    try
                    {
                        if (line.Trim().CompareTo("/begin AXIS_PTS") == 0)
                        {
                            A2LAXISPTS[i, j] = line;
                            flag = 1;
                            j++;
                            continue;
                        }
                        if (line.Trim().CompareTo("/end AXIS_PTS") == 0)
                        {
                            A2LAXISPTS[i, j] = line;
                            flag = 0;
                            i++;
                            j = 0;
                            continue;
                        }
                        if (flag == 1)
                        {
                            A2LAXISPTS[i, j] = line;
                            j++;
                        }
                    }
                    catch
                    {
                    }
                }
                #endregion 提取文本

                /**********对COMPU_METHOD解析************/
                i = 0;
                j = 0;
                while (string.IsNullOrEmpty(A2LCompuMethod[i, 0]) != true)
                {
                    try
                    {
                        while (string.IsNullOrEmpty(A2LCompuMethod[i, j]) != true)
                        {
                            string[] temp = A2LCompuMethod[i, j].Split('*');
                            if (temp.Length > 1)
                            {
                                /*Name of CompuMethod*/
                                if (temp[1].Trim().CompareTo("Name of CompuMethod") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    COMPU_METHODs[i].NameOfCompuMethod = temp2[1].Trim();
                                }
                                /*Long identifier*/
                                if (temp[1].Trim().CompareTo("Long identifier") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    COMPU_METHODs[i].LongIdentifier = temp2[1].Trim();
                                }
                                /*Conversion Type*/
                                if (temp[1].Trim().CompareTo("Conversion Type") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    COMPU_METHODs[i].Conversion_Type = temp2[1].Trim();
                                }
                                /*Format*/
                                if (temp[1].Trim().CompareTo("Format") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    COMPU_METHODs[i].Format = temp2[1].Trim();
                                }
                                /*Units*/
                                if (temp[1].Trim().CompareTo("Units") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    COMPU_METHODs[i].Units = temp2[1].Trim();
                                }
                                /*Coefficients*/
                                if (temp[1].Trim().CompareTo("Coefficients") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    COMPU_METHODs[i].Coefficients = temp2[1].Trim();
                                }
                            }
                            j++;
                        }
                    }
                    catch
                    {

                    }
                    j = 0;
                    i++;
                }


                /**********对AXIS_PTS解析************/
                i = 0;
                j = 0;
                while (string.IsNullOrEmpty(A2LAXISPTS[i, 0]) != true)
                {
                    try
                    {
                        while (string.IsNullOrEmpty(A2LAXISPTS[i, j]) != true)
                        {
                            string[] temp = A2LAXISPTS[i, j].Split('*');
                            if (temp.Length > 1)
                            {
                                /*Name of CompuMethod*/
                                if (temp[1].Trim().CompareTo("Name") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    AXIS_PTSs[i].Name = temp2[1].Trim();
                                }
                                /*Long identifier*/
                                if (temp[1].Trim().CompareTo("Long Identifier") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    AXIS_PTSs[i].LongIdentifier = temp2[1].Trim();
                                }

                                /*ECU Address*/
                                if (temp[1].Trim().CompareTo("ECU Address") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        AXIS_PTSs[i].ECU_Address = Convert.ToUInt64(temp2[1].Trim(), 16);
                                    }
                                    catch
                                    {

                                    }
                                }
                                /*Input Quantity*/
                                if (temp[1].Trim().CompareTo("Input Quantity") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    AXIS_PTSs[i].InputQuantity = temp2[1].Trim();
                                }

                                /*Record Layout*/
                                if (temp[1].Trim().CompareTo("Record Layout") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        AXIS_PTSs[i].Record_Layout = temp2[1].Trim();
                                        /*确实数据长度*/
                                        /*length*/
                                        if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_BOOLEAN") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_BOOLEAN") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }

                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_BYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_BYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_UBYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_UBYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_UWORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_UWORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_WORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_WORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_ULONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_ULONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_SLONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_LONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_FLOAT32_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_FLOAT32_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_FLOAT64_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x08;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup1D_X_FLOAT64_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x08;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_BOOLEAN") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_BOOLEAN") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }

                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_BYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_BYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_UBYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_UBYTE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x01;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_UWORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_UWORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_WORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_WORD") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x02;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_ULONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_ULONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_SLONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_LONG") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_FLOAT32_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_FLOAT32_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x04;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_FLOAT64_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x08;//length
                                        }
                                        else if (AXIS_PTSs[i].Record_Layout.CompareTo("Lookup2D_X_FLOAT64_IEEE") == 0)
                                        {
                                            AXIS_PTSs[i].DataLen = 0x08;//length
                                        }
                                        else
                                        {
                                            AXIS_PTSs[i].DataLen = 0x00;//length
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                /*Conversion Method*/
                                if (temp[1].Trim().CompareTo("Conversion Method") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        string Conversion_Method = temp2[1].Trim();
                                        // A2lParsedChart[i].Conversion_Method = temp2[1].Trim();
                                        int kk = 0;
                                        while (string.IsNullOrEmpty(COMPU_METHODs[kk].NameOfCompuMethod) == false)
                                        {
                                            if (COMPU_METHODs[kk].NameOfCompuMethod.CompareTo(Conversion_Method) == 0)
                                            {
                                                AXIS_PTSs[i].Conversion_Method = COMPU_METHODs[kk];
                                                break;
                                            }
                                            kk++;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }


                                /*Number of Axis Pts*/
                                if (temp[1].Trim().CompareTo("Number of Axis Pts") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        AXIS_PTSs[i].Number_of_Axis_Pts = Convert.ToUInt16(temp2[1].Trim());
                                    }
                                    catch
                                    {

                                    }
                                    //  ddd
                                }

                                /***Lower Limit***/
                                /***Upper Limit***/



                            }
                            j++;
                        }
                    }
                    catch
                    {

                    }
                    j = 0;
                    i++;
                }

                /********对chartic解析**********/

                i = 0;
                j = 0;

                while (string.IsNullOrEmpty(A2Lcharts[i, 0]) != true)
                {
                    try
                    {
                        while (string.IsNullOrEmpty(A2Lcharts[i, j]) != true)
                        {
                            string[] temp = A2Lcharts[i, j].Split('*');
                            if (temp.Length > 1)
                            {
                                string xx = temp[1].Trim();
                                /*Name*/
                                if (temp[1].Trim().CompareTo("Name") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedChart[i].cName = temp2[1].Trim();
                                }
                                /*Long Identifier*/
                                if (temp[1].Trim().CompareTo("Long Identifier") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedChart[i].LongIdentifier = temp2[1].Trim();
                                }
                                /*Type*/
                                if (temp[1].Trim().CompareTo("Type") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedChart[i].Characteristic_Type = temp2[1].Trim();
                                }
                                if (temp[1].Trim().CompareTo("Characteristic Type") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedChart[i].Characteristic_Type = temp2[1].Trim();
                                }

                                /*ECU Address*/
                                if (temp[1].Trim().CompareTo("ECU Address") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        A2lParsedChart[i].cAddress = Convert.ToUInt64(temp2[1].Trim(), 16);
                                    }
                                    catch
                                    {

                                    }
                                }
                                /*Record Layout*/
                                if (temp[1].Trim().CompareTo("Record Layout") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        A2lParsedChart[i].Record_Layout = temp2[1].Trim();
                                        /*确实数据长度*/
                                        /*length*/
                                        if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_BOOLEAN") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_UBYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_BYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_UWORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_SWORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_ULONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_LONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_FLOAT32_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Scalar_FLOAT64_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x08;//length
                                        }

                                        /*length*/
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_BOOLEAN") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_BOOLEAN") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }

                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_BYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_BYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_UBYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_UBYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_UWORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_UWORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_WORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_WORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_ULONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_ULONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_SLONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_LONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_FLOAT32_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_FLOAT32_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_FLOAT64_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x08;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup1D_X_FLOAT64_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x08;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_BOOLEAN") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_BOOLEAN") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }

                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_BYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_BYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_UBYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_UBYTE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x01;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_UWORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_UWORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_WORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_WORD") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x02;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_ULONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_ULONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_SLONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_LONG") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_FLOAT32_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_FLOAT32_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x04;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_FLOAT64_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x08;//length
                                        }
                                        else if (A2lParsedChart[i].Record_Layout.CompareTo("Lookup2D_X_FLOAT64_IEEE") == 0)
                                        {
                                            A2lParsedChart[i].DataLen = 0x08;//length
                                        }
                                        else
                                        {
                                            A2lParsedChart[i].DataLen = 0x00;//length
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }

                                /*Conversion Method*/
                                if (temp[1].Trim().CompareTo("Conversion Method") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    string yy = temp2[1].Trim();
                                    try
                                    {
                                        string Conversion_Method = temp2[1].Trim();
                                        // A2lParsedChart[i].Conversion_Method = temp2[1].Trim();
                                        int kk = 0;
                                        while (string.IsNullOrEmpty(COMPU_METHODs[kk].NameOfCompuMethod) == false)
                                        {
                                            if (COMPU_METHODs[kk].NameOfCompuMethod.CompareTo(Conversion_Method) == 0)
                                            {
                                                A2lParsedChart[i].Conversion_Method = COMPU_METHODs[kk];
                                                break;
                                            }
                                            kk++;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                                /*Lower Limit*/

                                if (temp[1].Trim().CompareTo("Lower Limit") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    try
                                    {
                                        string Lower_Limit = temp2[1].Trim();
                                        A2lParsedChart[i].Lower_Limit = double.Parse(Lower_Limit);
                                    }
                                    catch
                                    {

                                    }
                                }

                                /*Upper Limit*/
                                if (temp[1].Trim().CompareTo("Upper Limit") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    try
                                    {
                                        string Lower_Limit = temp2[1].Trim();
                                        A2lParsedChart[i].Upper_Limit = double.Parse(Lower_Limit);
                                    }
                                    catch
                                    {

                                    }
                                }

                                /*AXIS_DESCR*/
                                if (temp[1].Trim().CompareTo("Axis Type") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    if (string.IsNullOrEmpty(A2lParsedChart[i].AXIS_DESCR_X.Axis_Type) == true)
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_X.Axis_Type = temp2[1].Trim();
                                    }
                                    else
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_Y.Axis_Type = temp2[1].Trim();
                                    }
                                }
                                if (temp[1].Trim().CompareTo("Reference to Input") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    if (string.IsNullOrEmpty(A2lParsedChart[i].AXIS_DESCR_X.Reference_to_Input) == true)
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_X.Reference_to_Input = temp2[1].Trim();
                                    }
                                    else
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_Y.Reference_to_Input = temp2[1].Trim();
                                    }
                                }
                                if (temp[1].Trim().CompareTo("Conversion Method") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    if (string.IsNullOrEmpty(A2lParsedChart[i].AXIS_DESCR_X.Conversion_Method) == true)
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_X.Conversion_Method = temp2[1].Trim();
                                    }
                                    else
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_Y.Conversion_Method = temp2[1].Trim();
                                    }
                                    //A2lParsedChart[i].AXIS_DESCR_X.Conversion_Method = temp2[1].Trim();
                                }
                                if (temp[1].Trim().CompareTo("Number of Axis Pts") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    //  if (A2lParsedChart[i].AXIS_DESCR_X.Number_of_Axis_Pts == null || A2lParsedChart[i].AXIS_DESCR_X.Number_of_Axis_Pts == 0)
                                    if (A2lParsedChart[i].AXIS_DESCR_X.Number_of_Axis_Pts == 0)
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_X.Number_of_Axis_Pts = Convert.ToInt16(temp2[1].Trim());
                                    }
                                    else
                                    {
                                        A2lParsedChart[i].AXIS_DESCR_Y.Number_of_Axis_Pts = Convert.ToInt16(temp2[1].Trim());
                                    }
                                    // A2lParsedChart[i].AXIS_DESCR_X.Number_of_Axis_Pts = Convert.ToInt16(temp2[1].Trim());
                                }
                                /*  if (temp[0].Trim().CompareTo("AXIS_PTS_REF") == 0)
                                  {
                                      string[] temp2 = temp[2].Trim().Split('/');

                                      if (string.IsNullOrEmpty(A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF) == true)
                                      {
                                          A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF = temp2[1].Trim();
                                      }
                                      else
                                      {
                                          A2lParsedChart[i].AXIS_DESCR_Y.AXIS_PTS_REF = temp2[1].Trim();
                                      }

                                     // A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF = temp2[1].Trim();
                                  }*/

                            }
                            else if (A2Lcharts[i, j].Contains("AXIS_PTS_REF"))
                            {
                                string temp2 = A2Lcharts[i, j].Trim().Replace("AXIS_PTS_REF", "");
                                if (string.IsNullOrEmpty(A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF) == true)
                                {
                                    A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF = temp2.Trim();
                                    int kk = 0;
                                    while (string.IsNullOrEmpty(AXIS_PTSs[kk].Name) == false)
                                    {
                                        if (AXIS_PTSs[kk].Name.CompareTo(A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF) == 0)
                                        {
                                            A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS = AXIS_PTSs[kk];
                                            break;
                                        }
                                        kk++;
                                    }
                                }
                                else
                                {
                                    A2lParsedChart[i].AXIS_DESCR_Y.AXIS_PTS_REF = temp2.Trim();
                                    int kk = 0;
                                    while (string.IsNullOrEmpty(AXIS_PTSs[kk].Name) == false)
                                    {
                                        if (AXIS_PTSs[kk].Name.CompareTo(A2lParsedChart[i].AXIS_DESCR_Y.AXIS_PTS_REF) == 0)
                                        {
                                            A2lParsedChart[i].AXIS_DESCR_Y.AXIS_PTS = AXIS_PTSs[kk];
                                            break;
                                        }
                                        kk++;
                                    }
                                }

                                //  A2lParsedChart[i].AXIS_DESCR_X.AXIS_PTS_REF = temp2.Trim();
                            }
                            j++;

                        }
                    }
                    catch
                    {

                    }
                    j = 0;
                    i++;
                }


                /*提取出非表格*/
                //for (i = 0; i < frmM.charts.Length; i++)
                //{
                //    frmM.charts[i].cName = string.Empty;
                //    frmM.charts[i].cAddress = 0;
                //    frmM.charts[i].cValue = 0;
                //    frmM.charts[i].Characteristic_Type = string.Empty;
                //}
                i = 0;
                j = 0;
                while (string.IsNullOrEmpty(A2lParsedChart[i].Characteristic_Type) == false)
                {
                    if (A2lParsedChart[i].Characteristic_Type.CompareTo("VALUE") == 0)
                    {
                        /* frmM.charts[j].cName = A2lParsedChart[i].cName;
                         frmM.charts[j].cAddress = A2lParsedChart[i].cAddress;
                         frmM.charts[j].Characteristic_Type = A2lParsedChart[i].Characteristic_Type;
                         frmM.charts[j].cValue = A2lParsedChart[i].cValue;*/
                        //frmM.charts[j] = A2lParsedChart[i];

                        XCPSignal xcpSignal = new XCPSignal();
                        xcpSignal.SignalName = A2lParsedChart[i].cName;
                        xcpSignal.ECUAddress = A2lParsedChart[i].cAddress.ToString("X");
                        xcpSignal.MeaOrCal = false;
                        xcpSignal.Length = A2lParsedChart[i].DataLen;
                        xcpSignal.ValueType = XCPHelper.RecordStrConvert(A2lParsedChart[i].Record_Layout);
                        xcpSignal.Compu_Methd = A2lParsedChart[i].Conversion_Method;
                        xcpSignal.ByteOrder_int = (int)byteOrder;
                        xcpSignal.Comment = A2lParsedChart[i].LongIdentifier;
                        xcpSignal.Maximum = A2lParsedChart[i].Upper_Limit;
                        xcpSignal.Minimum = A2lParsedChart[i].Lower_Limit;

                        xcpSignals.Add(xcpSignal);
                        j++;
                    }
                    i++;
                }

                /*提取出表格*/
                //for (i = 0; i < frmM.charts_CURVE.Length; i++)
                //{
                //    frmM.charts_CURVE[i].cName = string.Empty;
                //    frmM.charts_CURVE[i].cAddress = 0;
                //    frmM.charts_CURVE[i].cValue = 0;
                //    frmM.charts_CURVE[i].Characteristic_Type = string.Empty;
                //}
                //i = 0;
                //j = 0;
                //while (string.IsNullOrEmpty(A2lParsedChart[i].Characteristic_Type) == false)
                //{
                //    if (A2lParsedChart[i].Characteristic_Type.CompareTo("CURVE") == 0)
                //    {
                //        frmM.charts_CURVE[j] = A2lParsedChart[i];
                //        j++;
                //    }
                //    i++;
                //}

                /*提取出表格*/
                //for (i = 0; i < frmM.charts_MAP.Length; i++)
                //{
                //    frmM.charts_MAP[i].cName = string.Empty;
                //    frmM.charts_MAP[i].cAddress = 0;
                //    frmM.charts_MAP[i].cValue = 0;
                //    frmM.charts_MAP[i].Characteristic_Type = string.Empty;
                //}
                //i = 0;
                //j = 0;
                //while (string.IsNullOrEmpty(A2lParsedChart[i].Characteristic_Type) == false)
                //{
                //    if (A2lParsedChart[i].Characteristic_Type.CompareTo("MAP") == 0)
                //    {
                //        frmM.charts_MAP[j] = A2lParsedChart[i];
                //        j++;
                //    }
                //    i++;
                //}

                /********对MEASUREMENT解析**********/

                i = 0;
                j = 0;

                while (string.IsNullOrEmpty(A2Lcharts[i, 0]) != true)
                {
                    try
                    {
                        while (string.IsNullOrEmpty(A2LMeasurement[i, j]) != true)
                        {
                            string[] temp = A2LMeasurement[i, j].Split('*');
                            if (temp.Length > 1)
                            {
                                string xx = temp[1].Trim();
                                /*Name*/
                                if (temp[1].Trim().CompareTo("Name") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedMeasurement[i].cName = temp2[1].Trim();
                                }
                                /*Long Identifier*/
                                if (temp[1].Trim().CompareTo("Long identifier") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedMeasurement[i].LongIdentifier = temp2[1].Trim();
                                }
                                /*Data Type*/
                                if (temp[1].Trim().CompareTo("Data type") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    A2lParsedMeasurement[i].Data_Type = temp2[1].Trim();
                                    A2lParsedMeasurement[i].DataLen = checkMeasureMentDatalen(A2lParsedMeasurement[i].Data_Type);

                                }
                                /*Conversion method*/
                                if (temp[1].Trim().CompareTo("Conversion method") == 0)
                                {
                                    string[] temp2 = temp[2].Trim().Split('/');
                                    try
                                    {
                                        string Conversion_Method = temp2[1].Trim();
                                        // A2lParsedChart[i].Conversion_Method = temp2[1].Trim();
                                        int kk = 0;
                                        while (string.IsNullOrEmpty(COMPU_METHODs[kk].NameOfCompuMethod) == false)
                                        {
                                            if (COMPU_METHODs[kk].NameOfCompuMethod.CompareTo(Conversion_Method) == 0)
                                            {
                                                A2lParsedMeasurement[i].Conversion_Method = COMPU_METHODs[kk];
                                                break;
                                            }
                                            kk++;
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }




                            }
                            else
                            {
                                //      temp = A2LMeasurement[i, j]
                                temp = A2LMeasurement[i, j].Trim().Split(' ');
                                if (temp.Length > 1)
                                {
                                    if (temp[0].Trim().CompareTo("ECU_ADDRESS") == 0)
                                    {
                                        /*ECU Address*/
                                        //      string[] temp2 = temp[2].Trim().Split('/');
                                        string yy = temp[1].Trim();
                                        try
                                        {
                                            A2lParsedMeasurement[i].cAddress = Convert.ToUInt64(temp[temp.Length - 1].Trim(), 16);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }
                            }
                            j++;

                        }
                    }
                    catch
                    {
                    }
                    j = 0;
                    i++;
                }

                /*提取出观察量*/
                //for (i = 0; i < frmM.measurements.Length; i++)
                //{
                //    frmM.measurements[i].cName = string.Empty;
                //    frmM.measurements[i].cAddress = 0;
                //    frmM.measurements[i].cValue = 0;
                //    frmM.measurements[i].Data_Type = string.Empty;
                //}
                i = 0;
                j = 0;
                while (string.IsNullOrEmpty(A2lParsedMeasurement[i].Data_Type) == false)
                {
                    //frmM.measurements[i] = A2lParsedMeasurement[i];
                    XCPSignal xcpSignal = new XCPSignal();
                    xcpSignal.SignalName = A2lParsedMeasurement[i].cName;
                    xcpSignal.ECUAddress = A2lParsedMeasurement[i].cAddress.ToString("X");
                    xcpSignal.MeaOrCal = true;
                    xcpSignal.Length = A2lParsedMeasurement[i].DataLen;
                    xcpSignal.ValueType = XCPHelper.RecordStrConvert(A2lParsedMeasurement[i].Data_Type);
                    //xcpSignal.Conversion = A2lParsedChart[i].Conversion_Method;
                    xcpSignal.ByteOrder_int = (int)byteOrder;
                    xcpSignal.Comment = A2lParsedMeasurement[i].LongIdentifier;
                    xcpSignal.Maximum = A2lParsedMeasurement[i].Upper_Limit;
                    xcpSignal.Minimum = A2lParsedMeasurement[i].Lower_Limit;
                    xcpSignals.Add(xcpSignal);
                    i++;
                }

            }
            else
            {
                return null;
            }

            return xcpSignals;
        }

        private async Task<List<BaseSignal>> A2lFileParseWithjar(string filePath)
        {
            List<BaseSignal> signals = new List<BaseSignal>();
            try
            {
                string jsonOutput = string.Empty;
                await Task.Run( () =>
                {
                    string jarPath = @".\Config\a2lparser.jar";
                    Process p = new Process();
                    p.StartInfo.FileName = "java";
                    p.StartInfo.Arguments = ($" -jar {jarPath} -mj -a2l {filePath}");
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                    jsonOutput = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();
                });

                var a2l = Asap2File.FromJson(jsonOutput);

                foreach (var module in a2l.Project.Modules)
                {
                    foreach (var meas in module.Measurements)
                    {
                        meas.Length = checkMeasureMentDatalen(meas.Datatype.ToString());
                        meas.ECUAddress = meas.EcuAddress.ToString("X");
                        meas.ValueType = XCPHelper.RecordStrConvert(meas.Datatype.ToString());
                        meas.MeaOrCal = true;
                        meas.Minimum = meas.LowerLimit;
                        meas.Maximum = meas.UpperLimit;
                        signals.Add(meas);
                    }

                    foreach (var chara in module.Characteristics)
                    {
                        chara.Length = checkMeasureMentDatalen(chara.Deposit.ToString());
                        chara.ValueType = XCPHelper.RecordStrConvert(chara.Deposit.ToString());
                        chara.ECUAddress = chara.Address.ToString("X");
                        chara.MeaOrCal = false;
                        chara.Minimum = chara.LowerLimit;
                        chara.Maximum = chara.UpperLimit;
                        signals.Add(chara);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error("A2lFileParseWithjar", ex);
            }
            return signals;
        }

        private byte checkMeasureMentDatalen(string datatype)
        {
            byte rnt = 0;
            string dataTypeUpper = datatype.ToUpper();
            if (dataTypeUpper.Contains("FLOAT64"))//double
            {
                rnt = 8;
            }
            else if (dataTypeUpper.Contains("ULONG"))//uint32
            {
                rnt = 4;
            }
            else if (dataTypeUpper.Contains("FLOAT32"))//single
            {
                rnt = 4;
            }
            else if (dataTypeUpper.Contains("LONG"))//int32
            {
                rnt = 4;
            }
            else if (dataTypeUpper.Contains("UWORD"))//uint16
            {
                rnt = 2;
            }
            else if (dataTypeUpper.Contains("SWORD"))//uint16
            {
                rnt = 2;
            }
            else if (dataTypeUpper.Contains("UBYTE"))//uint8
            {
                rnt = 1;
            }
            else if (dataTypeUpper.Contains("BYTE"))//int8
            {
                rnt = 1;
            }
            return rnt;
        }

        public override Dictionary<BaseSignal, string> Multip(CANReceiveFrame[] can_msg, List<BaseSignal> singals)
        {
            throw new NotImplementedException();
        }

        public override Task<List<BaseSignal>> ProtocolFileTask(string FilePath)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "\\" + ProtocolType.XCP.ToString() + "\\" + FilePath;
            //var items = A2LFileparsing(filePath);
            return A2lFileParseWithjar(filePath);
        }

        public override string Single(CANReceiveFrame[] can_msg, BaseSignal signalItem)
        {
            throw new NotImplementedException();
        }

        public override CANSendFrame[] BuildFrames(Dictionary<BaseSignal, string> signalValue)
        {
            throw new NotImplementedException();
        }

        public override CANSendFrame BuildFrame(BaseSignal signal, string value)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<BaseSignal> MultipYield(CANReceiveFrame[] can_msg, List<BaseSignal> singals)
        {
            throw new NotImplementedException();
        }

        public override List<BaseSignal> ProtocolFile(string FilePath)
        {
            throw new NotImplementedException();
        }
    }
}
