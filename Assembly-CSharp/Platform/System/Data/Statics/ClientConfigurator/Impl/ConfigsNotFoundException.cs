using System;

namespace Platform.System.Data.Statics.ClientConfigurator.Impl {
    public class ConfigsNotFoundException : Exception {
        public ConfigsNotFoundException(string message)
            : base(message) { }
    }
}