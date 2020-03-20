using IFramework.DapperExtension;

namespace IFramework.Infrastructure
{
    /// <summary>
    ///     Class is helper for use and close IDbConnection
    /// </summary>
    public class DapperContext : IDapperContext
    {
        /// <summary>
        /// 配置
        /// </summary>
        public DapperContextConfig DapperContextConfig
        {
            get;
            set;
        }

        public string ConnectionStr
        {
            get;
            set;
        }
        
        public DapperContext(string connectionStr)
        {
            ConnectionStr = connectionStr;
            DapperContextConfig = new DapperContextConfig()
            {
                SqlProvider = SqlProvider.MSSQL,
                LogicDeleteSql = " IsDeleted=1 ",
                RegisterUserId = true
            };
        }

        public DapperContext(string connectionStr, string logicDeleteSql, bool registerUserId)
        {
            ConnectionStr = connectionStr;
            DapperContextConfig = new DapperContextConfig()
            {
                SqlProvider = SqlProvider.MSSQL,
                LogicDeleteSql = logicDeleteSql,
                RegisterUserId = registerUserId
            };
        }

        public DapperContext(string connectionStr, DapperContextConfig config)
        {
            ConnectionStr = connectionStr;
            DapperContextConfig = config;
        }

    }
}