using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
#if NETSTANDARD
using Microsoft.AspNetCore.Http;
#endif
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

//[assembly: System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]// 解决：类型“XXX”违反了继承安全性规则。派生类型必须与基类型的安全可访问性匹配或者比基类型的安全可访问性低。
namespace Sean.Utility.Office.NPOI
{
    /// <summary>
    /// Excel文件操作类（支持xls、xlsx）
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 处理DateTime数据的Func委托
        /// </summary>
        public static Func<DateTime, string> HandleDateTimeDataFunc { get; set; }

        #region Public Methods
        /// <summary>
        /// DataTable转Excel（SheetName使用对应DataTable的TableName）。
        /// 支持在已存在的Excel文件中添加新的sheet。
        /// </summary>
        /// <param name="dt">待写入Excel的DataTable对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        /// <param name="createFlag">如何创建Excel文件的标志。0表示获取现有文件（文件存在）或创建新文件（文件不存在）；1表示获取现有文件（文件不存在则不做任何操作）；2表示创建新文件（保存时覆盖现有文件）。</param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcel(DataTable dt, string excelFilePath, bool isColumnWritten, CellStyle cellStyle, int createFlag = 0)
        {
            if (dt == null || dt.Rows.Count < 1) return;

            var ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            ToExcel(ds, excelFilePath, isColumnWritten, cellStyle, createFlag);
        }
        /// <summary>
        /// DataTable转Excel。
        /// 支持在已存在的Excel文件中添加新的sheet。
        /// </summary>
        /// <param name="dt">待写入Excel的DataTable对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时使用默认名称</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        /// <param name="createFlag">如何创建Excel文件的标志。0表示获取现有文件（文件存在）或创建新文件（文件不存在）；1表示获取现有文件（文件不存在则不做任何操作）；2表示创建新文件（保存时覆盖现有文件）。</param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcel(DataTable dt, string excelFilePath, string sheetName, bool isColumnWritten, CellStyle cellStyle, int createFlag = 0)
        {
            if (dt == null || dt.Rows.Count < 1) return;

            var dtTmp = dt.Copy();
            dtTmp.TableName = sheetName;
            ToExcel(dtTmp, excelFilePath, isColumnWritten, cellStyle, createFlag);
        }
        /// <summary>
        /// DataSet转Excel（SheetName使用对应DataTable的TableName）。
        /// 支持在已存在的Excel文件中添加新的sheet。
        /// </summary>
        /// <param name="ds">待写入Excel的DataSet对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        /// <param name="createFlag">如何创建Excel文件的标志。0表示获取现有文件（文件存在）或创建新文件（文件不存在）；1表示获取现有文件（文件不存在则不做任何操作）；2表示创建新文件（保存时覆盖现有文件）。</param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcel(DataSet ds, string excelFilePath, bool isColumnWritten, CellStyle cellStyle, int createFlag = 0)
        {
            var workbook = GetWorkbook(ds, excelFilePath, isColumnWritten, cellStyle, createFlag);
            if (workbook != null)
            {
                Save(workbook, excelFilePath);
            }
        }
        /// <summary>
        /// 泛型数据导出到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="excelFilePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="isHeaderWritten"></param>
        /// <param name="cellStyle"></param>
        /// <param name="createFlag"></param>
        public static void ToExcel<T>(IList<T> list, string excelFilePath, string sheetName = null, bool isHeaderWritten = true, CellStyle cellStyle = null, int createFlag = 0)
        {
            var workbook = GetWorkbook(list, excelFilePath, sheetName, isHeaderWritten, cellStyle, createFlag);
            if (workbook != null)
            {
                Save(workbook, excelFilePath);
            }
        }

