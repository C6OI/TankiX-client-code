using System;
using System.Runtime.InteropServices;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender {
    public class LocalSyslogAppender : AppenderSkeleton {
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

        readonly LevelMapping m_levelMapping = new();

        IntPtr m_handleToIdentity = IntPtr.Zero;

        public string Identity { get; set; }

        public SyslogFacility Facility { get; set; } = SyslogFacility.User;

        protected override bool RequiresLayout => true;

        public void AddMapping(LevelSeverity mapping) => m_levelMapping.Add(mapping);

        public override void ActivateOptions() {
            base.ActivateOptions();
            m_levelMapping.ActivateOptions();
            string text = Identity;

            if (text == null) {
                text = SystemInfo.ApplicationFriendlyName;
            }

            m_handleToIdentity = Marshal.StringToHGlobalAnsi(text);
            openlog(m_handleToIdentity, 1, Facility);
        }

        [PermissionSet(SecurityAction.Demand,
            XML =
                "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"UnmanagedCode\"/>\n</PermissionSet>\n")]
        protected override void Append(LoggingEvent loggingEvent) {
            int priority = GeneratePriority(Facility, GetSeverity(loggingEvent.Level));
            string message = RenderLoggingEvent(loggingEvent);
            syslog(priority, "%s", message);
        }

        protected override void OnClose() {
            base.OnClose();

            try {
                closelog();
            } catch (DllNotFoundException) { }

            if (m_handleToIdentity != IntPtr.Zero) {
                Marshal.FreeHGlobal(m_handleToIdentity);
            }
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

        static int GeneratePriority(SyslogFacility facility, SyslogSeverity severity) => (int)((int)facility * 8 + severity);

        [DllImport("libc")]
        static extern void openlog(IntPtr ident, int option, SyslogFacility facility);

        [DllImport("libc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        static extern void syslog(int priority, string format, string message);

        [DllImport("libc")]
        static extern void closelog();

        public class LevelSeverity : LevelMappingEntry {
            public SyslogSeverity Severity { get; set; }
        }
    }
}