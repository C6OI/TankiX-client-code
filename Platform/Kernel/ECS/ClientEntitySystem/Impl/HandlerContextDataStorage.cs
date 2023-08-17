using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerContextDataStorage : EntityListener {
        readonly MultiMap<long, HandlerContextDescription> contextDescriptionsByEntity = new();

        readonly Dictionary<HandlerContextDescription, HandlerInvokeData> invokeDataByDescription =
            new(new HandlerContextDescriptionComparer());

        [Inject] public static FlowInstancesCache Cache { get; set; }

        public void OnNodeAdded(Entity entity, NodeDescription nodeDescription) { }

        public void OnNodeRemoved(Entity entity, NodeDescription nodeDescription) { }

        public void OnEntityDeleted(Entity entity) {
            HashSet<HandlerContextDescription> value;

            if (contextDescriptionsByEntity.TryGetValue(entity.Id, out value)) {
                HashSet<HandlerContextDescription>.Enumerator enumerator = value.GetEnumerator();

                while (enumerator.MoveNext()) {
                    invokeDataByDescription.Remove(enumerator.Current);
                }

                contextDescriptionsByEntity.Remove(entity.Id);
            }
        }

        public HandlerInvokeData GetInvokeData(Handler handler, Type eventType, ICollection<Entity> entities) {
            if (handler.IsContextOnlyArguments || entities.Count == 0 || entities.Count > 2) {
                return Cache.flowInvokeData.GetInstance().Init(handler);
            }

            long num = 0L;
            long e = 0L;
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                if (num == 0L) {
                    num = enumerator.Current.Id;
                } else {
                    e = enumerator.Current.Id;
                }
            }

            HandlerContextDescription handlerContextDescription = new(handler, eventType, num, e);
            HandlerInvokeData value;

            if (invokeDataByDescription.TryGetValue(handlerContextDescription, out value)) {
                return value;
            }

            value = new HandlerInvokeData(handler);
            invokeDataByDescription.Add(handlerContextDescription, value);
            enumerator = Collections.GetEnumerator(entities);

            while (enumerator.MoveNext()) {
                contextDescriptionsByEntity.Add(enumerator.Current.Id, handlerContextDescription);
            }

            return value;
        }

        public struct HandlerContextDescription {
            public Handler handler;

            public long e1;

            public long e2;

            public Type eventType;

            public HandlerContextDescription(Handler handler, Type eventType, long e1, long e2) {
                this.handler = handler;
                this.e1 = e1;
                this.e2 = e2;
                this.eventType = eventType;
            }
        }

        public class HandlerContextDescriptionComparer : IEqualityComparer<HandlerContextDescription> {
            public bool Equals(HandlerContextDescription a, HandlerContextDescription b) =>
                a.handler == b.handler &&
                a.eventType == b.eventType &&
                (a.e1 == b.e1 || a.e1 == b.e2) &&
                (a.e2 == b.e1 || a.e2 == b.e2);

            public int GetHashCode(HandlerContextDescription obj) =>
                obj.e1.GetHashCode() + obj.e2.GetHashCode() + obj.handler.GetHashCode() ^ obj.eventType.GetHashCode();
        }
    }
}