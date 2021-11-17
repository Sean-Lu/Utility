using System;
using System.Collections.Generic;
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
using Sean.Utility.Office.NPOI.Models;

namespace Sean.Utility.Office.NPOI
{
    /// <summary>
    /// Excel文件操作，支持格式：xls、xlsx
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// 处理DateTime数据的Func委托
        /// </summary>
        public static Func<DateTime, string> DateTimeFormat { get; set; }

        #region Public Methods
        /// <summary>
        /// DataTable转Excel。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="options"></param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcel(DataTable dt, ExcelExportOptions options)
        {
            var workbook = GetWorkbook(dt, options);
            if (workbook != null)
            {
                Save(workbook, options.ExcelFilePath);
            }
        }
        /// <summary>
        /// 泛型数据导出到Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="options"></param>
        public static void ToExcel<T>(IList<T> list, ExcelExportOptions options)
        {
            var workbook = GetWorkbook(list, options);
            if (workbook != null)
            {
                Save(workbook, options.ExcelFilePath);
            }
        }

        /// <summary>
        /// DataTable转Excel，并在浏览器中输出。
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="options"></param>
        /// <returns>写入Excel的行数</returns>
        public static void ToExcelInBrowser(DataTable dt, ExcelExportOptions options)
        {
            var workbook = GetWorkbook(dt, options);
            if (workbook == null) return;

            using (var ms = new MemoryStream())
            {
                workbook.Write(ms);
                workbook.Close();

#if NETSTANDARD
                HttpContext httpContext = HttpContextExt.Current ?? throw new ArgumentNullException("HttpContextExt.Current");
                httpContext.Response.ContentType = "application/octet-stream; charset=utf-8";
                httpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename={HttpUtility.UrlEncode(options.ExcelFilePath, Encoding.UTF8)}");
                var data = ms.ToArray();
                httpContext.Response.Body.WriteAsync(data, 0, data.Length);
#else
                HttpContext httpContext = HttpContext.Current;
                httpContext.Response.Clear();
                httpContext.Response.Buffer = true;
                //httpContext.Response.ContentType = "application/ms-excel";
                httpContext.Response.ContentEncoding = Encoding.UTF8;
                //httpContext.Response.Charset = "UTF-8";
                httpContext.Response.AddHeader("Content-Disposition", $"attachment; filename={HttpUtility.UrlEncode(options.ExcelFilePath, Encoding.UTF8)}");
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
                    var underlyingProperties = xmlProps.ExtendedProperties.GetUnderlyingProperties();

                    coreProps.Creator = documentInfo.Author;
                    coreProps.Subject = documentInfo.Subject;
                    coreProps.Title = documentInfo.Title;
                    coreProps.Description = documentInfo.Comments;
                    coreProps.Category = documentInfo.Category;

                    underlyingProperties.Company = documentInfo.Company;
                    underlyingProperties.Manager = documentInfo.Manager;

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

        private static IWorkbook GetWorkbook(DataTable dt, ExcelExportOptions options)
        {
            if (dt == null) throw new ArgumentNullException(nameof(dt));

            IWorkbook workbook = null;
            switch (options.CreateFileType)
            {
                case CreateFileType.GetOrCreate:
                    workbook = GetOrCreateWorkbook(options.ExcelFilePath);
                    break;
                case CreateFileType.Get:
                    workbook = GetWorkbook(options.ExcelFilePath);
                    break;
                case CreateFileType.Create:
                    workbook = CreateWorkbook(options.ExcelFilePath);
                    break;
            }

            if (workbook == null) return null;

            #region 设置单元格样式
            ICellStyle cellStyleHeader = null;
            ICellStyle cellStyleContent = null;
            if (options.DefaultCellStyle != null)
            {
                #region 标题
                cellStyleHeader = workbook.CreateCellStyle();
                if (options.DefaultCellStyle.Border)
                {
                    //设置边框格式
                    cellStyleHeader.BorderTop = BorderStyle.Thin;
                    cellStyleHeader.BorderBottom = BorderStyle.Thin;
                    cellStyleHeader.BorderLeft = BorderStyle.Thin;
                    cellStyleHeader.BorderRight = BorderStyle.Thin;
                }
                if (options.DefaultCellStyle.TitleFontHorizontalCenter)
                {
                    cellStyleHeader.Alignment = HorizontalAlignment.Center; //字体水平居中
                }
                IFont fontHeader = workbook.CreateFont();
                fontHeader.IsBold = options.DefaultCellStyle.TitleFontBold; //字体是否加粗
                fontHeader.FontHeightInPoints = options.DefaultCellStyle.TitleFontSize; //字体大小
                cellStyleHeader.SetFont(fontHeader);
                #endregion

                #region 内容
                cellStyleContent = workbook.CreateCellStyle();
                if (options.DefaultCellStyle.Border)
                {
                    //设置边框格式
                    cellStyleContent.BorderTop = BorderStyle.Thin;
                    cellStyleContent.BorderBottom = BorderStyle.Thin;
                    cellStyleContent.BorderLeft = BorderStyle.Thin;
                    cellStyleContent.BorderRight = BorderStyle.Thin;
                }
                IFont fontContent = workbook.CreateFont();
                fontContent.FontHeightInPoints = options.DefaultCellStyle.ContentFontSize; //字体大小
                cellStyleContent.SetFont(fontContent);
                #endregion
            }
            #endregion

            int count = 0;

            #region 创建sheet
            ISheet sheet;
            if (string.IsNullOrWhiteSpace(options.SheetName))
            {
                sheet = workbook.CreateSheet(); //创建一个工作表
            }
            else
            {
                if (workbook.GetSheet(options.SheetName) == null)
                {
                    //sheet不存在
                    sheet = workbook.CreateSheet(options.SheetName); //创建一个带名称的工作表
                }
                else
                {
                    //sheet已存在
                    int index = workbook.GetSheetIndex(options.SheetName);
                    workbook.RemoveSheetAt(index);
                    sheet = workbook.CreateSheet(options.SheetName); //创建一个带名称的工作表
                    workbook.SetSheetOrder(options.SheetName, index);
                }
            }
            #endregion

            //在指定sheet中写数据
            if (sheet != null)
            {
                #region 写标题
                if (options.OutputHeader)
                {
                    IRow row = sheet.CreateRow(0);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        ICell cell = row.CreateCell(i);
                        if (cell != null)
                        {
                            cell.SetCellValue(dt.Columns[i].ColumnName);

                            if (options.DefaultCellStyle != null)
                            {
                                cell.CellStyle = cellStyleHeader;
                                sheet.SetColumnWidth(i, options.DefaultCellStyle.ColumnWidth);
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
                            if (DateTimeFormat != null && val is DateTime time)
                            {
                                cell.SetCellValue(DateTimeFormat(time));
                            }
                            else
                            {
                                cell.SetCellValue(val.ToString());
                            }

                            if (options.DefaultCellStyle != null)
                            {
                                cell.CellStyle = cellStyleContent;
                            }
                        }
                    }
                    count++;
                }
                #endregion
            }

            return workbook;
        }
        private static IWorkbook GetWorkbook<T>(IList<T> list, ExcelExportOptions options)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            IWorkbook workbook = null;
            ISheet sheet;

            switch (options.CreateFileType)
            {
                case CreateFileType.GetOrCreate:
                    workbook = GetOrCreateWorkbook(options.ExcelFilePath);
                    break;
                case CreateFileType.Get:
                    workbook = GetWorkbook(options.ExcelFilePath);
                    break;
                case CreateFileType.Create:
                    workbook = CreateWorkbook(options.ExcelFilePath);
                    break;
            }

            if (workbook == null) return null;

            #region 设置单元格样式
            ICellStyle cellStyleHeader = null;
            ICellStyle cellStyleContent = null;
            if (options.DefaultCellStyle != null)
            {
                #region 标题
                cellStyleHeader = workbook.CreateCellStyle();
                if (options.DefaultCellStyle.Border)
                {
                    //设置边框格式
                    cellStyleHeader.BorderTop = BorderStyle.Thin;
                    cellStyleHeader.BorderBottom = BorderStyle.Thin;
                    cellStyleHeader.BorderLeft = BorderStyle.Thin;
                    cellStyleHeader.BorderRight = BorderStyle.Thin;
                }
                if (options.DefaultCellStyle.TitleFontHorizontalCenter)
                {
                    cellStyleHeader.Alignment = HorizontalAlignment.Center; //字体水平居中
                }
                IFont fontHeader = workbook.CreateFont();
                fontHeader.IsBold = options.DefaultCellStyle.TitleFontBold; //字体是否加粗
                fontHeader.FontHeightInPoints = options.DefaultCellStyle.TitleFontSize; //字体大小
                cellStyleHeader.SetFont(fontHeader);
                #endregion

                #region 内容
                cellStyleContent = workbook.CreateCellStyle();
                if (options.DefaultCellStyle.Border)
                {
                    //设置边框格式
                    cellStyleContent.BorderTop = BorderStyle.Thin;
                    cellStyleContent.BorderBottom = BorderStyle.Thin;
                    cellStyleContent.BorderLeft = BorderStyle.Thin;
                    cellStyleContent.BorderRight = BorderStyle.Thin;
                }
                IFont fontContent = workbook.CreateFont();
                fontContent.FontHeightInPoints = options.DefaultCellStyle.ContentFontSize; //字体大小
                cellStyleContent.SetFont(fontContent);
                #endregion
            }
            #endregion

            var count = 0;

            #region 创建sheet
            if (string.IsNullOrWhiteSpace(options.SheetName))
            {
                options.SheetName = typeof(T).Name;
            }

            if (workbook.GetSheet(options.SheetName) == null)
            {
                //sheet不存在
                sheet = workbook.CreateSheet(options.SheetName); //创建一个带名称的工作表
            }
            else
            {
                //sheet已存在
                int index = workbook.GetSheetIndex(options.SheetName);
                workbook.RemoveSheetAt(index);
                sheet = workbook.CreateSheet(options.SheetName); //创建一个带名称的工作表
                workbook.SetSheetOrder(options.SheetName, index);
            }
            #endregion

            if (sheet == null)
            {
                return null;
            }

            var propertyInfos = typeof(T).GetProperties();

            #region 写标题
            if (options.OutputHeader)
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

                        if (options.DefaultCellStyle != null)
                        {
                            cell.CellStyle = cellStyleHeader;
                            sheet.SetColumnWidth(i, options.DefaultCellStyle.ColumnWidth);
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
                            if (DateTimeFormat != null)
                            {
                                cell.SetCellValue(DateTimeFormat(time));
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

                        if (options.DefaultCellStyle != null)
                        {
                            cell.CellStyle = cellStyleContent;
                        }
                    }
                }
                count++;
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
        /// 保存
        /// </summary>
        /// <param name="workbook">workbook对象</param>
        /// <param name="excelFilePath">Excel文件路径</param>
        private static void Save(IWorkbook workbook, string excelFilePath)
        {
            if (workbook == null) return;

            using (var fs = new FileStream(excelFilePath, FileMode.Create, FileAccess.Write))
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
