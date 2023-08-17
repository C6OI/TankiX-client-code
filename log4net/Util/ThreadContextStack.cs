using System;
using System.Collections;
using log4net.Core;

namespace log4net.Util {
    public sealed class ThreadContextStack : IFixingRequired {
        internal ThreadContextStack() { }

        public int Count => InternalStack.Count;

        internal Stack InternalStack { get; set; } = new();

        object IFixingRequired.GetFixedObject() => GetFullMessage();

        public void Clear() => InternalStack.Clear();

        public string Pop() {
            Stack stack = InternalStack;

            if (stack.Count > 0) {
                return ((StackFrame)stack.Pop()).Message;
            }

            return string.Empty;
        }

        public IDisposable Push(string message) {
            Stack stack = InternalStack;
            stack.Push(new StackFrame(message, stack.Count <= 0 ? null : (StackFrame)stack.Peek()));
            return new AutoPopStackFrame(stack, stack.Count - 1);
        }

        internal string GetFullMessage() {
            Stack stack = InternalStack;

            if (stack.Count > 0) {
                return ((StackFrame)stack.Peek()).FullMessage;
            }

            return null;
        }

        public override string ToString() => GetFullMessage();

        sealed class StackFrame {
            readonly StackFrame m_parent;

            string m_fullMessage;

            internal StackFrame(string message, StackFrame parent) {
                Message = message;
                m_parent = parent;

                if (parent == null) {
                    m_fullMessage = message;
                }
            }

            internal string Message { get; }

            internal string FullMessage {
                get {
                    if (m_fullMessage == null && m_parent != null) {
                        m_fullMessage = m_parent.FullMessage + " " + Message;
                    }

                    return m_fullMessage;
                }
            }
        }

        struct AutoPopStackFrame : IDisposable {
            readonly Stack m_frameStack;

            readonly int m_frameDepth;

            internal AutoPopStackFrame(Stack frameStack, int frameDepth) {
                m_frameStack = frameStack;
                m_frameDepth = frameDepth;
            }

            public void Dispose() {
                if (m_frameDepth >= 0 && m_frameStack != null) {
                    while (m_frameStack.Count > m_frameDepth) {
                        m_frameStack.Pop();
                    }
                }
            }
        }
    }
}