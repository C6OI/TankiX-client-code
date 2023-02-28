using System;

namespace Platform.Library.ClientProtocol.API {
    public class SerializationUidUtils {
        public static bool HasSelfUid(Type type) => Attribute.IsDefined(type, typeof(SerialVersionUIDAttribute), false);

        public static long GetUid(Type type) {
            if (!Attribute.IsDefined(type, typeof(SerialVersionUIDAttribute))) {
                throw new SerialVersionUidNotFoundException(type);
            }

            SerialVersionUIDAttribute serialVersionUIDAttribute = (SerialVersionUIDAttribute)Attribute.GetCustomAttribute(type, typeof(SerialVersionUIDAttribute));
            return serialVersionUIDAttribute.value;
        }
    }
}