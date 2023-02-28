using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerStateListener : EntityListener {
        readonly HandlerCollector handlerCollector;

        public HandlerStateListener(HandlerCollector handlerCollector) => this.handlerCollector = handlerCollector;

        public void OnNodeAdded(Entity entity, NodeDescription nodeDescription) {
            ICollection<Handler> handlersWithoutContext = handlerCollector.GetHandlersWithoutContext(nodeDescription);
            IEnumerator<Handler> enumerator = handlersWithoutContext.GetEnumerator();

            while (enumerator.MoveNext()) {
                Handler current = enumerator.Current;
                current.ChangeVersion();
            }
        }

        public void OnNodeRemoved(Entity entity, NodeDescription nodeDescription) {
            ICollection<Handler> handlersWithoutContext = handlerCollector.GetHandlersWithoutContext(nodeDescription);
            IEnumerator<Handler> enumerator = handlersWithoutContext.GetEnumerator();

            while (enumerator.MoveNext()) {
                Handler current = enumerator.Current;
                current.ChangeVersion();
            }
        }

        public void OnEntityDeleted(Entity entity) { }
    }
}