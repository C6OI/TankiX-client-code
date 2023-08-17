using System;

namespace Platform.Library.ClientResources.API {
    public class ParameterNotFoundException : Exception {
        public ParameterNotFoundException(string paramName)
            : base(paramName) { }
    }
}