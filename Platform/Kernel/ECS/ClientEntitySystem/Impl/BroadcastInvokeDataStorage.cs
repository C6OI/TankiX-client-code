using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class BroadcastInvokeDataStorage {
        readonly List<HandlerBroadcastInvokeData> datas = new(200);

        public IList<HandlerBroadcastInvokeData> ContextInvokeDatas => datas;

        public void Add(Entity entity, ICollection<Handler> handlers) {
            if (handlers.Count != 0) {
                Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator(handlers);

                while (enumerator.MoveNext()) {
                    Handler current = enumerator.Current;
                    HandlerBroadcastInvokeData item = new(current, entity);
                    datas.Add(item);
                }
            }
        }

        public void Remove(Entity entity, ICollection<Handler> handlers) {
            if (handlers.Count == 0) {
                return;
            }

            Collections.Enumerator<Handler> enumerator = Collections.GetEnumerator(handlers);

            while (enumerator.MoveNext()) {
                for (int num = datas.Count - 1; num >= 0; num--) {
                    HandlerBroadcastInvokeData handlerBroadcastInvokeData = datas[num];
                    Handler current = enumerator.Current;

                    if (handlerBroadcastInvokeData.Handler == current && handlerBroadcastInvokeData.Entity.Equals(entity)) {
                        datas.RemoveAt(num);
                    }
                }
            }
        }

        public void Remove(Entity entity) {
            for (int num = datas.Count - 1; num >= 0; num--) {
                HandlerBroadcastInvokeData handlerBroadcastInvokeData = datas[num];

                if (handlerBroadcastInvokeData.Entity.Equals(entity)) {
                    datas.RemoveAt(num);
                }
            }
        }
    }
}