namespace log4net.Util {
    public class FormattingInfo {
        public FormattingInfo() { }

        public FormattingInfo(int min, int max, bool leftAlign) {
            Min = min;
            Max = max;
            LeftAlign = leftAlign;
        }

        public int Min { get; set; } = -1;

        public int Max { get; set; } = int.MaxValue;

        public bool LeftAlign { get; set; }
    }
}