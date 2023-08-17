using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Xml;
using log4net.Appender;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Util;

namespace log4net.Repository.Hierarchy {
    public class XmlHierarchyConfigurator {
        const string CONFIGURATION_TAG = "log4net";

        const string RENDERER_TAG = "renderer";

        const string APPENDER_TAG = "appender";

        const string APPENDER_REF_TAG = "appender-ref";

        const string PARAM_TAG = "param";

        const string CATEGORY_TAG = "category";

        const string PRIORITY_TAG = "priority";

        const string LOGGER_TAG = "logger";

        const string NAME_ATTR = "name";

        const string TYPE_ATTR = "type";

        const string VALUE_ATTR = "value";

        const string ROOT_TAG = "root";

        const string LEVEL_TAG = "level";

        const string REF_ATTR = "ref";

        const string ADDITIVITY_ATTR = "additivity";

        const string THRESHOLD_ATTR = "threshold";

        const string CONFIG_DEBUG_ATTR = "configDebug";

        const string INTERNAL_DEBUG_ATTR = "debug";

        const string EMIT_INTERNAL_DEBUG_ATTR = "emitDebug";

        const string CONFIG_UPDATE_MODE_ATTR = "update";

        const string RENDERING_TYPE_ATTR = "renderingClass";

        const string RENDERED_TYPE_ATTR = "renderedClass";

        const string INHERITED = "inherited";

        static readonly Type declaringType = typeof(XmlHierarchyConfigurator);

        readonly Hashtable m_appenderBag;

        readonly Hierarchy m_hierarchy;

        public XmlHierarchyConfigurator(Hierarchy hierarchy) {
            m_hierarchy = hierarchy;
            m_appenderBag = new Hashtable();
        }

        bool HasCaseInsensitiveEnvironment {
            get {
                PlatformID platform = Environment.OSVersion.Platform;
                return platform != PlatformID.Unix && platform != PlatformID.MacOSX;
            }
        }

        public void Configure(XmlElement element) {
            if (element == null || m_hierarchy == null) {
                return;
            }

            string localName = element.LocalName;

            if (localName != "log4net") {
                LogLog.Error(declaringType, "Xml element is - not a <log4net> element.");
                return;
            }

            if (!LogLog.EmitInternalMessages) {
                string attribute = element.GetAttribute("emitDebug");
                LogLog.Debug(declaringType, "emitDebug attribute [" + attribute + "].");

                if (attribute.Length > 0 && attribute != "null") {
                    LogLog.EmitInternalMessages = OptionConverter.ToBoolean(attribute, true);
                } else {
                    LogLog.Debug(declaringType, "Ignoring emitDebug attribute.");
                }
            }

            if (!LogLog.InternalDebugging) {
                string attribute2 = element.GetAttribute("debug");
                LogLog.Debug(declaringType, "debug attribute [" + attribute2 + "].");

                if (attribute2.Length > 0 && attribute2 != "null") {
                    LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute2, true);
                } else {
                    LogLog.Debug(declaringType, "Ignoring debug attribute.");
                }

                string attribute3 = element.GetAttribute("configDebug");

                if (attribute3.Length > 0 && attribute3 != "null") {
                    LogLog.Warn(declaringType, "The \"configDebug\" attribute is deprecated.");
                    LogLog.Warn(declaringType, "Use the \"debug\" attribute instead.");
                    LogLog.InternalDebugging = OptionConverter.ToBoolean(attribute3, true);
                }
            }

            ConfigUpdateMode configUpdateMode = ConfigUpdateMode.Merge;
            string attribute4 = element.GetAttribute("update");

            if (attribute4 != null && attribute4.Length > 0) {
                try {
                    configUpdateMode =
                        (ConfigUpdateMode)(int)OptionConverter.ConvertStringTo(typeof(ConfigUpdateMode), attribute4);
                } catch {
                    LogLog.Error(declaringType, "Invalid update attribute value [" + attribute4 + "]");
                }
            }

