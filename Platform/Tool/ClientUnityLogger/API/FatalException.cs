using System;

namespace Platform.Tool.ClientUnityLogger.API {
    public class FatalException : Exception {
        public FatalException() { }

        public FatalException(string message)
            : base(message) { }
    }
}