using IFramework.DapperExtension;

namespace IFramework.Infrastructure
{
    public class DapperContextConfig
    {
        /// <summary>
        ///     Type Sql provider
        /// </summary>
        public SqlProvider SqlProvider { get; set; }

        /// <summary>
        ///     LogicDelete String
        /// </summary>
        public string LogicDeleteSql { get; set; }

        /// <summary>
        ///     Register UserId(implement the interface IUserManagerProvider)
        /// </summary>
        public bool RegisterUserId { get; set; }

    }
}