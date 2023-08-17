using System;
using log4net.Core;

namespace log4net.Filter {
    public class LevelRangeFilter : FilterSkeleton {
        public bool AcceptOnMatch { get; set; } = true;

        public Level LevelMin { get; set; }

        public Level LevelMax { get; set; }

        public override FilterDecision Decide(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (LevelMin != null && loggingEvent.Level < LevelMin) {
                return FilterDecision.Deny;
            }

            if (LevelMax != null && loggingEvent.Level > LevelMax) {
                return FilterDecision.Deny;
            }

            if (AcceptOnMatch) {
                return FilterDecision.Accept;
            }

            return FilterDecision.Neutral;
        }
    }
}