using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using log4net.Util;

namespace log4net.Core {
    [Serializable]
    public class StackFrameItem {
        const string NA = "?";

        static readonly Type declaringType = typeof(StackFrameItem);

        public StackFrameItem(StackFrame frame) {
            LineNumber = "?";
            FileName = "?";
            Method = new MethodItem();
            ClassName = "?";

            try {
                LineNumber = frame.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
                FileName = frame.GetFileName();
                MethodBase method = frame.GetMethod();

                if (method != null) {
                    if (method.DeclaringType != null) {
                        ClassName = method.DeclaringType.FullName;
                    }

                    Method = new MethodItem(method);
                }
            } catch (Exception exception) {
                LogLog.Error(declaringType, "An exception ocurred while retreiving stack frame information.", exception);
            }

            FullInfo = ClassName + '.' + Method.Name + '(' + FileName + ':' + LineNumber + ')';
        }

        public string ClassName { get; }

        public string FileName { get; }

        public string LineNumber { get; }

        public MethodItem Method { get; }

        public string FullInfo { get; }
    }
}