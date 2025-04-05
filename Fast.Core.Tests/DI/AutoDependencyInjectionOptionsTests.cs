using System;
using System.Reflection;
using Fast.Core.DI;

namespace Fast.Core.Tests.DI
{
    /// <summary>
    /// 自动依赖注入配置选项测试类
    /// </summary>
    public class AutoDependencyInjectionOptionsTests
    {
        /// <summary>
        /// 测试添加单个程序集
        /// </summary>
        [Fact]
        public void AddAssembly_WithValidAssembly_AddsToCollection()
        {
            // Arrange
            var options = new AutoDependencyInjectionOptions();
            var assembly = typeof(AutoDependencyInjectionOptionsTests).Assembly;
            
            // Act
            options.AddAssembly(assembly);
            
            // Assert
            Assert.Contains(assembly, options.AssembliesToScan);
            Assert.Single(options.AssembliesToScan);
        }

        /// <summary>
        /// 测试添加重复程序集时只添加一次
        /// </summary>
        [Fact]
        public void AddAssembly_WithDuplicateAssembly_AddsOnlyOnce()
        {
            // Arrange
            var options = new AutoDependencyInjectionOptions();
            var assembly = typeof(AutoDependencyInjectionOptionsTests).Assembly;
            
            // Act
            options.AddAssembly(assembly);
            options.AddAssembly(assembly); // 添加相同的程序集第二次
            
            // Assert
            Assert.Contains(assembly, options.AssembliesToScan);
            Assert.Single(options.AssembliesToScan);
        }

        /// <summary>
        /// 测试添加多个程序集
        /// </summary>
        [Fact]
        public void AddAssembly_WithMultipleAssemblies_AddsAllToCollection()
        {
            // Arrange
            var options = new AutoDependencyInjectionOptions();
            var assembly1 = typeof(AutoDependencyInjectionOptionsTests).Assembly;
            var assembly2 = typeof(AutoDependencyInjectionOptions).Assembly;
            
            // Act
            options.AddAssembly(assembly1);
            options.AddAssembly(assembly2);
            
            // Assert
            Assert.Contains(assembly1, options.AssembliesToScan);
            Assert.Contains(assembly2, options.AssembliesToScan);
            Assert.Equal(2, options.AssembliesToScan.Count);
        }

        /// <summary>
        /// 测试忽略指定的类型
        /// </summary>
        [Fact]
        public void IgnoreType_WithValidType_AddsToIgnoreCollection()
        {
            // Arrange
            var options = new AutoDependencyInjectionOptions();
            var type = typeof(string);
            
            // Act
            options.IgnoreType(type);
            
            // Assert
            Assert.Contains(type, options.TypesToIgnore);
            Assert.Single(options.TypesToIgnore);
        }

        /// <summary>
        /// 测试忽略相同类型多次时只添加一次
        /// </summary>
        [Fact]
        public void IgnoreType_WithDuplicateType_AddsOnlyOnce()
        {
            // Arrange
            var options = new AutoDependencyInjectionOptions();
            var type = typeof(string);
            
            // Act
            options.IgnoreType(type);
            options.IgnoreType(type); // 添加相同的类型第二次
            
            // Assert
            Assert.Contains(type, options.TypesToIgnore);
            Assert.Single(options.TypesToIgnore);
        }

        /// <summary>
        /// 测试使用泛型方法忽略类型
        /// </summary>
        [Fact]
        public void IgnoreTypeGeneric_WithValidType_AddsToIgnoreCollection()
        {
            // Arrange
            var options = new AutoDependencyInjectionOptions();
            
            // Act
            options.IgnoreType<string>();
            
            // Assert
            Assert.Contains(typeof(string), options.TypesToIgnore);
            Assert.Single(options.TypesToIgnore);
        }
    }
} 