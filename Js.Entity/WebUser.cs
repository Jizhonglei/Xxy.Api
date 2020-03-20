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
    [Description("用户信息表")]
    [Table("Txj_web_user")]
    public class WebUser : EntityBase

    {
        [Key]
        /// <summary>
        /// 用户id guid
        /// </summary>
        public string user_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string user_sex { get; set; }

        /// <summary>
        /// 所在单位
        /// </summary>
        public string user_company { get; set; }

        /// <summary>
        /// qq
        /// </summary>
        public string user_qqs { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string user_email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string user_tel { get; set; }
        public DateTime user_regdate { get; set; }
        public int user_state { get; set; }
        public string user_accont { get; set; }
        public string user_pwd { get; set; }
        public string user_wid { get; set; }
        public string user_xid { get; set; }
        public string user_r_uid { get; set; }
        public string user_score_all { get; set; }
        public string user_score { get; set; }
        public string user_rank { get; set; }
        public string Tablemarks1 { get; set; }
        public string Tablemarks2 { get; set; }
        public string user_img { get; set; }
        public string user_nick { get; set; }



    }
}
