using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Security;
using System.Threading;
using log4net.Repository;
using log4net.Util;

namespace log4net.Core {
    [Serializable]
    public class LoggingEvent : ISerializable {
        public const string HostNameProperty = "log4net:HostName";

        public const string IdentityProperty = "log4net:Identity";

        public const string UserNameProperty = "log4net:UserName";

        static readonly Type declaringType = typeof(LoggingEvent);

        readonly Type m_callerStackBoundaryDeclaringType;

        bool m_cacheUpdatable = true;

        CompositeProperties m_compositeProperties;

        LoggingEventData m_data;

        PropertiesDictionary m_eventProperties;

        FixFlags m_fixFlags;

        public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, string loggerName,
            Level level, object message, Exception exception) {
            m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
            MessageObject = message;
            Repository = repository;
            ExceptionObject = exception;
            m_data.LoggerName = loggerName;
            m_data.Level = level;
            m_data.TimeStamp = DateTime.Now;
        }

        public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data,
            FixFlags fixedData) {
            m_callerStackBoundaryDeclaringType = callerStackBoundaryDeclaringType;
            Repository = repository;
            m_data = data;
            m_fixFlags = fixedData;
        }

        public LoggingEvent(Type callerStackBoundaryDeclaringType, ILoggerRepository repository, LoggingEventData data)
            : this(callerStackBoundaryDeclaringType, repository, data, FixFlags.All) { }

        public LoggingEvent(LoggingEventData data)
            : this(null, null, data) { }

        protected LoggingEvent(SerializationInfo info, StreamingContext context) {
            m_data.LoggerName = info.GetString("LoggerName");
            m_data.Level = (Level)info.GetValue("Level", typeof(Level));
            m_data.Message = info.GetString("Message");
            m_data.ThreadName = info.GetString("ThreadName");
            m_data.TimeStamp = info.GetDateTime("TimeStamp");
            m_data.LocationInfo = (LocationInfo)info.GetValue("LocationInfo", typeof(LocationInfo));
            m_data.UserName = info.GetString("UserName");
            m_data.ExceptionString = info.GetString("ExceptionString");
            m_data.Properties = (PropertiesDictionary)info.GetValue("Properties", typeof(PropertiesDictionary));
            m_data.Domain = info.GetString("Domain");
            m_data.Identity = info.GetString("Identity");
            m_fixFlags = FixFlags.All;
        }

        public static DateTime StartTime => SystemInfo.ProcessStartTime;

        public Level Level => m_data.Level;

        public DateTime TimeStamp => m_data.TimeStamp;

        public string LoggerName => m_data.LoggerName;

        public LocationInfo LocationInformation {
            get {
                if (m_data.LocationInfo == null && m_cacheUpdatable) {
                    m_data.LocationInfo = new LocationInfo(m_callerStackBoundaryDeclaringType);
                }

                return m_data.LocationInfo;
            }
        }

        public object MessageObject { get; }

        public Exception ExceptionObject { get; }

        public ILoggerRepository Repository { get; private set; }

        public string RenderedMessage {
            get {
                if (m_data.Message == null && m_cacheUpdatable) {
                    if (MessageObject == null) {
                        m_data.Message = string.Empty;
                    } else if (MessageObject is string) {
                        m_data.Message = MessageObject as string;
                    } else if (Repository != null) {
                        m_data.Message = Repository.RendererMap.FindAndRender(MessageObject);
                    } else {
                        m_data.Message = MessageObject.ToString();
                    }
                }

                return m_data.Message;
            }
        }

        public string ThreadName {
            get {
                if (m_data.ThreadName == null && m_cacheUpdatable) {
                    m_data.ThreadName = Thread.CurrentThread.Name;

                    if (m_data.ThreadName == null || m_data.ThreadName.Length == 0) {
                        try {
                            m_data.ThreadName = SystemInfo.CurrentThreadId.ToString(NumberFormatInfo.InvariantInfo);
                        } catch (SecurityException) {
                            LogLog.Debug(declaringType,
                                "Security exception while trying to get current thread ID. Error Ignored. Empty thread name.");

                            m_data.ThreadName = Thread.CurrentThread.GetHashCode().ToString(CultureInfo.InvariantCulture);
                        }
                    }
                }

                return m_data.ThreadName;
            }
        }

        public string UserName {
            get {
                if (m_data.UserName == null && m_cacheUpdatable) {
                    m_data.UserName = SystemInfo.NotAvailableText;
                }

                return m_data.UserName;
            }
        }

        public string Identity {
            get {
                if (m_data.Identity == null && m_cacheUpdatable) {
                    m_data.Identity = SystemInfo.NotAvailableText;
                }

                return m_data.Identity;
            }
        }

        public string Domain {
            get {
                if (m_data.Domain == null && m_cacheUpdatable) {
                    m_data.Domain = SystemInfo.ApplicationFriendlyName;
                }

                return m_data.Domain;
            }
        }

        public PropertiesDictionary Properties {
            get {
                if (m_data.Properties != null) {
                    return m_data.Properties;
                }

                if (m_eventProperties == null) {
                    m_eventProperties = new PropertiesDictionary();
                }

                return m_eventProperties;
            }
        }

        public FixFlags Fix {
            get => m_fixFlags;
            set => FixVolatileData(value);
        }

        [PermissionSet(SecurityAction.Demand,
            XML =
                "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("LoggerName", m_data.LoggerName);
            info.AddValue("Level", m_data.Level);
            info.AddValue("Message", m_data.Message);
            info.AddValue("ThreadName", m_data.ThreadName);
            info.AddValue("TimeStamp", m_data.TimeStamp);
            info.AddValue("LocationInfo", m_data.LocationInfo);
            info.AddValue("UserName", m_data.UserName);
            info.AddValue("ExceptionString", m_data.ExceptionString);
            info.AddValue("Properties", m_data.Properties);
            info.AddValue("Domain", m_data.Domain);
            info.AddValue("Identity", m_data.Identity);
        }

        internal void EnsureRepository(ILoggerRepository repository) {
            if (repository != null) {
                Repository = repository;
            }
        }

        public void WriteRenderedMessage(TextWriter writer) {
            if (m_data.Message != null) {
                writer.Write(m_data.Message);
            } else if (MessageObject != null) {
                if (MessageObject is string) {
                    writer.Write(MessageObject as string);
                } else if (Repository != null) {
                    Repository.RendererMap.FindAndRender(MessageObject, writer);
                } else {
                    writer.Write(MessageObject.ToString());
                }
            }
        }

        public LoggingEventData GetLoggingEventData() => GetLoggingEventData(FixFlags.Partial);

        public LoggingEventData GetLoggingEventData(FixFlags fixFlags) {
            Fix = fixFlags;
            return m_data;
        }

        [Obsolete("Use GetExceptionString instead")]
        public string GetExceptionStrRep() => GetExceptionString();

        public string GetExceptionString() {
            if (m_data.ExceptionString == null && m_cacheUpdatable) {
                if (ExceptionObject != null) {
                    if (Repository != null) {
                        m_data.ExceptionString = Repository.RendererMap.FindAndRender(ExceptionObject);
                    } else {
                        m_data.ExceptionString = ExceptionObject.ToString();
                    }
                } else {
                    m_data.ExceptionString = string.Empty;
                }
            }

            return m_data.ExceptionString;
        }

        [Obsolete("Use Fix property")]
        public void FixVolatileData() => Fix = FixFlags.All;

        [Obsolete("Use Fix property")]
        public void FixVolatileData(bool fastButLoose) {
            if (fastButLoose) {
                Fix = FixFlags.Partial;
            } else {
                Fix = FixFlags.All;
            }
        }

        protected void FixVolatileData(FixFlags flags) {
            object obj = null;
            m_cacheUpdatable = true;
            FixFlags fixFlags = (flags ^ m_fixFlags) & flags;

            if (fixFlags > FixFlags.None) {
                if ((fixFlags & FixFlags.Message) != 0) {
                    obj = RenderedMessage;
                    m_fixFlags |= FixFlags.Message;
                }

                if ((fixFlags & FixFlags.ThreadName) != 0) {
                    obj = ThreadName;
                    m_fixFlags |= FixFlags.ThreadName;
                }

                if ((fixFlags & FixFlags.LocationInfo) != 0) {
                    obj = LocationInformation;
                    m_fixFlags |= FixFlags.LocationInfo;
                }

                if ((fixFlags & FixFlags.UserName) != 0) {
                    obj = UserName;
                    m_fixFlags |= FixFlags.UserName;
                }

                if ((fixFlags & FixFlags.Domain) != 0) {
                    obj = Domain;
                    m_fixFlags |= FixFlags.Domain;
                }

                if ((fixFlags & FixFlags.Identity) != 0) {
                    obj = Identity;
                    m_fixFlags |= FixFlags.Identity;
                }

                if ((fixFlags & FixFlags.Exception) != 0) {
                    obj = GetExceptionString();
                    m_fixFlags |= FixFlags.Exception;
                }

                if ((fixFlags & FixFlags.Properties) != 0) {
                    CacheProperties();
                    m_fixFlags |= FixFlags.Properties;
                }
            }

            if (obj != null) { }

            m_cacheUpdatable = false;
        }

        void CreateCompositeProperties() {
            m_compositeProperties = new CompositeProperties();

            if (m_eventProperties != null) {
                m_compositeProperties.Add(m_eventProperties);
            }

            PropertiesDictionary properties = ThreadContext.Properties.GetProperties(false);

            if (properties != null) {
                m_compositeProperties.Add(properties);
            }

            PropertiesDictionary propertiesDictionary = new();
            propertiesDictionary["log4net:UserName"] = UserName;
            propertiesDictionary["log4net:Identity"] = Identity;
            m_compositeProperties.Add(propertiesDictionary);
            m_compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());
        }

        void CacheProperties() {
            if (m_data.Properties != null || !m_cacheUpdatable) {
                return;
            }

            if (m_compositeProperties == null) {
                CreateCompositeProperties();
            }

            PropertiesDictionary propertiesDictionary = m_compositeProperties.Flatten();
            PropertiesDictionary propertiesDictionary2 = new();

            foreach (DictionaryEntry item in (IDictionary)propertiesDictionary) {
                string text = item.Key as string;

                if (text != null) {
                    object obj = item.Value;
                    IFixingRequired fixingRequired = obj as IFixingRequired;

                    if (fixingRequired != null) {
                        obj = fixingRequired.GetFixedObject();
                    }

                    if (obj != null) {
                        propertiesDictionary2[text] = obj;
                    }
                }
            }

            m_data.Properties = propertiesDictionary2;
        }

        public object LookupProperty(string key) {
            if (m_data.Properties != null) {
                return m_data.Properties[key];
            }

            if (m_compositeProperties == null) {
                CreateCompositeProperties();
            }

            return m_compositeProperties[key];
        }

        public PropertiesDictionary GetProperties() {
            if (m_data.Properties != null) {
                return m_data.Properties;
            }

            if (m_compositeProperties == null) {
                CreateCompositeProperties();
            }

            return m_compositeProperties.Flatten();
        }
    }
}