            LogLog.Debug(declaringType, "Configuration update mode [" + configUpdateMode + "].");

            if (configUpdateMode == ConfigUpdateMode.Overwrite) {
                m_hierarchy.ResetConfiguration();
                LogLog.Debug(declaringType, "Configuration reset before reading config.");
            }

            foreach (XmlNode childNode in element.ChildNodes) {
                if (childNode.NodeType == XmlNodeType.Element) {
                    XmlElement xmlElement = (XmlElement)childNode;

                    if (xmlElement.LocalName == "logger") {
                        ParseLogger(xmlElement);
                    } else if (xmlElement.LocalName == "category") {
                        ParseLogger(xmlElement);
                    } else if (xmlElement.LocalName == "root") {
                        ParseRoot(xmlElement);
                    } else if (xmlElement.LocalName == "renderer") {
                        ParseRenderer(xmlElement);
                    } else if (!(xmlElement.LocalName == "appender")) {
                        SetParameter(xmlElement, m_hierarchy);
                    }
                }
            }

            string attribute5 = element.GetAttribute("threshold");
            LogLog.Debug(declaringType, "Hierarchy Threshold [" + attribute5 + "]");

            if (attribute5.Length > 0 && attribute5 != "null") {
                Level level = (Level)ConvertStringTo(typeof(Level), attribute5);

                if (level != null) {
                    m_hierarchy.Threshold = level;
                } else {
                    LogLog.Warn(declaringType,
                        "Unable to set hierarchy threshold using value [" +
                        attribute5 +
                        "] (with acceptable conversion types)");
                }
            }
        }

        protected IAppender FindAppenderByReference(XmlElement appenderRef) {
            string attribute = appenderRef.GetAttribute("ref");
            IAppender appender = (IAppender)m_appenderBag[attribute];

            if (appender != null) {
                return appender;
            }

            XmlElement xmlElement = null;

            if (attribute != null && attribute.Length > 0) {
                foreach (XmlElement item in appenderRef.OwnerDocument.GetElementsByTagName("appender")) {
                    if (item.GetAttribute("name") == attribute) {
                        xmlElement = item;
                        break;
                    }
                }
            }

            if (xmlElement == null) {
                LogLog.Error(declaringType,
                    "XmlHierarchyConfigurator: No appender named [" + attribute + "] could be found.");

                return null;
            }

            appender = ParseAppender(xmlElement);

            if (appender != null) {
                m_appenderBag[attribute] = appender;
            }

            return appender;
        }

        protected IAppender ParseAppender(XmlElement appenderElement) {
            string attribute = appenderElement.GetAttribute("name");
            string attribute2 = appenderElement.GetAttribute("type");
            LogLog.Debug(declaringType, "Loading Appender [" + attribute + "] type: [" + attribute2 + "]");

            try {
                IAppender appender =
                    (IAppender)Activator.CreateInstance(SystemInfo.GetTypeFromString(attribute2, true, true));

                appender.Name = attribute;

                foreach (XmlNode childNode in appenderElement.ChildNodes) {
                    if (childNode.NodeType != XmlNodeType.Element) {
                        continue;
                    }

                    XmlElement xmlElement = (XmlElement)childNode;

                    if (xmlElement.LocalName == "appender-ref") {
                        string attribute3 = xmlElement.GetAttribute("ref");
                        IAppenderAttachable appenderAttachable = appender as IAppenderAttachable;

                        if (appenderAttachable != null) {
                            LogLog.Debug(declaringType,
                                "Attaching appender named [" + attribute3 + "] to appender named [" + appender.Name + "].");

                            IAppender appender2 = FindAppenderByReference(xmlElement);

                            if (appender2 != null) {
                                appenderAttachable.AddAppender(appender2);
                            }
                        } else {
                            LogLog.Error(declaringType,
                                "Requesting attachment of appender named [" +
                                attribute3 +
                                "] to appender named [" +
                                appender.Name +
                                "] which does not implement log4net.Core.IAppenderAttachable.");
                        }
                    } else {
                        SetParameter(xmlElement, appender);
                    }
                }

                IOptionHandler optionHandler = appender as IOptionHandler;

                if (optionHandler != null) {
                    optionHandler.ActivateOptions();
                }

                LogLog.Debug(declaringType, "Created Appender [" + attribute + "]");
                return appender;
            } catch (Exception exception) {
                LogLog.Error(declaringType,
                    "Could not create Appender [" + attribute + "] of type [" + attribute2 + "]. Reported error follows.",
                    exception);

                return null;
            }
        }

