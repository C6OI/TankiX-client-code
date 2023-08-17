using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using log4net.Util;

namespace log4net.Core {
    [Serializable]
    public class LocationInfo {
        const string NA = "?";

        static readonly Type declaringType = typeof(LocationInfo);

        public LocationInfo(Type callerStackBoundaryDeclaringType) {
            ClassName = "?";
            FileName = "?";
            LineNumber = "?";
            MethodName = "?";
            FullInfo = "?";

            if (callerStackBoundaryDeclaringType == null) {
                return;
            }

            try {
                StackTrace stackTrace = new(true);
                int i;

                for (i = 0; i < stackTrace.FrameCount; i++) {
                    StackFrame frame = stackTrace.GetFrame(i);

                    if (frame != null && frame.GetMethod().DeclaringType == callerStackBoundaryDeclaringType) {
                        break;
                    }
                }

                for (; i < stackTrace.FrameCount; i++) {
                    StackFrame frame2 = stackTrace.GetFrame(i);

                    if (frame2 != null && frame2.GetMethod().DeclaringType != callerStackBoundaryDeclaringType) {
                        break;
                    }
                }

                if (i >= stackTrace.FrameCount) {
                    return;
                }

                int num = stackTrace.FrameCount - i;
                ArrayList arrayList = new(num);
                StackFrames = new StackFrameItem[num];

                for (int j = i; j < stackTrace.FrameCount; j++) {
                    arrayList.Add(new StackFrameItem(stackTrace.GetFrame(j)));
                }

                arrayList.CopyTo(StackFrames, 0);
                StackFrame frame3 = stackTrace.GetFrame(i);

                if (frame3 == null) {
                    return;
                }

                MethodBase method = frame3.GetMethod();

                if (method != null) {
                    MethodName = method.Name;

                    if (method.DeclaringType != null) {
                        ClassName = method.DeclaringType.FullName;
                    }
                }

                FileName = frame3.GetFileName();
                LineNumber = frame3.GetFileLineNumber().ToString(NumberFormatInfo.InvariantInfo);
                FullInfo = ClassName + '.' + MethodName + '(' + FileName + ':' + LineNumber + ')';
            } catch (SecurityException) {
                LogLog.Debug(declaringType,
                    "Security exception while trying to get caller stack frame. Error Ignored. Location Information Not Available.");
            }
        }

        public LocationInfo(string className, string methodName, string fileName, string lineNumber) {
            ClassName = className;
            FileName = fileName;
            LineNumber = lineNumber;
            MethodName = methodName;
            FullInfo = ClassName + '.' + MethodName + '(' + FileName + ':' + LineNumber + ')';
        }

        public string ClassName { get; }

        public string FileName { get; }

        public string LineNumber { get; }

        public string MethodName { get; }

        public string FullInfo { get; }

        public StackFrameItem[] StackFrames { get; }
    }
}