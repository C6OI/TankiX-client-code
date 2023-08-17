using System;
using System.Text;

namespace log4net.Util.TypeConverters {
    class EncodingConverter : IConvertFrom {
        public bool CanConvertFrom(Type sourceType) => sourceType == typeof(string);

        public object ConvertFrom(object source) {
            string text = source as string;

            if (text != null) {
                return Encoding.GetEncoding(text);
            }

            throw ConversionNotSupportedException.Create(typeof(Encoding), source);
        }
    }
}