using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class Handler : IComparable<Handler> {
        static int activeHandlersCount;

        readonly MethodHandle methodHandle;

        public HandlerInvokeGraph HandlerInvokeGraph;

        public Handler(EventPhase eventPhase, Type eventType, MethodInfo method, MethodHandle methodHandle,
            HandlerArgumentsDescription handlerArgumentsDescription) {
            EventPhase = eventPhase;
            EventType = eventType;
            Method = method;
            this.methodHandle = methodHandle;
            HandlerArgumentsDescription = handlerArgumentsDescription;
            Mandatory = IsMandatory(method);
            SkipInfo = IsSkipInfo(method);
            ProjectName = GetProjectName();
            Name = GetHandlerName();
            bool isContextOnlyArguments = true;
            ContextArguments = new List<HandlerArgument>();

            foreach (HandlerArgument handlerArgument in HandlerArgumentsDescription.HandlerArguments) {
                if (handlerArgument.Context) {
                    ContextArguments.Add(handlerArgument);
                }

                if (handlerArgument.JoinType.IsPresent()) {
                    isContextOnlyArguments = false;
                }
            }

            IsContextOnlyArguments = isContextOnlyArguments;
            IsEventOnlyArguments = HandlerArgumentsDescription.HandlerArguments.Count == 0;
            HandlerInvokeGraph = new HandlerInvokeGraph(this);
        }

        public int Version { get; private set; }

        public IList<HandlerArgument> ContextArguments { get; }

        public bool IsContextOnlyArguments { get; private set; }

        public bool IsEventOnlyArguments { get; private set; }

        public MethodInfo Method { get; internal set; }

        public HandlerArgumentsDescription HandlerArgumentsDescription { get; internal set; }

        public bool Mandatory { get; internal set; }

        public bool SkipInfo { get; internal set; }

        public EventPhase EventPhase { get; internal set; }

        public Type EventType { get; internal set; }

        public string ProjectName { get; internal set; }

        public string Name { get; internal set; }

        public int CompareTo(Handler other) => getKey().CompareTo(other.getKey());

        static bool IsMandatory(MethodInfo method) {
            if (TestContext.IsTestMode && TestContext.Current.IsDataExists(typeof(MandatoryDisabled))) {
                return false;
            }

            return method.GetCustomAttributes(typeof(Mandatory), true).Length == 1;
        }

        static bool IsSkipInfo(MethodInfo method) => method.GetCustomAttributes(typeof(SkipInfo), true).Length == 1;

        public object Invoke(object[] args) => methodHandle.Invoke(args);

        public void ChangeVersion() => Version = Math.Max(0, Version + 1);

        string GetProjectName() {
            Type declaringType = Method.DeclaringType;

            if (declaringType != null) {
                string @namespace = declaringType.Namespace;

                if (!string.IsNullOrEmpty(@namespace)) {
                    int num = @namespace.IndexOf('.');
                    return num < 0 ? @namespace : @namespace.Substring(0, num);
                }
            }

            return string.Empty;
        }

        public string GetHandlerName() {
            Type declaringType = Method.DeclaringType;

            if (declaringType != null) {
                return declaringType.Name + "." + Method.Name;
            }

            return Method.Name;
        }

        string getKey() => Method.ToString();

        public override string ToString() => string.Format("[{0} {1}.{2}]", GetType().Name, ProjectName, Name);

        class NodeDescriptionState {
            public int count;
        }
    }
}