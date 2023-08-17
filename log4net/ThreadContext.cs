using log4net.Util;

namespace log4net {
    public sealed class ThreadContext {
        ThreadContext() { }

        public static ThreadContextProperties Properties { get; } = new();

        public static ThreadContextStacks Stacks { get; } = new(Properties);
    }
}