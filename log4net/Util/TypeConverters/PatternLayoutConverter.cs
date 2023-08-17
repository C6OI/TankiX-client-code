using System;
using log4net.Layout;

namespace log4net.Util.TypeConverters {
    class PatternLayoutConverter : IConvertFrom {
        public bool CanConvertFrom(Type sourceType) => sourceType == typeof(string);

        public object ConvertFrom(object source) {
            string text = source as string;

            if (text != null) {
                return new PatternLayout(text);
            }

            throw ConversionNotSupportedException.Create(typeof(PatternLayout), source);
        }
    }
}