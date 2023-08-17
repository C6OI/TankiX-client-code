using System;
using System.Text.RegularExpressions;
using log4net.Core;

namespace log4net.Filter {
    public class StringMatchFilter : FilterSkeleton {
        protected bool m_acceptOnMatch = true;

        protected Regex m_regexToMatch;

        protected string m_stringRegexToMatch;

        protected string m_stringToMatch;

        public bool AcceptOnMatch {
            get => m_acceptOnMatch;
            set => m_acceptOnMatch = value;
        }

        public string StringToMatch {
            get => m_stringToMatch;
            set => m_stringToMatch = value;
        }

        public string RegexToMatch {
            get => m_stringRegexToMatch;
            set => m_stringRegexToMatch = value;
        }

        public override void ActivateOptions() {
            if (m_stringRegexToMatch != null) {
                m_regexToMatch = new Regex(m_stringRegexToMatch, RegexOptions.None);
            }
        }

        public override FilterDecision Decide(LoggingEvent loggingEvent) {
            if (loggingEvent == null) {
                throw new ArgumentNullException("loggingEvent");
            }

            string renderedMessage = loggingEvent.RenderedMessage;

            if (renderedMessage == null || m_stringToMatch == null && m_regexToMatch == null) {
                return FilterDecision.Neutral;
            }

            if (m_regexToMatch != null) {
                if (!m_regexToMatch.Match(renderedMessage).Success) {
                    return FilterDecision.Neutral;
                }

                if (m_acceptOnMatch) {
                    return FilterDecision.Accept;
                }

                return FilterDecision.Deny;
            }

            if (m_stringToMatch != null) {
                if (renderedMessage.IndexOf(m_stringToMatch) == -1) {
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