        /// <summary>
        /// DataTable转Excel（SheetName使用对应DataTable的TableName），并在浏览器中输出。
        /// </summary>
        /// <param name="dt">待写入Excel的DataTable对象</param>
        /// <param name="excelFileName">Excel文件名称</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcelInBrowser(DataTable dt, string excelFileName, bool isColumnWritten, CellStyle cellStyle)
        {
            if (dt == null || dt.Rows.Count < 1) return;

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            ToExcelInBrowser(ds, excelFileName, isColumnWritten, cellStyle);
        }
        /// <summary>
        /// DataSet转Excel（SheetName使用对应DataTable的TableName），并在浏览器中输出。
        /// </summary>
        /// <param name="ds">待写入Excel的DataSet对象</param>
        /// <param name="excelFileName">Excel文件名称</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcelInBrowser(DataSet ds, string excelFileName, bool isColumnWritten, CellStyle cellStyle)
        {
            IWorkbook workbook = GetWorkbook(ds, excelFileName, isColumnWritten, cellStyle, 2);
            if (workbook == null) return;

            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                workbook.Close();

#if NETSTANDARD
                HttpContext httpContext = HttpContextExt.Current ?? throw new ArgumentNullException("HttpContextExt.Current");
                httpContext.Response.ContentType = "application/octet-stream; charset=utf-8";
                httpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={HttpUtility.UrlEncode(excelFileName, Encoding.UTF8)}");
                var data = ms.ToArray();
                httpContext.Response.Body.WriteAsync(data, 0, data.Length);
#else
                HttpContext httpContext = HttpContext.Current;
                httpContext.Response.Clear();
                httpContext.Response.Buffer = true;
                //httpContext.Response.ContentType = "application/ms-excel";
                httpContext.Response.ContentEncoding = Encoding.UTF8;
                //httpContext.Response.Charset = "UTF-8";
                httpContext.Response.AddHeader("Content-Disposition", $"attachment; filename={HttpUtility.UrlEncode(excelFileName, Encoding.UTF8)}");
                //httpContext.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", HttpUtility.UrlPathEncode(excelFileName)));
                httpContext.Response.BinaryWrite(ms.ToArray());
                httpContext.Response.End();
#endif
            }
        }

        /// <summary>
        /// Excel转DataTable（SheetName保存到对应DataTable的TableName中）。
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="contentStartPositionOffset">内容开始位置偏移量（从第几行开始）</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ToDataTable(string excelFilePath, string sheetName, bool isFirstRowColumn, int contentStartPositionOffset = 0)
        {
            DataSet ds = ToDataSet(excelFilePath, isFirstRowColumn, contentStartPositionOffset);
            return ds.Tables.Cast<DataTable>().FirstOrDefault(table => table.TableName == sheetName);
        }
        /// <summary>
        /// Excel转DataSet（SheetName保存到对应DataTable的TableName中）。
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="contentStartPositionOffset">内容开始位置偏移量（从第几行开始）</param>
        /// <returns>返回的DataSet</returns>
        public static DataSet ToDataSet(string excelFilePath, bool isFirstRowColumn, int contentStartPositionOffset = 0)
        {
            IWorkbook workbook = GetWorkbook(excelFilePath);
            if (workbook == null) return null;

            DataSet dsRet = new DataSet();
            int sheetCount = workbook.NumberOfSheets;
            for (int i = 0; i < sheetCount; i++)
            {
                ISheet sheet = GetSheet(workbook, i);
                if (sheet != null)
                {
                    DataTable dt = new DataTable { TableName = sheet.SheetName };

                    IRow rowFirst = sheet.GetRow(sheet.FirstRowNum); //sheet.GetRow(0);
                    if (rowFirst != null)
                    {
                        int cellCount = rowFirst.LastCellNum;

                        //写标题
                        for (int cellnum = rowFirst.FirstCellNum; cellnum < cellCount; cellnum++)
                        {
                            ICell cell = rowFirst.GetCell(cellnum);
                            if (cell != null)
                            {
                                DataColumn column = new DataColumn(isFirstRowColumn ? cell.StringCellValue : "Col" + cellnum);
                                dt.Columns.Add(column);
                            }
                        }

                        //写内容
                        for (int rownum = isFirstRowColumn ? sheet.FirstRowNum + 1 + contentStartPositionOffset : sheet.FirstRowNum + contentStartPositionOffset; rownum <= sheet.LastRowNum; rownum++)
                        {
                            IRow row = sheet.GetRow(rownum);
                            if (row == null) continue; //没有数据的行默认是null　　　　　　　

                            DataRow dr = dt.NewRow();
                            for (int cellnum = row.FirstCellNum; cellnum < cellCount; cellnum++)
                            {
                                if (row.GetCell(cellnum) != null) //同理，没有数据的单元格都默认是null
                                    dr[cellnum] = row.GetCell(cellnum).ToString();
                            }
                            dt.Rows.Add(dr);
                        }
                    }

                    dsRet.Tables.Add(dt);
                }
            }
            workbook.Close();
            return dsRet;
        }

        /// <summary>
        /// 插入行
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <param name="rowIndex">行索引。从0开始</param>
        /// <param name="insert">插入数据</param>
        public static void InsertRows(string excelFilePath, string sheetName, int rowIndex, List<string[]> insert)
        {
            if (!File.Exists(excelFilePath)) return;
            if (insert == null) return;

            int count = insert.Count;
            InsertEmptyRows(excelFilePath, sheetName, rowIndex, count);//插入空行

            IWorkbook workbook = GetWorkbook(excelFilePath);
            ISheet sheet = GetSheet(workbook, sheetName);
            if (sheet != null)
            {
                for (int i = 0; i < count; i++)
                {
                    IRow row = sheet.CreateRow(rowIndex + i);//如果在这之前没有插入空行，CreateRow时会删掉原有的行数据
                    for (int j = 0; j < insert[i].Length; j++)
                    {
                        row.CreateCell(j).SetCellValue(insert[i][j]);
                    }
                }

                Save(workbook, excelFilePath);
            }
        }
        /// <summary>
        /// 插入空行
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <param name="rowIndex">行索引。从0开始</param>
        /// <param name="count">行数量</param>
        public static void InsertEmptyRows(string excelFilePath, string sheetName, int rowIndex, int count)
        {
            if (!File.Exists(excelFilePath)) return;
            if (count <= 0) return;

            IWorkbook workbook = GetWorkbook(excelFilePath);
            ISheet sheet = GetSheet(workbook, sheetName);
            if (sheet != null)
            {
                sheet.ShiftRows(rowIndex, sheet.LastRowNum, count);
                Save(workbook, excelFilePath);
            }
        }
        /// <summary>
        /// 插入标题（在第一行添加一条数据）
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <param name="insert">插入数据</param>
        public static void InsertTitle(string excelFilePath, string sheetName, string[] insert)
        {
            InsertRows(excelFilePath, sheetName, 0, new List<string[]>() { insert });
        }

        /// <summary>
        /// 删除行数据（不移除行）
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <param name="rowIndex">行索引。从0开始</param>
        /// <param name="count">行数量</param>
        public static void DeleteRowsData(string excelFilePath, string sheetName, int rowIndex, int count)
        {
            if (!File.Exists(excelFilePath)) return;
            if (count <= 0) return;

            IWorkbook workbook = GetWorkbook(excelFilePath);
            ISheet sheet = GetSheet(workbook, sheetName);
            if (sheet != null)
            {
                for (int i = 0; i < count; i++)
                {
                    IRow row = sheet.GetRow(rowIndex + i);
                    if (row != null) sheet.RemoveRow(row);
                }

                Save(workbook, excelFilePath);
            }
        }
        /// <summary>
        /// 移除行（包含数据）
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <param name="rowIndex">行索引。从0开始</param>
        /// <param name="count">行数量</param>
        public static void RemoveRows(string excelFilePath, string sheetName, int rowIndex, int count)
        {
            if (!File.Exists(excelFilePath)) return;
            if (count <= 0) return;

            DeleteRowsData(excelFilePath, sheetName, rowIndex, count);// 删除行数据（不移除行）

            IWorkbook workbook = GetWorkbook(excelFilePath);
            ISheet sheet = GetSheet(workbook, sheetName);
            if (sheet != null)
            {
                sheet.ShiftRows(rowIndex + count, sheet.LastRowNum, -count);
                Save(workbook, excelFilePath);
            }
        }

        /// <summary>
        /// 另存为（仅保存数据，不保存原有Excel的格式）。
        /// 注：可作为xls转xlsx、xlsx转xls使用。
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="newFilePath">新的Excel文件路径</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        public static void SaveAs(string excelFilePath, string newFilePath, CellStyle cellStyle)
        {
            DataSet ds = ToDataSet(excelFilePath, false);
            ToExcel(ds, newFilePath, false, cellStyle);
        }

        /// <summary>
        /// 添加文件信息
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="documentInfo">文件信息。如果为null，则不设置文件信息</param>
        public static void AddDocumentInfo(string excelFilePath, DocumentInfo documentInfo)
        {
            if (!File.Exists(excelFilePath)) return;
            if (documentInfo == null) return;

            IWorkbook workbook = GetWorkbook(excelFilePath);
            if (workbook != null)
            {
                if (workbook is HSSFWorkbook)
                {
                    HSSFWorkbook hssfWorkbook = workbook as HSSFWorkbook;

                    var documentSummaryInfo = hssfWorkbook.DocumentSummaryInformation;
                    var summaryInfo = hssfWorkbook.SummaryInformation;

                    DocumentSummaryInformation dsi = documentSummaryInfo ?? PropertySetFactory.CreateDocumentSummaryInformation();
                    dsi.Company = documentInfo.Company;
                    dsi.Category = documentInfo.Category;
                    dsi.Manager = documentInfo.Manager;
                    if (documentSummaryInfo == null)
                        hssfWorkbook.DocumentSummaryInformation = dsi;

                    SummaryInformation si = summaryInfo ?? PropertySetFactory.CreateSummaryInformation();
                    si.Author = documentInfo.Author;
                    si.Subject = documentInfo.Subject;
                    si.Title = documentInfo.Title;
                    si.Comments = documentInfo.Comments;
                    if (summaryInfo == null)
                        hssfWorkbook.SummaryInformation = si;

                    Save(hssfWorkbook, excelFilePath);
                }
                else if (workbook is XSSFWorkbook)
                {
                    XSSFWorkbook xssfWorkbook = workbook as XSSFWorkbook;

                    var xmlProps = xssfWorkbook.GetProperties();
                    var coreProps = xmlProps.CoreProperties;

                    coreProps.Category = documentInfo.Category;

                    coreProps.Creator = documentInfo.Author;
                    coreProps.Subject = documentInfo.Subject;
                    coreProps.Title = documentInfo.Title;
                    coreProps.Description = documentInfo.Comments;

                    Save(xssfWorkbook, excelFilePath);
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 私有构造函数（防止类被实例化）
        /// </summary>
        private ExcelHelper() { }

        /// <summary>
        /// 获取Workbook
        /// </summary>
        /// <param name="ds">待写入Excel的DataSet对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="isColumnWritten">DataTable的列名是否要写入</param>
        /// <param name="cellStyle">单元格格式。如果为null，则不设置单元格格式。</param>
        /// <param name="createFlag">如何创建Excel文件的标志。0表示获取现有文件（文件存在）或创建新文件（文件不存在）；1表示获取现有文件（文件不存在则不做任何操作）；2表示创建新文件（保存时覆盖现有文件）。</param>
        /// <returns>IWorkbook对象</returns>
        private static IWorkbook GetWorkbook(DataSet ds, string excelFilePath, bool isColumnWritten, CellStyle cellStyle, int createFlag)
        {
            if (ds == null || ds.Tables.Count <= 0) return null;

            List<int> list = new List<int>();//写入Excel的行数
            IWorkbook workbook = null;
            ISheet sheet = null;

            switch (createFlag)
            {
                case 0:
                    workbook = GetOrCreateWorkbook(excelFilePath);
                    break;
                case 1:
                    workbook = GetWorkbook(excelFilePath);
                    break;
                case 2:
                    workbook = CreateWorkbook(excelFilePath);
                    break;
            }

            if (workbook == null) return null;

            #region 设置单元格样式
            ICellStyle cellStyleHeader = null;
            ICellStyle cellStyleContent = null;
            if (cellStyle != null)
            {
                #region 标题
                cellStyleHeader = workbook.CreateCellStyle();
                if (cellStyle.Border)
                {
                    //设置边框格式
                    cellStyleHeader.BorderTop = BorderStyle.Thin;
                    cellStyleHeader.BorderBottom = BorderStyle.Thin;
                    cellStyleHeader.BorderLeft = BorderStyle.Thin;
                    cellStyleHeader.BorderRight = BorderStyle.Thin;
                }
                if (cellStyle.TitleFontHorizontalCenter)
                {
                    cellStyleHeader.Alignment = HorizontalAlignment.Center; //字体水平居中
                }
                IFont fontHeader = workbook.CreateFont();
                fontHeader.IsBold = cellStyle.TitleFontBold; //字体是否加粗
                fontHeader.FontHeightInPoints = cellStyle.TitleFontSize; //字体大小
                cellStyleHeader.SetFont(fontHeader);
                #endregion

                #region 内容
                cellStyleContent = workbook.CreateCellStyle();
                if (cellStyle.Border)
                {
                    //设置边框格式
                    cellStyleContent.BorderTop = BorderStyle.Thin;
                    cellStyleContent.BorderBottom = BorderStyle.Thin;
                    cellStyleContent.BorderLeft = BorderStyle.Thin;
                    cellStyleContent.BorderRight = BorderStyle.Thin;
                }
                IFont fontContent = workbook.CreateFont();
                fontContent.FontHeightInPoints = cellStyle.ContentFontSize; //字体大小
                cellStyleContent.SetFont(fontContent);
                #endregion
            }
            #endregion

            for (int k = 0; k < ds.Tables.Count; k++)
            {
                DataTable dt = ds.Tables[k];
                if (dt == null || dt.Rows.Count <= 0) continue;

                int count = 0;
                string sheetName = GetValidSheetName(ds.Tables[k].TableName);

                #region 创建sheet
                if (string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.CreateSheet(); //创建一个工作表
                }
                else
                {
                    if (workbook.GetSheet(sheetName) == null)
                    {
                        //sheet不存在
                        sheet = workbook.CreateSheet(sheetName); //创建一个带名称的工作表
                    }
                    else
                    {
                        //sheet已存在
                        int index = workbook.GetSheetIndex(sheetName);
                        workbook.RemoveSheetAt(index);
                        sheet = workbook.CreateSheet(sheetName); //创建一个带名称的工作表
                        workbook.SetSheetOrder(sheetName, index);
                    }
                }
                #endregion

                //在指定sheet中写数据
                if (sheet != null)
                {
                    #region 写标题
                    if (isColumnWritten)
                    {
                        IRow row = sheet.CreateRow(0);
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            ICell cell = row.CreateCell(i);
                            if (cell != null)
                            {
                                cell.SetCellValue(dt.Columns[i].ColumnName);

                                if (cellStyle != null)
                                {
                                    cell.CellStyle = cellStyleHeader;
                                    sheet.SetColumnWidth(i, cellStyle.ColumnWidth);
                                }
                            }
                        }
                        count++;
                    }
                    #endregion

                    #region 写内容
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        IRow row = sheet.CreateRow(count);
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);
                            if (cell != null)
                            {
                                var val = dt.Rows[i][j];
                                if (HandleDateTimeDataFunc != null && val is DateTime time)
                                {
                                    cell.SetCellValue(HandleDateTimeDataFunc(time));
                                }
                                else
                                {
                                    cell.SetCellValue(val.ToString());
                                }

                                if (cellStyle != null)
                                {
                                    cell.CellStyle = cellStyleContent;
                                }
                            }
                        }
                        count++;
                    }
                    #endregion
                }
                list.Add(count);
            }

            //return list;
            return workbook;
        }
        private static IWorkbook GetWorkbook<T>(IList<T> list, string excelFilePath, string sheetName, bool isHeaderWritten, CellStyle cellStyle, int createFlag)
        {
            if (list == null || list.Count <= 0) return null;

            var listRows = new List<int>();//写入Excel的行数
            IWorkbook workbook = null;
            ISheet sheet = null;

            switch (createFlag)
            {
                case 0:
                    workbook = GetOrCreateWorkbook(excelFilePath);
                    break;
                case 1:
                    workbook = GetWorkbook(excelFilePath);
                    break;
                case 2:
                    workbook = CreateWorkbook(excelFilePath);
                    break;
            }

            if (workbook == null) return null;

            #region 设置单元格样式
            ICellStyle cellStyleHeader = null;
            ICellStyle cellStyleContent = null;
            if (cellStyle != null)
            {
                #region 标题
                cellStyleHeader = workbook.CreateCellStyle();
                if (cellStyle.Border)
                {
                    //设置边框格式
                    cellStyleHeader.BorderTop = BorderStyle.Thin;
                    cellStyleHeader.BorderBottom = BorderStyle.Thin;
                    cellStyleHeader.BorderLeft = BorderStyle.Thin;
                    cellStyleHeader.BorderRight = BorderStyle.Thin;
                }
                if (cellStyle.TitleFontHorizontalCenter)
                {
                    cellStyleHeader.Alignment = HorizontalAlignment.Center; //字体水平居中
                }
                IFont fontHeader = workbook.CreateFont();
                fontHeader.IsBold = cellStyle.TitleFontBold; //字体是否加粗
                fontHeader.FontHeightInPoints = cellStyle.TitleFontSize; //字体大小
                cellStyleHeader.SetFont(fontHeader);
                #endregion

                #region 内容
                cellStyleContent = workbook.CreateCellStyle();
                if (cellStyle.Border)
                {
                    //设置边框格式
                    cellStyleContent.BorderTop = BorderStyle.Thin;
                    cellStyleContent.BorderBottom = BorderStyle.Thin;
                    cellStyleContent.BorderLeft = BorderStyle.Thin;
                    cellStyleContent.BorderRight = BorderStyle.Thin;
                }
                IFont fontContent = workbook.CreateFont();
                fontContent.FontHeightInPoints = cellStyle.ContentFontSize; //字体大小
                cellStyleContent.SetFont(fontContent);
                #endregion
            }
            #endregion

            var count = 0;
            sheetName = string.IsNullOrEmpty(sheetName) ? GetValidSheetName(typeof(T).Name) : GetValidSheetName(sheetName);

            #region 创建sheet
            if (string.IsNullOrEmpty(sheetName))
            {
                sheet = workbook.CreateSheet(); //创建一个工作表
            }
            else
            {
                if (workbook.GetSheet(sheetName) == null)
                {
                    //sheet不存在
                    sheet = workbook.CreateSheet(sheetName); //创建一个带名称的工作表
                }
                else
                {
                    //sheet已存在
                    int index = workbook.GetSheetIndex(sheetName);
                    workbook.RemoveSheetAt(index);
                    sheet = workbook.CreateSheet(sheetName); //创建一个带名称的工作表
                    workbook.SetSheetOrder(sheetName, index);
                }
            }
            #endregion

            if (sheet == null)
            {
                return null;
            }

            var propertyInfos = typeof(T).GetProperties();

            #region 写标题
            if (isHeaderWritten)
            {
                IRow row = sheet.CreateRow(0);
                for (var i = 0; i < propertyInfos.Length; i++)
                {
                    var propertyInfo = propertyInfos[i];
                    var propertyInfoName = propertyInfo.Name;

                    ICell cell = row.CreateCell(i);
                    if (cell != null)
                    {
                        cell.SetCellValue(propertyInfoName);

                        if (cellStyle != null)
                        {
                            cell.CellStyle = cellStyleHeader;
                            sheet.SetColumnWidth(i, cellStyle.ColumnWidth);
                        }
                    }
                }
                count++;
            }
            #endregion

            #region 写内容
            foreach (var model in list)
            {
                if (model == null) continue;

                IRow row = sheet.CreateRow(count);
                for (var i = 0; i < propertyInfos.Length; i++)
                {
                    var propertyInfo = propertyInfos[i];
                    var propertyValue = propertyInfo.GetValue(model, null);

                    ICell cell = row.CreateCell(i);
                    if (cell != null)
                    {
                        if (propertyValue is DateTime time)
                        {
                            if (HandleDateTimeDataFunc != null)
                            {
                                cell.SetCellValue(HandleDateTimeDataFunc(time));
                            }
                            else
                            {
                                cell.SetCellValue(time);
                            }
                        }
                        else if (propertyValue is double doubleValue)
                        {
                            cell.SetCellValue(doubleValue);
                        }
                        else if (propertyValue is bool booleanValue)
                        {
                            cell.SetCellValue(booleanValue);
                        }
                        else
                        {
                            cell.SetCellValue(propertyValue?.ToString());
                        }

                        if (cellStyle != null)
                        {
                            cell.CellStyle = cellStyleContent;
                        }
                    }
                }
                count++;
                listRows.Add(count);
            }
            #endregion

            return workbook;
        }
        /// <summary>
        /// 获取Workbook（文件存在则获取，不存在则返回null）
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <returns></returns>
        private static IWorkbook GetWorkbook(string excelFilePath)
        {
            if (!File.Exists(excelFilePath)) return null;

            IWorkbook workbook;
            using (FileStream fs = new FileStream(excelFilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                workbook = GetOrCreateWorkbook(Path.GetExtension(excelFilePath), fs);
                fs.Close();
            }

            return workbook;
        }
        /// <summary>
        /// 创建Workbook（文件存在则覆盖现有文件）
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <returns></returns>
        private static IWorkbook CreateWorkbook(string excelFilePath)
        {
            return GetOrCreateWorkbook(Path.GetExtension(excelFilePath), null);
        }
        /// <summary>
        /// 获取或创建Workbook（文件存在则获取，不存在则创建）
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <returns></returns>
        private static IWorkbook GetOrCreateWorkbook(string excelFilePath)
        {
            return File.Exists(excelFilePath) ? GetWorkbook(excelFilePath) : CreateWorkbook(excelFilePath);
        }
        /// <summary>
        /// 获取或创建Workbook
        /// </summary>
        /// <param name="extension">文件扩展名</param>
        /// <param name="fileStream">文件流。如果为null，则创建新的Workbook；如果不为null，则根据文件流创建Workbook。</param>
        /// <returns></returns>
        private static IWorkbook GetOrCreateWorkbook(string extension, Stream fileStream)
        {
            bool exist = fileStream != null;
            IWorkbook workbook = null;
            if (!string.IsNullOrEmpty(extension))
            {
                switch (extension.ToLower())
                {
                    case ".xls":
                        workbook = exist ? new HSSFWorkbook(fileStream) : new HSSFWorkbook(); //HSSF适用2007以前的版本
                        break;
                    case ".xlsx":
                        workbook = exist ? new XSSFWorkbook(fileStream) : new XSSFWorkbook(); //XSSF适用2007版本及其以上的
                        break;
                }
            }
            return workbook;
        }

        /// <summary>
        /// 获取Sheet
        /// </summary>
        /// <param name="workbook">workbook对象</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <returns></returns>
        private static ISheet GetSheet(IWorkbook workbook, string sheetName)
        {
            if (workbook == null) return null;

            return !string.IsNullOrEmpty(sheetName) ? workbook.GetSheet(sheetName)  //获取指定sheetName的sheet
                                                    : workbook.GetSheetAt(0);       //获取第一个sheet
        }
        /// <summary>
        /// 获取Sheet
        /// </summary>
        /// <param name="workbook">workbook对象</param>
        /// <param name="index">sheet索引，从0开始</param>
        /// <returns></returns>
        private static ISheet GetSheet(IWorkbook workbook, int index)
        {
            if (workbook == null) return null;

            return workbook.GetSheetAt(index);
        }
        /// <summary>
        /// 获取Sheet
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="sheetName">sheet名称。值为空时取第一个sheet</param>
        /// <returns></returns>
        private static ISheet GetSheet(string excelFilePath, string sheetName)
        {
            IWorkbook workbook = GetWorkbook(excelFilePath);
            return GetSheet(workbook, sheetName);
        }
        /// <summary>
        /// 获取Sheet
        /// </summary>
        /// <param name="excelFilePath">Excel文件路径</param>
        /// <param name="index">sheet索引，从0开始</param>
        /// <returns></returns>
        private static ISheet GetSheet(string excelFilePath, int index)
        {
            IWorkbook workbook = GetWorkbook(excelFilePath);
            return GetSheet(workbook, index);
        }

        /// <summary>
        /// 获取合法sheet名称
        /// </summary>
        /// <param name="sheetName">可能包含非法字符的sheet名称</param>
        /// <returns></returns>
        private static string GetValidSheetName(string sheetName)
        {
            if (!string.IsNullOrEmpty(sheetName))
            {
                //名称不多于31个字符
                if (sheetName.Length > 31)
                    sheetName = sheetName.Substring(0, 31);

                //名称不包含下列任一字符
                char[] invalidSheetNameChars = { ':', '\\', '/', '?', '*', '[', ']' };

                StringBuilder builder = new StringBuilder(sheetName);
                foreach (char invalidChar in invalidSheetNameChars)
                    builder.Replace(invalidChar, ' ');
                sheetName = builder.ToString();
            }
            return sheetName;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="workbook">workbook对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        private static void Save(IWorkbook workbook, string excelFilePath)
        {
            if (workbook == null) return;

            using (FileStream fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
                workbook.Close();
            }
        }

        private static object GetCellValue(ICell cell)
        {
            object value = null;
            if (cell.CellType != CellType.Blank)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        // Date Type的数据CellType是Numeric
                        if (DateUtil.IsCellDateFormatted(cell))
                        {
                            value = cell.DateCellValue;
                        }
                        else
                        {
                            // Numeric type
                            value = cell.NumericCellValue;
                        }
                        break;
                    case CellType.Boolean:
                        // Boolean type
                        value = cell.BooleanCellValue;
                        break;
                    default:
                        // String type
                        value = cell.StringCellValue;
                        break;
                }
            }
            return value;
        }
        private static void SetCellValue(ICell cell, object obj)
        {
            if (obj is string)
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (obj is bool)
            {
                cell.SetCellValue((bool)obj);
            }
            else if (obj is int)
            {
                cell.SetCellValue((int)obj);
            }
            else if (obj is double)
            {
                cell.SetCellValue((double)obj);
            }
            else if (obj is DateTime)
            {
                cell.SetCellValue((DateTime)obj);
            }
            else if (obj.GetType() == typeof(IRichTextString))
            {
                cell.SetCellValue((IRichTextString)obj);
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }
        #endregion
    }
}
