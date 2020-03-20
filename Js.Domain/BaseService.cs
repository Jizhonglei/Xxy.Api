using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFramework.Base;
using IFramework.Infrastructure;
using IFramework.IdHelper;
using IFramework.Ioc;
using IFramework.Logger.Logging;
using IFramework.Utility.Extension;
using IFramework.Utility.Helper;
using Js.Entity;
using Js.DomainDto.Enum;
using Js.DomainDto.Base;

namespace Js.Domain
{
    public class BaseService
    {
        public IDbSession DbSession { get; }

        public BaseService(IDbSession dbSession)
        {
            DbSession = dbSession;
        }
        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="message"></param>
        public void AppException(AppErrorEnum message)
        {
            throw new ApplicationException(message.ToString());
        }
        /// <summary>
        /// 返回值创建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="isSucceed"></param>
        /// <param name="err"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public BaseResponse<T> Response<T>(T data, bool isSucceed = false, string err = "", int code = 100)
        {

            return new BaseResponse<T>()
            {
                Code = code,
                Data = data,
                IsSucceed = isSucceed,
                Err = err
            };
        }
    }
    public partial class DbSession : UnitOfWork, IDbSession
    {
        /// <summary>
        ///用户表
        /// </summary>
        public IRepository<AppUser> AppUserRepository => GetRepository<AppUser>();
        /// <summary>
        ///小组信息表
        /// </summary>
        public IRepository<Group> GroupRepository => GetRepository<Group>();
        /// <summary>
        ///用户信息表
        /// </summary>
        public IRepository<WebUser> WebUserRepository => GetRepository<WebUser>();
        /// <summary>
        ///用户登录凭证表
        /// </summary>
        public IRepository<WebUserToken> WebUserTokenRepository => GetRepository<WebUserToken>();
    }

    public partial interface IDbSession : IUnitOfWork, IDependency
    {
        /// <summary>
        ///用户表
        /// </summary>
        IRepository<AppUser> AppUserRepository { get; }
        /// <summary>
        ///小组信息表
        /// </summary>
        IRepository<Group> GroupRepository { get; }
        /// <summary>
        ///用户信息表
        /// </summary>
        IRepository<WebUser> WebUserRepository { get; }
        /// <summary>
        ///用户登录凭证表
        /// </summary>
        IRepository<WebUserToken> WebUserTokenRepository { get; }
    }

}
