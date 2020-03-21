using System;
using System.Linq;
using System.Reflection;
using Autofac;
using IFramework.Base;

namespace IFramework.Ioc
{
    public class IocRegister : IIocRegister
    {
        public static IocRegister Instance => (Singleton<IocRegister>.Instance ?? (Singleton<IocRegister>.Instance = new IocRegister()));

        public ContainerBuilder Builder { get; private set; }

        private IContainer _container;
        public IContainer Container
        {
            get { return _container ?? (_container = Builder.Build()); }
        }

        public delegate void BuilderAction(ContainerBuilder builderAction);

        public event BuilderAction BuilderHandler;

        /// <summary> 注册依赖 </summary>
        /// <param name="startWithName"></param>
        /// <param name="executingAssembly"></param>
        public void Registers(string startWithName, Assembly executingAssembly = null)
        {
            if (executingAssembly == null)
                executingAssembly = Assembly.GetExecutingAssembly();

            Builder = new ContainerBuilder();

            var assemblies = AssemblyFinder.Instance(startWithName).FindAll().Union(new[] { executingAssembly }).ToArray();

            Builder.RegisterAssemblyTypes(assemblies)
                .Where(type => typeof(ILifetimeDependency).IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf() //自身服务，用于没有接口的类
                .AsImplementedInterfaces() //接口服务
                .PropertiesAutowired()//属性注入
                .InstancePerLifetimeScope();
            
            Builder.RegisterAssemblyTypes(assemblies)
                .Where(type => typeof(IRequestDependency).IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf() //自身服务，用于没有接口的类
                .AsImplementedInterfaces() //接口服务
                .PropertiesAutowired()//属性注入
                .InstancePerRequest();//保证生命周期基于请求

            Builder.RegisterAssemblyTypes(assemblies)
                .Where(type => typeof(ISingleInstanceDependency).IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf() //自身服务，用于没有接口的类
                .AsImplementedInterfaces() //接口服务
                .PropertiesAutowired()//属性注入
                .SingleInstance();//单例

            Builder.RegisterAssemblyTypes(assemblies)
                .Where(type => typeof(IDependency).IsAssignableFrom(type) && !type.IsAbstract)
                .AsSelf() //自身服务，用于没有接口的类
                .AsImplementedInterfaces() //接口服务
                .PropertiesAutowired();//属性注入

            BuilderHandler?.Invoke(Builder);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
