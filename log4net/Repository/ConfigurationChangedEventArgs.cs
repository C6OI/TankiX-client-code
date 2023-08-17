using System;
using System.Collections;

namespace log4net.Repository {
    public class ConfigurationChangedEventArgs : EventArgs {
        public ConfigurationChangedEventArgs(ICollection configurationMessages) =>
            ConfigurationMessages = configurationMessages;

        public ICollection ConfigurationMessages { get; }
    }
}