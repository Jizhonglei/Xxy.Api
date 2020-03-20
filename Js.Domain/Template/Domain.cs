/*using System.Linq;
using IFramework.AutoMapper;
using IFramework.Base;
using IFramework.DapperExtension;
using IFramework.IdHelper;
using IFramework.Utility.Extension;
using Js.Domain.DbContext;
using Js.DomainDto.WebUserToken;
using Js.Entity;

namespace Js.Domain.Service
{
    public partial interface IWebUserTokenService : IBaseService<WebUserTokenRequest, WebUserTokenResponse, WebUserTokenSearch>, IDependency
    {

    }

    public partial class WebUserTokenService : BaseService, IWebUserTokenService
    {
        public WebUserTokenService(IDbSession dbSession) : base(dbSession)
        {
        }

        #region 添加用户登录凭证表
        public Result<long> Add(WebUserTokenRequest requestModel)
        {
            if (requestModel == null)
                return Result.Error<long>(ParameterError);

            var newWebUserToken = new WebUserToken()
            {
				guids=IdHelper.Instance.LongId,
				web_userid=requestModel.web_userid,
				web_usertoken=requestModel.web_usertoken,
				login_date=requestModel.login_date,
				expirydate=requestModel.expirydate,
				autoid=requestModel.autoid,

            };

            var result = DbSession.WebUserTokenRepository.Add(newWebUserToken);
            return result ?  Result.Success(newWebUserToken.guids) : Result.Error<long>(ResultAddError);
        }
        #endregion

        #region 修改用户登录凭证表
        public Result Edit(long id, WebUserTokenRequest requestModel)
        {
            if (id < 1 || requestModel == null)
                return Result.Error(ParameterError);

            var currentWebUserToken = DbSession.WebUserTokenRepository.Get(id);
            if (currentWebUserToken == null)
                return Result.Error("该用户登录凭证表不存在！");

			currentWebUserToken.web_userid=requestModel.web_userid;
			currentWebUserToken.web_usertoken=requestModel.web_usertoken;
			currentWebUserToken.login_date=requestModel.login_date;
			currentWebUserToken.expirydate=requestModel.expirydate;
			currentWebUserToken.autoid=requestModel.autoid;


            var result = DbSession.WebUserTokenRepository.Update(currentWebUserToken);
            return result ?  Result.Success(ResultEditSuccess) : Result.Error(ResultEditError);
        }
        #endregion

        #region 删除用户登录凭证表
        public Result Delete(params long[] ids)
        {
            if (ids == null || ids.Length < 1)
                return Result.Error(ParameterError);

            bool result;
            if (ids.Length == 1)
            {
                var id = ids.First();
                result = DbSession.WebUserTokenRepository.Delete(u => u.guids == id);
            }
            else
            {
                result = DbSession.WebUserTokenRepository.Delete(u => ids.Contains(u.guids));
            }

            return result ?  Result.Success(ResultDeleteSuccess) : Result.Error(ResultDeleteError);
        }
        #endregion

        #region 查询用户登录凭证表
        public Result<WebUserTokenResponse> GetById(long id)
        {
            if (id < 0)
                return Result.Error<WebUserTokenResponse>(ParameterError);

            var result = DbSession.WebUserTokenRepository.Get(id);
            return  Result.Success(result.MapTo<WebUserTokenResponse>());
        }

        public Result<PagedList<WebUserTokenResponse>> GetByPage(WebUserTokenSearch search)
        {
            if (search == null)
                return Result.Error<PagedList<WebUserTokenResponse>>(ParameterError);

            var condition = DbSession.WebUserTokenRepository.ExpressionTrue();
            condition = condition.And(u => u.IsDeleted == false);

            var result = DbSession.WebUserTokenRepository.GetPageList(condition, search.PageIndex, search.PageSize, "CreateTime desc");
            return  Result.Success(result.MapTo<PagedList<WebUserTokenResponse>>());
        }
        #endregion
    }
}
*/