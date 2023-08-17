using System;

namespace log4net.Core {
    public class ExceptionEvaluator : ITriggeringEventEvaluator {
        public ExceptionEvaluator() { }

        public ExceptionEvaluator(Type exType, bool triggerOnSubClass) {
            if (exType == null) {
                throw new ArgumentNullException("exType");
            }

            ExceptionType = exType;
            TriggerOnSubclass = triggerOnSubClass;
        }

        public Type ExceptionType { get; set; }

        public bool TriggerOnSubclass { get; set; }

        public bool IsTriggeringEvent(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (TriggerOnSubclass && loggingEvent.ExceptionObject != null) {
                Type type = loggingEvent.ExceptionObject.GetType();
                return type == ExceptionType || type.IsSubclassOf(ExceptionType);
            }

            if (!TriggerOnSubclass && loggingEvent.ExceptionObject != null) {
                return loggingEvent.ExceptionObject.GetType() == ExceptionType;
            }

            return false;
        }
    }
}