using NLog;

namespace Shaligram_Recruitment_Api.Logger
{
  
        public class LoggerManager : ILoggerManager
        {
            private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

            public void Information(string message)
            {
                logger.Info(message);
            }

            public void Warning(string message)
            {
                logger.Warn(message);
            }

            public void Debug(string message)
            {
                logger.Debug(message);
            }

            public void Error(string message)
            {
                logger.Error(message);
            }
        }
    }

    

