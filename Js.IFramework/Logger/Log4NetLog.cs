using System;
using System.Diagnostics;
using System.Text;
using IFramework.Logger.Logging;
using IFramework.Utility.Extension;
using log4net.Core;
using ILogger = log4net.Core.ILogger;
using LoggerManager = IFramework.Logger.Logging.LoggerManager;


namespace IFramework.Logger
{
    public class Log4NetLog : LogBase
    {
        private readonly ILogger _logger;

        public Log4NetLog(ILoggerWrapper wrapper)
        {
            _logger = wrapper.Logger;
        }

        private static StackFrame CurrentStrace()
        {
            var strace = new StackTrace(true);
            for (var i = 0; i < strace.FrameCount; i++)
            {
                var frame = strace.GetFrame(i);
                if (string.IsNullOrWhiteSpace(frame.GetFileName()))
                    continue;
                var method = frame.GetMethod();
                if (method.Name == "OnException" || method.DeclaringType == typeof(LogBase) ||
                    method.DeclaringType == (typeof(LoggerManager)) ||
                    method.DeclaringType == typeof(Log4NetLog))
                    continue;
                return frame;
            }
            return null;
        }

        private static LogInfo Format(object msgObject, Exception ex = null)
        {
            var frame = CurrentStrace();
            string method = string.Empty, fileInfo = string.Empty;
            if (frame != null)
            {
                method = string.Concat(frame.GetMethod().DeclaringType, " - ", frame.GetMethod().Name);
                fileInfo = string.Format("{0}[{1}]", frame.GetFileName(), frame.GetFileLineNumber());
            }
            if (ex != null && string.IsNullOrWhiteSpace(method))
            {
                method = string.Format("{0} - {1}", ex.TargetSite.DeclaringType, ex.TargetSite.Name);
            }
            string msg;
            if (msgObject == null || msgObject is string)
            {
                msg = (msgObject ?? string.Empty).ToString();
            }
            else
            {
                msg = msgObject.ToJson();
            }
            var result = new LogInfo
            {
                Method = method,
                File = fileInfo,
                Message = msg,
                Detail = string.Empty
            };
            if (ex != null)
            {
                result.Detail = FormatEx(ex);
            }
            return result;
        }

        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            _logger.Log(typeof(Log4NetLog), ParseLevel(level), Format(message, exception),
                exception);
        }

        public override bool IsTraceEnabled
        {
            get { return _logger.IsEnabledFor(Level.Trace) || _logger.IsEnabledFor(Level.All); }
        }

        public override bool IsDebugEnabled
        {
            get { return _logger.IsEnabledFor(Level.Debug) || _logger.IsEnabledFor(Level.All); }
        }

        public override bool IsInfoEnabled
        {
            get { return _logger.IsEnabledFor(Level.Info) || _logger.IsEnabledFor(Level.All); }
        }

        public override bool IsWarnEnabled
        {
            get { return _logger.IsEnabledFor(Level.Warn) || _logger.IsEnabledFor(Level.All); }
        }

        public override bool IsErrorEnabled
        {
            get { return _logger.IsEnabledFor(Level.Error) || _logger.IsEnabledFor(Level.All); }
        }

        public override bool IsFatalEnabled
        {
            get { return _logger.IsEnabledFor(Level.Fatal) || _logger.IsEnabledFor(Level.All); }
        }

        private Level ParseLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.All:
                    return Level.All;
                case LogLevel.Trace:
                    return Level.Trace;
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Info:
                    return Level.Info;
                case LogLevel.Warn:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Fatal:
                    return Level.Fatal;
                case LogLevel.Off:
                    return Level.Off;
                default:
                    return Level.Off;
            }
        }

        #region 异常信息格式化
        /// <summary>
        /// 异常信息格式化
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="isHideStackTrace"></param>
        /// <returns></returns>
        private static string FormatEx(Exception ex, bool isHideStackTrace = false)
        {
            var sb = new StringBuilder();
            var count = 0;
            var appString = string.Empty;
            while (ex != null)
            {
                if (count > 0)
                {
                    appString += "  ";
                }
                sb.AppendLine(string.Format("{0}异常消息：{1}", appString, ex.Message));
                sb.AppendLine(string.Format("{0}异常类型：{1}", appString, ex.GetType().FullName));
                sb.AppendLine(string.Format("{0}异常方法：{1}", appString,
                    (ex.TargetSite == null ? null : ex.TargetSite.Name)));
                sb.AppendLine(string.Format("{0}异常源：{1}", appString, ex.Source));
                if (!isHideStackTrace && ex.StackTrace != null)
                {
                    sb.AppendLine(string.Format("{0}异常堆栈：{1}", appString, ex.StackTrace));
                }
                if (ex.InnerException != null)
                {
                    sb.AppendLine(string.Format("{0}内部异常：", appString));
                    count++;
                }
                ex = ex.InnerException;
            }
            return sb.ToString();
        }
        #endregion
    }
}
