namespace Sean.Utility.Office.NPOI.Models
{
    public class ExcelExportOptions
    {
        /// <summary>
        /// Excel文件路径
        /// </summary>
        public string ExcelFilePath { get; set; }
        /// <summary>
        /// Excel中的SheetName
        /// </summary>
        public string SheetName { get; set; }
        /// <summary>
        /// 是否输出头部信息（DataTable=>ColumnName，Model=>属性名称）
        /// </summary>
        public bool OutputHeader { get; set; }
        /// <summary>
        /// 默认的单元格格式
        /// </summary>
        public CellStyle DefaultCellStyle { get; set; }
        /// <summary>
        /// 如何创建Excel文件的标志
        /// </summary>
        public CreateFileType CreateFileType { get; set; }
    }
}
