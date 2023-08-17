using System;
using log4net.Util.TypeConverters;

namespace log4net.Layout {
    public class RawLayoutConverter : IConvertFrom {
        public bool CanConvertFrom(Type sourceType) => typeof(ILayout).IsAssignableFrom(sourceType);

        public object ConvertFrom(object source) {
            ILayout layout = source as ILayout;

            if (layout != null) {
                return new Layout2RawLayoutAdapter(layout);
            }

            throw ConversionNotSupportedException.Create(typeof(IRawLayout), source);
        }
    }
}