using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using log4net.Core;
using log4net.Util.TypeConverters;

namespace log4net.Util {
    public sealed class OptionConverter {
        const string DELIM_START = "${";

        const char DELIM_STOP = '}';

        const int DELIM_START_LEN = 2;

        const int DELIM_STOP_LEN = 1;

        static readonly Type declaringType = typeof(OptionConverter);

        OptionConverter() { }

        public static bool ToBoolean(string argValue, bool defaultValue) {
            if (argValue != null && argValue.Length > 0) {
                try {
                    return bool.Parse(argValue);
                } catch (Exception exception) {
                    LogLog.Error(declaringType, "[" + argValue + "] is not in proper bool form.", exception);
                }
            }

            return defaultValue;
        }

        public static long ToFileSize(string argValue, long defaultValue) {
            if (argValue == null) {
                return defaultValue;
            }

            string text = argValue.Trim().ToUpper(CultureInfo.InvariantCulture);
            long num = 1L;
            int length;

            if ((length = text.IndexOf("KB")) != -1) {
                num = 1024L;
                text = text.Substring(0, length);
            } else if ((length = text.IndexOf("MB")) != -1) {
                num = 1048576L;
                text = text.Substring(0, length);
            } else if ((length = text.IndexOf("GB")) != -1) {
                num = 1073741824L;
                text = text.Substring(0, length);
            }

            if (text != null) {
                text = text.Trim();
                long val;

                if (SystemInfo.TryParse(text, out val)) {
                    return val * num;
                }

                LogLog.Error(declaringType, "OptionConverter: [" + text + "] is not in the correct file size syntax.");
            }

            return defaultValue;
        }

        public static object ConvertStringTo(Type target, string txt) {
            if (target == null) {
                throw new ArgumentNullException("target");
            }

            if (typeof(string) == target || typeof(object) == target) {
                return txt;
            }

            IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(target);

            if (convertFrom != null && convertFrom.CanConvertFrom(typeof(string))) {
                return convertFrom.ConvertFrom(txt);
            }

            if (target.IsEnum) {
                return ParseEnum(target, txt, true);
            }

            MethodInfo method = target.GetMethod("Parse", new Type[1] { typeof(string) });

            if (method != null) {
                return method.Invoke(null,
                    BindingFlags.InvokeMethod,
                    null,
                    new object[1] { txt },
                    CultureInfo.InvariantCulture);
            }

            return null;
        }

        public static bool CanConvertTypeTo(Type sourceType, Type targetType) {
            if (sourceType == null || targetType == null) {
                return false;
            }

            if (targetType.IsAssignableFrom(sourceType)) {
                return true;
            }

            IConvertTo convertTo = ConverterRegistry.GetConvertTo(sourceType, targetType);

            if (convertTo != null && convertTo.CanConvertTo(targetType)) {
                return true;
            }

            IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);

            if (convertFrom != null && convertFrom.CanConvertFrom(sourceType)) {
                return true;
            }

            return false;
        }

        public static object ConvertTypeTo(object sourceInstance, Type targetType) {
            Type type = sourceInstance.GetType();

            if (targetType.IsAssignableFrom(type)) {
                return sourceInstance;
            }

            IConvertTo convertTo = ConverterRegistry.GetConvertTo(type, targetType);

            if (convertTo != null && convertTo.CanConvertTo(targetType)) {
                return convertTo.ConvertTo(sourceInstance, targetType);
            }

            IConvertFrom convertFrom = ConverterRegistry.GetConvertFrom(targetType);

            if (convertFrom != null && convertFrom.CanConvertFrom(type)) {
                return convertFrom.ConvertFrom(sourceInstance);
            }

            throw new ArgumentException(
                "Cannot convert source object [" + sourceInstance + "] to target type [" + targetType.Name + "]",
                "sourceInstance");
        }

        public static object InstantiateByClassName(string className, Type superClass, object defaultValue) {
            if (className != null) {
                try {
                    Type typeFromString = SystemInfo.GetTypeFromString(className, true, true);

                    if (!superClass.IsAssignableFrom(typeFromString)) {
                        LogLog.Error(declaringType,
                            "OptionConverter: A [" +
                            className +
                            "] object is not assignable to a [" +
                            superClass.FullName +
                            "] variable.");

                        return defaultValue;
                    }

                    return Activator.CreateInstance(typeFromString);
                } catch (Exception exception) {
                    LogLog.Error(declaringType, "Could not instantiate class [" + className + "].", exception);
                }
            }

            return defaultValue;
        }

        public static string SubstituteVariables(string value, IDictionary props) {
            StringBuilder stringBuilder = new();
            int num = 0;
            int num2;

            while (true) {
                num2 = value.IndexOf("${", num);

                if (num2 == -1) {
                    if (num == 0) {
                        return value;
                    }

                    stringBuilder.Append(value.Substring(num, value.Length - num));
                    return stringBuilder.ToString();
                }

                stringBuilder.Append(value.Substring(num, num2 - num));
                int num3 = value.IndexOf('}', num2);

                if (num3 == -1) {
                    break;
                }

                num2 += 2;
                string key = value.Substring(num2, num3 - num2);
                string text = props[key] as string;

                if (text != null) {
                    stringBuilder.Append(text);
                }

                num = num3 + 1;
            }

            throw new LogException("[" + value + "] has no closing brace. Opening brace at position [" + num2 + "]");
        }

        static object ParseEnum(Type enumType, string value, bool ignoreCase) => Enum.Parse(enumType, value, ignoreCase);
    }
}