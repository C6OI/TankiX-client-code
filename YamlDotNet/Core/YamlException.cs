using System;
using System.Runtime.Serialization;

namespace YamlDotNet.Core {
    [Serializable]
    public class YamlException : Exception {
        public YamlException() { }

        public YamlException(string message)
            : base(message) { }

        public YamlException(Mark start, Mark end, string message)
            : this(start, end, message, null) { }

        public YamlException(Mark start, Mark end, string message, Exception innerException)
            : base(string.Format("({0}) - ({1}): {2}", start, end, message), innerException) {
            Start = start;
            End = end;
        }

        public YamlException(string message, Exception inner)
            : base(message, inner) { }

        protected YamlException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
            Start = (Mark)info.GetValue("Start", typeof(Mark));
            End = (Mark)info.GetValue("End", typeof(Mark));
        }

        public Mark Start { get; }

        public Mark End { get; }

        [PermissionSet(SecurityAction.LinkDemand,
            XML =
                "<PermissionSet class=\"System.Security.PermissionSet\"\nversion=\"1\">\n<IPermission class=\"System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\nversion=\"1\"\nFlags=\"SerializationFormatter\"/>\n</PermissionSet>\n")]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("Start", Start);
            info.AddValue("End", End);
        }
    }
}