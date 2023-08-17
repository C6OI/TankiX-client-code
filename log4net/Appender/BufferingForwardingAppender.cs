using System;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender {
    public class BufferingForwardingAppender : BufferingAppenderSkeleton, IAppenderAttachable {
        AppenderAttachedImpl m_appenderAttachedImpl;

        public virtual AppenderCollection Appenders {
            get {
                lock (this) {
                    if (m_appenderAttachedImpl == null) {
                        return AppenderCollection.EmptyCollection;
                    }

                    return m_appenderAttachedImpl.Appenders;
                }
            }
        }

        public virtual void AddAppender(IAppender newAppender) {
            if (newAppender == null) {
                throw new ArgumentNullException("newAppender");
            }

            lock (this) {
                if (m_appenderAttachedImpl == null) {
                    m_appenderAttachedImpl = new AppenderAttachedImpl();
                }

                m_appenderAttachedImpl.AddAppender(newAppender);
            }
        }

        public virtual IAppender GetAppender(string name) {
            lock (this) {
                if (m_appenderAttachedImpl == null || name == null) {
                    return null;
                }

                return m_appenderAttachedImpl.GetAppender(name);
            }
        }

        public virtual void RemoveAllAppenders() {
            lock (this) {
                if (m_appenderAttachedImpl != null) {
                    m_appenderAttachedImpl.RemoveAllAppenders();
                    m_appenderAttachedImpl = null;
                }
            }
        }

        public virtual IAppender RemoveAppender(IAppender appender) {
            lock (this) {
                if (appender != null && m_appenderAttachedImpl != null) {
                    return m_appenderAttachedImpl.RemoveAppender(appender);
                }
            }

            return null;
        }

        public virtual IAppender RemoveAppender(string name) {
            lock (this) {
                if (name != null && m_appenderAttachedImpl != null) {
                    return m_appenderAttachedImpl.RemoveAppender(name);
                }
            }

            return null;
        }

        protected override void OnClose() {
            lock (this) {
                base.OnClose();

                if (m_appenderAttachedImpl != null) {
                    m_appenderAttachedImpl.RemoveAllAppenders();
                }
            }
        }

        protected override void SendBuffer(LoggingEvent[] events) {
            if (m_appenderAttachedImpl != null) {
                m_appenderAttachedImpl.AppendLoopOnAppenders(events);
            }
        }
    }
}