using System;
using System.Collections;
using log4net.Repository;

namespace log4net.Core {
    public class WrapperMap {
        readonly WrapperCreationHandler m_createWrapperHandler;

        readonly LoggerRepositoryShutdownEventHandler m_shutdownHandler;

        public WrapperMap(WrapperCreationHandler createWrapperHandler) {
            m_createWrapperHandler = createWrapperHandler;
            m_shutdownHandler = ILoggerRepository_Shutdown;
        }

        protected Hashtable Repositories { get; } = new();

        public virtual ILoggerWrapper GetWrapper(ILogger logger) {
            if (logger == null) {
                return null;
            }

            lock (this) {
                Hashtable hashtable = (Hashtable)Repositories[logger.Repository];

                if (hashtable == null) {
                    hashtable = new Hashtable();
                    Repositories[logger.Repository] = hashtable;
                    logger.Repository.ShutdownEvent += m_shutdownHandler;
                }

                ILoggerWrapper loggerWrapper = hashtable[logger] as ILoggerWrapper;

                if (loggerWrapper == null) {
                    loggerWrapper = (ILoggerWrapper)(hashtable[logger] = CreateNewWrapperObject(logger));
                }

                return loggerWrapper;
            }
        }

        protected virtual ILoggerWrapper CreateNewWrapperObject(ILogger logger) {
            if (m_createWrapperHandler != null) {
                return m_createWrapperHandler(logger);
            }

            return null;
        }

        protected virtual void RepositoryShutdown(ILoggerRepository repository) {
            lock (this) {
                Repositories.Remove(repository);
                repository.ShutdownEvent -= m_shutdownHandler;
            }
        }

        void ILoggerRepository_Shutdown(object sender, EventArgs e) {
            ILoggerRepository loggerRepository = sender as ILoggerRepository;

            if (loggerRepository != null) {
                RepositoryShutdown(loggerRepository);
            }
        }
    }
}