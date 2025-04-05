using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Fast.Core.DI
{
    /// <summary>
    /// IServiceCollection的扩展方法，用于自动依赖注入
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Fast.Core自动依赖注入
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="optionsAction">配置选项委托</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddFastCoreDependencies(
            this IServiceCollection services,
            Action<AutoDependencyInjectionOptions>? optionsAction = null)
        {
            var options = new AutoDependencyInjectionOptions();
            optionsAction?.Invoke(options);

            // 如果未指定任何程序集，则默认使用调用程序集
            if (options.AssembliesToScan.Count == 0)
            {
                options.AddAssembly(Assembly.GetCallingAssembly());
            }

            foreach (var assembly in options.AssembliesToScan)
            {
                RegisterAssemblyTypes(services, assembly, options);
            }

            return services;
        }

        /// <summary>
        /// 扫描程序集中的类型并注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="assembly">要扫描的程序集</param>
        /// <param name="options">配置选项</param>
        private static void RegisterAssemblyTypes(
            IServiceCollection services,
            Assembly assembly,
            AutoDependencyInjectionOptions options)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.IsClass && 
                               !type.IsAbstract && 
                               !options.TypesToIgnore.Contains(type))
                .ToList();

            // 注册实现标记接口的类型
            RegisterMarkerInterfaceTypes(services, types);
        }

        /// <summary>
        /// 注册实现了标记接口的类型
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="types">要扫描的类型列表</param>
        private static void RegisterMarkerInterfaceTypes(IServiceCollection services, List<Type> types)
        {
            foreach (var type in types)
            {
                // 单例服务
                if (typeof(ISingletonDependency).IsAssignableFrom(type))
                {
                    var interfaces = GetImplementedInterfaces(type);
                    foreach (var interfaceType in interfaces)
                    {
                        RegisterService(services, interfaceType, type, ServiceLifetime.Singleton);
                    }
                    RegisterService(services, type, type, ServiceLifetime.Singleton);
                }
                // 范围服务
                else if (typeof(IScopedDependency).IsAssignableFrom(type))
                {
                    var interfaces = GetImplementedInterfaces(type);
                    foreach (var interfaceType in interfaces)
                    {
                        RegisterService(services, interfaceType, type, ServiceLifetime.Scoped);
                    }
                    RegisterService(services, type, type, ServiceLifetime.Scoped);
                }
                // 瞬态服务
                else if (typeof(ITransientDependency).IsAssignableFrom(type))
                {
                    var interfaces = GetImplementedInterfaces(type);
                    foreach (var interfaceType in interfaces)
                    {
                        RegisterService(services, interfaceType, type, ServiceLifetime.Transient);
                    }
                    RegisterService(services, type, type, ServiceLifetime.Transient);
                }
            }
        }

        /// <summary>
        /// 获取类型实现的所有接口，除了标记接口
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>接口类型集合</returns>
        private static IEnumerable<Type> GetImplementedInterfaces(Type type)
        {
            var interfaces = type.GetInterfaces();
            var markerInterfaces = new[] { typeof(ISingletonDependency), typeof(IScopedDependency), typeof(ITransientDependency) };
            
            return interfaces.Where(i => !markerInterfaces.Contains(i));
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="serviceType">服务类型</param>
        /// <param name="implementationType">实现类型</param>
        /// <param name="lifetime">生命周期</param>
        private static void RegisterService(
            IServiceCollection services,
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime)
        {
            if (serviceType == null || implementationType == null) return;

            // 检查是否为泛型类型，不支持泛型类型的依赖注入
            if (serviceType.IsGenericTypeDefinition || implementationType.IsGenericTypeDefinition ||
                (serviceType.IsGenericType && implementationType.IsGenericTypeDefinition))
            {
                return; // 跳过泛型类型的注册
            }

            var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            services.TryAdd(descriptor);
        }
    }
} 