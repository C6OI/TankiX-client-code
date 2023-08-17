using log4net.Core;

namespace log4net.Filter {
    public abstract class FilterSkeleton : IOptionHandler, IFilter {
        public IFilter Next { get; set; }

        public abstract FilterDecision Decide(LoggingEvent loggingEvent);

        public virtual void ActivateOptions() { }
    }
}