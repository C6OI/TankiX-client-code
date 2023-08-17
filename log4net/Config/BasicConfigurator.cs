using System;
using System.Collections;
using System.Reflection;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository;
using log4net.Util;

namespace log4net.Config {
    public sealed class BasicConfigurator {
        static readonly Type declaringType = typeof(BasicConfigurator);

        BasicConfigurator() { }

        public static ICollection Configure() => Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

        public static ICollection Configure(IAppender appender) => Configure(new IAppender[1] { appender });

        public static ICollection Configure(params IAppender[] appenders) {
            ArrayList arrayList = new();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, appenders);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection Configure(ILoggerRepository repository) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                PatternLayout patternLayout = new();
                patternLayout.ConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";
                patternLayout.ActivateOptions();
                ConsoleAppender consoleAppender = new();
                consoleAppender.Layout = patternLayout;
                consoleAppender.ActivateOptions();
                InternalConfigure(repository, consoleAppender);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection Configure(ILoggerRepository repository, IAppender appender) =>
            Configure(repository, new IAppender[1] { appender });

        public static ICollection Configure(ILoggerRepository repository, params IAppender[] appenders) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, appenders);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        static void InternalConfigure(ILoggerRepository repository, params IAppender[] appenders) {
            IBasicRepositoryConfigurator basicRepositoryConfigurator = repository as IBasicRepositoryConfigurator;

            if (basicRepositoryConfigurator != null) {
                basicRepositoryConfigurator.Configure(appenders);
            } else {
                LogLog.Warn(declaringType,
                    string.Concat("BasicConfigurator: Repository [",
                        repository,
                        "] does not support the BasicConfigurator"));
            }
        }
    }
}