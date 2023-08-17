using System;
using log4net.Core;

namespace log4net.Util {
    public class OnlyOnceErrorHandler : IErrorHandler {
        static readonly Type declaringType = typeof(OnlyOnceErrorHandler);

        readonly string m_prefix;

        public OnlyOnceErrorHandler() => m_prefix = string.Empty;

        public OnlyOnceErrorHandler(string prefix) => m_prefix = prefix;

        public bool IsEnabled { get; private set; } = true;

        public DateTime EnabledDate { get; private set; }

        public string ErrorMessage { get; private set; }

        public Exception Exception { get; private set; }

        public ErrorCode ErrorCode { get; private set; }

        public void Error(string message, Exception e, ErrorCode errorCode) {
            if (IsEnabled) {
                FirstError(message, e, errorCode);
            }
        }

        public void Error(string message, Exception e) => Error(message, e, ErrorCode.GenericFailure);

        public void Error(string message) => Error(message, null, ErrorCode.GenericFailure);

        public void Reset() {
            EnabledDate = DateTime.MinValue;
            ErrorCode = ErrorCode.GenericFailure;
            Exception = null;
            ErrorMessage = null;
            IsEnabled = true;
        }

        public virtual void FirstError(string message, Exception e, ErrorCode errorCode) {
            EnabledDate = DateTime.Now;
            ErrorCode = errorCode;
            Exception = e;
            ErrorMessage = message;
            IsEnabled = false;

            if (LogLog.InternalDebugging && !LogLog.QuietMode) {
                LogLog.Error(declaringType, "[" + m_prefix + "] ErrorCode: " + errorCode + ". " + message, e);
            }
        }
    }
}