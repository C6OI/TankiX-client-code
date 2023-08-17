using System;
using System.Globalization;
using System.Text;
using log4net.Core;
using log4net.Util;

namespace log4net.Appender {
    public class AnsiColorTerminalAppender : AppenderSkeleton {
        [Flags]
        public enum AnsiAttributes {
            Bright = 1,
            Dim = 2,
            Underscore = 4,
            Blink = 8,
            Reverse = 0x10,
            Hidden = 0x20,
            Strikethrough = 0x40,
            Light = 0x80
        }

        public enum AnsiColor {
            Black = 0,
            Red = 1,
            Green = 2,
            Yellow = 3,
            Blue = 4,
            Magenta = 5,
            Cyan = 6,
            White = 7
        }

        public const string ConsoleOut = "Console.Out";

        public const string ConsoleError = "Console.Error";

        const string PostEventCodes = "\u001b[0m";

        readonly LevelMapping m_levelMapping = new();

        bool m_writeToErrorStream;

        public virtual string Target {
            get => !m_writeToErrorStream ? "Console.Out" : "Console.Error";
            set {
                string strB = value.Trim();

                if (string.Compare("Console.Error", strB, true, CultureInfo.InvariantCulture) == 0) {
                    m_writeToErrorStream = true;
                } else {
                    m_writeToErrorStream = false;
                }
            }
        }

        protected override bool RequiresLayout => true;

        public void AddMapping(LevelColors mapping) => m_levelMapping.Add(mapping);

        protected override void Append(LoggingEvent loggingEvent) {
            string text = RenderLoggingEvent(loggingEvent);
            LevelColors levelColors = m_levelMapping.Lookup(loggingEvent.Level) as LevelColors;

            if (levelColors != null) {
                text = levelColors.CombinedColor + text;
            }

            text = text.Length > 1
                       ? !text.EndsWith("\r\n") && !text.EndsWith("\n\r")
                             ? !text.EndsWith("\n") && !text.EndsWith("\r") ? text + "\u001b[0m"
                                   : text.Insert(text.Length - 1, "\u001b[0m") : text.Insert(text.Length - 2, "\u001b[0m")
                       : text[0] != '\n' && text[0] != '\r'
                           ? text + "\u001b[0m"
                           : "\u001b[0m" + text;

            if (m_writeToErrorStream) {
                Console.Error.Write(text);
            } else {
                Console.Write(text);
            }
        }

        public override void ActivateOptions() {
            base.ActivateOptions();
            m_levelMapping.ActivateOptions();
        }

        public class LevelColors : LevelMappingEntry {
            public AnsiColor ForeColor { get; set; }

            public AnsiColor BackColor { get; set; }

            public AnsiAttributes Attributes { get; set; }

            internal string CombinedColor { get; private set; } = string.Empty;

            public override void ActivateOptions() {
                base.ActivateOptions();
                StringBuilder stringBuilder = new();
                stringBuilder.Append("\u001b[0;");
                int num = (Attributes & AnsiAttributes.Light) > 0 ? 60 : 0;
                stringBuilder.Append((int)(30 + num + ForeColor));
                stringBuilder.Append(';');
                stringBuilder.Append((int)(40 + num + BackColor));

                if ((Attributes & AnsiAttributes.Bright) > 0) {
                    stringBuilder.Append(";1");
                }

                if ((Attributes & AnsiAttributes.Dim) > 0) {
                    stringBuilder.Append(";2");
                }

                if ((Attributes & AnsiAttributes.Underscore) > 0) {
                    stringBuilder.Append(";4");
                }

                if ((Attributes & AnsiAttributes.Blink) > 0) {
                    stringBuilder.Append(";5");
                }

                if ((Attributes & AnsiAttributes.Reverse) > 0) {
                    stringBuilder.Append(";7");
                }

                if ((Attributes & AnsiAttributes.Hidden) > 0) {
                    stringBuilder.Append(";8");
                }

                if ((Attributes & AnsiAttributes.Strikethrough) > 0) {
                    stringBuilder.Append(";9");
                }

                stringBuilder.Append('m');
                CombinedColor = stringBuilder.ToString();
            }
        }
    }
}