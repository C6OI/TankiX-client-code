using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientLogger.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class Flow {
        static Flow current;

        readonly BroadcastHandlerResolver broadcastHandlerResolver;
        readonly EngineServiceInternal engineService;

        readonly EventMaker eventMaker;

        readonly HandlerResolver handlerResolver = new();

        readonly NodeChangedHandlerResolver nodeChangedHandlerResolver = new();

        Consumer<Engine> firstTask;

        FlowListener flowListener;

        bool initialized;

        bool started;

        public Flow(EngineServiceInternal engineService) {
            this.engineService = engineService;
            NodeCollector = engineService.NodeCollector;
            EntityRegistry = engineService.EntityRegistry;
            eventMaker = engineService.EventMaker;
            handlerResolver = new HandlerResolver();
            nodeChangedHandlerResolver = new NodeChangedHandlerResolver();
            broadcastHandlerResolver = new BroadcastHandlerResolver(engineService.BroadcastEventHandlerCollector);
        }

        [Inject] public static FlowInstancesCache Cache { get; set; }

        public static bool CurrentFlowExist => current != null;

        public static Flow Current {
            get {
                if (!CurrentFlowExist) {
                    throw new CurrentThreadNotFlowException();
                }

                return current;
            }
            private set => current = value;
        }

        public NodeCollectorImpl NodeCollector { get; internal set; }

        public EntityRegistry EntityRegistry { get; internal set; }

        public bool SkipLogError { get; set; }

        public void ScheduleWith(Consumer<Engine> consumer) {
            if (!started) {
                throw new FlowNotStartedException(this);
            }

            consumer(engineService.Engine);
        }

        public void TryInvoke(ICollection<Handler> handlers, Event eventInstance, ICollection<Entity> contextEntities) {
            if (!started) {
                throw new FlowNotStartedException(this);
            }

            IList<HandlerInvokeData> list = handlerResolver.Resolve(handlers, eventInstance, contextEntities);

            for (int i = 0; i < list.Count; i++) {
                list[i].Invoke(list);
            }
        }

        public void TryInvoke(ICollection<Handler> fireHandlers, ICollection<Handler> completeHandlers, Event eventInstance,
            Entity entity, ICollection<NodeDescription> changedNodes) {
            if (!started) {
                throw new FlowNotStartedException(this);
            }

            IList<HandlerInvokeData> list =
                nodeChangedHandlerResolver.Resolve(fireHandlers, eventInstance, entity, changedNodes);

            IList<HandlerInvokeData> list2 =
                nodeChangedHandlerResolver.Resolve(completeHandlers, eventInstance, entity, changedNodes);

            for (int i = 0; i < list.Count; i++) {
                list[i].Invoke(list);
            }

            for (int j = 0; j < list2.Count; j++) {
                list2[j].Invoke(list2);
            }
        }

        public void TryInvoke(Event eventInstance, Type handlerType) {
            if (!started) {
                throw new FlowNotStartedException(this);
            }

            IList<HandlerInvokeData> list = broadcastHandlerResolver.Resolve(eventInstance, handlerType);

            for (int i = 0; i < list.Count; i++) {
                list[i].Invoke(list);
            }
        }

        public void SendEvent(Event e, Entity entity) => SendEvent(e, Collections.SingletonList(entity));

        public virtual void SendEvent(Event e, ICollection<Entity> entities) {
            if (e.GetType().IsDefined(typeof(Shared), true)) {
                InternalSendEvent e2 = Cache.internalSendEvent.GetInstance().Init(e, entities);
                eventMaker.Send(this, e2, entities);
            }

            SendEventWithoutInternal(e, entities);
        }

        public void SendEventWithoutInternal(Event e, ICollection<Entity> entities) => eventMaker.Send(this, e, entities);

        public Flow StartWith(Consumer<Engine> consumer) {
            firstTask = consumer;
            FlowInitialized();
            return this;
        }

        void FlowInitialized() {
            if (initialized) {
                throw new FlowAlreadyInitializedException(this);
            }

            initialized = true;
        }

        internal void Execute() {
            if (CurrentFlowExist) {
                FlowAlreadyExistsException ex = new();
                LoggerProvider.GetLogger<Flow>().Fatal(ex.Message, ex);
                throw ex;
            }

            Current = this;

            try {
                FlowStarted();
                ScheduleWith(firstTask);

                Collections.ForEach(engineService.FlowListeners,
                    delegate(FlowListener l) {
                        l.OnFlowSuccess();
                    });
            } catch (Exception e) {
                WriteErrorLog(e);

                Collections.ForEach(engineService.FlowListeners,
                    delegate(FlowListener l) {
                        l.OnFlowFatalError();
                    });

                throw;
            } finally {
                Current = null;
                started = false;
                initialized = false;
                firstTask = null;
            }
        }

        public void Clean() => Collections.ForEach(engineService.FlowListeners,
            delegate(FlowListener l) {
                l.OnFlowClean();
            });

        void WriteErrorLog(Exception e) {
            if (!SkipLogError) {
                LoggerProvider.GetLogger<Flow>().Fatal(e.Message, e);
            }
        }

        void FlowStarted() {
            if (started) {
                throw new FlowAlreadyStartedException(this);
            }

            started = true;
        }
    }
}