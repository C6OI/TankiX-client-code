using System;
using System.Collections;
using log4net.Core;

namespace log4net.Util {
    public sealed class LevelMapping : IOptionHandler {
        readonly Hashtable m_entriesMap = new();
        LevelMappingEntry[] m_entries;

        public void ActivateOptions() {
            Level[] array = new Level[m_entriesMap.Count];
            LevelMappingEntry[] array2 = new LevelMappingEntry[m_entriesMap.Count];
            m_entriesMap.Keys.CopyTo(array, 0);
            m_entriesMap.Values.CopyTo(array2, 0);
            Array.Sort(array, array2, 0, array.Length, null);
            Array.Reverse(array2, 0, array2.Length);
            LevelMappingEntry[] array3 = array2;

            foreach (LevelMappingEntry levelMappingEntry in array3) {
                levelMappingEntry.ActivateOptions();
            }

            m_entries = array2;
        }

        public void Add(LevelMappingEntry entry) {
            if (m_entriesMap.ContainsKey(entry.Level)) {
                m_entriesMap.Remove(entry.Level);
            }

            m_entriesMap.Add(entry.Level, entry);
        }

        public LevelMappingEntry Lookup(Level level) {
            if (m_entries != null) {
                LevelMappingEntry[] entries = m_entries;

                foreach (LevelMappingEntry levelMappingEntry in entries) {
                    if (level >= levelMappingEntry.Level) {
                        return levelMappingEntry;
                    }
                }
            }

            return null;
        }
    }
}