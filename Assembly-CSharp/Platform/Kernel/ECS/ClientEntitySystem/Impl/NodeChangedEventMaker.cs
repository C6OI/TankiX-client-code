using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class NodeChangedEventMaker {
        readonly Type _completeHandlerType;

        readonly Type _fireHandlerType;
        readonly Event eventInstance;

        readonly HandlerCollector handlerCollector;

        public NodeChangedEventMaker(Event eventInstance, Type fireHandlerType, Type completeHandlerType, HandlerCollector handlerCollector) {
            this.eventInstance = eventInstance;
            _fireHandlerType = fireHandlerType;
            _completeHandlerType = completeHandlerType;
            this.handlerCollector = handlerCollector;
        }

        public void MakeIfNeed(Entity entity, Type componentType) {
            ICollection<NodeDescription> nodeDescriptions = ((EntityInternal)entity).NodeDescriptionStorage.GetNodeDescriptions(componentType);
            Make(entity, nodeDescriptions);
        }

        void Make(Entity entity, ICollection<NodeDescription> changedNodes) {
            if (changedNodes.Count != 0) {
                ICollection<Handler> fireHandlers = CollectHandlers(handlerCollector, _fireHandlerType, changedNodes);
                ICollection<Handler> completeHandlers = CollectHandlers(handlerCollector, _completeHandlerType, changedNodes);
                Flow.Current.TryInvoke(fireHandlers, completeHandlers, eventInstance, entity, changedNodes);
            }
        }

        public static ICollection<Handler> CollectHandlers(HandlerCollector handlerCollector, Type handlerType, ICollection<NodeDescription> changedNodes) {
            Collections.Enumerator<NodeDescription> enumerator = Collections.GetEnumerator(changedNodes);
            enumerator.MoveNext();
            ICollection<Handler> handlers = handlerCollector.GetHandlers(handlerType, enumerator.Current);

            if (!enumerator.MoveNext()) {
                return handlers;
            }

            List<Handler> list = new(handlers);

            do {
                list.AddRange(handlerCollector.GetHandlers(handlerType, enumerator.Current));
            } while (enumerator.MoveNext());

            return list;
        }
    }
}