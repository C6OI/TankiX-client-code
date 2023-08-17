using System;
using System.Collections;
using log4net.Util;

namespace log4net.Core {
    public sealed class LevelMap {
        readonly Hashtable m_mapName2Level = SystemInfo.CreateCaseInsensitiveHashtable();

        public Level this[string name] {
            get {
                if (name == null) {
                    throw new ArgumentNullException("name");
                }

                lock (this) {
                    return (Level)m_mapName2Level[name];
                }
            }
        }

        public LevelCollection AllLevels {
            get {
                lock (this) {
                    return new LevelCollection(m_mapName2Level.Values);
                }
            }
        }

        public void Clear() => m_mapName2Level.Clear();

        public void Add(string name, int value) => Add(name, value, null);

        public void Add(string name, int value, string displayName) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (name.Length == 0) {
                throw SystemInfo.CreateArgumentOutOfRangeException("name",
                    name,
                    "Parameter: name, Value: [" + name + "] out of range. Level name must not be empty");
            }

            if (displayName == null || displayName.Length == 0) {
                displayName = name;
            }

            Add(new Level(value, name, displayName));
        }

        public void Add(Level level) {
            if (level == null) {
                throw new ArgumentNullException("level");
            }

            lock (this) {
                m_mapName2Level[level.Name] = level;
            }
        }

        public Level LookupWithDefault(Level defaultLevel) {
            if (defaultLevel == null) {
                throw new ArgumentNullException("defaultLevel");
            }

            lock (this) {
                Level level = (Level)m_mapName2Level[defaultLevel.Name];

                if (level == null) {
                    m_mapName2Level[defaultLevel.Name] = defaultLevel;
                    return defaultLevel;
                }

                return level;
            }
        }
    }
}