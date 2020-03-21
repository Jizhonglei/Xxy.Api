using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace IFramework.Ioc
{
    public static class IocManager
    {
        private static IocRegister CurrentIocManager { get; set; }

        static IocManager()
        {
            CurrentIocManager = IocRegister.Instance;
        }

        public static T Resolve<T>()
        {
            if (CurrentIocManager.Container.IsRegistered<T>())
            {
                return CurrentIocManager.Container.Resolve<T>();
            }
            return default;
        }

        public static void RegisterGeneric(Type sourseType, Type targeType)
        {
            CurrentIocManager.Builder.RegisterGeneric(sourseType).As(targeType);
        }

        public static void RegisterType(Type sourseType, Type targeType)
        {
            CurrentIocManager.Builder.RegisterType(sourseType).As(targeType);
        }

        public static T Resolve<T>(Dictionary<Type, object> parametersDic)
        {
            var parameterList = new List<Parameter>();

            if (parametersDic == null || parametersDic.Count <= 0)
                return CurrentIocManager.Container.Resolve<T>(parameterList);

            parameterList.AddRange(parametersDic.Select(paras => new TypedParameter(paras.Key, paras.Value)));

            return CurrentIocManager.Container.Resolve<T>(parameterList);
        }
    }
}
