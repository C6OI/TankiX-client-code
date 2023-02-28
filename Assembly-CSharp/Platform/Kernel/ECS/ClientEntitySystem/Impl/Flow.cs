using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class Flow {
        static Flow current;
        readonly EngineServiceInternal engineService;

        readonly BroadcastHandlerResolver broadcastHandlerResolver;

        readonly EventMaker eventMaker;

        FlowListener flowListener;

        readonly HandlerResolver handlerResolver = new();

        readonly NodeChangedHandlerResolver nodeChangedHandlerResolver = new();

        public Flow(EngineServiceInternal engineService) {
            Current = this;
            this.engineService = engineService;
            NodeCollector = engineService.NodeCollector;
            EntityRegistry = engineService.EntityRegistry;
            eventMaker = engineService.EventMaker;
            handlerResolver = new HandlerResolver();
            nodeChangedHandlerResolver = new NodeChangedHandlerResolver();
            broadcastHandlerResolver = new BroadcastHandlerResolver(engineService.BroadcastEventHandlerCollector);
        }

        [Inject] public static FlowInstancesCache Cache { get; set; }

        [Inject] public static SharedEntityRegistry sharedEntityRegistry { get; set; }

        public static Flow Current { get; private set; }

        public NodeCollectorImpl NodeCollector { get; internal set; }

        public EntityRegistry EntityRegistry { get; internal set; }

        public void ScheduleWith(Consumer<Engine> consumer) {
            consumer(engineService.Engine);
        }

        public void TryInvoke(ICollection<Handler> handlers, Event eventInstance, ICollection<Entity> contextEntities) {
            IList<HandlerInvokeData> list = handlerResolver.Resolve(handlers, eventInstance, contextEntities);

            for (int i = 0; i < list.Count; i++) {
                list[i].Invoke(list);
            }
        }

        public void TryInvoke(ICollection<Handler> fireHandlers, ICollection<Handler> completeHandlers, Event eventInstance, Entity entity, ICollection<NodeDescription> changedNodes) {
            IList<HandlerInvokeData> list = nodeChangedHandlerResolver.Resolve(fireHandlers, eventInstance, entity, changedNodes);
            IList<HandlerInvokeData> list2 = nodeChangedHandlerResolver.Resolve(completeHandlers, eventInstance, entity, changedNodes);

            for (int i = 0; i < list.Count; i++) {
                list[i].Invoke(list);
            }

            for (int j = 0; j < list2.Count; j++) {
                list2[j].Invoke(list2);
            }
        }

        public void TryInvoke(Event eventInstance, Type handlerType) {
            IList<HandlerInvokeData> list = broadcastHandlerResolver.Resolve(eventInstance, handlerType);

            for (int i = 0; i < list.Count; i++) {
                list[i].Invoke(list);
            }
        }

        public void SendEvent(Event e, Entity entity) {
            SendEvent(e, Collections.SingletonList(entity));
        }

        public virtual void SendEvent(Event e, ICollection<Entity> entities) {
            NotifySendEvent(e, entities);
            SendEventSilent(e, entities);
        }

        void NotifySendEvent(Event e, ICollection<Entity> entities) {
            Collections.Enumerator<EventListener> enumerator = Collections.GetEnumerator(engineService.EventListeners);

            while (enumerator.MoveNext()) {
                enumerator.Current.OnEventSend(e, entities);
            }
        }

        public void SendEventSilent(Event e, ICollection<Entity> entities) {
            eventMaker.Send(this, e, entities);
        }

        public void Finish() {
            Collections.ForEach(engineService.FlowListeners, delegate(FlowListener l) {
                l.OnFlowFinish();
            });
        }

        public void Clean() {
            Collections.ForEach(engineService.FlowListeners, delegate(FlowListener l) {
                l.OnFlowClean();
            });
        }
    }
}