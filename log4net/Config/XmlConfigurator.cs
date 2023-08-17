using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Xml;
using log4net.Repository;
using log4net.Util;

namespace log4net.Config {
    public sealed class XmlConfigurator {
        static readonly Hashtable m_repositoryName2ConfigAndWatchHandler = new();

        static readonly Type declaringType = typeof(XmlConfigurator);

        XmlConfigurator() { }

        public static ICollection Configure() => Configure(LogManager.GetRepository(Assembly.GetCallingAssembly()));

        public static ICollection Configure(ILoggerRepository repository) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        static void InternalConfigure(ILoggerRepository repository) {
            LogLog.Debug(declaringType, "configuring repository [" + repository.Name + "] using .config file section");

            try {
                LogLog.Debug(declaringType, "Application config file is [" + SystemInfo.ConfigurationFileLocation + "]");
            } catch {
                LogLog.Debug(declaringType, "Application config file location unknown");
            }

            try {
                XmlElement xmlElement = null;
                xmlElement = ConfigurationSettings.GetConfig("log4net") as XmlElement;

                if (xmlElement == null) {
                    LogLog.Error(declaringType,
                        "Failed to find configuration section 'log4net' in the application's .config file. Check your .config file for the <log4net> and <configSections> elements. The configuration section should look like: <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net\" />");
                } else {
                    InternalConfigureFromXml(repository, xmlElement);
                }
            } catch (ConfigurationException ex) {
                if (ex.BareMessage.IndexOf("Unrecognized element") >= 0) {
                    LogLog.Error(declaringType,
                        "Failed to parse config file. Check your .config file is well formed XML.",
                        ex);

                    return;
                }

                string text = "<section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler," +
                              Assembly.GetExecutingAssembly().FullName +
                              "\" />";

                LogLog.Error(declaringType,
                    "Failed to parse config file. Is the <configSections> specified as: " + text,
                    ex);
            }
        }

        public static ICollection Configure(XmlElement element) {
            ArrayList arrayList = new();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigureFromXml(repository, element);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection Configure(ILoggerRepository repository, XmlElement element) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                LogLog.Debug(declaringType, "configuring repository [" + repository.Name + "] using XML element");
                InternalConfigureFromXml(repository, element);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection Configure(FileInfo configFile) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(LogManager.GetRepository(Assembly.GetCallingAssembly()), configFile);
                return arrayList;
            }
        }

