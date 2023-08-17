using log4net.Appender;
using Platform.Library.ClientLogger.Impl;

namespace Platform.Library.ClientLogger.API {
    public class FileAppenderBuilder : AppenderBuilder {
        public FileAppenderBuilder(string path, bool appendToFile = false) {
            FileAppender fileAppender = new();
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.File = path;
            fileAppender.AppendToFile = appendToFile;
            fileAppender.ActivateOptions();
            Init(fileAppender);
        }
    }
}