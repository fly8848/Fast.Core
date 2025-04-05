namespace Fast.Core.Extensions
{
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 判断字符串是否为空或空白
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>是否为空或空白</returns>
        public static bool IsNullOrWhiteSpace(this string? input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// 判断字符串是否不为空或空白
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>是否不为空或空白</returns>
        public static bool IsNotNullOrWhiteSpace(this string? input)
        {
            return !string.IsNullOrWhiteSpace(input);
        }
    }
}