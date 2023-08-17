using log4net.Util;

namespace log4net.Layout {
    public class DynamicPatternLayout : PatternLayout {
        PatternString m_footerPatternString = new(string.Empty);
        PatternString m_headerPatternString = new(string.Empty);

        public DynamicPatternLayout() { }

        public DynamicPatternLayout(string pattern)
            : base(pattern) { }

        public override string Header {
            get => m_headerPatternString.Format();
            set {
                base.Header = value;
                m_headerPatternString = new PatternString(value);
            }
        }

        public override string Footer {
            get => m_footerPatternString.Format();
            set {
                base.Footer = value;
                m_footerPatternString = new PatternString(value);
            }
        }
    }
}