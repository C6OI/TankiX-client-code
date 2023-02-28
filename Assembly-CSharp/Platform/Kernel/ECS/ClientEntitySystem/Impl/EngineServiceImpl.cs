using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EngineServiceImpl : EngineServiceInternal, EngineService {
        protected readonly Flow flow;

        readonly bool instanceFieldsInitialized;

        protected readonly TemplateRegistry templateRegistry;

        EngineDefaultRegistrator engineDefaultRegistrator;

        public EngineServiceImpl(TemplateRegistry templateRegistry, HandlerCollector handlerCollector, EventMaker eventMaker, ComponentBitIdRegistry componentBitIdRegistry) {
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
            SystemRegistry = new SystemRegistry(templateRegistry, this);
            NodeCache = new NodeCache(this);
            ComponentBitIdRegistry = componentBitIdRegistry;
            HandlerContextDataStorage = new HandlerContextDataStorage();
            FlowListeners = new HashSet<FlowListener>();
            ComponentListeners = new HashSet<ComponentListener>();
            EventListeners = new HashSet<EventListener>();
            EventInstancesStorageForReuse = new TypeInstancesStorage<Event>();
            engineDefaultRegistrator.Register();
            CollectEmptyEventInstancesForReuse();
            flow = new Flow(this);
        }

        public bool IsRunning { get; private set; }

        public ICollection<ComponentConstructor> ComponentConstructors { get; }

        public HandlerCollector HandlerCollector { get; }

        public EventMaker EventMaker { get; }

        public BroadcastEventHandlerCollector BroadcastEventHandlerCollector { get; }

        public DelayedEventManager DelayedEventManager { get; }

        public EntityRegistry EntityRegistry { get; }

        public virtual NodeCollectorImpl NodeCollector { get; protected set; }

        public Entity EntityStub { get; private set; }

        public Engine Engine { get; }

        public ComponentBitIdRegistry ComponentBitIdRegistry { get; }

        public NodeCache NodeCache { get; protected set; }

        public HandlerStateListener HandlerStateListener { get; }

        public HandlerContextDataStorage HandlerContextDataStorage { get; }

        public ICollection<FlowListener> FlowListeners { get; }

        public ICollection<ComponentListener> ComponentListeners { get; }

        public ICollection<EventListener> EventListeners { get; }

        public TypeInstancesStorage<Event> EventInstancesStorageForReuse { get; }

        public SystemRegistry SystemRegistry { get; }

        public void RunECSKernel() {
            if (!IsRunning) {
                HandlerCollector.SortHandlers();
                IsRunning = true;
                EntityStub = new EntityStub();
                EntityRegistry.RegisterEntity(EntityStub);
            }
        }

        public void RegisterTasksForHandler(Type handlerType) {
            HandlerCollector.RegisterTasksForHandler(handlerType);
        }

        public void RegisterHandlerFactory(HandlerFactory factory) {
            HandlerCollector.RegisterHandlerFactory(factory);
        }

        public void RegisterSystem(ECSSystem system) {
            SystemRegistry.RegisterSystem(system);
        }

        public void ForceRegisterSystem(ECSSystem system) {
            SystemRegistry.ForceRegisterSystem(system);
        }

        public void AddSystemProcessingListener(EngineHandlerRegistrationListener listener) {
            HandlerCollector.AddHandlerListener(listener);
        }

        public Flow GetFlow() => flow;

        public void ExecuteInFlow(Consumer<Engine> consumer) {
            Flow.Current.ScheduleWith(consumer);
        }

        public void RegisterComponentConstructor(ComponentConstructor componentConstructor) {
            ComponentConstructors.Add(componentConstructor);
        }

        public virtual void RequireInitState() {
            if (IsRunning) {
                throw new RegistrationAfterStartECSException();
            }
        }

        public virtual EntityBuilder CreateEntityBuilder() => new(this, EntityRegistry, templateRegistry);

        public void AddFlowListener(FlowListener flowListener) {
            FlowListeners.Add(flowListener);
        }

        public void RemoveFlowListener(FlowListener flowListener) {
            FlowListeners.Remove(flowListener);
        }

        public void AddComponentListener(ComponentListener componentListener) {
            ComponentListeners.Add(componentListener);
        }

        public void AddEventListener(EventListener eventListener) {
            EventListeners.Add(eventListener);
        }

        void InitializeInstanceFields() {
            engineDefaultRegistrator = new EngineDefaultRegistrator(this);
        }

        Engine CreateDefaultEngine(DelayedEventManager delayedEventManager) {
            EngineImpl engineImpl = new();
            engineImpl.Init(templateRegistry, delayedEventManager);
            return engineImpl;
        }

        public void CollectEmptyEventInstancesForReuse() {
            List<Type> list = new(512);
            AssemblyTypeCollector.CollectEmptyEventTypes(list);

            foreach (Type item in list) {
                EventInstancesStorageForReuse.AddInstance(item);
            }
        }

        public Flow NewFlow() {
            RequireRunningState();
            return flow;
        }

        void RequireRunningState() {
            if (!IsRunning) {
                throw new ECSNotRunningException();
            }
        }
    }
}