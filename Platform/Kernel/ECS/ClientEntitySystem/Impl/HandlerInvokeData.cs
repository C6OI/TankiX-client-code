using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerInvokeData {
        readonly List<HandlerExecutor> handlerExecutors = new();
        int handlerVersion = -1;

        int illegalCombineIndex;

        public HandlerInvokeData() { }

        public HandlerInvokeData(Handler handler) {
            Handler = handler;
            HandlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
        }

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public Handler Handler { get; private set; }

        public IList<HandlerArgument> HandlerArguments { get; private set; }

        public HandlerInvokeData Init(Handler handler) {
            handlerExecutors.Clear();
            handlerVersion = -1;
            Handler = handler;
            HandlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
            return this;
        }

        public bool IsActual() => Handler != null && handlerVersion == Handler.Version;

        public bool Reuse(Event eventInstance) {
            if (IsActual()) {
                for (int i = 0; i < handlerExecutors.Count; i++) {
                    HandlerExecutor handlerExecutor = handlerExecutors[i];
                    handlerExecutor.SetEvent(eventInstance);
                }

                return true;
            }

            return false;
        }

        public virtual void UpdateForEventOnlyArguments(Event eventInstance) {
            handlerVersion = Handler.Version;
            handlerExecutors.Clear();
            HandlerExecutor handlerExecutor = CreateExecutor();
            handlerExecutor.ArgumentForMethod[0] = eventInstance;
            handlerExecutors.Add(handlerExecutor);
        }

        public virtual void UpdateForEmptyCall() {
            handlerVersion = Handler.Version;
            handlerExecutors.Clear();
        }

        public virtual void Update(Event eventInstance, HandlerInvokeGraph invokeGraph) {
            handlerVersion = Handler.Version;
            handlerExecutors.Clear();
            ArgumentNode argumentNode = invokeGraph.ArgumentNodes[0];
            List<EntityNode> entityNodes = argumentNode.entityNodes;
            HandlerArgument argument = argumentNode.argument;
            bool flag = false;

            for (int i = 0; i < entityNodes.Count; i++) {
                if (CollectExecutors(invokeGraph, entityNodes[i], 0)) {
                    if (!argument.Combine && flag) {
                        throw new IllegalCombineException(Handler, argumentNode);
                    }

                    flag = true;
                }
            }

            if (!flag) {
                throw new InvalidInvokeGraphException(Handler);
            }

            for (int j = 0; j < handlerExecutors.Count; j++) {
                handlerExecutors[j].SetEvent(eventInstance);
            }
        }

        bool CollectExecutors(HandlerInvokeGraph invokeGraph, EntityNode entityNode, int argumentIndex) {
            HandlerArgument handlerArgument = HandlerArguments[argumentIndex];

            if (argumentIndex == HandlerArguments.Count - 1) {
                HandlerExecutor handlerExecutor = CreateExecutor();
                handlerExecutor.ArgumentForMethod[argumentIndex + 1] = entityNode.invokeArgument;
                handlerExecutors.Add(handlerExecutor);
                return true;
            }

            ArgumentNode argumentNode = invokeGraph.ArgumentNodes[argumentIndex + 1];

            List<EntityNode> list = !handlerArgument.Collection && !argumentNode.linkBreak
                                        ? entityNode.nextArgumentEntityNodes : argumentNode.entityNodes;

            int count = handlerExecutors.Count;
            bool flag = false;

            for (int i = 0; i < list.Count(); i++) {
                if (CollectExecutors(invokeGraph, list[i], argumentIndex + 1)) {
                    if (flag && !argumentNode.argument.Combine) {
                        throw new IllegalCombineException(Handler, argumentNode);
                    }

                    flag = true;
                }
            }

            for (int j = count; j < handlerExecutors.Count; j++) {
                handlerExecutors[j].ArgumentForMethod[argumentIndex + 1] = entityNode.invokeArgument;
            }

            return flag;
        }

        protected virtual HandlerExecutor CreateExecutor() {
            object[] argumentForMethod = new object[HandlerArguments.Count + 1];
            return new HandlerExecutor(Handler, argumentForMethod);
        }

        public virtual void Invoke(IList<HandlerInvokeData> otherInvokeArguments) {
            int count = handlerExecutors.Count;

            for (int i = 0; i < count; i++) {
                HandlerExecutor handlerExecutor = handlerExecutors[i];

                if (handlerVersion != Handler.Version) {
                    try {
                        CheckMethodArgumentsIsActual(handlerExecutor.ArgumentForMethod);
                    } catch (HandlerCallContextChangedException exception) {
                        if (Handler.Mandatory) {
                            string message = CollectHandlerCalls(otherInvokeArguments, this);
                            LoggerProvider.GetLogger<Flow>().Warn(message, exception);
                        }

                        continue;
                    }
                }

                handlerExecutors[i].Execute();
            }
        }

        void CheckMethodArgumentsIsActual(object[] args) {
            for (int i = 1; i < args.Length; i++) {
                object obj = args[i];
                Type type = obj.GetType();

                if (type == typeof(Node)) {
                    continue;
                }

                if (obj is Node) {
                    CheckNodeIsActual((Node)obj);
                } else if (obj is ICollection) {
                    IEnumerator enumerator = (obj as ICollection).GetEnumerator();

                    while (enumerator.MoveNext()) {
                        CheckNodeIsActual((Node)enumerator.Current);
                    }
                }
            }
        }

        void CheckNodeIsActual(Node node) {
            NodeClassInstanceDescription orCreateNodeClassDescription =
                NodeDescriptionRegistry.GetOrCreateNodeClassDescription(node.GetType());

            if (!((EntityImpl)node.Entity).NodeDescriptionStorage.Contains(orCreateNodeClassDescription.NodeDescription)) {
                throw new HandlerCallContextChangedException(Handler, orCreateNodeClassDescription, node.Entity);
            }
        }

        static string CollectHandlerCalls(IList<HandlerInvokeData> invokeArguments, HandlerInvokeData handlerInvokeData) {
            string text = "Handlers before: \n";

            foreach (HandlerInvokeData invokeArgument in invokeArguments) {
                if (invokeArgument == handlerInvokeData) {
                    break;
                }

                string text2 = text;
                text = string.Concat(text2, "\t", invokeArgument.Handler, "\n");
            }

            return text;
        }
    }
}