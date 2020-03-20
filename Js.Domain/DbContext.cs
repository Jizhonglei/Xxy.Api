using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IFramework.Base;
using IFramework.Infrastructure;

namespace Js.Domain
{
    public class DbContext : DapperContext, IDependency
    {
        /// <summary>
        /// server=.;uid=yj;pwd=111;database=Test;
        /// </summary>
        public DbContext() : base(AppSetting.ConnectionString, "  IsDeleted=1 ", true)
        {

        }
    }
}
