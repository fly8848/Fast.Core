using Fast.Core.DI;

namespace Fast.Core.Tests.DI.Mocks
{
    /// <summary>
    /// 测试单例服务的接口
    /// </summary>
    public interface ITestSingletonService
    {
        string GetServiceType();
    }

    /// <summary>
    /// 测试范围服务的接口
    /// </summary>
    public interface ITestScopedService
    {
        string GetServiceType();
    }

    /// <summary>
    /// 测试瞬态服务的接口
    /// </summary>
    public interface ITestTransientService
    {
        string GetServiceType();
    }

    /// <summary>
    /// 测试单例服务的实现
    /// </summary>
    public class TestSingletonService : ITestSingletonService, ISingletonDependency
    {
        public string GetServiceType() => "Singleton";
    }

    /// <summary>
    /// 测试范围服务的实现
    /// </summary>
    public class TestScopedService : ITestScopedService, IScopedDependency
    {
        public string GetServiceType() => "Scoped";
    }

    /// <summary>
    /// 测试瞬态服务的实现
    /// </summary>
    public class TestTransientService : ITestTransientService, ITransientDependency
    {
        public string GetServiceType() => "Transient";
    }

    /// <summary>
    /// 同时实现多个接口的测试服务
    /// </summary>
    public interface IMultiInterfaceService
    {
        string GetPrimaryType();
    }

    /// <summary>
    /// 次要接口
    /// </summary>
    public interface ISecondaryInterface
    {
        string GetSecondaryType();
    }

    /// <summary>
    /// 实现多个接口的单例服务
    /// </summary>
    public class MultiInterfaceService : IMultiInterfaceService, ISecondaryInterface, ISingletonDependency
    {
        public string GetPrimaryType() => "Primary";
        public string GetSecondaryType() => "Secondary";
    }

    /// <summary>
    /// 不实现任何依赖标记接口的服务，应该不会被自动注册
    /// </summary>
    public interface INonDependencyService
    {
        string GetServiceType();
    }

    /// <summary>
    /// 不实现任何依赖标记接口的实现类
    /// </summary>
    public class NonDependencyService : INonDependencyService
    {
        public string GetServiceType() => "NonDependency";
    }
    
    /// <summary>
    /// 泛型接口测试服务
    /// </summary>
    /// <typeparam name="T">泛型参数</typeparam>
    public interface IGenericService<T>
    {
        string GetServiceType();
        T? GetValue();
    }
    
    /// <summary>
    /// 泛型单例服务实现
    /// </summary>
    public class GenericSingletonService<T> : IGenericService<T>, ISingletonDependency
    {
        private readonly T? _value;
        
        public GenericSingletonService(T? value = default)
        {
            _value = value;
        }
        
        public string GetServiceType() => $"GenericSingleton<{typeof(T).Name}>";
        public T? GetValue() => _value;
    }
    
    /// <summary>
    /// 泛型范围服务实现
    /// </summary>
    public class GenericScopedService<T> : IGenericService<T>, IScopedDependency
    {
        private readonly T? _value;
        
        public GenericScopedService(T? value = default)
        {
            _value = value;
        }
        
        public string GetServiceType() => $"GenericScoped<{typeof(T).Name}>";
        public T? GetValue() => _value;
    }
    
    /// <summary>
    /// 泛型瞬态服务实现
    /// </summary>
    public class GenericTransientService<T> : IGenericService<T>, ITransientDependency
    {
        private readonly T? _value;
        
        public GenericTransientService(T? value = default)
        {
            _value = value;
        }
        
        public string GetServiceType() => $"GenericTransient<{typeof(T).Name}>";
        public T? GetValue() => _value;
    }
    
    /// <summary>
    /// 具体泛型约束的接口
    /// </summary>
    public interface IConstrainedGenericService<T> where T : class
    {
        string GetServiceType();
        T? GetValue();
    }
    
    /// <summary>
    /// 具体泛型约束的实现
    /// </summary>
    public class ConstrainedGenericService<T> : IConstrainedGenericService<T>, ISingletonDependency where T : class
    {
        private readonly T? _value;
        
        public ConstrainedGenericService(T? value = null)
        {
            _value = value;
        }
        
        public string GetServiceType() => $"ConstrainedGeneric<{typeof(T).Name}>";
        public T? GetValue() => _value;
    }
} 