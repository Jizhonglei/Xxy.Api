using System;

namespace IFramework.Ioc.Reflection
{
    /// <summary> 类型查找器 </summary>
    public interface ITypeFinder
    {
        Type[] Find(Func<Type, bool> expression);

        Type[] FindAll();
    }
}
