using System;
using System.Collections;
using System.Reflection;
using log4net.Util;

namespace log4net.Core {
    [Serializable]
    public class MethodItem {
        const string NA = "?";

        static readonly Type declaringType = typeof(MethodItem);

        public MethodItem() {
            Name = "?";
            Parameters = new string[0];
        }

        public MethodItem(string name)
            : this() => Name = name;

        public MethodItem(string name, string[] parameters)
            : this(name) => Parameters = parameters;

        public MethodItem(MethodBase methodBase)
            : this(methodBase.Name, GetMethodParameterNames(methodBase)) { }

        public string Name { get; }

        public string[] Parameters { get; }

        static string[] GetMethodParameterNames(MethodBase methodBase) {
            ArrayList arrayList = new();

            try {
                ParameterInfo[] parameters = methodBase.GetParameters();
                int upperBound = parameters.GetUpperBound(0);

                for (int i = 0; i <= upperBound; i++) {
                    arrayList.Add(string.Concat(parameters[i].ParameterType, " ", parameters[i].Name));
                }
            } catch (Exception exception) {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method parameters.", exception);
            }

            return (string[])arrayList.ToArray(typeof(string));
        }
    }
}