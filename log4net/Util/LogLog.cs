using System;
using System.Collections;

namespace log4net.Util {
    public sealed class LogLog {
        const string PREFIX = "log4net: ";

        const string ERR_PREFIX = "log4net:ERROR ";

        const string WARN_PREFIX = "log4net:WARN ";

        static LogLog() => EmitInternalMessages = true;

        public LogLog(Type source, string prefix, string message, Exception exception) {
            TimeStamp = DateTime.Now;
            Source = source;
            Prefix = prefix;
            Message = message;
            Exception = exception;
        }

        public Type Source { get; }

        public DateTime TimeStamp { get; }

        public string Prefix { get; }

        public string Message { get; }

        public Exception Exception { get; }

        public static bool InternalDebugging { get; set; }

        public static bool QuietMode { get; set; }

        public static bool EmitInternalMessages { get; set; }

        public static bool IsDebugEnabled => InternalDebugging && !QuietMode;

        public static bool IsWarnEnabled => !QuietMode;

        public static bool IsErrorEnabled => !QuietMode;

        public static event LogReceivedEventHandler LogReceived;

        public override string ToString() => Prefix + Source.Name + ": " + Message;

        public static void OnLogReceived(Type source, string prefix, string message, Exception exception) {
            if (LogReceived != null) {
                LogReceived(null, new LogReceivedEventArgs(new LogLog(source, prefix, message, exception)));
            }
        }

        public static void Debug(Type source, string message) {
            if (IsDebugEnabled) {
                if (EmitInternalMessages) {
                    EmitOutLine("log4net: " + message);
                }

                OnLogReceived(source, "log4net: ", message, null);
            }
        }

        public static void Debug(Type source, string message, Exception exception) {
            if (!IsDebugEnabled) {
                return;
            }

            if (EmitInternalMessages) {
                EmitOutLine("log4net: " + message);

                if (exception != null) {
                    EmitOutLine(exception.ToString());
                }
            }

            OnLogReceived(source, "log4net: ", message, exception);
        }

        public static void Warn(Type source, string message) {
            if (IsWarnEnabled) {
                if (EmitInternalMessages) {
                    EmitErrorLine("log4net:WARN " + message);
                }

                OnLogReceived(source, "log4net:WARN ", message, null);
            }
        }

        public static void Warn(Type source, string message, Exception exception) {
            if (!IsWarnEnabled) {
                return;
            }

            if (EmitInternalMessages) {
                EmitErrorLine("log4net:WARN " + message);

                if (exception != null) {
                    EmitErrorLine(exception.ToString());
                }
            }

            OnLogReceived(source, "log4net:WARN ", message, exception);
        }

        public static void Error(Type source, string message) {
            if (IsErrorEnabled) {
                if (EmitInternalMessages) {
                    EmitErrorLine("log4net:ERROR " + message);
                }

                OnLogReceived(source, "log4net:ERROR ", message, null);
            }
        }

        public static void Error(Type source, string message, Exception exception) {
            if (!IsErrorEnabled) {
                return;
            }

            if (EmitInternalMessages) {
                EmitErrorLine("log4net:ERROR " + message);

                if (exception != null) {
                    EmitErrorLine(exception.ToString());
                }
            }

            OnLogReceived(source, "log4net:ERROR ", message, exception);
        }

        static void EmitOutLine(string message) {
            try {
                Console.Out.WriteLine(message);
            } catch { }
        }

        static void EmitErrorLine(string message) {
            try {
                Console.Error.WriteLine(message);
            } catch { }
        }

        public class LogReceivedAdapter : IDisposable {
            readonly LogReceivedEventHandler handler;

            public LogReceivedAdapter(IList items) {
                Items = items;
                handler = LogLog_LogReceived;
                LogReceived = (LogReceivedEventHandler)Delegate.Combine(LogReceived, handler);
            }

            public IList Items { get; }

            public void Dispose() => LogReceived = (LogReceivedEventHandler)Delegate.Remove(LogReceived, handler);

            void LogLog_LogReceived(object source, LogReceivedEventArgs e) => Items.Add(e.LogLog);
        }
    }
}