namespace log4net.Util {
    public class PropertyEntry {
        public string Key { get; set; }

        public object Value { get; set; }

        public override string ToString() => string.Concat("PropertyEntry(Key=", Key, ", Value=", Value, ")");
    }
}