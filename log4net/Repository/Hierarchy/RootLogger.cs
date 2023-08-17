using System;
using log4net.Core;
using log4net.Util;

namespace log4net.Repository.Hierarchy {
    public class RootLogger : Logger {
        static readonly Type declaringType = typeof(RootLogger);

        public RootLogger(Level level)
            : base("root") => Level = level;

        public override Level EffectiveLevel => base.Level;

        public override Level Level {
            get => base.Level;
            set {
                if (value == null) {
                    LogLog.Error(declaringType, "You have tried to set a null level to root.", new LogException());
                } else {
                    base.Level = value;
                }
            }
        }
    }
}