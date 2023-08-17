using System;
using System.Collections;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender {
    public abstract class BufferingAppenderSkeleton : AppenderSkeleton {
        const int DEFAULT_BUFFER_SIZE = 512;

        readonly bool m_eventMustBeFixed;

        CyclicBuffer m_cb;

        protected BufferingAppenderSkeleton()
            : this(true) { }

        protected BufferingAppenderSkeleton(bool eventMustBeFixed) => m_eventMustBeFixed = eventMustBeFixed;

        public bool Lossy { get; set; }

        public int BufferSize { get; set; } = 512;

        public ITriggeringEventEvaluator Evaluator { get; set; }

        public ITriggeringEventEvaluator LossyEvaluator { get; set; }

        [Obsolete("Use Fix property")] public virtual bool OnlyFixPartialEventData {
            get => Fix == FixFlags.Partial;
            set {
                if (value) {
                    Fix = FixFlags.Partial;
                } else {
                    Fix = FixFlags.All;
                }
            }
        }

        public virtual FixFlags Fix { get; set; } = FixFlags.All;

        public virtual void Flush() => Flush(false);

        public virtual void Flush(bool flushLossyBuffer) {
            lock (this) {
                if (m_cb == null || m_cb.Length <= 0) {
                    return;
                }

                if (Lossy) {
                    if (!flushLossyBuffer) {
                        return;
                    }

                    if (LossyEvaluator != null) {
                        LoggingEvent[] array = m_cb.PopAll();
                        ArrayList arrayList = new(array.Length);
                        LoggingEvent[] array2 = array;

                        foreach (LoggingEvent loggingEvent in array2) {
                            if (LossyEvaluator.IsTriggeringEvent(loggingEvent)) {
                                arrayList.Add(loggingEvent);
                            }
                        }

                        if (arrayList.Count > 0) {
                            SendBuffer((LoggingEvent[])arrayList.ToArray(typeof(LoggingEvent)));
                        }
                    } else {
                        m_cb.Clear();
                    }
                } else {
                    SendFromBuffer(null, m_cb);
                }
            }
        }

        public override void ActivateOptions() {
            base.ActivateOptions();

            if (Lossy && Evaluator == null) {
                ErrorHandler.Error("Appender [" + Name + "] is Lossy but has no Evaluator. The buffer will never be sent!");
            }

            if (BufferSize > 1) {
                m_cb = new CyclicBuffer(BufferSize);
            } else {
                m_cb = null;
            }
        }

        protected override void OnClose() => Flush(true);

        protected override void Append(LoggingEvent loggingEvent) {
            if (m_cb == null || BufferSize <= 1) {
                if (!Lossy ||
                    Evaluator != null && Evaluator.IsTriggeringEvent(loggingEvent) ||
                    LossyEvaluator != null && LossyEvaluator.IsTriggeringEvent(loggingEvent)) {
                    if (m_eventMustBeFixed) {
                        loggingEvent.Fix = Fix;
                    }

                    SendBuffer(new LoggingEvent[1] { loggingEvent });
                }

                return;
            }

            loggingEvent.Fix = Fix;
            LoggingEvent loggingEvent2 = m_cb.Append(loggingEvent);

            if (loggingEvent2 != null) {
                if (!Lossy) {
                    SendFromBuffer(loggingEvent2, m_cb);
                    return;
                }

                if (LossyEvaluator == null || !LossyEvaluator.IsTriggeringEvent(loggingEvent2)) {
                    loggingEvent2 = null;
                }

                if (Evaluator != null && Evaluator.IsTriggeringEvent(loggingEvent)) {
                    SendFromBuffer(loggingEvent2, m_cb);
                } else if (loggingEvent2 != null) {
                    SendBuffer(new LoggingEvent[1] { loggingEvent2 });
                }
            } else if (Evaluator != null && Evaluator.IsTriggeringEvent(loggingEvent)) {
                SendFromBuffer(null, m_cb);
            }
        }

        protected virtual void SendFromBuffer(LoggingEvent firstLoggingEvent, CyclicBuffer buffer) {
            LoggingEvent[] array = buffer.PopAll();

            if (firstLoggingEvent == null) {
                SendBuffer(array);
                return;
            }

            if (array.Length == 0) {
                SendBuffer(new LoggingEvent[1] { firstLoggingEvent });
                return;
            }

            LoggingEvent[] array2 = new LoggingEvent[array.Length + 1];
            Array.Copy(array, 0, array2, 1, array.Length);
            array2[0] = firstLoggingEvent;
            SendBuffer(array2);
        }

        protected abstract void SendBuffer(LoggingEvent[] events);
    }
}