using System;
using System.IO;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace log4net.Appender {
    public class TextWriterAppender : AppenderSkeleton {
        static readonly Type declaringType = typeof(TextWriterAppender);

        public TextWriterAppender() { }

        [Obsolete("Instead use the default constructor and set the Layout & Writer properties")]
        public TextWriterAppender(ILayout layout, Stream os)
            : this(layout, new StreamWriter(os)) { }

        [Obsolete("Instead use the default constructor and set the Layout & Writer properties")]
        public TextWriterAppender(ILayout layout, TextWriter writer) {
            Layout = layout;
            Writer = writer;
        }

        public bool ImmediateFlush { get; set; } = true;

        public virtual TextWriter Writer {
            get => QuietWriter;
            set {
                lock (this) {
                    Reset();

                    if (value != null) {
                        QuietWriter = new QuietTextWriter(value, ErrorHandler);
                        WriteHeader();
                    }
                }
            }
        }

        public override IErrorHandler ErrorHandler {
            get => base.ErrorHandler;
            set {
                lock (this) {
                    if (value == null) {
                        LogLog.Warn(declaringType, "TextWriterAppender: You have tried to set a null error-handler.");
                        return;
                    }

                    base.ErrorHandler = value;

                    if (QuietWriter != null) {
                        QuietWriter.ErrorHandler = value;
                    }
                }
            }
        }

        protected override bool RequiresLayout => true;

        protected QuietTextWriter QuietWriter { get; set; }

        protected override bool PreAppendCheck() {
            if (!base.PreAppendCheck()) {
                return false;
            }

            if (QuietWriter == null) {
                PrepareWriter();

                if (QuietWriter == null) {
                    ErrorHandler.Error("No output stream or file set for the appender named [" + Name + "].");
                    return false;
                }
            }

            if (QuietWriter.Closed) {
                ErrorHandler.Error("Output stream for appender named [" + Name + "] has been closed.");
                return false;
            }

            return true;
        }

        protected override void Append(LoggingEvent loggingEvent) {
            RenderLoggingEvent(QuietWriter, loggingEvent);

            if (ImmediateFlush) {
                QuietWriter.Flush();
            }
        }

        protected override void Append(LoggingEvent[] loggingEvents) {
            foreach (LoggingEvent loggingEvent in loggingEvents) {
                RenderLoggingEvent(QuietWriter, loggingEvent);
            }

            if (ImmediateFlush) {
                QuietWriter.Flush();
            }
        }

        protected override void OnClose() {
            lock (this) {
                Reset();
            }
        }

        protected virtual void WriteFooterAndCloseWriter() {
            WriteFooter();
            CloseWriter();
        }

        protected virtual void CloseWriter() {
            if (QuietWriter != null) {
                try {
                    QuietWriter.Close();
                } catch (Exception e) {
                    ErrorHandler.Error(string.Concat("Could not close writer [", QuietWriter, "]"), e);
                }
            }
        }

        protected virtual void Reset() {
            WriteFooterAndCloseWriter();
            QuietWriter = null;
        }

        protected virtual void WriteFooter() {
            if (Layout != null && QuietWriter != null && !QuietWriter.Closed) {
                string footer = Layout.Footer;

                if (footer != null) {
                    QuietWriter.Write(footer);
                }
            }
        }

        protected virtual void WriteHeader() {
            if (Layout != null && QuietWriter != null && !QuietWriter.Closed) {
                string header = Layout.Header;

                if (header != null) {
                    QuietWriter.Write(header);
                }
            }
        }

        protected virtual void PrepareWriter() { }
    }
}