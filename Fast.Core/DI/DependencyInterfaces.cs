using System;

namespace Fast.Core.DI
{
    /// <summary>
    /// 标记接口，表示实现该接口的类型将作为其所实现的接口类型被注册为单例（Singleton）服务
    /// </summary>
    public interface ISingletonDependency
    {
    }

    /// <summary>
    /// 标记接口，表示实现该接口的类型将作为其所实现的接口类型被注册为范围（Scoped）服务
    /// </summary>
    public interface IScopedDependency
    {
    }

    /// <summary>
    /// 标记接口，表示实现该接口的类型将作为其所实现的接口类型被注册为瞬态（Transient）服务
    /// </summary>
    public interface ITransientDependency
    {
    }
} 