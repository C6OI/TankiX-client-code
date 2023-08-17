using System.Text;
using System.Text.RegularExpressions;

namespace Tanks.Battle.ClientHUD.Impl {
    public static class ChatMessageUtil {
        public static string RemoveWhiteSpaces(string text) => text.Trim();

        public static string RemoveTags(string text, string[] tags) {
            StringBuilder stringBuilder = new();

            foreach (string arg in tags) {
                if (stringBuilder.Length > 0) {
                    stringBuilder.Append("|");
                }

                stringBuilder.AppendFormat("(<{0}.*?>)|(</{0}.*?>)", arg);
            }

            Regex regex = new(stringBuilder.ToString(), RegexOptions.IgnoreCase);
            return regex.Replace(text, string.Empty);
        }
    }
}