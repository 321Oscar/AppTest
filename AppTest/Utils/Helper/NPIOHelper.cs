using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppTest.Helper
{
    public class NPIOHelper
    {
        private static readonly ILog loger = LogManager.GetLogger("NPIOLogger");

        #region 从DataTable导出到excel文件中，支持xls和xlsx
        #region 导出为xls文件内部方法
        /// <summary>
        /// 从datatable中导出到excel
        /// </summary>
        /// <param name="strFileName">excel文件名</param>
        /// <param name="dtSource">datatable数据源</param>
        /// <param name="strHeaderText">表名</param>
        /// <param name="dir"></param>
        /// <param name="sheetNum">sheet编号</param>
        /// <returns></returns>
        static MemoryStream ExportDT(string strFileName, DataTable dtSource, string strHeaderText, Dictionary<string, string> dir, int sheetNum,string dateFormat = "yyyy-MM-dd HH:mm:ss fff")
        {
            IWorkbook workbook = new HSSFWorkbook();
            using (Stream writefile = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (writefile.Length > 0 && sheetNum > 0)
                {
                    workbook = WorkbookFactory.Create(writefile);
                }
            }

            ISheet sheet = null;
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat(dateFormat);
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 0)
                {
                    string sheetName = strHeaderText + (sheetNum == 0 ? "" : sheetNum.ToString());
                    if (workbook.GetSheetIndex(sheetName) >= 0)
                    {
                        workbook.RemoveSheetAt(workbook.GetSheetIndex(sheetName));
                    }
                    sheet = workbook.CreateSheet(sheetName);
                    #region 表头及样式
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.HeightInPoints = 25;
                    headerRow.CreateCell(0).SetCellValue(strHeaderText);
                    ICellStyle headerstyle = workbook.CreateCellStyle();
                    headerstyle.Alignment = HorizontalAlignment.Center;
                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 200;
                    font.IsBold = true;
                    headerstyle.SetFont(font);
                    headerRow.GetCell(0).CellStyle = headerstyle;

                    rowIndex = 1;

                    #endregion
                }

                #region 列头以及样式

                if (rowIndex == 1)
                {
                    IRow headerRow = sheet.CreateRow(1);
                    ICellStyle headerstyle = workbook.CreateCellStyle();
                    headerstyle.Alignment = HorizontalAlignment.Center;
                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.IsBold = true;
                    headerstyle.SetFont(font);
                    //写入列标题
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(dir[column.ColumnName]);
                        headerRow.GetCell(column.Ordinal).CellStyle = headerstyle;
                        //设置列宽
                        sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256 * 2);
                    }
                    rowIndex = 2;
                }
                #endregion
                #endregion

                #region 填充内容

                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            double result;
                            if (isNumeric(drValue, out result))
                            {
                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                            }
                            break;
                        case "System.DateTime":
                            DateTime dateV;
                            try
                            {
                                dateV = (DateTime)row[column];
                            }
                            catch (Exception)
                            {
                                //无法强转则用string
                                DateTime.TryParse(drValue, out dateV);
                            }
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;
                            break;
                        case "System.Boolean":
                            bool boolV;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            double doubleV = 0;
                            double.TryParse(drValue, out doubleV);
                            newCell.SetCellValue(doubleV);
                            break;
                        case "System.DBNull":
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue(drValue.ToString());
                            break;
                    }
                }

                #endregion

                rowIndex++;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }
        #endregion

        #region 导出为xlsx文件内部方法
        static void ExportDTI(DataTable dtSource, string strHeaderText, FileStream fs, MemoryStream readfs, Dictionary<string, string> dir, int sheetNum)
        {
            IWorkbook workbook = new XSSFWorkbook();
            if (readfs.Length > 0 && sheetNum > 0)
            {
                workbook = WorkbookFactory.Create(readfs);
            }
            ISheet sheet = null;
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 0)
                {
                    string sheetName = strHeaderText + (sheetNum == 0 ? "" : sheetNum.ToString());
                    if (workbook.GetSheetIndex(sheetName) >= 0)
                    {
                        workbook.RemoveSheetAt(workbook.GetSheetIndex(sheetName));
                    }
                    sheet = workbook.CreateSheet(sheetName);
                    #region 表头及样式
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.HeightInPoints = 25;
                    headerRow.CreateCell(0).SetCellValue(strHeaderText);

                    ICellStyle headerstyle = workbook.CreateCellStyle();
                    headerstyle.Alignment = HorizontalAlignment.Center;
                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 200;
                    font.IsBold = true;
                    headerstyle.SetFont(font);
                    headerRow.GetCell(0).CellStyle = headerstyle;

                    rowIndex = 1;

                    #endregion
                }

                #region 列头以及样式

                if (rowIndex == 1)
                {
                    IRow headerRow = sheet.CreateRow(1);
                    ICellStyle headerstyle = workbook.CreateCellStyle();
                    headerstyle.Alignment = HorizontalAlignment.Center;
                    IFont font = workbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.IsBold = true;
                    headerstyle.SetFont(font);
                    //写入列标题
                    foreach (DataColumn column in dtSource.Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(dir[column.ColumnName]);
                        headerRow.GetCell(column.Ordinal).CellStyle = headerstyle;
                        //设置列宽
                        sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256 * 2);
                    }
                    rowIndex = 2;
                }
                #endregion
                #endregion

                #region 填充内容

                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String":
                            double result;
                            if (isNumeric(drValue, out result))
                            {
                                double.TryParse(drValue, out result);
                                newCell.SetCellValue(result);
                                break;
                            }
                            else
                            {
                                newCell.SetCellValue(drValue);
                            }
                            break;
                        case "System.DateTime":
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;
                            break;
                        case "System.Boolean":
                            bool boolV;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal":
                        case "System.Double":
                            double doubleV = 0;
                            double.TryParse(drValue, out doubleV);
                            newCell.SetCellValue(doubleV);
                            break;
                        case "System.DBNull":
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue(drValue.ToString());
                            break;
                    }
                }

                #endregion

                rowIndex++;
            }
            workbook.Write(fs);
            fs.Close();
        }
        #endregion

        #region 导出excel表格
        public static void ExportDTtoExcel(DataTable dtSource, string strHeaderText, string strFileName, Dictionary<string, string> dir, bool isNew, int sheetRow = 50000)
        {
            int currentSheetCount = GetSheetNumber(strFileName);
            if (sheetRow <= 0)
            {
                sheetRow = dtSource.Rows.Count;
            }
            string[] temp = strFileName.Split('.');
            string fileExtens = temp[temp.Length - 1];
            int sheetCount = (int)Math.Ceiling((double)dtSource.Rows.Count / sheetRow);
            if (temp[temp.Length - 1] == "xls" && dtSource.Columns.Count < 256 && sheetRow < 65536)
            {
                if (isNew)
                {
                    currentSheetCount = 0;
                }
                for (int i = currentSheetCount; i < currentSheetCount + sheetCount; i++)
                {
                    DataTable pageDataTable = dtSource.Clone();
                    int hasRowCount = dtSource.Rows.Count - sheetRow * (i - currentSheetCount) < sheetRow ? dtSource.Rows.Count - sheetRow * (i - currentSheetCount) : sheetRow;
                    for (int j = 0; j < hasRowCount; j++)
                    {
                        pageDataTable.ImportRow(dtSource.Rows[(i - currentSheetCount) * sheetRow + j]);
                    }

                    using (MemoryStream ms = ExportDT(strFileName, pageDataTable, strHeaderText, dir, i))
                    {
                        using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                        {
                            byte[] data = ms.ToArray();
                            fs.Write(data, 0, data.Length);
                            fs.Flush();
                        }
                    }
                }
            }
            else
            {
                if (temp[temp.Length - 1] == "xls")
                    strFileName += "x";
                if (isNew)
                {
                    currentSheetCount = 0;
                }
                for (int i = currentSheetCount; i < currentSheetCount + sheetCount; i++)
                {
                    DataTable pageDataTable = dtSource.Clone();
                    int hasRowCount = dtSource.Rows.Count - sheetRow * (i - currentSheetCount) < sheetRow ? dtSource.Rows.Count - sheetRow * (i - currentSheetCount) : sheetRow;
                    for (int j = 0; j < hasRowCount; j++)
                    {
                        pageDataTable.ImportRow(dtSource.Rows[(i - currentSheetCount) * sheetRow + j]);
                    }
                    FileStream readfs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Read);
                    MemoryStream readfms = new MemoryStream();
                    readfs.CopyTo(readfms);
                    readfs.Close();
                    using (FileStream writefs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                    {
                        ExportDTI(pageDataTable, strHeaderText, writefs, readfms, dir, i);
                    }
                    readfms.Close();
                }
            }
        }
        #endregion

        #endregion

        #region 从excel文件中将数据导出到datatable
        static DataTable ImportDt(ISheet sheet, int HeaderRowIndex, Dictionary<string, string> dir)
        {
            DataTable table = new DataTable();
            IRow headerRow;
            int cellCount;
            try
            {
                if (HeaderRowIndex < 0)
                {
                    headerRow = sheet.GetRow(0);
                    cellCount = headerRow.LastCellNum;

                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        DataColumn column = new DataColumn(Convert.ToString(i));
                        table.Columns.Add(column);

                    }
                }
                else
                {
                    headerRow = sheet.GetRow(HeaderRowIndex);
                    cellCount = headerRow.LastCellNum;
                    for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                    {
                        //如果excel某一列列名不存在：以该列的序号作为datatable的列明，如果Datatable中包含了这个序号为名的列，那么列名为重复列明+ 序号
                        if (headerRow.GetCell(i) == null)
                        {
                            if (table.Columns.IndexOf(Convert.ToString(i)) > 0)
                            {
                                DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                                table.Columns.Add(column);
                            }
                            else
                            {
                                DataColumn column = new DataColumn(Convert.ToString(i));
                                table.Columns.Add(column);
                            }
                        }

                        //
                        else if (table.Columns.IndexOf(headerRow.GetCell(i).ToString()) > 0)
                        {
                            DataColumn column = new DataColumn(Convert.ToString("重复列名" + i));
                            table.Columns.Add(column);
                        }
                        else
                        {
                            string colName = dir.Where(s => s.Value == headerRow.GetCell(i).ToString()).First().Key;
                            DataColumn column = new DataColumn(colName);
                            table.Columns.Add(column);
                        }
                    }
                }
                int rowCount = sheet.LastRowNum;
                for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
                {
                    try
                    {
                        IRow row;
                        if (sheet.GetRow(i) == null)
                        {
                            row = sheet.CreateRow(i);
                        }
                        else
                        {
                            row = sheet.GetRow(i);
                        }
                        DataRow dataRow = table.NewRow();

                        for (int j = row.FirstCellNum; j <= cellCount; j++)
                        {
                            try
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    {
                                        case CellType.Unknown:
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                            }
                                            else
                                            {
                                                dataRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                            }
                                            break;
                                        case CellType.String:
                                            string str = row.GetCell(j).StringCellValue;
                                            if (str != null && str.Length > 0)
                                            {
                                                dataRow[j] = str;
                                            }
                                            else
                                            {
                                                dataRow[j] = default(string);
                                            }
                                            break;
                                        case CellType.Formula:
                                            break;
                                        case CellType.Blank:
                                            break;
                                        case CellType.Boolean:
                                            dataRow[j] = Convert.ToDouble(row.GetCell(j).BooleanCellValue);
                                            break;
                                        case CellType.Error:
                                            dataRow[j] = Convert.ToDouble(row.GetCell(j).ErrorCellValue);

                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                loger.Error(ex.ToString());
                            }
                        }

                        table.Rows.Add(dataRow);
                    }
                    catch (Exception ex)
                    {

                        loger.Error(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                loger.Error(ex.ToString());
            }
            return table;
        }

        public static DataTable ImportExceltoDt(string strFileName, Dictionary<string, string> dir, string SheetName, int HeaderRowIndex = -1)
        {
            DataTable dt = new DataTable();
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (file.Length > 0)
                {
                    IWorkbook wb = WorkbookFactory.Create(file);
                    ISheet sheet = wb.GetSheet(SheetName);
                    dt = ImportDt(sheet, HeaderRowIndex, dir);
                    sheet = null;
                }
            }
            return dt;
        }
        public static DataTable ImportExceltoDt(string strFileName, Dictionary<string, string> dir, int sheetIndex, int HeaderRowIndex = -1)
        {
            DataTable dt = new DataTable();
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (file.Length > 0)
                {
                    IWorkbook wb = WorkbookFactory.Create(file);
                    ISheet sheet = wb.GetSheetAt(sheetIndex);
                    dt = ImportDt(sheet, HeaderRowIndex, dir);
                    sheet = null;
                }
            }
            return dt;
        }
        #endregion

        /// <summary>
        /// 获取excel中sheet数量
        /// </summary>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static int GetSheetNumber(string outputFile)
        {
            int number = 0;
            using (FileStream readFile = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                if (readFile.Length > 0)
                {
                    IWorkbook wb = WorkbookFactory.Create(readFile);
                    number = wb.NumberOfSheets;
                }
            }
            return number;
        }

        public static bool isNumeric(string message, out double result)
        {
            Regex rex = new Regex(@"^[-]?\d+[.]?\d*$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = double.Parse(message);
                return true;
            }
            else return false;
        }

        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook wb = new HSSFWorkbook(excelFileStream);
                if (wb.NumberOfSheets > 0)
                {
                    ISheet sheet = wb.GetSheetAt(0);
                    return sheet.PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }

        public static DataTable ListToDataTable<T>(List<T> list, string[] titles,out Dictionary<string,string> titleKey)
        {
            DataTable dt = new DataTable();
            Type listType = typeof(T);
            PropertyInfo[] properties = listType.GetProperties();
            titleKey = new Dictionary<string, string>();
            //标题行
            if (titles != null && properties.Length == titles.Length)
            {
                 
            }
            else
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyInfo property = properties[i];
                    dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                    titleKey.Add(property.Name, property.Name);
                }
            }
            //
            foreach (T item in list)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dr[i] = properties[i].GetValue(item, null);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static List<T> DataTableToList<T>(DataTable dt)where T:class, new ()
        {
            List<T> list = new List<T>();
            T t = new T();
            PropertyInfo[] prop = t.GetType().GetProperties();
            foreach (DataRow dataRow in dt.Rows)
            {
                t = new T();
                foreach (PropertyInfo pi in prop)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if(dataRow[pi.Name] != DBNull.Value)
                        {
                            object value = Convert.ChangeType(dataRow[pi.Name], pi.PropertyType);
                            pi.SetValue(t,value,null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        public static void ListExportExcel<T>(List<T> list, string strFileName)
        {
            //先转datatable
            DataTable dt = ListToDataTable(list, null,out Dictionary<string,string> dir);

            ExportDTtoExcel(dt, "HistoryData", strFileName, dir, true);
        }

        public static List<T> ExcelToList<T>(string strFileName)where T:class,new()
        {
            Type listType = typeof(T);
            PropertyInfo[] properties = listType.GetProperties();
            Dictionary<string, string> titleKey = new Dictionary<string, string>();

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyInfo property = properties[i];
                //dt.Columns.Add(new DataColumn(property.Name, property.PropertyType));
                titleKey.Add(property.Name, property.Name);
            }

            DataTable dt = ImportExceltoDt(strFileName, titleKey, 0,1);
            List<T> list = DataTableToList<T>(dt);
            return list;
        }
    }
}
