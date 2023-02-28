using System;
using System.Collections;
using System.IO;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Platform.Library.ClientLogger.Impl;

namespace Platform.Library.ClientLogger.API {
    public static class LoggerProvider {
        public static void Init() {
            ConfigureRootLogger();
        }

        public static ILog GetLogger<T>() => GetLogger(typeof(T));

        public static ILog GetLogger(Type t) => LogManager.GetLogger(t);

        public static ILog GetLogger(object obj) => GetLogger(obj.GetType());

        static void ConfigureRootLogger() {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();
            hierarchy.Root.Level = Level.All;
        }

        public static ICollection LoadConfiguration(FileInfo fileInfo) => XmlConfigurator.Configure(fileInfo);

        public static ICollection LoadConfiguration(Stream configStream) => XmlConfigurator.Configure(configStream);

        public static void AddApender(AppenderBuilder appenderBuilder) {
            AddApender(appenderBuilder.Configure());
        }

        public static void AddApender(AppenderSkeleton appender) {
            BasicConfigurator.Configure(appender);
        }

        public static void RemoveApender(string appenderName) {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAppender(appenderName);
        }

        public static IAppender GetAppender(string appenderName) {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            return hierarchy.Root.GetAppender(appenderName);
        }

        public static ILogger[] GetCurrentLoggers() {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            return hierarchy.GetCurrentLoggers();
        }
    }
}