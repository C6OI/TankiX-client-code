using System;
using System.Net;
using System.Text;
using log4net.Core;
using log4net.Layout;
using log4net.Util;

namespace log4net.Appender {
    public class RemoteSyslogAppender : UdpAppender {
        public enum SyslogFacility {
            Kernel = 0,
            User = 1,
            Mail = 2,
            Daemons = 3,
            Authorization = 4,
            Syslog = 5,
            Printer = 6,
            News = 7,
            Uucp = 8,
            Clock = 9,
            Authorization2 = 10,
            Ftp = 11,
            Ntp = 12,
            Audit = 13,
            Alert = 14,
            Clock2 = 15,
            Local0 = 16,
            Local1 = 17,
            Local2 = 18,
            Local3 = 19,
            Local4 = 20,
            Local5 = 21,
            Local6 = 22,
            Local7 = 23
        }

        public enum SyslogSeverity {
            Emergency = 0,
            Alert = 1,
            Critical = 2,
            Error = 3,
            Warning = 4,
            Notice = 5,
            Informational = 6,
            Debug = 7
        }

        const int DefaultSyslogPort = 514;

        const int c_renderBufferSize = 256;

        const int c_renderBufferMaxCapacity = 1024;

        readonly LevelMapping m_levelMapping = new();

        public RemoteSyslogAppender() {
            RemotePort = 514;
            RemoteAddress = IPAddress.Parse("127.0.0.1");
            Encoding = Encoding.ASCII;
        }

        public PatternLayout Identity { get; set; }

        public SyslogFacility Facility { get; set; } = SyslogFacility.User;

        public void AddMapping(LevelSeverity mapping) => m_levelMapping.Add(mapping);

        protected override void Append(LoggingEvent loggingEvent) {
            try {
                int value = GeneratePriority(Facility, GetSeverity(loggingEvent.Level));
                string value2 = Identity == null ? loggingEvent.Domain : Identity.Format(loggingEvent);
                string text = RenderLoggingEvent(loggingEvent);
                int i = 0;
                StringBuilder stringBuilder = new();

                while (i < text.Length) {
                    stringBuilder.Length = 0;
                    stringBuilder.Append('<');
                    stringBuilder.Append(value);
                    stringBuilder.Append('>');
                    stringBuilder.Append(value2);
                    stringBuilder.Append(": ");

                    for (; i < text.Length; i++) {
                        char c = text[i];

                        if (c >= ' ' && c <= '~') {
                            stringBuilder.Append(c);
                        } else if (c == '\r' || c == '\n') {
                            if (text.Length > i + 1 && (text[i + 1] == '\r' || text[i + 1] == '\n')) {
                                i++;
                            }

                            i++;
                            break;
                        }
                    }

                    byte[] bytes = Encoding.GetBytes(stringBuilder.ToString());
                    Client.Send(bytes, bytes.Length, RemoteEndPoint);
                }
            } catch (Exception e) {
                ErrorHandler.Error(
                    "Unable to send logging event to remote syslog " + RemoteAddress + " on port " + RemotePort + ".",
                    e,
                    ErrorCode.WriteFailure);
            }
        }

        public override void ActivateOptions() {
            base.ActivateOptions();
            m_levelMapping.ActivateOptions();
        }

        protected virtual SyslogSeverity GetSeverity(Level level) {
            LevelSeverity levelSeverity = m_levelMapping.Lookup(level) as LevelSeverity;

            if (levelSeverity != null) {
                return levelSeverity.Severity;
            }

            if (level >= Level.Alert) {
                return SyslogSeverity.Alert;
            }

            if (level >= Level.Critical) {
                return SyslogSeverity.Critical;
            }

            if (level >= Level.Error) {
                return SyslogSeverity.Error;
            }

            if (level >= Level.Warn) {
                return SyslogSeverity.Warning;
            }

            if (level >= Level.Notice) {
                return SyslogSeverity.Notice;
            }

            if (level >= Level.Info) {
                return SyslogSeverity.Informational;
            }

            return SyslogSeverity.Debug;
        }

        public static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity) {
            if (facility < SyslogFacility.Kernel || facility > SyslogFacility.Local7) {
                throw new ArgumentException("SyslogFacility out of range", "facility");
            }

            if (severity < SyslogSeverity.Emergency || severity > SyslogSeverity.Debug) {
                throw new ArgumentException("SyslogSeverity out of range", "severity");
            }

            return (int)((int)facility * 8 + severity);
        }

        public class LevelSeverity : LevelMappingEntry {
            public SyslogSeverity Severity { get; set; }
        }
    }
}