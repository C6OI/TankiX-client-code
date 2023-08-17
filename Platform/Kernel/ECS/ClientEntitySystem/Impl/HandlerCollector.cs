using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerCollector {
        readonly IList<HandlerFactory> factories = new List<HandlerFactory>();

        readonly ICollection<EngineHandlerRegistrationListener> handlerListeners =
            new List<EngineHandlerRegistrationListener>();

        readonly IDictionary<Type, IDictionary<NodeDescription, List<Handler>>> handlersByContextNode =
            new Dictionary<Type, IDictionary<NodeDescription, List<Handler>>>();

        readonly IDictionary<Type, IDictionary<Type, List<Handler>>> handlersByEvent =
            new Dictionary<Type, IDictionary<Type, List<Handler>>>();

        readonly IDictionary<NodeDescription, List<Handler>> handlersByNode =
            new Dictionary<NodeDescription, List<Handler>>();

        [Inject] public static FlowInstancesCache Cache { get; set; }

        protected virtual Type InheritableEventLimit => typeof(Event);

        public void RegisterHandlerFactory(HandlerFactory factory) => factories.Add(factory);

        public void RegisterTasksForHandler(Type handlerType) {
            handlersByEvent[handlerType] = new Dictionary<Type, List<Handler>>();
            handlersByContextNode[handlerType] = new Dictionary<NodeDescription, List<Handler>>();
        }

        public ICollection<Handler> CollectHandlers(ECSSystem system) {
            ICollection<Handler> collection = new List<Handler>();

            for (Type type = system.GetType(); type != typeof(ECSSystem); type = type.BaseType) {
                MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly |
                                                       BindingFlags.Instance |
                                                       BindingFlags.Static |
                                                       BindingFlags.Public |
                                                       BindingFlags.NonPublic);

                MethodInfo[] array = methods;

                foreach (MethodInfo methodInfo in array) {
                    if (!AddHandlerIfNeed(methodInfo, system, collection)) {
                        CheckMethodIsNotHandler(methodInfo);
                    }
                }
            }

            return collection;
        }

        bool AddHandlerIfNeed(MethodInfo declaredMethod, ECSSystem system, ICollection<Handler> systemHandler) {
            foreach (HandlerFactory factory in factories) {
                Handler handler = factory.CreateHandler(declaredMethod, system);

                if (handler == null) {
                    continue;
                }

                if (!declaredMethod.IsPublic) {
                    throw new PrivateHandlerFoundException(declaredMethod);
                }

                foreach (EngineHandlerRegistrationListener handlerListener in handlerListeners) {
                    handlerListener.OnHandlerAdded(handler);
                }

                Register(handler);
                systemHandler.Add(handler);
                return true;
            }

            return false;
        }

        void CheckMethodIsNotHandler(MethodInfo method) {
            if (method.GetParameters().Length == 0 ||
                !method.IsPublic ||
                !method.GetParameters()[0].ParameterType.IsSubclassOf(typeof(Event))) {
                return;
            }

            throw new MissingHandlerAnnotationException(method);
        }

        void Register(Handler handler) {
            RegisterByEvent(handler);
            RegisterByContextNode(handler);
            RegisterByNode(handler);
        }

        void RegisterByEvent(Handler handler) {
            IDictionary<Type, List<Handler>> dictionary = handlersByEvent[handler.GetType()];
            ICollection<Handler> collection = dictionary.ComputeIfAbsent(handler.EventType, t => new List<Handler>());
            collection.Add(handler);
        }

        void RegisterByContextNode(Handler handler) {
            IDictionary<NodeDescription, List<Handler>> handlersByTask = handlersByContextNode[handler.GetType()];
            HashSet<NodeDescription> nodes = new();

            handler.ContextArguments.ForEach(delegate(HandlerArgument a) {
                nodes.Add(a.NodeDescription);
            });

            nodes.ForEach(delegate(NodeDescription desc) {
                ICollection<Handler> collection = handlersByTask.ComputeIfAbsent(desc, t => new List<Handler>());
                collection.Add(handler);
            });
        }

        void RegisterByNode(Handler handler) {
            HashSet<NodeDescription> nodes = new();

            handler.HandlerArgumentsDescription.HandlerArguments.ForEach(delegate(HandlerArgument a) {
                nodes.Add(a.NodeDescription);
            });

            nodes.ForEach(delegate(NodeDescription desc) {
                ICollection<Handler> collection = handlersByNode.ComputeIfAbsent(desc, t => new List<Handler>());
                collection.Add(handler);
            });
        }

        public void AddHandlerListener(EngineHandlerRegistrationListener listener) {
            handlerListeners.Add(listener);

            handlersByEvent.Values.ForEach(delegate(IDictionary<Type, List<Handler>> m) {
                m.Values.ForEach(delegate(List<Handler> h) {
                    h.ForEach(listener.OnHandlerAdded);
                });
            });
        }

        public ICollection<Handler> GetHandlers(Type handlerType, Type eventType) {
            if (IsNotInheritableEvent(eventType)) {
                return Get(handlerType, eventType);
            }

            IList<Type> inheritableEventTypes = GetInheritableEventTypes(eventType);
            List<Handler> list = new();
            int count = inheritableEventTypes.Count;

            for (int i = 0; i < count; i++) {
                list.AddRange(Get(handlerType, inheritableEventTypes[i]));
            }

            return list;
        }

        bool IsNotInheritableEvent(Type eventType) {
            if (!eventType.IsGenericTypeDefinition && eventType.BaseType == InheritableEventLimit) {
                return true;
            }

            return false;
        }

        ICollection<Handler> Get(Type handlerType, Type eventType) {
            List<Handler> value;
            IList<Handler> result;

            if (handlersByEvent[handlerType].TryGetValue(eventType, out value)) {
                IList<Handler> list = value;
                result = list;
            } else {
                result = Collections.EmptyList<Handler>();
            }

            return result;
        }

        IList<Type> GetInheritableEventTypes(Type eventType) {
            if (!eventType.IsGenericTypeDefinition) {
                List<Type> instance = Cache.listType.GetInstance();
                return ClassUtils.GetClasses(eventType, InheritableEventLimit, instance);
            }

            throw new InvalidOperationException();
        }

        public ICollection<Handler> GetHandlers(Type handlerType, NodeDescription nodeDescription) {
            List<Handler> value;
            IList<Handler> result;

            if (handlersByContextNode[handlerType].TryGetValue(nodeDescription, out value)) {
                IList<Handler> list = value;
                result = list;
            } else {
                result = Collections.EmptyList<Handler>();
            }

            return result;
        }

        public ICollection<Handler> GetHandlersWithoutContext(NodeDescription nodeDescription) {
            List<Handler> value;
            IList<Handler> result;

            if (handlersByNode.TryGetValue(nodeDescription, out value)) {
                IList<Handler> list = value;
                result = list;
            } else {
                result = Collections.EmptyList<Handler>();
            }

            return result;
        }

        public void SortHandlers() {
            handlersByEvent.Values.ForEach(delegate(IDictionary<Type, List<Handler>> m) {
                m.Values.ForEach(delegate(List<Handler> c) {
                    c.Sort();
                });
            });

            handlersByContextNode.Values.ForEach(delegate(IDictionary<NodeDescription, List<Handler>> m) {
                m.Values.ForEach(delegate(List<Handler> c) {
                    c.Sort();
                });
            });
        }
    }
}