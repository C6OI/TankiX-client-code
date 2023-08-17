using System;
using System.Collections;
using log4net.Core;

namespace log4net.Appender {
    public class MemoryAppender : AppenderSkeleton {
        protected ArrayList m_eventsList;

        protected FixFlags m_fixFlags = FixFlags.All;

        public MemoryAppender() => m_eventsList = new ArrayList();

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

        public virtual FixFlags Fix {
            get => m_fixFlags;
            set => m_fixFlags = value;
        }

        public virtual LoggingEvent[] GetEvents() {
            lock (m_eventsList.SyncRoot) {
                return (LoggingEvent[])m_eventsList.ToArray(typeof(LoggingEvent));
            }
        }

        protected override void Append(LoggingEvent loggingEvent) {
            loggingEvent.Fix = Fix;

            lock (m_eventsList.SyncRoot) {
                m_eventsList.Add(loggingEvent);
            }
        }

        public virtual void Clear() {
            lock (m_eventsList.SyncRoot) {
                m_eventsList.Clear();
            }
        }
    }
}