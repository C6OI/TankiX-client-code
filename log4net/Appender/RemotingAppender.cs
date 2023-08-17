using System;
using System.Collections;
using System.Threading;
using log4net.Core;

namespace log4net.Appender {
    public class RemotingAppender : BufferingAppenderSkeleton {
        readonly ManualResetEvent m_workQueueEmptyEvent = new(true);
        int m_queuedCallbackCount;

        IRemoteLoggingSink m_sinkObj;

        public string Sink { get; set; }

        public override void ActivateOptions() {
            base.ActivateOptions();
            IDictionary dictionary = new Hashtable();
            dictionary["typeFilterLevel"] = "Full";
            m_sinkObj = (IRemoteLoggingSink)Activator.GetObject(typeof(IRemoteLoggingSink), m_sinkUrl, dictionary);
        }

        protected override void SendBuffer(LoggingEvent[] events) {
            BeginAsyncSend();

            if (!ThreadPool.QueueUserWorkItem(SendBufferCallback, events)) {
                EndAsyncSend();

                ErrorHandler.Error("RemotingAppender [" +
                                   Name +
                                   "] failed to ThreadPool.QueueUserWorkItem logging events in SendBuffer.");
            }
        }

        protected override void OnClose() {
            base.OnClose();

            if (!m_workQueueEmptyEvent.WaitOne(30000, false)) {
                ErrorHandler.Error("RemotingAppender [" +
                                   Name +
                                   "] failed to send all queued events before close, in OnClose.");
            }
        }

        void BeginAsyncSend() {
            m_workQueueEmptyEvent.Reset();
            Interlocked.Increment(ref m_queuedCallbackCount);
        }

        void EndAsyncSend() {
            if (Interlocked.Decrement(ref m_queuedCallbackCount) <= 0) {
                m_workQueueEmptyEvent.Set();
            }
        }

        void SendBufferCallback(object state) {
            try {
                LoggingEvent[] events = (LoggingEvent[])state;
                m_sinkObj.LogEvents(events);
            } catch (Exception e) {
                ErrorHandler.Error("Failed in SendBufferCallback", e);
            } finally {
                EndAsyncSend();
            }
        }

        public interface IRemoteLoggingSink {
            void LogEvents(LoggingEvent[] events);
        }
    }
}