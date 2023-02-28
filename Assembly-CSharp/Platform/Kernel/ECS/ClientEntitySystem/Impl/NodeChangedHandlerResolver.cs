using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeChangedHandlerResolver {
        protected static HandlerArgumetCombinator combinator = new();

        protected static NodeChangedReverseCombinator reverseCombinator = new();

        protected List<Entity> changedEntityAsList = new();

        [Inject] public static FlowInstancesCache Cache { get; set; }

        public virtual IList<HandlerInvokeData> Resolve(ICollection<Handler> handlers, Event eventInstance, Entity entity, ICollection<NodeDescription> changedNodes) {
            if (handlers.Count == 0) {
                return Collections.EmptyList<HandlerInvokeData>();
            }

            List<HandlerInvokeData> instance = Cache.listHandlersInvokeData.GetInstance();
            Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator(handlers);

            while (enumerator.MoveNext()) {
                Handler current = enumerator.Current;
                HandlerInvokeData handlerInvokeData = Cache.flowInvokeData.GetInstance().Init(current);

                if (UpdateInvokeData(handlerInvokeData, current, eventInstance, entity, changedNodes)) {
                    instance.Add(handlerInvokeData);
                }
            }

            return instance;
        }

        protected bool UpdateInvokeData(HandlerInvokeData invokeData, Handler handler, Event eventInstance, Entity entity, ICollection<NodeDescription> changedNodes) {
            HandlerInvokeGraph handlerInvokeGraph = handler.HandlerInvokeGraph.Init();
            bool flag = reverseCombinator.Combine(handlerInvokeGraph, entity, changedNodes) && combinator.Combine(handlerInvokeGraph, null);

            if (flag) {
                invokeData.Update(eventInstance, handlerInvokeGraph);
            }

            handlerInvokeGraph.Clear();
            return flag;
        }
    }
}