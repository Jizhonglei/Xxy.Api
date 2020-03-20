using IFramework.Base;
using IFramework.DapperExtension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Entity
{
    [Description("用户表")]
    [Table("Txj_app_user")]
    public class AppUser: EntityBase
    {
        [Key]
        /// <summary>
        /// 用户id
        /// </summary>
        public long auto_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nick_name { get; set; }

        /// <summary>
        /// 用户微信openID
        /// </summary>
        public string wechat_openid { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public int source { get; set; }
    }
}
