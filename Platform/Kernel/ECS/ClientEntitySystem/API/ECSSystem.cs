using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientLogger.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public class ECSSystem : EngineImpl {
        protected ILog Log { get; private set; }

        public void Init(TemplateRegistry templateRegistry, DelayedEventManager delayedEventManager,
            EngineServiceInternal engineService, NodeRegistrator nodeRegistrator) {
            base.Init(templateRegistry, delayedEventManager, engineService);
            Log = LoggerProvider.GetLogger(this);
        }

        protected Entity GetEntityById(long entityId) => Flow.Current.EntityRegistry.GetEntity(entityId);
    }
}