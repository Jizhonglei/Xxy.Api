using IFramework.Base;
using IFramework.Infrastructure;
using Jsp.Entity;
namespace Jsp.Domain.DbContext
{
    public partial class DbSession: UnitOfWork, IDbSession
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

    public partial interface IDbSession: IUnitOfWork, IDependency
    {
		/// <summary>
		///用户表
		/// </summary>
		IRepository<AppUser> AppUserRepository{get;}
		/// <summary>
		///小组信息表
		/// </summary>
		IRepository<Group> GroupRepository{get;}
		/// <summary>
		///用户信息表
		/// </summary>
		IRepository<WebUser> WebUserRepository{get;}
		/// <summary>
		///用户登录凭证表
		/// </summary>
		IRepository<WebUserToken> WebUserTokenRepository{get;}

    }
}

