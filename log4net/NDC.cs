using System;
using System.Collections;
using log4net.Util;

namespace log4net {
    public sealed class NDC {
        NDC() { }

        public static int Depth => ThreadContext.Stacks["NDC"].Count;

        public static void Clear() => ThreadContext.Stacks["NDC"].Clear();

        public static Stack CloneStack() => ThreadContext.Stacks["NDC"].InternalStack;

        public static void Inherit(Stack stack) => ThreadContext.Stacks["NDC"].InternalStack = stack;

        public static string Pop() => ThreadContext.Stacks["NDC"].Pop();

        public static IDisposable Push(string message) => ThreadContext.Stacks["NDC"].Push(message);

        public static void Remove() { }

        public static void SetMaxDepth(int maxDepth) {
            if (maxDepth < 0) {
                return;
            }

            ThreadContextStack threadContextStack = ThreadContext.Stacks["NDC"];

            if (maxDepth == 0) {
                threadContextStack.Clear();
                return;
            }

            while (threadContextStack.Count > maxDepth) {
                threadContextStack.Pop();
            }
        }
    }
}