        public static ICollection Configure(Uri configUri) {
            ArrayList arrayList = new();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, configUri);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection Configure(Stream configStream) {
            ArrayList arrayList = new();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, configStream);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection Configure(ILoggerRepository repository, FileInfo configFile) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, configFile);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        static void InternalConfigure(ILoggerRepository repository, FileInfo configFile) {
            LogLog.Debug(declaringType,
                string.Concat("configuring repository [", repository.Name, "] using file [", configFile, "]"));

            if (configFile == null) {
                LogLog.Error(declaringType, "Configure called with null 'configFile' parameter");
            } else if (File.Exists(configFile.FullName)) {
                FileStream fileStream = null;
                int num = 5;

                while (--num >= 0) {
                    try {
                        fileStream = configFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                    } catch (IOException exception) {
                        if (num == 0) {
                            LogLog.Error(declaringType,
                                "Failed to open XML config file [" + configFile.Name + "]",
                                exception);

                            fileStream = null;
                        }

                        Thread.Sleep(250);
                        continue;
                    }

                    break;
                }

                if (fileStream != null) {
                    try {
                        InternalConfigure(repository, fileStream);
                    } finally {
                        fileStream.Close();
                    }
                }
            } else {
                LogLog.Debug(declaringType, "config file [" + configFile.FullName + "] not found. Configuration unchanged.");
            }
        }

        public static ICollection Configure(ILoggerRepository repository, Uri configUri) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, configUri);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        static void InternalConfigure(ILoggerRepository repository, Uri configUri) {
            LogLog.Debug(declaringType,
                string.Concat("configuring repository [", repository.Name, "] using URI [", configUri, "]"));

            if (configUri == null) {
                LogLog.Error(declaringType, "Configure called with null 'configUri' parameter");
                return;
            }

            if (configUri.IsFile) {
                InternalConfigure(repository, new FileInfo(configUri.LocalPath));
                return;
            }

            WebRequest webRequest = null;

            try {
                webRequest = WebRequest.Create(configUri);
            } catch (Exception exception) {
                LogLog.Error(declaringType,
                    string.Concat("Failed to create WebRequest for URI [", configUri, "]"),
                    exception);
            }

            if (webRequest == null) {
                return;
            }

            try {
                webRequest.Credentials = CredentialCache.DefaultCredentials;
            } catch { }

            try {
                WebResponse response = webRequest.GetResponse();

                if (response == null) {
                    return;
                }

                try {
                    using (Stream configStream = response.GetResponseStream()) {
                        InternalConfigure(repository, configStream);
                    }
                } finally {
                    response.Close();
                }
            } catch (Exception exception2) {
                LogLog.Error(declaringType,
                    string.Concat("Failed to request config from URI [", configUri, "]"),
                    exception2);
            }
        }

        public static ICollection Configure(ILoggerRepository repository, Stream configStream) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigure(repository, configStream);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        static void InternalConfigure(ILoggerRepository repository, Stream configStream) {
            LogLog.Debug(declaringType, "configuring repository [" + repository.Name + "] using stream");

            if (configStream == null) {
                LogLog.Error(declaringType, "Configure called with null 'configStream' parameter");
                return;
            }

            XmlDocument xmlDocument = new();

            try {
                XmlValidatingReader xmlValidatingReader = new(new XmlTextReader(configStream));
                xmlValidatingReader.ValidationType = ValidationType.None;
                xmlValidatingReader.EntityHandling = EntityHandling.ExpandEntities;
                xmlDocument.Load(xmlValidatingReader);
            } catch (Exception exception) {
                LogLog.Error(declaringType, "Error while loading XML configuration", exception);
                xmlDocument = null;
            }

            if (xmlDocument != null) {
                LogLog.Debug(declaringType, "loading XML configuration");
                XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("log4net");

                if (elementsByTagName.Count == 0) {
                    LogLog.Debug(declaringType,
                        "XML configuration does not contain a <log4net> element. Configuration Aborted.");
                } else if (elementsByTagName.Count > 1) {
                    LogLog.Error(declaringType,
                        "XML configuration contains [" +
                        elementsByTagName.Count +
                        "] <log4net> elements. Only one is allowed. Configuration Aborted.");
                } else {
                    InternalConfigureFromXml(repository, elementsByTagName[0] as XmlElement);
                }
            }
        }

        public static ICollection ConfigureAndWatch(FileInfo configFile) {
            ArrayList arrayList = new();
            ILoggerRepository repository = LogManager.GetRepository(Assembly.GetCallingAssembly());

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigureAndWatch(repository, configFile);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        public static ICollection ConfigureAndWatch(ILoggerRepository repository, FileInfo configFile) {
            ArrayList arrayList = new();

            using (new LogLog.LogReceivedAdapter(arrayList)) {
                InternalConfigureAndWatch(repository, configFile);
            }

            repository.ConfigurationMessages = arrayList;
            return arrayList;
        }

        static void InternalConfigureAndWatch(ILoggerRepository repository, FileInfo configFile) {
            LogLog.Debug(declaringType,
                string.Concat("configuring repository [",
                    repository.Name,
                    "] using file [",
                    configFile,
                    "] watching for file updates"));

            if (configFile == null) {
                LogLog.Error(declaringType, "ConfigureAndWatch called with null 'configFile' parameter");
                return;
            }

            InternalConfigure(repository, configFile);

            try {
                lock (m_repositoryName2ConfigAndWatchHandler) {
                    ConfigureAndWatchHandler configureAndWatchHandler =
                        (ConfigureAndWatchHandler)m_repositoryName2ConfigAndWatchHandler[configFile.FullName];

                    if (configureAndWatchHandler != null) {
                        m_repositoryName2ConfigAndWatchHandler.Remove(configFile.FullName);
                        configureAndWatchHandler.Dispose();
                    }

                    configureAndWatchHandler = new ConfigureAndWatchHandler(repository, configFile);
                    m_repositoryName2ConfigAndWatchHandler[configFile.FullName] = configureAndWatchHandler;
                }
            } catch (Exception exception) {
                LogLog.Error(declaringType,
                    "Failed to initialize configuration file watcher for file [" + configFile.FullName + "]",
                    exception);
            }
        }

        static void InternalConfigureFromXml(ILoggerRepository repository, XmlElement element) {
            if (element == null) {
                LogLog.Error(declaringType, "ConfigureFromXml called with null 'element' parameter");
                return;
            }

            if (repository == null) {
                LogLog.Error(declaringType, "ConfigureFromXml called with null 'repository' parameter");
                return;
            }

            LogLog.Debug(declaringType, "Configuring Repository [" + repository.Name + "]");
            IXmlRepositoryConfigurator xmlRepositoryConfigurator = repository as IXmlRepositoryConfigurator;

            if (xmlRepositoryConfigurator == null) {
                LogLog.Warn(declaringType,
                    string.Concat("Repository [", repository, "] does not support the XmlConfigurator"));

                return;
            }

            XmlDocument xmlDocument = new();
            XmlElement element2 = (XmlElement)xmlDocument.AppendChild(xmlDocument.ImportNode(element, true));
            xmlRepositoryConfigurator.Configure(element2);
        }

        sealed class ConfigureAndWatchHandler : IDisposable {
            const int TimeoutMillis = 500;

            readonly FileInfo m_configFile;

            readonly ILoggerRepository m_repository;

            readonly Timer m_timer;

            readonly FileSystemWatcher m_watcher;

            public ConfigureAndWatchHandler(ILoggerRepository repository, FileInfo configFile) {
                m_repository = repository;
                m_configFile = configFile;
                m_watcher = new FileSystemWatcher();
                m_watcher.Path = m_configFile.DirectoryName;
                m_watcher.Filter = m_configFile.Name;
                m_watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;
                m_watcher.Changed += ConfigureAndWatchHandler_OnChanged;
                m_watcher.Created += ConfigureAndWatchHandler_OnChanged;
                m_watcher.Deleted += ConfigureAndWatchHandler_OnChanged;
                m_watcher.Renamed += ConfigureAndWatchHandler_OnRenamed;
                m_watcher.EnableRaisingEvents = true;
                m_timer = new Timer(OnWatchedFileChange, null, -1, -1);
            }

            public void Dispose() {
                m_watcher.EnableRaisingEvents = false;
                m_watcher.Dispose();
                m_timer.Dispose();
            }

            void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e) {
                LogLog.Debug(declaringType,
                    string.Concat("ConfigureAndWatchHandler: ", e.ChangeType, " [", m_configFile.FullName, "]"));

                m_timer.Change(500, -1);
            }

            void ConfigureAndWatchHandler_OnRenamed(object source, RenamedEventArgs e) {
                LogLog.Debug(declaringType,
                    string.Concat("ConfigureAndWatchHandler: ", e.ChangeType, " [", m_configFile.FullName, "]"));

                m_timer.Change(500, -1);
            }

            void OnWatchedFileChange(object state) => InternalConfigure(m_repository, m_configFile);
        }
    }
}