using System;
using System.Text;
using log4net.Core;
using log4net.Util;

namespace log4net.Layout.Pattern {
    class StackTraceDetailPatternConverter : StackTracePatternConverter {
        static readonly Type declaringType = typeof(StackTracePatternConverter);

        internal override string GetMethodInformation(MethodItem method) {
            string result = string.Empty;

            try {
                string text = string.Empty;
                string[] parameters = method.Parameters;
                StringBuilder stringBuilder = new();

                if (parameters != null && parameters.GetUpperBound(0) > 0) {
                    for (int i = 0; i <= parameters.GetUpperBound(0); i++) {
                        stringBuilder.AppendFormat("{0}, ", parameters[i]);
                    }
                }

                if (stringBuilder.Length > 0) {
                    stringBuilder.Remove(stringBuilder.Length - 2, 2);
                    text = stringBuilder.ToString();
                }

                result = base.GetMethodInformation(method) + "(" + text + ")";
            } catch (Exception exception) {
                LogLog.Error(declaringType, "An exception ocurred while retreiving method information.", exception);
            }

            return result;
        }
    }
}