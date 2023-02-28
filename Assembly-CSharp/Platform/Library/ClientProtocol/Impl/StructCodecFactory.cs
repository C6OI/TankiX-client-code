using System;
using System.Collections.Generic;
using System.Reflection;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class StructCodecFactory : CodecFactory {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            Type type = !ReflectionUtils.IsNullableType(codecInfoWithAttrs.Info.type) ? codecInfoWithAttrs.Info.type : ReflectionUtils.GetNullableInnerType(codecInfoWithAttrs.Info.type);
            List<PropertyInfo> sortedProperties = GetSortedProperties(type, protocol);
            List<PropertyRequest> list = new(sortedProperties.Count);

            foreach (PropertyInfo item in sortedProperties) {
                bool optional = Attribute.IsDefined(item, typeof(ProtocolOptionalAttribute));
                bool varied = Attribute.IsDefined(item, typeof(ProtocolVariedAttribute));
                CodecInfoWithAttributes codecInfoWithAttributes = new(item.PropertyType, optional, varied);
                object[] customAttributes = item.GetCustomAttributes(true);
                object[] array = customAttributes;

                for (int i = 0; i < array.Length; i++) {
                    Attribute attribute = (Attribute)array[i];
                    codecInfoWithAttributes.AddAttribute(attribute);
                }

                list.Add(new PropertyRequest(item, codecInfoWithAttributes));
            }

            return new StructCodec(type, list);
        }

        static List<PropertyInfo> GetSortedProperties(Type structType, Protocol protocol) {
            List<PropertyInfo> list = new();
            PropertyInfo[] properties = structType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);

            foreach (PropertyInfo propertyInfo in properties) {
                ProtocolTransientAttribute protocolTransientAttribute = Attribute.GetCustomAttribute(propertyInfo, typeof(ProtocolTransientAttribute)) as ProtocolTransientAttribute;
                int num = protocolTransientAttribute != null ? protocolTransientAttribute.MinimalServerProtocolVersion : 0;

                if (num <= protocol.ServerProtocolVersion) {
                    list.Add(propertyInfo);
                }
            }

            list.Sort(delegate(PropertyInfo a, PropertyInfo b) {
                int order = GetOrder(a);
                int order2 = GetOrder(b);
                int num2 = Math.Sign(order - order2);

                if (num2 == 0) {
                    num2 = string.CompareOrdinal(a.Name, b.Name);
                }

                return num2;
            });

            return list;
        }

        static int GetOrder(PropertyInfo a) {
            int result = int.MaxValue;

            if (Attribute.IsDefined(a, typeof(ProtocolParameterOrderAttribute))) {
                result = ((ProtocolParameterOrderAttribute)Attribute.GetCustomAttribute(a, typeof(ProtocolParameterOrderAttribute))).Order;
            }

            return result;
        }
    }
}