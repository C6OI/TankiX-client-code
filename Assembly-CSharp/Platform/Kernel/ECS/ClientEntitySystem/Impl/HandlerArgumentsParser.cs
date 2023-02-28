using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class HandlerArgumentsParser {
        readonly MethodInfo method;

        readonly Type[] NodeChangeEventTypes = new Type[2] {
            typeof(NodeAddedEvent),
            typeof(NodeRemoveEvent)
        };

        public HandlerArgumentsParser(MethodInfo method) => this.method = method;

        [Inject] public static NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }

        public HandlerArgumentsDescription Parse() {
            HashSet<Type> hashSet = ParseEvents();
            List<HandlerArgument> handlerArguments = ParseHandlerArguments(IsNodeChangeHandler(hashSet));
            HashSet<Type> additionalComponentClasses = ParseComponents(handlerArguments);
            return new HandlerArgumentsDescription(handlerArguments, hashSet, additionalComponentClasses);
        }

        bool IsNodeChangeHandler(IEnumerable<Type> eventClasses) {
            return eventClasses.Any(e => NodeChangeEventTypes.Contains(e));
        }

        List<HandlerArgument> ParseHandlerArguments(bool isNodeChangeHandler) {
            ParameterInfo[] parameters = method.GetParameters();
            HandlerArgument[] array = new HandlerArgument[parameters.Length - 1];
            int num = 0;
            Optional<JoinType> rightJoin = Optional<JoinType>.empty();

            for (int num2 = parameters.Length - 1; num2 > 0; num2--) {
                num = num2 - 1;
                ParameterInfo parameterInfo = parameters[num2];
                object[] customAttributes = parameterInfo.GetCustomAttributes(true);
                Optional<JoinType> joinType = GetJoinType(customAttributes);
                HandlerArgument handlerArgument = CreateNodeType(num, parameterInfo.ParameterType, joinType, rightJoin, customAttributes, isNodeChangeHandler);
                rightJoin = joinType;

                if (handlerArgument == null) {
                    throw new ArgumentMustBeNodeException(method, parameterInfo);
                }

                array[num] = handlerArgument;
            }

            CheckArguments(array);
            return array.ToList();
        }

        static HandlerArgument CreateNodeType(int position, Type type, Optional<JoinType> join, Optional<JoinType> rightJoin, object[] annotatedTypes, bool isNodeChangeHandler) {
            Type nodeType = GetNodeType(type);

            if (nodeType == null) {
                return null;
            }

            HashSet<Type> hashSet = new();
            bool flag = IsContextNode(annotatedTypes, join);

            if (isNodeChangeHandler && flag) {
                CollectGroupComponent(join, hashSet);
                CollectGroupComponent(rightJoin, hashSet);
            }

            NodeClassInstanceDescription orCreateNodeClassDescription = NodeDescriptionRegistry.GetOrCreateNodeClassDescription(nodeType, hashSet);

            return new HandlerArgumentBuilder().SetPosition(position).SetType(type).SetJoinType(join)
                .SetContext(flag)
                .SetCollection(IsCollection(type))
                .SetNodeClassInstanceDescription(orCreateNodeClassDescription)
                .SetMandatory(IsMandatory(annotatedTypes))
                .SetCombine(IsCombine(annotatedTypes))
                .SetOptional(IsOptional(type))
                .Build();
        }

        static void CollectGroupComponent(Optional<JoinType> join, HashSet<Type> components) {
            if (join.IsPresent()) {
                Optional<Type> contextComponent = join.Get().ContextComponent;

                if (contextComponent.IsPresent()) {
                    components.Add(contextComponent.Get());
                }
            }
        }

        HashSet<Type> ParseEvents() => ParseClasses(typeof(Event));

        static Type GetNodeType(Type type) {
            if (IsOptional(type)) {
                return GetNodeType(type.GetGenericArguments()[0]);
            }

            if (IsNode(type)) {
                return type;
            }

            if (IsCollection(type)) {
                Type type2 = type.GetGenericArguments()[0];

                if (IsNode(type2)) {
                    return type2;
                }

                if (type2.IsSubclassOf(typeof(AbstractSingleNode))) {
                    Type type3 = type2.GetGenericArguments()[0];

                    if (IsNode(type3)) {
                        return type3;
                    }
                }
            }

            return null;
        }

        static Optional<JoinType> GetJoinType(object[] annotatedTypes) {
            List<object> list = new();

            foreach (object obj in annotatedTypes) {
                list.Add(obj);
                object[] customAttributes = obj.GetType().GetCustomAttributes(true);
                list.AddRange(customAttributes);
            }

            foreach (object item in list) {
                if (item is JoinAll) {
                    return Optional<JoinType>.of(new JoinAllType());
                }

                if (item is JoinBy) {
                    JoinBy joinBy = (JoinBy)item;
                    return Optional<JoinType>.of(new JoinByType(joinBy.value));
                }

                if (item is JoinSelf) {
                    return Optional<JoinType>.of(new JoinSelfType());
                }
            }

            return Optional<JoinType>.empty();
        }

        void CheckArguments(IList<HandlerArgument> arguments) {
            CheckFirstNotJoin(arguments);
        }

        void CheckFirstNotJoin(IList<HandlerArgument> arguments) {
            if (arguments.Count > 0) {
                HandlerArgument handlerArgument = arguments[0];

                if (handlerArgument.JoinType.IsPresent() && handlerArgument.JoinType.Get().ContextComponent.IsPresent()) {
                    throw new JoinFirstNodeArgumentException(method, handlerArgument);
                }
            }
        }

        HashSet<Type> ParseComponents(IList<HandlerArgument> handlerArguments) {
            HashSet<Type> s = ParseClasses(typeof(Component));
            HashSet<Type> componentsFromNodes = GetComponentsFromNodes(handlerArguments);
            return Concat(s, componentsFromNodes);
        }

        static HashSet<Type> GetComponentsFromNodes(IList<HandlerArgument> handlerArguments) {
            HashSet<Type> hashSet = new();

            foreach (HandlerArgument handlerArgument in handlerArguments) {
                NodeDescription nodeDescription = handlerArgument.NodeDescription;

                foreach (Type component in nodeDescription.Components) {
                    hashSet.Add(component);
                }

                foreach (Type notComponent in nodeDescription.NotComponents) {
                    hashSet.Add(notComponent);
                }
            }

            return hashSet;
        }

        HashSet<Type> ParseClasses(Type clazz) {
            HashSet<Type> hashSet = new();
            ParameterInfo[] parameters = method.GetParameters();
            ParameterInfo[] array = parameters;

            foreach (ParameterInfo parameterInfo in array) {
                hashSet.Add(parameterInfo.ParameterType);
                Type[] genericArguments = parameterInfo.ParameterType.GetGenericArguments();
                Type[] array2 = genericArguments;

                foreach (Type item in array2) {
                    hashSet.Add(item);
                }
            }

            HashSet<Type> hashSet2 = new();

            foreach (Type item2 in hashSet) {
                if (item2.IsSubclassOf(clazz)) {
                    hashSet2.Add(item2);
                }
            }

            return hashSet2;
        }

        static bool IsContextNode(object[] annotatedTypes, Optional<JoinType> joinType) {
            if (HasAttrubte(annotatedTypes, typeof(Context))) {
                return true;
            }

            return !joinType.IsPresent();
        }

        static bool IsCollection(Type type) {
            if (IsOptional(type)) {
                return IsCollection(type.GetGenericArguments()[0]);
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>)) {
                return true;
            }

            return false;
        }

        static bool IsNode(Type type) => type.IsSubclassOf(typeof(Node)) || type == typeof(Node);

        static bool IsMandatory(object[] annotatedTypes) {
            if (TestContext.IsTestMode && TestContext.Current.IsDataExists(typeof(MandatoryDisabled))) {
                return false;
            }

            return HasAttrubte(annotatedTypes, typeof(Mandatory));
        }

        static bool IsCombine(object[] annotatedTypes) => HasAttrubte(annotatedTypes, typeof(Combine));

        static bool HasAttrubte(object[] attrubtes, Type type) {
            foreach (object obj in attrubtes) {
                if (obj.GetType() == type) {
                    return true;
                }
            }

            return false;
        }

        static bool IsOptional(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>);

        static HashSet<Type> Concat(HashSet<Type> s1, HashSet<Type> s2) {
            HashSet<Type> hashSet = new(s1);

            foreach (Type item in s2) {
                hashSet.Add(item);
            }

            return hashSet;
        }
    }
}