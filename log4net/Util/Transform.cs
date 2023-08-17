using System.Text.RegularExpressions;
using System.Xml;

namespace log4net.Util {
    public sealed class Transform {
        const string CDATA_END = "]]>";

        const string CDATA_UNESCAPABLE_TOKEN = "]]";

        static readonly Regex INVALIDCHARS = new("[^\\x09\\x0A\\x0D\\x20-\\uD7FF\\uE000-\\uFFFD]", RegexOptions.None);

        Transform() { }

        public static void WriteEscapedXmlString(XmlWriter writer, string textData, string invalidCharReplacement) {
            string text = MaskXmlInvalidCharacters(textData, invalidCharReplacement);
            int num = 12 * (1 + CountSubstrings(text, "]]>"));
            int num2 = 3 * (CountSubstrings(text, "<") + CountSubstrings(text, ">")) + 4 * CountSubstrings(text, "&");

            if (num2 <= num) {
                writer.WriteString(text);
                return;
            }

            int num3 = text.IndexOf("]]>");

            if (num3 < 0) {
                writer.WriteCData(text);
                return;
            }

            int num4 = 0;

            while (num3 > -1) {
                writer.WriteCData(text.Substring(num4, num3 - num4));

                if (num3 == text.Length - 3) {
                    num4 = text.Length;
                    writer.WriteString("]]>");
                    break;
                }

                writer.WriteString("]]");
                num4 = num3 + 2;
                num3 = text.IndexOf("]]>", num4);
            }

            if (num4 < text.Length) {
                writer.WriteCData(text.Substring(num4));
            }
        }

        public static string MaskXmlInvalidCharacters(string textData, string mask) => INVALIDCHARS.Replace(textData, mask);

        static int CountSubstrings(string text, string substring) {
            int num = 0;
            int num2 = 0;
            int length = text.Length;
            int length2 = substring.Length;

            if (length == 0) {
                return 0;
            }

            if (length2 == 0) {
                return 0;
            }

            while (num2 < length) {
                int num3 = text.IndexOf(substring, num2);

                if (num3 == -1) {
                    break;
                }

                num++;
                num2 = num3 + length2;
            }

            return num;
        }
    }
}