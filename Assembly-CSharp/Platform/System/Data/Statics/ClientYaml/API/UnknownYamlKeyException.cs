using System;

namespace Platform.System.Data.Statics.ClientYaml.API {
    public class UnknownYamlKeyException : Exception {
        public UnknownYamlKeyException(string key)
            : base(key) { }
    }
}