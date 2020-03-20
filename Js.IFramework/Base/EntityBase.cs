using IFramework.DapperExtension;
using System;

namespace IFramework.Base
{
    public partial class EntityBase
    {
        ///// <summary>
        ///// 版本号（控制并发）
        ///// </summary>
        //public DateTime version { get; set; }

        ///// <summary>
        ///// 数据创建用户ID
        ///// </summary>
        //public long create_userid { get; set; }

        ///// <summary>
        ///// 数据创建时间
        ///// </summary>
        //public DateTime create_time { get; set; }

        ///// <summary>
        ///// 数据最后的修改用户ID
        ///// </summary>
        //public long update_userid { get; set; }

        ///// <summary>
        ///// 数据最后的修改时间
        ///// </summary>
        //public DateTime update_time { get; set; }

        ///// <summary>
        ///// 是否删除
        ///// </summary>
        //public bool is_deleted { get; set; }

        /// <summary>
        /// ID自增长
        /// </summary>
        [AutoIncrementKey]
        public int autoid { get; set; }

        /// <summary>
        /// 设置创建记录公共字段
        /// </summary>
        /// <param name="userId"></param>
        public void SetCreateAudit(long userId)
        {
            //var now = DateTime.Now;
            //is_deleted = false;
            //create_userid = userId;
            //create_time = now;
            //update_userid = userId;
            //update_time = now;
            //version = now;
        }

        /// <summary>
        /// 设置修改记录公共字段
        /// </summary>
        /// <param name="userId"></param>
        public void SetUpdateAudit(long userId)
        {
            //var now = DateTime.Now;
            //// UpdateUserId = userId;
            //update_time = now;
            //version = now;
        }
    }
}
