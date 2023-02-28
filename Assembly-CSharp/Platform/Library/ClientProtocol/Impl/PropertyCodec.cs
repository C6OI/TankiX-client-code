using System.Reflection;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class PropertyCodec {
        public PropertyCodec(Codec codec, PropertyInfo propertyInfo) {
            Codec = codec;
            PropertyInfo = propertyInfo;
        }

        public Codec Codec { get; }

        public PropertyInfo PropertyInfo { get; }
    }
}