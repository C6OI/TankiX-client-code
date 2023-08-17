using System;

namespace log4net.Util {
    public sealed class ThreadContextStacks {
        static readonly Type declaringType = typeof(ThreadContextStacks);
        readonly ContextPropertiesBase m_properties;

        internal ThreadContextStacks(ContextPropertiesBase properties) => m_properties = properties;

        public ThreadContextStack this[string key] {
            get {
                ThreadContextStack threadContextStack = null;
                object obj = m_properties[key];

                if (obj == null) {
                    threadContextStack = new ThreadContextStack();
                    m_properties[key] = threadContextStack;
                } else {
                    threadContextStack = obj as ThreadContextStack;

                    if (threadContextStack == null) {
                        string text = SystemInfo.NullText;

                        try {
                            text = obj.ToString();
                        } catch { }

                        LogLog.Error(declaringType,
                            "ThreadContextStacks: Request for stack named [" +
                            key +
                            "] failed because a property with the same name exists which is a [" +
                            obj.GetType().Name +
                            "] with value [" +
                            text +
                            "]");

                        threadContextStack = new ThreadContextStack();
                    }
                }

                return threadContextStack;
            }
        }
    }
}