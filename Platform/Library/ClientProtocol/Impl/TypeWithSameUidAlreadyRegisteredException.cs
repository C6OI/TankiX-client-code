using System;

namespace Platform.Library.ClientProtocol.Impl {
    public class TypeWithSameUidAlreadyRegisteredException : Exception {
        public TypeWithSameUidAlreadyRegisteredException(long uid, Type existsType, Type type)
            : base(string.Concat("uid = ", uid, ", exists type = ", existsType, ", new type = ", type)) { }
    }
}