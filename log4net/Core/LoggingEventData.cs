using System;
using log4net.Util;

namespace log4net.Core {
    public struct LoggingEventData {
        public string LoggerName;

        public Level Level;

        public string Message;

        public string ThreadName;

        public DateTime TimeStamp;

        public LocationInfo LocationInfo;

        public string UserName;

        public string Identity;

        public string ExceptionString;

        public string Domain;

        public PropertiesDictionary Properties;
    }
}