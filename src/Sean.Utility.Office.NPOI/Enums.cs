namespace Sean.Utility.Office.NPOI
{
    public enum CreateFileType
    {
        /// <summary>
        /// 获取现有文件（文件存在）或创建新文件（文件不存在）
        /// </summary>
        GetOrCreate,
        /// <summary>
        /// 获取现有文件（文件不存在则不做任何操作）
        /// </summary>
        Get,
        /// <summary>
        /// 创建新文件（保存时覆盖现有文件）
        /// </summary>
        Create
    }
}
