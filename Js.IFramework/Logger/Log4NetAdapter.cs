using System.Configuration;
using System.IO;
using IFramework.Logger.Logging;
using log4net;
using log4net.Config;
using log4net.Repository;
using NLog.Internal;
using ILog = IFramework.Logger.Logging.ILog;

namespace IFramework.Logger
{
    public class Log4NetAdapter : LoggerAdapterBase
    {
        private static string _fileName = "log4net.config";
        private static string _configPath = string.Empty;

        /// <summary>
        /// 初始化一个<see cref="Log4NetAdapter"/>类型的新实例
        /// </summary>k
        public Log4NetAdapter(string configFullPathSettingName, string webSite = "")
        {
            _fileName = string.Empty;
            _configPath = System.Configuration.ConfigurationManager.AppSettings[configFullPathSettingName];

            AdapterInit(webSite);
        }

        public Log4NetAdapter(string configPath, string configName = "log4net.config", string webSite = "")
        {
            _fileName = configName;
            _configPath = configPath;

            AdapterInit(webSite);
        }
        public static ILoggerRepository repository { get; set; }
        private void AdapterInit(string webSite)
        {
            var configFile = Path.Combine(_configPath, _fileName);
            if (!File.Exists(configFile))
                return;
            GlobalContext.Properties["WebSite"] = string.IsNullOrEmpty(webSite) ? "logs" : webSite;

#if NETSTANDARD2_0
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(configFile));
#else
            XmlConfigurator.ConfigureAndWatch(new FileInfo(configFile));
#endif


            //            var appender = new RollingFileAppender
            //            {
            //                Name = "root",
            //                File = "logs\\log_",
            //                AppendToFile = true,
            //                LockingModel = new FileAppender.MinimalLock(),
            //                RollingStyle = RollingFileAppender.RollingMode.Date,
            //                DatePattern = "yyyyMMdd-HH\".log\"",
            //                StaticLogFileName = false,
            //                MaxSizeRollBackups = 10,
            //                Layout = new PatternLayout("[%d{yyyy-MM-dd HH:mm:ss.fff}] %-5p %c %t %w %n%m%n")
            //                //Layout = new PatternLayout("[%d [%t] %-5p %c [%x] - %m%n]")
            //            };
            //            appender.ClearFilters();
            //            appender.AddFilter(new LevelRangeFilter
            //            {
            //                LevelMin = Level.Debug,
            //                LevelMax = Level.Fatal
            //            });
            //            appender.ActivateOptions();
            //            BasicConfigurator.Configure(appender);
        }

        protected override ILog CreateLogger(string name)
        {
#if NETSTANDARD2_0
            var log = LogManager.GetLogger(repository.Name, name);
            return new Log4NetLog(log);
#else
            var log = LogManager.GetLogger(name);
            return new Log4NetLog(log);
#endif
        }
    }
}
