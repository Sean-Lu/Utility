namespace Sean.Utility.Office.NPOI
{
    /// <summary>
    /// 单元格格式
    /// </summary>
    public class CellStyle
    {
        #region Fields
        /// <summary>
        /// 列宽
        /// </summary>
        private ushort _columnWidth = 10 * 256;
        #endregion

        #region Properties
        /// <summary>
        /// 是否加边框（标题和内容）
        /// </summary>
        public bool Border { get; set; } = false;

        /// <summary>
        /// 标题字体是否水平居中
        /// </summary>
        public bool TitleFontHorizontalCenter { get; set; } = false;

        /// <summary>
        /// 标题字体是否加粗
        /// </summary>
        public bool TitleFontBold { get; set; } = false;

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
        public ushort ColumnWidth
        {
            get => _columnWidth;
            set => _columnWidth = (ushort)(value * 256);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 设置自定义值
        /// </summary>
        public void SetCustomValue()
        {
            Border = true;
            TitleFontHorizontalCenter = true;
            TitleFontBold = true;
            TitleFontSize = 10;
            ContentFontSize = 10;
            _columnWidth = 10 * 256;
        }
        #endregion
    }
}
