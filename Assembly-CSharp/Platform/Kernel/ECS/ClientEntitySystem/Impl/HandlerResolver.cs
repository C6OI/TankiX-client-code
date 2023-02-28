using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerResolver {
        protected static HandlerArgumetCombinator combinator = new();

        [Inject] public static FlowInstancesCache Cache { get; set; }

        [Inject] public static EngineService EngineService { get; set; }

        public virtual IList<HandlerInvokeData> Resolve(ICollection<Handler> handlers, Event eventInstance, ICollection<Entity> contextEntities) {
            if (handlers.Count == 0) {
                return Collections.EmptyList<HandlerInvokeData>();
            }

            List<HandlerInvokeData> instance = Cache.listHandlersInvokeData.GetInstance();
            Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator(handlers);

            while (enumerator.MoveNext()) {
                Handler current = enumerator.Current;
                HandlerInvokeData invokeData = ((EngineServiceImpl)EngineService).HandlerContextDataStorage.GetInvokeData(current, eventInstance.GetType(), contextEntities);

                if (invokeData.Reuse(eventInstance) || UpdateInvokeData(invokeData, current, eventInstance, contextEntities)) {
                    instance.Add(invokeData);
                }
            }

            return instance;
        }

        protected virtual bool UpdateInvokeData(HandlerInvokeData invokeData, Handler handler, Event eventInstance, ICollection<Entity> contextEntities) {
            if (handler.IsEventOnlyArguments) {
                invokeData.UpdateForEventOnlyArguments(eventInstance);
                return true;
            }

            HandlerInvokeGraph handlerInvokeGraph = handler.HandlerInvokeGraph.Init();
            bool flag = combinator.Combine(handlerInvokeGraph, contextEntities);

            if (flag) {
                invokeData.Update(eventInstance, handlerInvokeGraph);
            } else {
                invokeData.UpdateForEmptyCall();
            }

            handlerInvokeGraph.Clear();
            return flag;
        }
    }
}