using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Js.Domain
{
    public  class AppSetting
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static string ConnectionString;

        /// <summary>
        /// Swagger文档名称
        /// </summary>
        public static string SwaggerAppName;

        public static void  Init()
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            SwaggerAppName = ConfigurationManager.AppSettings["SwaggerAppName"];

        }

    }
}
