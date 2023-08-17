using System;
using System.Text;

namespace log4net.Util {
    public sealed class SystemStringFormat {
        static readonly Type declaringType = typeof(SystemStringFormat);

        readonly object[] m_args;

        readonly string m_format;
        readonly IFormatProvider m_provider;

        public SystemStringFormat(IFormatProvider provider, string format, params object[] args) {
            m_provider = provider;
            m_format = format;
            m_args = args;
        }

        public override string ToString() => StringFormat(m_provider, m_format, m_args);

        static string StringFormat(IFormatProvider provider, string format, params object[] args) {
            try {
                if (format == null) {
                    return null;
                }

                if (args == null) {
                    return format;
                }

                return string.Format(provider, format, args);
            } catch (Exception ex) {
                LogLog.Warn(declaringType, "Exception while rendering format [" + format + "]", ex);
                return StringFormatError(ex, format, args);
            } catch {
                LogLog.Warn(declaringType, "Exception while rendering format [" + format + "]");
                return StringFormatError(null, format, args);
            }
        }

        static string StringFormatError(Exception formatException, string format, object[] args) {
            try {
                StringBuilder stringBuilder = new("<log4net.Error>");

                if (formatException != null) {
                    stringBuilder.Append("Exception during StringFormat: ").Append(formatException.Message);
                } else {
                    stringBuilder.Append("Exception during StringFormat");
                }

                stringBuilder.Append(" <format>").Append(format).Append("</format>");
                stringBuilder.Append("<args>");
                RenderArray(args, stringBuilder);
                stringBuilder.Append("</args>");
                stringBuilder.Append("</log4net.Error>");
                return stringBuilder.ToString();
            } catch (Exception exception) {
                LogLog.Error(declaringType, "INTERNAL ERROR during StringFormat error handling", exception);
                return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
            } catch {
                LogLog.Error(declaringType, "INTERNAL ERROR during StringFormat error handling");
                return "<log4net.Error>Exception during StringFormat. See Internal Log.</log4net.Error>";
            }
        }

        static void RenderArray(Array array, StringBuilder buffer) {
            if (array == null) {
                buffer.Append(SystemInfo.NullText);
                return;
            }

            if (array.Rank != 1) {
                buffer.Append(array);
                return;
            }

            buffer.Append("{");
            int length = array.Length;

            if (length > 0) {
                RenderObject(array.GetValue(0), buffer);

                for (int i = 1; i < length; i++) {
                    buffer.Append(", ");
                    RenderObject(array.GetValue(i), buffer);
                }
            }

            buffer.Append("}");
        }

        static void RenderObject(object obj, StringBuilder buffer) {
            if (obj == null) {
                buffer.Append(SystemInfo.NullText);
                return;
            }

            try {
                buffer.Append(obj);
            } catch (Exception ex) {
                buffer.Append("<Exception: ").Append(ex.Message).Append(">");
            } catch {
                buffer.Append("<Exception>");
            }
        }
    }
}