        protected void ParseLogger(XmlElement loggerElement) {
            string attribute = loggerElement.GetAttribute("name");

            LogLog.Debug(declaringType,
                "Retrieving an instance of log4net.Repository.Logger for logger [" + attribute + "].");

            Logger logger = m_hierarchy.GetLogger(attribute) as Logger;

            lock (logger) {
                bool flag = OptionConverter.ToBoolean(loggerElement.GetAttribute("additivity"), true);
                LogLog.Debug(declaringType, "Setting [" + logger.Name + "] additivity to [" + flag + "].");
                logger.Additivity = flag;
                ParseChildrenOfLoggerElement(loggerElement, logger, false);
            }
        }

        protected void ParseRoot(XmlElement rootElement) {
            Logger root = m_hierarchy.Root;

            lock (root) {
                ParseChildrenOfLoggerElement(rootElement, root, true);
            }
        }

        protected void ParseChildrenOfLoggerElement(XmlElement catElement, Logger log, bool isRoot) {
            log.RemoveAllAppenders();

            foreach (XmlNode childNode in catElement.ChildNodes) {
                if (childNode.NodeType != XmlNodeType.Element) {
                    continue;
                }

                XmlElement xmlElement = (XmlElement)childNode;

                if (xmlElement.LocalName == "appender-ref") {
                    IAppender appender = FindAppenderByReference(xmlElement);
                    string attribute = xmlElement.GetAttribute("ref");

                    if (appender != null) {
                        LogLog.Debug(declaringType,
                            "Adding appender named [" + attribute + "] to logger [" + log.Name + "].");

                        log.AddAppender(appender);
                    } else {
                        LogLog.Error(declaringType, "Appender named [" + attribute + "] not found.");
                    }
                } else if (xmlElement.LocalName == "level" || xmlElement.LocalName == "priority") {
                    ParseLevel(xmlElement, log, isRoot);
                } else {
                    SetParameter(xmlElement, log);
                }
            }

            IOptionHandler optionHandler = log as IOptionHandler;

            if (optionHandler != null) {
                optionHandler.ActivateOptions();
            }
        }

        protected void ParseRenderer(XmlElement element) {
            string attribute = element.GetAttribute("renderingClass");
            string attribute2 = element.GetAttribute("renderedClass");
            LogLog.Debug(declaringType, "Rendering class [" + attribute + "], Rendered class [" + attribute2 + "].");

            IObjectRenderer objectRenderer =
                (IObjectRenderer)OptionConverter.InstantiateByClassName(attribute, typeof(IObjectRenderer), null);

            if (objectRenderer == null) {
                LogLog.Error(declaringType, "Could not instantiate renderer [" + attribute + "].");
                return;
            }

            try {
                m_hierarchy.RendererMap.Put(SystemInfo.GetTypeFromString(attribute2, true, true), objectRenderer);
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Could not find class [" + attribute2 + "].", exception);
            }
        }

