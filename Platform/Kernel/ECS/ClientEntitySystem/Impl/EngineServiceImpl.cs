using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EngineServiceImpl : EngineService, EngineServiceInternal {
        protected readonly Flow flow;

        readonly bool instanceFieldsInitialized;

        readonly SystemRegistry systemRegistry;

        protected readonly TemplateRegistry templateRegistry;

        EngineDefaultRegistrator engineDefaultRegistrator;

        bool runningState;

        public EngineServiceImpl(TemplateRegistry templateRegistry, HandlerCollector handlerCollector, EventMaker eventMaker,
            ComponentBitIdRegistry componentBitIdRegistry) {
            this.templateRegistry = templateRegistry;

            if (!instanceFieldsInitialized) {
                InitializeInstanceFields();
                instanceFieldsInitialized = true;
            }

            HandlerCollector = handlerCollector;
            EventMaker = eventMaker;
            BroadcastEventHandlerCollector = new BroadcastEventHandlerCollector(HandlerCollector);
            HandlerStateListener = new HandlerStateListener(HandlerCollector);
            ComponentConstructors = new List<ComponentConstructor>();
            DelayedEventManager = new DelayedEventManager(this);
            Engine = CreateDefaultEngine(DelayedEventManager);
            EntityRegistry = new EntityRegistryImpl();
            NodeCollector = new NodeCollectorImpl();
            systemRegistry = new SystemRegistry(templateRegistry, this);
            NodeCache = new NodeCache(this);
            ComponentBitIdRegistry = componentBitIdRegistry;
            HandlerContextDataStorage = new HandlerContextDataStorage();
            FlowListeners = new HashSet<FlowListener>();
            engineDefaultRegistrator.Register();
            flow = new Flow(this);
        }

        public Engine Engine { get; }

        public event Consumer<Flow> FlowStartEvents;

        public event Consumer<Flow> FlowFinishEvents;

        public void RegisterTasksForHandler(Type handlerType) => HandlerCollector.RegisterTasksForHandler(handlerType);

        public void RegisterHandlerFactory(HandlerFactory factory) => HandlerCollector.RegisterHandlerFactory(factory);

        public void RegisterSystem(ECSSystem system) => systemRegistry.RegisterSystem(system);

        public void AddSystemProcessingListener(EngineHandlerRegistrationListener listener) =>
            HandlerCollector.AddHandlerListener(listener);

        public void ExecuteInFlow(Consumer<Engine> consumer) {
            if (Flow.CurrentFlowExist) {
                Flow.Current.ScheduleWith(consumer);
                return;
            }

            Flow flow = NewFlow();
            flow.StartWith(consumer);
            ExecuteFlow(flow);
        }

        public virtual EntityBuilder CreateEntityBuilder() => new(this, EntityRegistry, templateRegistry);

        public void AddFlowListener(FlowListener flowListener) => FlowListeners.Add(flowListener);

        public void RemoveFlowListener(FlowListener flowListener) => FlowListeners.Remove(flowListener);

        public ICollection<ComponentConstructor> ComponentConstructors { get; }

        public HandlerCollector HandlerCollector { get; }

        public EventMaker EventMaker { get; }

        public BroadcastEventHandlerCollector BroadcastEventHandlerCollector { get; }

        public DelayedEventManager DelayedEventManager { get; }

        public EntityRegistry EntityRegistry { get; }

        public virtual NodeCollectorImpl NodeCollector { get; protected set; }

        public Entity EntityStub { get; private set; }

        public ComponentBitIdRegistry ComponentBitIdRegistry { get; }

        public NodeCache NodeCache { get; protected set; }

        public HandlerStateListener HandlerStateListener { get; }

        public HandlerContextDataStorage HandlerContextDataStorage { get; }

        public ICollection<FlowListener> FlowListeners { get; }

        public void RunECSKernel() {
            if (!runningState) {
                HandlerCollector.SortHandlers();
                runningState = true;
                EntityStub = new EntityStub();
                EntityRegistry.RegisterEntity(EntityStub);
            }
        }

        public void ForceRegisterSystem(ECSSystem system) => systemRegistry.ForceRegisterSystem(system);

        public Flow NewFlow() {
            RequireRunningState();
            return flow;
        }

        public Flow GetFlow() => flow;

        public void ExecuteFlow(Flow flow) {
            RequireRunningState();

            if (FlowStartEvents != null) {
                FlowStartEvents(flow);
            }

            flow.Execute();

            if (FlowFinishEvents != null) {
                FlowFinishEvents(flow);
            }
        }

        public void RegisterComponentConstructor(ComponentConstructor componentConstructor) =>
            ComponentConstructors.Add(componentConstructor);

        public virtual void RequireInitState() {
            if (runningState) {
                throw new RegistrationAfterStartECSException();
            }
        }

        void InitializeInstanceFields() => engineDefaultRegistrator = new EngineDefaultRegistrator(this);

        Engine CreateDefaultEngine(DelayedEventManager delayedEventManager) {
            EngineImpl engineImpl = new();
            engineImpl.Init(templateRegistry, delayedEventManager, this);
            return engineImpl;
        }

        void RequireRunningState() {
            if (!runningState) {
                throw new ECSNotRunningException();
            }
        }
    }
}