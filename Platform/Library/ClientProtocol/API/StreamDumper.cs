using System.IO;

namespace Platform.Library.ClientProtocol.API {
    public class StreamDumper {
        public static string Dump(Stream data) {
            long position = data.Position;
            data.Seek(0L, SeekOrigin.Begin);
            BinaryReader binaryReader = new(data);
            string text = "\n=== Dump data ===\n";
            int num = 0;
            string text2 = string.Empty;

            for (long num2 = 0L; num2 < data.Length; num2++) {
                byte b = binaryReader.ReadByte();
                char c = (char)b;
                text = text + b.ToString("X2") + " ";
                text2 = !char.IsWhiteSpace(c) && !char.IsControl(c) ? text2 + c : text2 + '.';
                num++;

                if (num > 16) {
                    text = text + "\t" + text2 + "\n";
                    num = 0;
                    text2 = string.Empty;
                }
            }

            if (num != 0) {
                while (num < 18) {
                    num++;
                    text += "   ";
                }

                text = text + "\t" + text2 + "\n";
            }

            data.Seek(position, SeekOrigin.Begin);
            return text;
        }
    }
}