using System;
using System.Reflection;

namespace IFramework.Ioc
{
    /// <summary>
    /// 启动类接口
    /// </summary>
    public interface IIocRegister : IDisposable
    {
        void Registers(string startWithName, Assembly executingAssembly = null);
    }
}
