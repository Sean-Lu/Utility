namespace Sean.Utility.Models
{
    /// <summary>
    /// 二叉树模型
    /// </summary>
    public class BinaryTree
    {
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 左子树
        /// </summary>
        public BinaryTree Left { get; set; }
        /// <summary>
        /// 右子树
        /// </summary>
        public BinaryTree Right { get; set; }
    }
}
