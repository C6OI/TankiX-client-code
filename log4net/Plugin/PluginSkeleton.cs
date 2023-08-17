using log4net.Repository;

namespace log4net.Plugin {
    public abstract class PluginSkeleton : IPlugin {
        string m_name;

        ILoggerRepository m_repository;

        protected PluginSkeleton(string name) => m_name = name;

        protected virtual ILoggerRepository LoggerRepository {
            get => m_repository;
            set => m_repository = value;
        }

        public virtual string Name {
            get => m_name;
            set => m_name = value;
        }

        public virtual void Attach(ILoggerRepository repository) => m_repository = repository;

        public virtual void Shutdown() { }
    }
}