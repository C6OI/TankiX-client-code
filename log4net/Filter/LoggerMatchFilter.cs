using System;
using log4net.Core;

namespace log4net.Filter {
    public class LoggerMatchFilter : FilterSkeleton {
        public bool AcceptOnMatch { get; set; } = true;

        public string LoggerToMatch { get; set; }

        public override FilterDecision Decide(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (LoggerToMatch != null && LoggerToMatch.Length != 0 && loggingEvent.LoggerName.StartsWith(LoggerToMatch)) {
                if (AcceptOnMatch) {
                    return FilterDecision.Accept;
                }

                return FilterDecision.Deny;
            }

            return FilterDecision.Neutral;
        }
    }
}