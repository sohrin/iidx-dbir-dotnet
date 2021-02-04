using System;
using NLog;

namespace dbir
{
    class ExceptionUtils
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void PrintStackTrace(Exception ex)
        {
            logger.Error("message: " + ex.Message);
            logger.Error("stack trace: ");
            logger.Error(ex.StackTrace);
        }
    }
}
