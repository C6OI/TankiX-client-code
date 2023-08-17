using System;
using System.Reflection;
using log4net.Repository;

namespace log4net.Config {
    [AttributeUsage(AttributeTargets.Assembly)]
    public abstract class ConfiguratorAttribute : Attribute, IComparable {
        readonly int m_priority;

        protected ConfiguratorAttribute(int priority) => m_priority = priority;

        public int CompareTo(object obj) {
            if (this == obj) {
                return 0;
            }

            int num = -1;
            ConfiguratorAttribute configuratorAttribute = obj as ConfiguratorAttribute;

            if (configuratorAttribute != null) {
                num = configuratorAttribute.m_priority.CompareTo(m_priority);

                if (num == 0) {
                    num = -1;
                }
            }

            return num;
        }

        public abstract void Configure(Assembly sourceAssembly, ILoggerRepository targetRepository);
    }
}