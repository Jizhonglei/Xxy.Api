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
    /// <summary>
    /// 用户登录凭证表
    /// </summary>
    [Description("用户登录凭证表")]
    [Table("Tjx_web_usertoken")]
    public class WebUserToken: EntityBase
    {
        [Key]
        /// <summary>
        /// guid
        /// </summary>
        public string guids { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string web_userid { get; set; }

        /// <summary>
        /// 登录凭证
        /// </summary>
        public string web_usertoken { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime login_date { get; set; }

        /// <summary>
        /// 到期时间
        /// </summary>
        public DateTime expirydate { get; set; }

    }
}
