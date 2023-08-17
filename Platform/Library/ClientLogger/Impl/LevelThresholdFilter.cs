using System;
using log4net.Core;
using log4net.Filter;

namespace Platform.Library.ClientLogger.Impl {
    public class LevelThresholdFilter : FilterSkeleton {
        public LevelThresholdFilter() => ThresholdLevel = Level.All;

        public Level ThresholdLevel { get; set; }

        public override FilterDecision Decide(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException(loggingEvent.GetType().FullName);
            }

            if (loggingEvent.Level >= ThresholdLevel) {
                return FilterDecision.Accept;
            }

            return FilterDecision.Neutral;
        }
    }
}