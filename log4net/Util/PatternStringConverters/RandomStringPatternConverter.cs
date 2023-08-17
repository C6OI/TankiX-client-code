using System;
using System.IO;
using log4net.Core;

namespace log4net.Util.PatternStringConverters {
    sealed class RandomStringPatternConverter : PatternConverter, IOptionHandler {
        static readonly Random s_random = new();

        static readonly Type declaringType = typeof(RandomStringPatternConverter);

        int m_length = 4;

        public void ActivateOptions() {
            string option = Option;

            if (option != null && option.Length > 0) {
                int val;

                if (SystemInfo.TryParse(option, out val)) {
                    m_length = val;
                } else {
                    LogLog.Error(declaringType,
                        "RandomStringPatternConverter: Could not convert Option [" + option + "] to Length Int32");
                }
            }
        }

        protected override void Convert(TextWriter writer, object state) {
            try {
                lock (s_random) {
                    for (int i = 0; i < m_length; i++) {
                        int num = s_random.Next(36);

                        if (num < 26) {
                            char value = (char)(65 + num);
                            writer.Write(value);
                        } else if (num < 36) {
                            char value2 = (char)(48 + (num - 26));
                            writer.Write(value2);
                        } else {
                            writer.Write('X');
                        }
                    }
                }
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Error occurred while converting.", exception);
            }
        }
    }
}