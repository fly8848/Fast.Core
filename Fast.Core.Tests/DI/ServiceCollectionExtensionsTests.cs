using System;
using System.Linq;
using System.Reflection;
using Fast.Core.DI;
using Fast.Core.Tests.DI.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace Fast.Core.Tests.DI
{
    /// <summary>
    /// 服务集合扩展方法的测试类
    /// </summary>
    public class ServiceCollectionExtensionsTests
    {
        /// <summary>
        /// 测试添加Fast.Core依赖注入而不指定选项的情况（使用默认配置）
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithDefaultOptions_UsesCallingAssembly()
        {
            // Arrange
            var services = new ServiceCollection();
            
            // Act
            services.AddFastCoreDependencies();
            
            // Assert
            Assert.NotEmpty(services);
        }

        /// <summary>
        /// 测试添加单例服务
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithSingletonService_RegistersCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestSingletonService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            // Assert
            var singletonDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(ITestSingletonService) && 
                d.ImplementationType == typeof(TestSingletonService));
            
            Assert.NotNull(singletonDescriptor);
            Assert.Equal(ServiceLifetime.Singleton, singletonDescriptor!.Lifetime);
        }

        /// <summary>
        /// 测试添加范围服务
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithScopedService_RegistersCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestScopedService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            // Assert
            var scopedDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(ITestScopedService) && 
                d.ImplementationType == typeof(TestScopedService));
            
            Assert.NotNull(scopedDescriptor);
            Assert.Equal(ServiceLifetime.Scoped, scopedDescriptor!.Lifetime);
        }

        /// <summary>
        /// 测试添加瞬态服务
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithTransientService_RegistersCorrectly()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestTransientService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            // Assert
            var transientDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(ITestTransientService) && 
                d.ImplementationType == typeof(TestTransientService));
            
            Assert.NotNull(transientDescriptor);
            Assert.Equal(ServiceLifetime.Transient, transientDescriptor!.Lifetime);
        }

        /// <summary>
        /// 测试添加实现多个接口的服务
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithMultiInterfaceService_RegistersAllInterfaces()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(MultiInterfaceService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            // Assert
            var primaryDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(IMultiInterfaceService) && 
                d.ImplementationType == typeof(MultiInterfaceService));
            
            var secondaryDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(ISecondaryInterface) && 
                d.ImplementationType == typeof(MultiInterfaceService));
            
            Assert.NotNull(primaryDescriptor);
            Assert.NotNull(secondaryDescriptor);
            Assert.Equal(ServiceLifetime.Singleton, primaryDescriptor!.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, secondaryDescriptor!.Lifetime);
        }

        /// <summary>
        /// 测试服务也被注册为自身类型
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_RegistersImplementationTypeAsService()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestSingletonService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            // Assert
            var selfDescriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(TestSingletonService) && 
                d.ImplementationType == typeof(TestSingletonService));
            
            Assert.NotNull(selfDescriptor);
            Assert.Equal(ServiceLifetime.Singleton, selfDescriptor!.Lifetime);
        }

        /// <summary>
        /// 测试忽略的类型不会被注册
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithIgnoredType_DoesNotRegisterIgnoredType()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestSingletonService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
                options.IgnoreType<TestSingletonService>();
            });
            
            // Assert
            var descriptor = services.FirstOrDefault(d => 
                d.ImplementationType == typeof(TestSingletonService));
            
            Assert.Null(descriptor);
        }

        /// <summary>
        /// 测试不实现依赖接口的服务不会被注册
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_WithNonDependencyService_DoesNotRegisterService()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(NonDependencyService).Assembly;
            
            // Act
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            // Assert
            var descriptor = services.FirstOrDefault(d => 
                d.ServiceType == typeof(INonDependencyService) || 
                d.ImplementationType == typeof(NonDependencyService));
            
            Assert.Null(descriptor);
        }

        /// <summary>
        /// 测试获取服务实例并验证其生命周期
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_CanResolveRegisteredServices()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestSingletonService).Assembly;
            
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
            });
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Act
            var singletonService1 = serviceProvider.GetService<ITestSingletonService>();
            var singletonService2 = serviceProvider.GetService<ITestSingletonService>();
            
            var scopedService1 = serviceProvider.GetService<ITestScopedService>();
            var scopedService2 = serviceProvider.GetService<ITestScopedService>();
            
            var transientService1 = serviceProvider.GetService<ITestTransientService>();
            var transientService2 = serviceProvider.GetService<ITestTransientService>();
            
            // Assert
            Assert.NotNull(singletonService1);
            Assert.NotNull(scopedService1);
            Assert.NotNull(transientService1);
            
            // 验证单例服务实例相同
            Assert.Same(singletonService1!, singletonService2);
            
            // 在相同作用域中，范围服务应该是相同实例
            Assert.Same(scopedService1!, scopedService2);
            
            // 瞬态服务每次应该是不同实例
            Assert.NotSame(transientService1!, transientService2);
        }

        /// <summary>
        /// 测试在不同作用域中，范围服务应该是不同实例
        /// </summary>
        [Fact]
        public void AddFastCoreDependencies_ScopedServicesAreDifferentInDifferentScopes()
        {
            // Arrange
            var services = new ServiceCollection();
            var testAssembly = typeof(TestScopedService).Assembly;
            
            services.AddFastCoreDependencies(options => 
            {
                options.AddAssembly(testAssembly);
                // 泛型服务已不再被支持，不需要明确忽略
            });
            
            var rootProvider = services.BuildServiceProvider();
            
            // Act
            // 创建两个不同的作用域
            using var scope1 = rootProvider.CreateScope();
            using var scope2 = rootProvider.CreateScope();
            
            var scopedService1 = scope1.ServiceProvider.GetService<ITestScopedService>();
            var scopedService2 = scope2.ServiceProvider.GetService<ITestScopedService>();
            
            // Assert
            Assert.NotNull(scopedService1);
            Assert.NotNull(scopedService2);
            
            // 在不同作用域中，范围服务应该是不同实例
            Assert.NotSame(scopedService1!, scopedService2);
        }
    }
} 