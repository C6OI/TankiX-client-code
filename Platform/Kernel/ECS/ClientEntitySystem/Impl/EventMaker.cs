using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class EventMaker {
        static readonly EventSender STUB = new StubEvenSender();

        readonly HandlerCollector handlerCollector;

        readonly Dictionary<Type, EventSender> senderByEventType = new();

        public EventMaker(HandlerCollector handlerCollector) => this.handlerCollector = handlerCollector;

        public virtual void Send(Flow flow, Event e, ICollection<Entity> entities) =>
            GetSender(e.GetType()).Send(flow, e, entities);

        EventSender GetSender(Type eventType) => senderByEventType.ComputeIfAbsent(eventType, CreateSender);

        protected EventSender CreateSender(Type eventType) {
            ICollection<Handler> handlers = handlerCollector.GetHandlers(typeof(EventFireHandler), eventType);
            ICollection<Handler> handlers2 = handlerCollector.GetHandlers(typeof(EventCompleteHandler), eventType);

            if (handlers.Count > 0 && handlers2.Count > 0) {
                return new FireAndCompleteEventSender(handlers, handlers2);
            }

            if (handlers.Count > 0) {
                return new FireOnlyEventSender(handlers);
            }

            if (handlers2.Count > 0) {
                return new CompleteOnlyEventSender(handlers2);
            }

            return STUB;
        }
    }
}