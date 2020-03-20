using IFramework.IdHelper;
using IFramework.Utility.Extension;
using IFramework.Utility.Helper;
using Js.DomainDto.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    /// <summary>
    /// 实现
    /// </summary>
    public class AppUserService : BaseService, IAppUserService
    {
        public AppUserService(IDbSession dbSession) : base(dbSession)
        {

        }
        public bool CreateUser()
        {
            var result = DbSession.AppUserRepository.Add(new Entity.AppUser()
            {
                auto_id = IdHelper.Instance.LongId,
                nick_name = "杨" + new Guid().ToString(),
                source = 1,
                user_name = new Guid().ToString()
            });
            return result;
        }

        public bool UpdateUser()
        {
            try
            {
                var data = DbSession.AppUserRepository.Get(u => u.auto_id == 1238059796469518336);
                var datas = DbSession.AppUserRepository.Get(u => u.auto_id == 1238060022513143808);

                DbSession.Transaction(dt =>
                {
                    data.nick_name = "123";
                    DbSession.AppUserRepository.Update(data, dt);
                    datas.nick_name = "432";
                    DbSession.AppUserRepository.Update(datas, dt);
                    throw new NotImplementedException();

                });

            }
            catch (Exception)
            {

                throw;
            }
            return false;

        }

        public string Getlist()
        {

            // var list=  DbSession.AppUserRepository.GetList(u=>u.auto_id> 1238059796469518336);
            var condition = DbSession.AppUserRepository.ExpressionTrue();
            condition = condition.And(u => u.auto_id >= 0);
            var page = DbSession.AppUserRepository.GetPageList(condition, 1, 5, "");
            return "";
        }
    }
}
