using System;

namespace log4net.Core {
    public class LevelEvaluator : ITriggeringEventEvaluator {
        public LevelEvaluator()
            : this(Level.Off) { }

        public LevelEvaluator(Level threshold) {
            if (threshold == null) {
                throw new ArgumentNullException("threshold");
            }

            Threshold = threshold;
        }

        public Level Threshold { get; set; }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            return loggingEvent.Level >= Threshold;
        }
    }
}