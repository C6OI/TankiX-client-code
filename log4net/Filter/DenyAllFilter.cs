using log4net.Core;

namespace log4net.Filter {
    public sealed class DenyAllFilter : FilterSkeleton {
        public override FilterDecision Decide(LoggingEvent loggingEvent) => FilterDecision.Deny;
    }
}