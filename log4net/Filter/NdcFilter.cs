namespace log4net.Filter {
    public class NdcFilter : PropertyFilter {
        public NdcFilter() => Key = "NDC";
    }
}