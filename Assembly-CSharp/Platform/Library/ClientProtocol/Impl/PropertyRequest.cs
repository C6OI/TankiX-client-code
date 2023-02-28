using System.Reflection;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class PropertyRequest {
        public PropertyRequest(PropertyInfo propertyInfo, CodecInfoWithAttributes codecInfoWithAttributes) {
            PropertyInfo = propertyInfo;
            CodecInfoWithAttributes = codecInfoWithAttributes;
        }

        public PropertyInfo PropertyInfo { get; }

        public CodecInfoWithAttributes CodecInfoWithAttributes { get; }
    }
}