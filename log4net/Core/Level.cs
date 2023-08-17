using System;

namespace log4net.Core {
    [Serializable]
    public sealed class Level : IComparable {
        public static readonly Level Off = new(int.MaxValue, "OFF");

        public static readonly Level Log4Net_Debug = new(120000, "log4net:DEBUG");

        public static readonly Level Emergency = new(120000, "EMERGENCY");

        public static readonly Level Fatal = new(110000, "FATAL");

        public static readonly Level Alert = new(100000, "ALERT");

        public static readonly Level Critical = new(90000, "CRITICAL");

        public static readonly Level Severe = new(80000, "SEVERE");

        public static readonly Level Error = new(70000, "ERROR");

        public static readonly Level Warn = new(60000, "WARN");

        public static readonly Level Notice = new(50000, "NOTICE");

        public static readonly Level Info = new(40000, "INFO");

        public static readonly Level Debug = new(30000, "DEBUG");

        public static readonly Level Fine = new(30000, "FINE");

        public static readonly Level Trace = new(20000, "TRACE");

        public static readonly Level Finer = new(20000, "FINER");

        public static readonly Level Verbose = new(10000, "VERBOSE");

        public static readonly Level Finest = new(10000, "FINEST");

        public static readonly Level All = new(int.MinValue, "ALL");

        public Level(int level, string levelName, string displayName) {
            if (levelName == null) {
                throw new ArgumentNullException("levelName");
            }

            if (displayName == null) {
                throw new ArgumentNullException("displayName");
            }

            Value = level;
            Name = string.Intern(levelName);
            DisplayName = displayName;
        }

        public Level(int level, string levelName)
            : this(level, levelName, levelName) { }

        public string Name { get; }

        public int Value { get; }

        public string DisplayName { get; }

        public int CompareTo(object r) {
            Level level = r as Level;

            if (level != null) {
                return Compare(this, level);
            }

            throw new ArgumentException(string.Concat("Parameter: r, Value: [", r, "] is not an instance of Level"));
        }

        public override string ToString() => Name;

        public override bool Equals(object o) {
            Level level = o as Level;

            if (level != null) {
                return Value == level.Value;
            }

            return base.Equals(o);
        }

        public override int GetHashCode() => Value;

        public static int Compare(Level l, Level r) {
            if ((object)l == r) {
                return 0;
            }

            if (l == null && r == null) {
                return 0;
            }

            if (l == null) {
                return -1;
            }

            if (r == null) {
                return 1;
            }

            return l.Value.CompareTo(r.Value);
        }

        public static bool operator >(Level l, Level r) => l.Value > r.Value;

        public static bool operator <(Level l, Level r) => l.Value < r.Value;

        public static bool operator >=(Level l, Level r) => l.Value >= r.Value;

        public static bool operator <=(Level l, Level r) => l.Value <= r.Value;

        public static bool operator ==(Level l, Level r) {
            if ((object)l != null && (object)r != null) {
                return l.Value == r.Value;
            }

            return (object)l == r;
        }

        public static bool operator !=(Level l, Level r) => !(l == r);
    }
}