using System;
using System.Reflection;
using log4net.Core;
using log4net.Repository;

namespace log4net {
    public sealed class LogManager {
        static readonly WrapperMap s_wrapperMap = new(WrapperCreationHandler);

        LogManager() { }

        public static ILog Exists(string name) => Exists(Assembly.GetCallingAssembly(), name);

        public static ILog Exists(string repository, string name) => WrapLogger(LoggerManager.Exists(repository, name));

        public static ILog Exists(Assembly repositoryAssembly, string name) =>
            WrapLogger(LoggerManager.Exists(repositoryAssembly, name));

        public static ILog[] GetCurrentLoggers() => GetCurrentLoggers(Assembly.GetCallingAssembly());

        public static ILog[] GetCurrentLoggers(string repository) =>
            WrapLoggers(LoggerManager.GetCurrentLoggers(repository));

        public static ILog[] GetCurrentLoggers(Assembly repositoryAssembly) =>
            WrapLoggers(LoggerManager.GetCurrentLoggers(repositoryAssembly));

        public static ILog GetLogger(string name) => GetLogger(Assembly.GetCallingAssembly(), name);

        public static ILog GetLogger(string repository, string name) =>
            WrapLogger(LoggerManager.GetLogger(repository, name));

        public static ILog GetLogger(Assembly repositoryAssembly, string name) =>
            WrapLogger(LoggerManager.GetLogger(repositoryAssembly, name));

        public static ILog GetLogger(Type type) => GetLogger(Assembly.GetCallingAssembly(), type.FullName);

        public static ILog GetLogger(string repository, Type type) => WrapLogger(LoggerManager.GetLogger(repository, type));

        public static ILog GetLogger(Assembly repositoryAssembly, Type type) =>
            WrapLogger(LoggerManager.GetLogger(repositoryAssembly, type));

        public static void Shutdown() => LoggerManager.Shutdown();

        public static void ShutdownRepository() => ShutdownRepository(Assembly.GetCallingAssembly());

        public static void ShutdownRepository(string repository) => LoggerManager.ShutdownRepository(repository);

        public static void ShutdownRepository(Assembly repositoryAssembly) =>
            LoggerManager.ShutdownRepository(repositoryAssembly);

        public static void ResetConfiguration() => ResetConfiguration(Assembly.GetCallingAssembly());

        public static void ResetConfiguration(string repository) => LoggerManager.ResetConfiguration(repository);

        public static void ResetConfiguration(Assembly repositoryAssembly) =>
            LoggerManager.ResetConfiguration(repositoryAssembly);

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository() => GetRepository(Assembly.GetCallingAssembly());

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository(string repository) => GetRepository(repository);

        [Obsolete("Use GetRepository instead of GetLoggerRepository")]
        public static ILoggerRepository GetLoggerRepository(Assembly repositoryAssembly) =>
            GetRepository(repositoryAssembly);

        public static ILoggerRepository GetRepository() => GetRepository(Assembly.GetCallingAssembly());

        public static ILoggerRepository GetRepository(string repository) => LoggerManager.GetRepository(repository);

        public static ILoggerRepository GetRepository(Assembly repositoryAssembly) =>
            LoggerManager.GetRepository(repositoryAssembly);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(Type repositoryType) =>
            CreateRepository(Assembly.GetCallingAssembly(), repositoryType);

        public static ILoggerRepository CreateRepository(Type repositoryType) =>
            CreateRepository(Assembly.GetCallingAssembly(), repositoryType);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(string repository) => LoggerManager.CreateRepository(repository);

        public static ILoggerRepository CreateRepository(string repository) => LoggerManager.CreateRepository(repository);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(string repository, Type repositoryType) =>
            LoggerManager.CreateRepository(repository, repositoryType);

        public static ILoggerRepository CreateRepository(string repository, Type repositoryType) =>
            LoggerManager.CreateRepository(repository, repositoryType);

        [Obsolete("Use CreateRepository instead of CreateDomain")]
        public static ILoggerRepository CreateDomain(Assembly repositoryAssembly, Type repositoryType) =>
            LoggerManager.CreateRepository(repositoryAssembly, repositoryType);

        public static ILoggerRepository CreateRepository(Assembly repositoryAssembly, Type repositoryType) =>
            LoggerManager.CreateRepository(repositoryAssembly, repositoryType);

        public static ILoggerRepository[] GetAllRepositories() => LoggerManager.GetAllRepositories();

        static ILog WrapLogger(ILogger logger) => (ILog)s_wrapperMap.GetWrapper(logger);

        static ILog[] WrapLoggers(ILogger[] loggers) {
            ILog[] array = new ILog[loggers.Length];

            for (int i = 0; i < loggers.Length; i++) {
                array[i] = WrapLogger(loggers[i]);
            }

            return array;
        }

        static ILoggerWrapper WrapperCreationHandler(ILogger logger) => new LogImpl(logger);
    }
}