        protected void ParseLevel(XmlElement element, Logger log, bool isRoot) {
            string text = log.Name;

            if (isRoot) {
                text = "root";
            }

            string attribute = element.GetAttribute("value");
            LogLog.Debug(declaringType, "Logger [" + text + "] Level string is [" + attribute + "].");

            if ("inherited" == attribute) {
                if (isRoot) {
                    LogLog.Error(declaringType, "Root level cannot be inherited. Ignoring directive.");
                    return;
                }

                LogLog.Debug(declaringType, "Logger [" + text + "] level set to inherit from parent.");
                log.Level = null;
                return;
            }

            log.Level = log.Hierarchy.LevelMap[attribute];

            if (log.Level == null) {
                LogLog.Error(declaringType, "Undefined level [" + attribute + "] on Logger [" + text + "].");
            } else {
                LogLog.Debug(declaringType,
                    "Logger [" + text + "] level set to [name=\"" + log.Level.Name + "\",value=" + log.Level.Value + "].");
            }
        }

        protected void SetParameter(XmlElement element, object target) {
            string text = element.GetAttribute("name");

            if (element.LocalName != "param" || text == null || text.Length == 0) {
                text = element.LocalName;
            }

            Type type = target.GetType();
            Type type2 = null;
            PropertyInfo propertyInfo = null;
            MethodInfo methodInfo = null;

            propertyInfo = type.GetProperty(text,
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (propertyInfo != null && propertyInfo.CanWrite) {
                type2 = propertyInfo.PropertyType;
            } else {
                propertyInfo = null;
                methodInfo = FindMethodInfo(type, text);

                if (methodInfo != null) {
                    type2 = methodInfo.GetParameters()[0].ParameterType;
                }
            }

            if (type2 == null) {
                LogLog.Error(declaringType,
                    "XmlHierarchyConfigurator: Cannot find Property [" + text + "] to set object on [" + target + "]");

                return;
            }

            string text2 = null;

            if (element.GetAttributeNode("value") != null) {
                text2 = element.GetAttribute("value");
            } else if (element.HasChildNodes) {
                foreach (XmlNode childNode in element.ChildNodes) {
                    if (childNode.NodeType == XmlNodeType.CDATA || childNode.NodeType == XmlNodeType.Text) {
                        text2 = text2 != null ? text2 + childNode.InnerText : childNode.InnerText;
                    }
                }
            }

            if (text2 != null) {
                try {
                    IDictionary dictionary = Environment.GetEnvironmentVariables();

                    if (HasCaseInsensitiveEnvironment) {
                        dictionary = CreateCaseInsensitiveWrapper(dictionary);
                    }

                    text2 = OptionConverter.SubstituteVariables(text2, dictionary);
                } catch (SecurityException) {
                    LogLog.Debug(declaringType,
                        "Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
                }

                Type type3 = null;
                string attribute = element.GetAttribute("type");

                if (attribute != null && attribute.Length > 0) {
                    try {
                        Type typeFromString = SystemInfo.GetTypeFromString(attribute, true, true);

                        LogLog.Debug(declaringType,
                            "Parameter [" + text + "] specified subtype [" + typeFromString.FullName + "]");

                        if (!type2.IsAssignableFrom(typeFromString)) {
                            if (OptionConverter.CanConvertTypeTo(typeFromString, type2)) {
                                type3 = type2;
                                type2 = typeFromString;
                            } else {
                                LogLog.Error(declaringType,
                                    "subtype [" +
                                    typeFromString.FullName +
                                    "] set on [" +
                                    text +
                                    "] is not a subclass of property type [" +
                                    type2.FullName +
                                    "] and there are no acceptable type conversions.");
                            }
                        } else {
                            type2 = typeFromString;
                        }
                    } catch (Exception exception) {
                        LogLog.Error(declaringType,
                            "Failed to find type [" + attribute + "] set on [" + text + "]",
                            exception);
                    }
                }

                object obj = ConvertStringTo(type2, text2);

                if (obj != null && type3 != null) {
                    LogLog.Debug(declaringType,
                        "Performing additional conversion of value from [" +
                        obj.GetType().Name +
                        "] to [" +
                        type3.Name +
                        "]");

                    obj = OptionConverter.ConvertTypeTo(obj, type3);
                }

                if (obj != null) {
                    if (propertyInfo != null) {
                        LogLog.Debug(declaringType,
                            "Setting Property [" +
                            propertyInfo.Name +
                            "] to " +
                            obj.GetType().Name +
                            " value [" +
                            obj +
                            "]");

                        try {
                            propertyInfo.SetValue(target,
                                obj,
                                BindingFlags.SetProperty,
                                null,
                                null,
                                CultureInfo.InvariantCulture);

                            return;
                        } catch (TargetInvocationException ex2) {
                            LogLog.Error(declaringType,
                                string.Concat("Failed to set parameter [",
                                    propertyInfo.Name,
                                    "] on object [",
                                    target,
                                    "] using value [",
                                    obj,
                                    "]"),
                                ex2.InnerException);

                            return;
                        }
                    }

                    if (methodInfo != null) {
                        LogLog.Debug(declaringType,
                            "Setting Collection Property [" +
                            methodInfo.Name +
                            "] to " +
                            obj.GetType().Name +
                            " value [" +
                            obj +
                            "]");

                        try {
                            methodInfo.Invoke(target,
                                BindingFlags.InvokeMethod,
                                null,
                                new object[1] { obj },
                                CultureInfo.InvariantCulture);

                            return;
                        } catch (TargetInvocationException ex3) {
                            LogLog.Error(declaringType,
                                string.Concat("Failed to set parameter [",
                                    text,
                                    "] on object [",
                                    target,
                                    "] using value [",
                                    obj,
                                    "]"),
                                ex3.InnerException);

                            return;
                        }
                    }
                } else {
                    LogLog.Warn(declaringType,
                        string.Concat("Unable to set property [",
                            text,
                            "] on object [",
                            target,
                            "] using value [",
                            text2,
                            "] (with acceptable conversion types)"));
                }

                return;
            }

            object obj2 = null;

            if (type2 == typeof(string) && !HasAttributesOrElements(element)) {
                obj2 = string.Empty;
            } else {
                Type defaultTargetType = null;

                if (IsTypeConstructible(type2)) {
                    defaultTargetType = type2;
                }

                obj2 = CreateObjectFromXml(element, defaultTargetType, type2);
            }

            if (obj2 == null) {
                LogLog.Error(declaringType, "Failed to create object to set param: " + text);
                return;
            }

            if (propertyInfo != null) {
                LogLog.Debug(declaringType,
                    string.Concat("Setting Property [", propertyInfo.Name, "] to object [", obj2, "]"));

                try {
                    propertyInfo.SetValue(target, obj2, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                    return;
                } catch (TargetInvocationException ex4) {
                    LogLog.Error(declaringType,
                        string.Concat("Failed to set parameter [",
                            propertyInfo.Name,
                            "] on object [",
                            target,
                            "] using value [",
                            obj2,
                            "]"),
                        ex4.InnerException);

                    return;
                }
            }

            if (methodInfo == null) {
                return;
            }

            LogLog.Debug(declaringType,
                string.Concat("Setting Collection Property [", methodInfo.Name, "] to object [", obj2, "]"));

            try {
                methodInfo.Invoke(target,
                    BindingFlags.InvokeMethod,
                    null,
                    new object[1] { obj2 },
                    CultureInfo.InvariantCulture);
            } catch (TargetInvocationException ex5) {
                LogLog.Error(declaringType,
                    string.Concat("Failed to set parameter [",
                        methodInfo.Name,
                        "] on object [",
                        target,
                        "] using value [",
                        obj2,
                        "]"),
                    ex5.InnerException);
            }
        }

        bool HasAttributesOrElements(XmlElement element) {
            foreach (XmlNode childNode in element.ChildNodes) {
                if (childNode.NodeType == XmlNodeType.Attribute || childNode.NodeType == XmlNodeType.Element) {
                    return true;
                }
            }

            return false;
        }

        static bool IsTypeConstructible(Type type) {
            if (type.IsClass && !type.IsAbstract) {
                ConstructorInfo constructor = type.GetConstructor(new Type[0]);

                if (constructor != null && !constructor.IsAbstract && !constructor.IsPrivate) {
                    return true;
                }
            }

            return false;
        }

        MethodInfo FindMethodInfo(Type targetType, string name) {
            string strB = "Add" + name;

            MethodInfo[] methods =
                targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            MethodInfo[] array = methods;

            foreach (MethodInfo methodInfo in array) {
                if (!methodInfo.IsStatic &&
                    (string.Compare(methodInfo.Name, name, true, CultureInfo.InvariantCulture) == 0 ||
                     string.Compare(methodInfo.Name, strB, true, CultureInfo.InvariantCulture) == 0)) {
                    ParameterInfo[] parameters = methodInfo.GetParameters();

                    if (parameters.Length == 1) {
                        return methodInfo;
                    }
                }
            }

            return null;
        }

        protected object ConvertStringTo(Type type, string value) {
            if (typeof(Level) == type) {
                Level level = m_hierarchy.LevelMap[value];

                if (level == null) {
                    LogLog.Error(declaringType, "XmlHierarchyConfigurator: Unknown Level Specified [" + value + "]");
                }

                return level;
            }

            return OptionConverter.ConvertStringTo(type, value);
        }

        protected object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint) {
            Type type = null;
            string attribute = element.GetAttribute("type");

            if (attribute == null || attribute.Length == 0) {
                if (defaultTargetType == null) {
                    LogLog.Error(declaringType,
                        "Object type not specified. Cannot create object of type [" +
                        typeConstraint.FullName +
                        "]. Missing Value or Type.");

                    return null;
                }

                type = defaultTargetType;
            } else {
                try {
                    type = SystemInfo.GetTypeFromString(attribute, true, true);
                } catch (Exception exception) {
                    LogLog.Error(declaringType, "Failed to find type [" + attribute + "]", exception);
                    return null;
                }
            }

            bool flag = false;

            if (typeConstraint != null && !typeConstraint.IsAssignableFrom(type)) {
                if (!OptionConverter.CanConvertTypeTo(type, typeConstraint)) {
                    LogLog.Error(declaringType,
                        "Object type [" +
                        type.FullName +
                        "] is not assignable to type [" +
                        typeConstraint.FullName +
                        "]. There are no acceptable type conversions.");

                    return null;
                }

                flag = true;
            }

            object obj = null;

            try {
                obj = Activator.CreateInstance(type);
            } catch (Exception ex) {
                LogLog.Error(declaringType,
                    "XmlHierarchyConfigurator: Failed to construct object of type [" + type.FullName + "] Exception: " + ex);
            }

            foreach (XmlNode childNode in element.ChildNodes) {
                if (childNode.NodeType == XmlNodeType.Element) {
                    SetParameter((XmlElement)childNode, obj);
                }
            }

            IOptionHandler optionHandler = obj as IOptionHandler;

            if (optionHandler != null) {
                optionHandler.ActivateOptions();
            }

            if (flag) {
                return OptionConverter.ConvertTypeTo(obj, typeConstraint);
            }

            return obj;
        }

        IDictionary CreateCaseInsensitiveWrapper(IDictionary dict) {
            if (dict == null) {
                return dict;
            }

            Hashtable hashtable = SystemInfo.CreateCaseInsensitiveHashtable();

            foreach (DictionaryEntry item in dict) {
                hashtable[item.Key] = item.Value;
            }

            return hashtable;
        }

        enum ConfigUpdateMode {
            Merge = 0,
            Overwrite = 1
        }
    }
}