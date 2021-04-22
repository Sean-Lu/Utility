namespace Sean.Utility.Office.NPOI.Models
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public class DocumentInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comments { get; set; } = "Created By NPOI";

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; } = "Sean";
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 管理者
        /// </summary>
        public string Manager { get; set; }
    }
}
