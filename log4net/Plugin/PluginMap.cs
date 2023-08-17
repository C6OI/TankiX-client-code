using System;
using System.Collections;
using log4net.Repository;

namespace log4net.Plugin {
    public sealed class PluginMap {
        readonly Hashtable m_mapName2Plugin = new();

        readonly ILoggerRepository m_repository;

        public PluginMap(ILoggerRepository repository) => m_repository = repository;

        public IPlugin this[string name] {
            get {
                if (name == null) {
                    throw new ArgumentNullException("name");
                }

                lock (this) {
                    return (IPlugin)m_mapName2Plugin[name];
                }
            }
        }

        public PluginCollection AllPlugins {
            get {
                lock (this) {
                    return new PluginCollection(m_mapName2Plugin.Values);
                }
            }
        }

        public void Add(IPlugin plugin) {
            if (plugin == null) {
                throw new ArgumentNullException("plugin");
            }

            IPlugin plugin2 = null;

            lock (this) {
                plugin2 = m_mapName2Plugin[plugin.Name] as IPlugin;
                m_mapName2Plugin[plugin.Name] = plugin;
            }

            if (plugin2 != null) {
                plugin2.Shutdown();
            }

            plugin.Attach(m_repository);
        }

        public void Remove(IPlugin plugin) {
            if (plugin == null) {
                throw new ArgumentNullException("plugin");
            }

            lock (this) {
                m_mapName2Plugin.Remove(plugin.Name);
            }
        }
    }
}