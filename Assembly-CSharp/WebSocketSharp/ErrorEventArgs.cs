using System;

namespace WebSocketSharp {
    public class ErrorEventArgs : EventArgs {
        internal ErrorEventArgs(string message)
            : this(message, null) { }

        internal ErrorEventArgs(string message, Exception exception) {
            Message = message;
            Exception = exception;
        }

        public Exception Exception { get; }

        public string Message { get; }
    }
}