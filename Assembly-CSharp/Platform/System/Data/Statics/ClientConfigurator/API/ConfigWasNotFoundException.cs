using System;

namespace Platform.System.Data.Statics.ClientConfigurator.API {
    public class ConfigWasNotFoundException : Exception {
        public ConfigWasNotFoundException(string configPath)
            : base("configPath: " + configPath) { }
    }
}