using System;
using System.Linq;

namespace Platform.Library.ClientResources.API {
    public class CommandLineParser {
        readonly string[] args;

        public CommandLineParser(string[] args) => this.args = args;

        public string GetValue(string paramName) {
            string paramValue;

            if (TryGetValue(paramName, out paramValue)) {
                return paramValue;
            }

            throw new ParameterNotFoundException(paramName);
        }

        public string GetValueOrDefault(string paramName, string defaultValue) {
            string paramValue;
            return !TryGetValue(paramName, out paramValue) ? defaultValue : paramValue;
        }

        public bool TryGetValue(string paramName, out string paramValue) {
            string[] array = args;

            foreach (string text in array) {
                if (text.StartsWith(paramName, StringComparison.Ordinal)) {
                    if (paramName.Length + 1 < text.Length) {
                        paramValue = text.Substring(paramName.Length + 1);
                    } else {
                        paramValue = string.Empty;
                    }

                    return true;
                }
            }

            paramValue = string.Empty;
            return false;
        }

        public bool IsExist(string paramName) {
            return args.Any(arg => arg.StartsWith(paramName, StringComparison.Ordinal));
        }

        public string[] GetValues(string paramName) => GetValue(paramName).Split(',');

        public string GetSubLine(string[] paramsName) {
            string text = string.Empty;

            foreach (string text2 in paramsName) {
                if (IsExist(text2)) {
                    string valueOrDefault = GetValueOrDefault(text2, string.Empty);
                    string text3 = !(valueOrDefault == string.Empty) ? text2 + "=" + valueOrDefault : text2;
                    text = text + text3 + " ";
                }
            }

            return text.Trim();
        }
    }
}