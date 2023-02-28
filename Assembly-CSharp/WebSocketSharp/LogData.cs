using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace WebSocketSharp {
    public class LogData {
        internal LogData(LogLevel level, StackFrame caller, string message) {
            Level = level;
            Caller = caller;
            Message = message ?? string.Empty;
            Date = DateTime.Now;
        }

        public StackFrame Caller { get; }

        public DateTime Date { get; }

        public LogLevel Level { get; }

        public string Message { get; }

        public override string ToString() {
            string text = string.Format("{0}|{1,-5}|", Date, Level);
            MethodBase method = Caller.GetMethod();
            Type declaringType = method.DeclaringType;
            string arg = string.Format("{0}{1}.{2}|", text, declaringType.Name, method.Name);
            string[] array = Message.Replace("\r\n", "\n").TrimEnd('\n').Split('\n');

            if (array.Length <= 1) {
                return string.Format("{0}{1}", arg, Message);
            }

            StringBuilder stringBuilder = new(string.Format("{0}{1}\n", arg, array[0]), 64);
            string format = string.Format("{{0,{0}}}{{1}}\n", text.Length);

            for (int i = 1; i < array.Length; i++) {
                stringBuilder.AppendFormat(format, string.Empty, array[i]);
            }

            stringBuilder.Length--;
            return stringBuilder.ToString();
        }
    }
}