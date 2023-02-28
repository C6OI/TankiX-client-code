using System;

namespace Steamworks {
    [AttributeUsage(AttributeTargets.Struct)]
    internal class CallbackIdentityAttribute : Attribute {
        public CallbackIdentityAttribute(int callbackNum) => Identity = callbackNum;

        public int Identity { get; set; }
    }
}