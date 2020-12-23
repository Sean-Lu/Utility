namespace Sean.Utility.Enums
{
    /// <summary>
    /// HTTP请求类型
    /// </summary>
    public enum HttpRequestType
    {
        /// <summary>
        /// Get
        /// </summary>
        Get = 0,
        /// <summary>
        /// Post
        /// </summary>
        Post = 1,
        /// <summary>
        /// Put
        /// </summary>
        Put = 2,
        /// <summary>
        /// Delete
        /// </summary>
        Delete = 3
    }

    /// <summary>
    /// HTTP请求返回结果类型
    /// </summary>
    public enum HttpRequestResultType
    {
        /// <summary>
        /// Json格式
        /// </summary>
        Json = 0,
        /// <summary>
        /// Xml格式
        /// </summary>
        Xml = 1
    }
}
