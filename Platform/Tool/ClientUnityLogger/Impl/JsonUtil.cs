using System.Collections.Generic;

namespace Platform.Tool.ClientUnityLogger.Impl {
    public static class JsonUtil {
        public static string ToJSONString(string text) {
            char[] array = text.ToCharArray();
            List<string> list = new();
            char[] array2 = array;

            for (int i = 0; i < array2.Length; i++) {
                char c = array2[i];

                if (c == '\b') {
                    list.Add("\\b");
                } else if (c == '\t') {
                    list.Add("\\t");
                } else if (c == '\n') {
                    list.Add("\\n");
                } else if (c == '\f') {
                    list.Add("\\f");
                } else if (c == '\r') {
                    list.Add("\\n");
                } else if (c == '"') {
                    list.Add("\\" + c);
                } else if (c == '/') {
                    list.Add("\\" + c);
                } else if (c == '\\') {
                    list.Add("\\" + c);
                } else if (c > '\u001f') {
                    list.Add(c.ToString());
                }
            }

            return string.Join(string.Empty, list.ToArray());
        }
    }
}