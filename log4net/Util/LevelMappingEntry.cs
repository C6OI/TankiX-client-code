using log4net.Core;

namespace log4net.Util {
    public abstract class LevelMappingEntry : IOptionHandler {
        public Level Level { get; set; }

        public virtual void ActivateOptions() { }
    }
}