using IFramework.AutoMapper;
using IFramework.Base;
using IFramework.IdHelper;
using IFramework.Ioc;
using IFramework.Logger.Logging;
using Js.Entity;
using Jsp.DomainDto.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(IDbSession dbSession) : base(dbSession)
        {

        }
        public Result<long> Add(GroupBaseResponse requestModel)
        {
            var id = IdHelper.Instance.LongId;
            var result = DbSession.GroupRepository.Add(new Group()
            {
                GroupId = id
            });
            return new Result<long>(1);
        }

        public Result Delete(params long[] ids)
        {
            return null;
        }

        public Result Edit(GroupBaseResponse requestModel)
        {
            throw new NotImplementedException();
        }

        public Result<List<GroupBaseResponse>> GetAllByCurrentLab(GroupBaseResponse request)
        {
            throw new NotImplementedException();
        }

        public Result<List<GroupBaseResponse>> GetAllByLabId(long id)
        {
            throw new NotImplementedException();
        }

        public Result<GroupBaseResponse> GetById(long id)
        {
            throw new NotImplementedException();
        }
    }
}
