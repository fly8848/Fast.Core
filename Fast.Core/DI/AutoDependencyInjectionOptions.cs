using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fast.Core.DI
{
    /// <summary>
    /// 自动依赖注入配置选项类
    /// </summary>
    public class AutoDependencyInjectionOptions
    {
        private readonly List<Assembly> _assembliesToScan = new List<Assembly>();
        private readonly List<Type> _typesToIgnore = new List<Type>();

        /// <summary>
        /// 获取要扫描的程序集列表
        /// </summary>
        public IReadOnlyList<Assembly> AssembliesToScan => _assembliesToScan;

        /// <summary>
        /// 获取要排除的类型列表
        /// </summary>
        public IReadOnlyList<Type> TypesToIgnore => _typesToIgnore;

        /// <summary>
        /// 添加要扫描的程序集
        /// </summary>
        /// <param name="assembly">要扫描的程序集</param>
        /// <returns>配置选项</returns>
        public AutoDependencyInjectionOptions AddAssembly(Assembly assembly)
        {
            if (assembly != null && !_assembliesToScan.Contains(assembly))
            {
                _assembliesToScan.Add(assembly);
            }
            return this;
        }

        /// <summary>
        /// 添加要忽略的类型
        /// </summary>
        /// <param name="type">要忽略的类型</param>
        /// <returns>配置选项</returns>
        public AutoDependencyInjectionOptions IgnoreType(Type type)
        {
            if (type != null && !_typesToIgnore.Contains(type))
            {
                _typesToIgnore.Add(type);
            }
            return this;
        }

        /// <summary>
        /// 添加要忽略的类型
        /// </summary>
        /// <typeparam name="T">要忽略的类型</typeparam>
        /// <returns>配置选项</returns>
        public AutoDependencyInjectionOptions IgnoreType<T>()
        {
            return IgnoreType(typeof(T));
        }
    }
} 