using System.Globalization;
using log4net.Core;

namespace log4net.Util.PatternStringConverters {
    sealed class NewLinePatternConverter : LiteralPatternConverter, IOptionHandler {
        public void ActivateOptions() {
            if (string.Compare(Option, "DOS", true, CultureInfo.InvariantCulture) == 0) {
                Option = "\r\n";
            } else if (string.Compare(Option, "UNIX", true, CultureInfo.InvariantCulture) == 0) {
                Option = "\n";
            } else {
                Option = SystemInfo.NewLine;
            }
        }
    }
}