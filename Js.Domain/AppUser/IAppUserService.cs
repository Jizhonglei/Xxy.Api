using IFramework.Base;
using Js.DomainDto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    /// <summary>
    /// App用户接口
    /// </summary>
    public interface IAppUserService: IDependency
    {
        bool CreateUser();

        bool UpdateUser();

        string Getlist();
    }
}
