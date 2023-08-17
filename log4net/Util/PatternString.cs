using System;
using System.Collections;
using System.Globalization;
using System.IO;
using log4net.Core;
using log4net.Util.PatternStringConverters;

namespace log4net.Util {
    public class PatternString : IOptionHandler {
        static readonly Hashtable s_globalRulesRegistry;

        readonly Hashtable m_instanceRulesRegistry = new();

        PatternConverter m_head;

        static PatternString() {
            s_globalRulesRegistry = new Hashtable(15);
            s_globalRulesRegistry.Add("appdomain", typeof(AppDomainPatternConverter));
            s_globalRulesRegistry.Add("date", typeof(DatePatternConverter));
            s_globalRulesRegistry.Add("env", typeof(EnvironmentPatternConverter));
            s_globalRulesRegistry.Add("envFolderPath", typeof(EnvironmentFolderPathPatternConverter));
            s_globalRulesRegistry.Add("identity", typeof(IdentityPatternConverter));
            s_globalRulesRegistry.Add("literal", typeof(LiteralPatternConverter));
            s_globalRulesRegistry.Add("newline", typeof(NewLinePatternConverter));
            s_globalRulesRegistry.Add("processid", typeof(ProcessIdPatternConverter));
            s_globalRulesRegistry.Add("property", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("random", typeof(RandomStringPatternConverter));
            s_globalRulesRegistry.Add("username", typeof(UserNamePatternConverter));
            s_globalRulesRegistry.Add("utcdate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("utcDate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("UtcDate", typeof(UtcDatePatternConverter));
        }

        public PatternString() { }

        public PatternString(string pattern) {
            ConversionPattern = pattern;
            ActivateOptions();
        }

        public string ConversionPattern { get; set; }

        public virtual void ActivateOptions() => m_head = CreatePatternParser(ConversionPattern).Parse();

        PatternParser CreatePatternParser(string pattern) {
            PatternParser patternParser = new(pattern);

            foreach (DictionaryEntry item in s_globalRulesRegistry) {
                ConverterInfo converterInfo = new();
                converterInfo.Name = (string)item.Key;
                converterInfo.Type = (Type)item.Value;
                patternParser.PatternConverters.Add(item.Key, converterInfo);
            }

            foreach (DictionaryEntry item2 in m_instanceRulesRegistry) {
                patternParser.PatternConverters[item2.Key] = item2.Value;
            }

            return patternParser;
        }

        public void Format(TextWriter writer) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }

            for (PatternConverter patternConverter = m_head;
                 patternConverter != null;
                 patternConverter = patternConverter.Next) {
                patternConverter.Format(writer, null);
            }
        }

        public string Format() {
            StringWriter stringWriter = new(CultureInfo.InvariantCulture);
            Format(stringWriter);
            return stringWriter.ToString();
        }

        public void AddConverter(ConverterInfo converterInfo) {
            if (converterInfo == null) {
                throw new ArgumentNullException("converterInfo");
            }

            if (!typeof(PatternConverter).IsAssignableFrom(converterInfo.Type)) {
                throw new ArgumentException(string.Concat("The converter type specified [",
                        converterInfo.Type,
                        "] must be a subclass of log4net.Util.PatternConverter"),
                    "converterInfo");
            }

            m_instanceRulesRegistry[converterInfo.Name] = converterInfo;
        }

        public void AddConverter(string name, Type type) {
            if (name == null) {
                throw new ArgumentNullException("name");
            }

            if (type == null) {
                throw new ArgumentNullException("type");
            }

            ConverterInfo converterInfo = new();
            converterInfo.Name = name;
            converterInfo.Type = type;
            AddConverter(converterInfo);
        }
    }
}