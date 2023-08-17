using System;

namespace log4net.Core {
    public class TimeEvaluator : ITriggeringEventEvaluator {
        const int DEFAULT_INTERVAL = 0;

        DateTime m_lasttime;

        public TimeEvaluator()
            : this(0) { }

        public TimeEvaluator(int interval) {
            Interval = interval;
            m_lasttime = DateTime.Now;
        }

        public int Interval { get; set; }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (Interval == 0) {
                return false;
            }

            lock (this) {
                if (DateTime.Now.Subtract(m_lasttime).TotalSeconds > Interval) {
                    m_lasttime = DateTime.Now;
                    return true;
                }

                return false;
            }
        }
    }
}