using System;
using log4net.Core;

namespace log4net.Util {
    public class CyclicBuffer {
        readonly LoggingEvent[] m_events;

        readonly int m_maxSize;

        int m_first;

        int m_last;

        int m_numElems;

        public CyclicBuffer(int maxSize) {
            if (maxSize < 1) {
                throw SystemInfo.CreateArgumentOutOfRangeException("maxSize",
                    maxSize,
                    "Parameter: maxSize, Value: [" + maxSize + "] out of range. Non zero positive integer required");
            }

            m_maxSize = maxSize;
            m_events = new LoggingEvent[maxSize];
            m_first = 0;
            m_last = 0;
            m_numElems = 0;
        }

        public LoggingEvent this[int i] {
            get {
                lock (this) {
                    if (i < 0 || i >= m_numElems) {
                        return null;
                    }

                    return m_events[(m_first + i) % m_maxSize];
                }
            }
        }

        public int MaxSize {
            get {
                lock (this) {
                    return m_maxSize;
                }
            }
        }

        public int Length {
            get {
                lock (this) {
                    return m_numElems;
                }
            }
        }

        public LoggingEvent Append(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            lock (this) {
                LoggingEvent result = m_events[m_last];
                m_events[m_last] = loggingEvent;

                if (++m_last == m_maxSize) {
                    m_last = 0;
                }

                if (m_numElems < m_maxSize) {
                    m_numElems++;
                } else if (++m_first == m_maxSize) {
                    m_first = 0;
                }

                if (m_numElems < m_maxSize) {
                    return null;
                }

                return result;
            }
        }

        public LoggingEvent PopOldest() {
            lock (this) {
                LoggingEvent result = null;

                if (m_numElems > 0) {
                    m_numElems--;
                    result = m_events[m_first];
                    m_events[m_first] = null;

                    if (++m_first == m_maxSize) {
                        m_first = 0;
                    }
                }

                return result;
            }
        }

        public LoggingEvent[] PopAll() {
            lock (this) {
                LoggingEvent[] array = new LoggingEvent[m_numElems];

                if (m_numElems > 0) {
                    if (m_first < m_last) {
                        Array.Copy(m_events, m_first, array, 0, m_numElems);
                    } else {
                        Array.Copy(m_events, m_first, array, 0, m_maxSize - m_first);
                        Array.Copy(m_events, 0, array, m_maxSize - m_first, m_last);
                    }
                }

                Clear();
                return array;
            }
        }

        public void Clear() {
            lock (this) {
                Array.Clear(m_events, 0, m_events.Length);
                m_first = 0;
                m_last = 0;
                m_numElems = 0;
            }
        }
    }
}