using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerInvokeGraph {
        public HandlerInvokeGraph(Handler handler) {
            Handler = handler;
            IList<HandlerArgument> handlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
            ArgumentNodes = new ArgumentNode[handlerArguments.Count];

            for (int i = 0; i < handlerArguments.Count; i++) {
                ArgumentNodes[i] = new ArgumentNode(handlerArguments[i]);
            }
        }

        public Handler Handler { get; }

        public ArgumentNode[] ArgumentNodes { get; }

        public HandlerInvokeGraph Init() {
            Clear();
            return this;
        }

        public void Clear() {
            IList<HandlerArgument> handlerArguments = Handler.HandlerArgumentsDescription.HandlerArguments;

            for (int i = 0; i < handlerArguments.Count; i++) {
                ArgumentNodes[i].Clear();
            }
        }
    }
}