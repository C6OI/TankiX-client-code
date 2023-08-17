using System;
using System.Collections;
using System.IO;
using log4net.Core;
using log4net.Layout.Pattern;
using log4net.Util;
using log4net.Util.PatternStringConverters;
using AppDomainPatternConverter = log4net.Layout.Pattern.AppDomainPatternConverter;
using DatePatternConverter = log4net.Layout.Pattern.DatePatternConverter;
using IdentityPatternConverter = log4net.Layout.Pattern.IdentityPatternConverter;
using PropertyPatternConverter = log4net.Layout.Pattern.PropertyPatternConverter;
using UserNamePatternConverter = log4net.Layout.Pattern.UserNamePatternConverter;
using UtcDatePatternConverter = log4net.Layout.Pattern.UtcDatePatternConverter;

namespace log4net.Layout {
    public class PatternLayout : LayoutSkeleton {
        public const string DefaultConversionPattern = "%message%newline";

        public const string DetailConversionPattern = "%timestamp [%thread] %level %logger %ndc - %message%newline";

        static readonly Hashtable s_globalRulesRegistry;

        readonly Hashtable m_instanceRulesRegistry = new();

        PatternConverter m_head;

        static PatternLayout() {
            s_globalRulesRegistry = new Hashtable(45);
            s_globalRulesRegistry.Add("literal", typeof(LiteralPatternConverter));
            s_globalRulesRegistry.Add("newline", typeof(NewLinePatternConverter));
            s_globalRulesRegistry.Add("n", typeof(NewLinePatternConverter));
            s_globalRulesRegistry.Add("c", typeof(LoggerPatternConverter));
            s_globalRulesRegistry.Add("logger", typeof(LoggerPatternConverter));
            s_globalRulesRegistry.Add("C", typeof(TypeNamePatternConverter));
            s_globalRulesRegistry.Add("class", typeof(TypeNamePatternConverter));
            s_globalRulesRegistry.Add("type", typeof(TypeNamePatternConverter));
            s_globalRulesRegistry.Add("d", typeof(DatePatternConverter));
            s_globalRulesRegistry.Add("date", typeof(DatePatternConverter));
            s_globalRulesRegistry.Add("exception", typeof(ExceptionPatternConverter));
            s_globalRulesRegistry.Add("F", typeof(FileLocationPatternConverter));
            s_globalRulesRegistry.Add("file", typeof(FileLocationPatternConverter));
            s_globalRulesRegistry.Add("l", typeof(FullLocationPatternConverter));
            s_globalRulesRegistry.Add("location", typeof(FullLocationPatternConverter));
            s_globalRulesRegistry.Add("L", typeof(LineLocationPatternConverter));
            s_globalRulesRegistry.Add("line", typeof(LineLocationPatternConverter));
            s_globalRulesRegistry.Add("m", typeof(MessagePatternConverter));
            s_globalRulesRegistry.Add("message", typeof(MessagePatternConverter));
            s_globalRulesRegistry.Add("M", typeof(MethodLocationPatternConverter));
            s_globalRulesRegistry.Add("method", typeof(MethodLocationPatternConverter));
            s_globalRulesRegistry.Add("p", typeof(LevelPatternConverter));
            s_globalRulesRegistry.Add("level", typeof(LevelPatternConverter));
            s_globalRulesRegistry.Add("P", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("property", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("properties", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("r", typeof(RelativeTimePatternConverter));
            s_globalRulesRegistry.Add("timestamp", typeof(RelativeTimePatternConverter));
            s_globalRulesRegistry.Add("stacktrace", typeof(StackTracePatternConverter));
            s_globalRulesRegistry.Add("stacktracedetail", typeof(StackTraceDetailPatternConverter));
            s_globalRulesRegistry.Add("t", typeof(ThreadPatternConverter));
            s_globalRulesRegistry.Add("thread", typeof(ThreadPatternConverter));
            s_globalRulesRegistry.Add("x", typeof(NdcPatternConverter));
            s_globalRulesRegistry.Add("ndc", typeof(NdcPatternConverter));
            s_globalRulesRegistry.Add("X", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("mdc", typeof(PropertyPatternConverter));
            s_globalRulesRegistry.Add("a", typeof(AppDomainPatternConverter));
            s_globalRulesRegistry.Add("appdomain", typeof(AppDomainPatternConverter));
            s_globalRulesRegistry.Add("u", typeof(IdentityPatternConverter));
            s_globalRulesRegistry.Add("identity", typeof(IdentityPatternConverter));
            s_globalRulesRegistry.Add("utcdate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("utcDate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("UtcDate", typeof(UtcDatePatternConverter));
            s_globalRulesRegistry.Add("w", typeof(UserNamePatternConverter));
            s_globalRulesRegistry.Add("username", typeof(UserNamePatternConverter));
        }

        public PatternLayout()
            : this("%message%newline") { }

        public PatternLayout(string pattern) {
            IgnoresException = true;
            ConversionPattern = pattern;

            if (ConversionPattern == null) {
                ConversionPattern = "%message%newline";
            }

            ActivateOptions();
        }

        public string ConversionPattern { get; set; }

        protected virtual PatternParser CreatePatternParser(string pattern) {
            PatternParser patternParser = new(pattern);

            foreach (DictionaryEntry item in s_globalRulesRegistry) {
                ConverterInfo converterInfo = new();
                converterInfo.Name = (string)item.Key;
                converterInfo.Type = (Type)item.Value;
                patternParser.PatternConverters[item.Key] = converterInfo;
            }

            foreach (DictionaryEntry item2 in m_instanceRulesRegistry) {
                patternParser.PatternConverters[item2.Key] = item2.Value;
            }

            return patternParser;
        }

        public override void ActivateOptions() {
            m_head = CreatePatternParser(ConversionPattern).Parse();

            for (PatternConverter patternConverter = m_head;
                 patternConverter != null;
                 patternConverter = patternConverter.Next) {
                PatternLayoutConverter patternLayoutConverter = patternConverter as PatternLayoutConverter;

                if (patternLayoutConverter != null && !patternLayoutConverter.IgnoresException) {
                    IgnoresException = false;
                    break;
                }
            }
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent) {
            if (writer == null) {
                throw new ArgumentNullException("writer");
            }

            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            for (PatternConverter patternConverter = m_head;
                 patternConverter != null;
                 patternConverter = patternConverter.Next) {
                patternConverter.Format(writer, loggingEvent);
            }
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