using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;

namespace log4net.Util {
    public sealed class SystemInfo {
        const string DEFAULT_NULL_TEXT = "(null)";

        const string DEFAULT_NOT_AVAILABLE_TEXT = "NOT AVAILABLE";

        public static readonly Type[] EmptyTypes;

        static readonly Type declaringType;

        static string s_hostName;

        static string s_appFriendlyName;

        static SystemInfo() {
            EmptyTypes = new Type[0];
            declaringType = typeof(SystemInfo);
            ProcessStartTime = DateTime.Now;
            string text = "(null)";
            string text2 = "NOT AVAILABLE";
            NotAvailableText = text2;
            NullText = text;
        }

        SystemInfo() { }

        public static string NewLine => Environment.NewLine;

        public static string ApplicationBaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        public static string ConfigurationFileLocation => AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        public static string EntryAssemblyLocation => "-";

        public static int CurrentThreadId => AppDomain.GetCurrentThreadId();

        public static string HostName {
            get {
                if (s_hostName == null) {
                    try {
                        s_hostName = Dns.GetHostName();
                    } catch (SocketException) {
                        LogLog.Debug(declaringType,
                            "Socket exception occurred while getting the dns hostname. Error Ignored.");
                    } catch (SecurityException) {
                        LogLog.Debug(declaringType,
                            "Security exception occurred while getting the dns hostname. Error Ignored.");
                    } catch (Exception exception) {
                        LogLog.Debug(declaringType,
                            "Some other exception occurred while getting the dns hostname. Error Ignored.",
                            exception);
                    }

                    if (s_hostName == null || s_hostName.Length == 0) {
                        try {
                            s_hostName = Environment.MachineName;
                        } catch (InvalidOperationException) { } catch (SecurityException) { }
                    }

                    if (s_hostName == null || s_hostName.Length == 0) {
                        s_hostName = NotAvailableText;

                        LogLog.Debug(declaringType,
                            "Could not determine the hostname. Error Ignored. Empty host name will be used");
                    }
                }

                return s_hostName;
            }
        }

        public static string ApplicationFriendlyName {
            get {
                if (s_appFriendlyName == null) {
                    try {
                        s_appFriendlyName = AppDomain.CurrentDomain.FriendlyName;
                    } catch (SecurityException) {
                        LogLog.Debug(declaringType,
                            "Security exception while trying to get current domain friendly name. Error Ignored.");
                    }

                    if (s_appFriendlyName == null || s_appFriendlyName.Length == 0) {
                        try {
                            string entryAssemblyLocation = EntryAssemblyLocation;
                            s_appFriendlyName = Path.GetFileName(entryAssemblyLocation);
                        } catch (SecurityException) { }
                    }

                    if (s_appFriendlyName == null || s_appFriendlyName.Length == 0) {
                        s_appFriendlyName = NotAvailableText;
                    }
                }

                return s_appFriendlyName;
            }
        }

        public static DateTime ProcessStartTime { get; }

        public static string NullText { get; set; }

        public static string NotAvailableText { get; set; }

        public static string AssemblyLocationInfo(Assembly myAssembly) {
            if (myAssembly.GlobalAssemblyCache) {
                return "Global Assembly Cache";
            }

            try {
                if (myAssembly is AssemblyBuilder) {
                    return "Dynamic Assembly";
                }

                if (myAssembly.GetType().FullName == "System.Reflection.Emit.InternalAssemblyBuilder") {
                    return "Dynamic Assembly";
                }

                return myAssembly.Location;
            } catch (NotSupportedException) {
                return "Dynamic Assembly";
            } catch (TargetInvocationException ex2) {
                return "Location Detect Failed (" + ex2.Message + ")";
            } catch (ArgumentException ex3) {
                return "Location Detect Failed (" + ex3.Message + ")";
            } catch (SecurityException) {
                return "Location Permission Denied";
            }
        }

        public static string AssemblyQualifiedName(Type type) => type.FullName + ", " + type.Assembly.FullName;

        public static string AssemblyShortName(Assembly myAssembly) {
            string text = myAssembly.FullName;
            int num = text.IndexOf(',');

            if (num > 0) {
                text = text.Substring(0, num);
            }

            return text.Trim();
        }

        public static string AssemblyFileName(Assembly myAssembly) => Path.GetFileName(myAssembly.Location);

        public static Type GetTypeFromString(Type relativeType, string typeName, bool throwOnError, bool ignoreCase) =>
            GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);

        public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase) =>
            GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);

        public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError,
            bool ignoreCase) {
            if (typeName.IndexOf(',') == -1) {
                Type type = relativeAssembly.GetType(typeName, false, ignoreCase);

                if (type != null) {
                    return type;
                }

                Assembly[] array = null;

                try {
                    array = AppDomain.CurrentDomain.GetAssemblies();
                } catch (SecurityException) { }

                if (array != null) {
                    Assembly[] array2 = array;

                    foreach (Assembly assembly in array2) {
                        type = assembly.GetType(typeName, false, ignoreCase);

                        if (type != null) {
                            LogLog.Debug(declaringType,
                                "Loaded type [" +
                                typeName +
                                "] from assembly [" +
                                assembly.FullName +
                                "] by searching loaded assemblies.");

                            return type;
                        }
                    }
                }

                if (throwOnError) {
                    throw new TypeLoadException("Could not load type [" +
                                                typeName +
                                                "]. Tried assembly [" +
                                                relativeAssembly.FullName +
                                                "] and all loaded assemblies");
                }

                return null;
            }

            return Type.GetType(typeName, throwOnError, ignoreCase);
        }

        public static Guid NewGuid() => Guid.NewGuid();

        public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string parameterName, object actualValue,
            string message) => new(parameterName, actualValue, message);

        public static bool TryParse(string s, out int val) {
            val = 0;

            try {
                double result;

                if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result)) {
                    val = Convert.ToInt32(result);
                    return true;
                }
            } catch { }

            return false;
        }

        public static bool TryParse(string s, out long val) {
            val = 0L;

            try {
                double result;

                if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result)) {
                    val = Convert.ToInt64(result);
                    return true;
                }
            } catch { }

            return false;
        }

        public static bool TryParse(string s, out short val) {
            val = 0;

            try {
                double result;

                if (double.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out result)) {
                    val = Convert.ToInt16(result);
                    return true;
                }
            } catch { }

            return false;
        }

        public static string ConvertToFullPath(string path) {
            if (path == null) {
                throw new ArgumentNullException("path");
            }

            string text = string.Empty;

            try {
                string applicationBaseDirectory = ApplicationBaseDirectory;

                if (applicationBaseDirectory != null) {
                    Uri uri = new(applicationBaseDirectory);

                    if (uri.IsFile) {
                        text = uri.LocalPath;
                    }
                }
            } catch { }

            if (text != null && text.Length > 0) {
                return Path.GetFullPath(Path.Combine(text, path));
            }

            return Path.GetFullPath(path);
        }

        public static Hashtable CreateCaseInsensitiveHashtable() => new(StringComparer.OrdinalIgnoreCase);
    }
}