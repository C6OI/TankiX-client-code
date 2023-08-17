using System;

namespace log4net.Util.TypeConverters {
    class PatternStringConverter : IConvertFrom, IConvertTo {
        public bool CanConvertFrom(Type sourceType) => sourceType == typeof(string);

        public object ConvertFrom(object source) {
            string text = source as string;

            if (text != null) {
                return new PatternString(text);
            }

            throw ConversionNotSupportedException.Create(typeof(PatternString), source);
        }

        public bool CanConvertTo(Type targetType) => typeof(string).IsAssignableFrom(targetType);

        public object ConvertTo(object source, Type targetType) {
            PatternString patternString = source as PatternString;

            if (patternString != null && CanConvertTo(targetType)) {
                return patternString.Format();
            }

            throw ConversionNotSupportedException.Create(targetType, source);
        }
    }
}