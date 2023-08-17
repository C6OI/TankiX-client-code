namespace log4net {
    public sealed class MDC {
        MDC() { }

        public static string Get(string key) {
            object obj = ThreadContext.Properties[key];

            if (obj == null) {
                return null;
            }

            return obj.ToString();
        }

        public static void Set(string key, string value) => ThreadContext.Properties[key] = value;

        public static void Remove(string key) => ThreadContext.Properties.Remove(key);

        public static void Clear() => ThreadContext.Properties.Clear();
    }
}