using System;
using log4net.Appender;
using log4net.Core;

namespace log4net.Util {
    public class AppenderAttachedImpl : IAppenderAttachable {
        static readonly Type declaringType = typeof(AppenderAttachedImpl);

        IAppender[] m_appenderArray;
        AppenderCollection m_appenderList;

        public AppenderCollection Appenders {
            get {
                if (m_appenderList == null) {
                    return AppenderCollection.EmptyCollection;
                }

                return AppenderCollection.ReadOnly(m_appenderList);
            }
        }

        public void AddAppender(IAppender newAppender) {
            if (newAppender == null) {
                throw new ArgumentNullException("newAppender");
            }

            m_appenderArray = null;

            if (m_appenderList == null) {
                m_appenderList = new AppenderCollection(1);
            }

            if (!m_appenderList.Contains(newAppender)) {
                m_appenderList.Add(newAppender);
            }
        }

        public IAppender GetAppender(string name) {
            if (m_appenderList != null && name != null) {
                AppenderCollection.IAppenderCollectionEnumerator enumerator = m_appenderList.GetEnumerator();

                try {
                    while (enumerator.MoveNext()) {
                        IAppender current = enumerator.Current;

                        if (name == current.Name) {
                            return current;
                        }
                    }
                } finally {
                    IDisposable disposable = enumerator as IDisposable;

                    if (disposable != null) {
                        disposable.Dispose();
                    }
                }
            }

            return null;
        }

        public void RemoveAllAppenders() {
            if (m_appenderList == null) {
                return;
            }

            AppenderCollection.IAppenderCollectionEnumerator enumerator = m_appenderList.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    IAppender current = enumerator.Current;

                    try {
                        current.Close();
                    } catch (Exception exception) {
                        LogLog.Error(declaringType, "Failed to Close appender [" + current.Name + "]", exception);
                    }
                }
            } finally {
                IDisposable disposable = enumerator as IDisposable;

                if (disposable != null) {
                    disposable.Dispose();
                }
            }

            m_appenderList = null;
            m_appenderArray = null;
        }

        public IAppender RemoveAppender(IAppender appender) {
            if (appender != null && m_appenderList != null) {
                m_appenderList.Remove(appender);

                if (m_appenderList.Count == 0) {
                    m_appenderList = null;
                }

                m_appenderArray = null;
            }

            return appender;
        }

        public IAppender RemoveAppender(string name) => RemoveAppender(GetAppender(name));

        public int AppendLoopOnAppenders(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (m_appenderList == null) {
                return 0;
            }

            if (m_appenderArray == null) {
                m_appenderArray = m_appenderList.ToArray();
            }

            IAppender[] appenderArray = m_appenderArray;

            foreach (IAppender appender in appenderArray) {
                try {
                    appender.DoAppend(loggingEvent);
                } catch (Exception exception) {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", exception);
                }
            }

            return m_appenderList.Count;
        }

        public int AppendLoopOnAppenders(LoggingEvent[] loggingEvents) {
            if (loggingEvents == null) {
                throw new ArgumentNullException("loggingEvents");
            }

            if (loggingEvents.Length == 0) {
                throw new ArgumentException("loggingEvents array must not be empty", "loggingEvents");
            }

            if (loggingEvents.Length == 1) {
                return AppendLoopOnAppenders(loggingEvents[0]);
            }

            if (m_appenderList == null) {
                return 0;
            }

            if (m_appenderArray == null) {
                m_appenderArray = m_appenderList.ToArray();
            }

            IAppender[] appenderArray = m_appenderArray;

            foreach (IAppender appender in appenderArray) {
                try {
                    CallAppend(appender, loggingEvents);
                } catch (Exception exception) {
                    LogLog.Error(declaringType, "Failed to append to appender [" + appender.Name + "]", exception);
                }
            }

            return m_appenderList.Count;
        }

        static void CallAppend(IAppender appender, LoggingEvent[] loggingEvents) {
            IBulkAppender bulkAppender = appender as IBulkAppender;

            if (bulkAppender != null) {
                bulkAppender.DoAppend(loggingEvents);
                return;
            }

            foreach (LoggingEvent loggingEvent in loggingEvents) {
                appender.DoAppend(loggingEvent);
            }
        }
    }
}