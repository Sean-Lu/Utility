namespace Sean.Utility.Office.NPOI.Models
{
    /// <summary>
    /// 单元格格式
    /// </summary>
    public class CellStyle
    {
        /// <summary>
        /// 是否加边框（标题和内容）
        /// </summary>
        public bool Border { get; set; }

        /// <summary>
        /// 标题字体是否水平居中
        /// </summary>
        public bool TitleFontHorizontalCenter { get; set; }

        /// <summary>
        /// 标题字体是否加粗
        /// </summary>
        public bool TitleFontBold { get; set; }

        /// <summary>
        /// 标题字体大小
        /// </summary>
        public short TitleFontSize { get; set; } = 10;

        /// <summary>
        /// 内容字体大小
        /// </summary>
        public short ContentFontSize { get; set; } = 10;

        /// <summary>
        /// 列宽
        /// </summary>
        public ushort ColumnWidth { get; set; } = 10 * 256;
    }
}
