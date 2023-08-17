using System;
using System.Reflection;
using log4net.Repository;

namespace log4net.Core {
    public interface IRepositorySelector {
        event LoggerRepositoryCreationEventHandler LoggerRepositoryCreatedEvent;

        ILoggerRepository GetRepository(Assembly assembly);

        ILoggerRepository GetRepository(string repositoryName);

        ILoggerRepository CreateRepository(Assembly assembly, Type repositoryType);

        ILoggerRepository CreateRepository(string repositoryName, Type repositoryType);

        bool ExistsRepository(string repositoryName);

        ILoggerRepository[] GetAllRepositories();
    }
}