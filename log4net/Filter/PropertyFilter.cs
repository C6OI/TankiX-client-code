using System;
using log4net.Core;

namespace log4net.Filter {
    public class PropertyFilter : StringMatchFilter {
        public string Key { get; set; }

        public override FilterDecision Decide(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            if (Key == null) {
                return FilterDecision.Neutral;
            }

            object obj = loggingEvent.LookupProperty(Key);
            string text = loggingEvent.Repository.RendererMap.FindAndRender(obj);

            if (text == null || m_stringToMatch == null && m_regexToMatch == null) {
                return FilterDecision.Neutral;
            }

            if (m_regexToMatch != null) {
                if (!m_regexToMatch.Match(text).Success) {
                    return FilterDecision.Neutral;
                }

                if (m_acceptOnMatch) {
                    return FilterDecision.Accept;
                }

                return FilterDecision.Deny;
            }

            if (m_stringToMatch != null) {
                if (text.IndexOf(m_stringToMatch) == -1) {
                    return FilterDecision.Neutral;
                }

                if (m_acceptOnMatch) {
                    return FilterDecision.Accept;
                }

                return FilterDecision.Deny;
            }

            return FilterDecision.Neutral;
        }
    }
}