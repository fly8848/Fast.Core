using Fast.Core.Extensions;

namespace Fast.Core.Extensions.Tests
{
    /// <summary>
    /// 字符串扩展方法测试类
    /// </summary>
    public class StringExtensionsTests
    {
        /// <summary>
        /// 测试 IsNullOrWhiteSpace 在各种输入情况下是否返回预期结果
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="expected">预期结果</param>
        [Theory]
        [InlineData(null, true)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        [InlineData("\t\n\r", true)]
        [InlineData("  Test  ", false)]
        [InlineData("Test", false)]
        public void IsNullOrWhiteSpace_WithVariousInputs_ReturnsExpectedResult(string? input, bool expected)
        {
            // Arrange - 通过参数传入测试数据，不需要额外准备
            
            // Act
            bool result = input.IsNullOrWhiteSpace();
            
            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>
        /// 测试 IsNotNullOrWhiteSpace 在各种输入情况下是否返回预期结果
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="expected">预期结果</param>
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("   ", false)]
        [InlineData("\t\n\r", false)]
        [InlineData("  Test  ", true)]
        [InlineData("Test", true)]
        public void IsNotNullOrWhiteSpace_WithVariousInputs_ReturnsExpectedResult(string? input, bool expected)
        {
            // Arrange - 通过参数传入测试数据，不需要额外准备
            
            // Act
            bool result = input.IsNotNullOrWhiteSpace();
            
            // Assert
            Assert.Equal(expected, result);
        }
    }
}