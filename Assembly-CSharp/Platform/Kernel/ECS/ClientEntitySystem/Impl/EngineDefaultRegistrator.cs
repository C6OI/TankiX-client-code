using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EngineDefaultRegistrator {
        readonly EngineServiceImpl engineServiceImpl;

        public EngineDefaultRegistrator(EngineServiceImpl engineServiceImpl) => this.engineServiceImpl = engineServiceImpl;

        void RegisterComponentConstructor() {
            engineServiceImpl.RegisterComponentConstructor(new ConfigComponentConstructor());
        }

        void RegisterSystems() {
            engineServiceImpl.RegisterSystem(new AutoAddComponentsSystem());
            engineServiceImpl.RegisterSystem(new AutoRemoveComponentsSystem(new AutoRemoveComponentsRegistryImpl(engineServiceImpl)));
        }

        void RegisterTasks() {
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(TimeUpdateFireHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(TimeUpdateCompleteHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(EarlyUpdateFireHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(EarlyUpdateCompleteHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventFireHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventCompleteHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventFireHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventCompleteHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(FixedUpdateEventFireHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(FixedUpdateEventCompleteHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(AfterFixedUpdateEventFireHandler));
            engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(AfterFixedUpdateEventCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(TimeUpdateFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(TimeUpdateCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(EarlyUpdateFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(EarlyUpdateCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(UpdateEventFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(UpdateEventCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(FixedUpdateEventFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(FixedUpdateEventCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(AfterFixedUpdateEventFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(AfterFixedUpdateEventCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(NodeAddedFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(NodeAddedCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(NodeRemovedFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(NodeRemovedCompleteHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(EventFireHandler));
            engineServiceImpl.RegisterTasksForHandler(typeof(EventCompleteHandler));
        }

        void RegisterHandlerFactory() {
            engineServiceImpl.RegisterHandlerFactory(new TimeUpdateFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new TimeUpdateCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new EarlyUpdateFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new EarlyUpdateCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new UpdateEventFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new UpdateEventCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new FixedUpdateEventFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new FixedUpdateEventCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new AfterFixedUpdateEventFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new AfterFixedUpdateEventCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new NodeAddedFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new NodeAddedCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new NodeRemovedFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new NodeRemovedCompleteHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new EventFireHandlerFactory());
            engineServiceImpl.RegisterHandlerFactory(new EventCompleteHandlerFactory());
        }

        public void Register() {
            RegisterComponentConstructor();
            RegisterHandlerFactory();
            RegisterTasks();
            RegisterSystems();
        }
    }
}