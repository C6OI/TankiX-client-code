using System.IO;

namespace log4net.Util.PatternStringConverters {
    sealed class PropertyPatternConverter : PatternConverter {
        protected override void Convert(TextWriter writer, object state) {
            CompositeProperties compositeProperties = new();
            PropertiesDictionary propertiesDictionary = ThreadContext.Properties.GetProperties(false);

            if (propertiesDictionary != null) {
                compositeProperties.Add(propertiesDictionary);
            }

            compositeProperties.Add(GlobalContext.Properties.GetReadOnlyProperties());

            if (Option != null) {
                WriteObject(writer, null, compositeProperties[Option]);
            } else {
                WriteDictionary(writer, null, compositeProperties.Flatten());
            }
        }
    }
}