using System;
using log4net.Core;

namespace log4net.Filter {
    public class LevelMatchFilter : FilterSkeleton {
        public bool AcceptOnMatch { get; set; } = true;

        public Level LevelToMatch { get; set; }

        public override FilterDecision Decide(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (LevelToMatch != null && LevelToMatch == loggingEvent.Level) {
                return AcceptOnMatch ? FilterDecision.Accept : FilterDecision.Deny;
            }

            return FilterDecision.Neutral;
        }
    }
}