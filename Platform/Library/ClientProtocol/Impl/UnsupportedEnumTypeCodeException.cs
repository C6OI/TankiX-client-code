using System;

namespace Platform.Library.ClientProtocol.Impl {
    public class UnsupportedEnumTypeCodeException : Exception {
        public UnsupportedEnumTypeCodeException(TypeCode typeCode)
            : base(string.Format("Unsupported enum TypeCode {0}. Use Byte instead!",
                Enum.GetName(typeof(TypeCode), typeCode))) { }
    }
}