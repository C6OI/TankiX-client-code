using System.Text;

namespace SharpCompress.Common {
    public class ArchiveEncoding {
        public static Encoding Default;

        public static Encoding Password;

        static ArchiveEncoding() {
            Default = Encoding.UTF8;
            Password = Encoding.UTF8;
        }
    }
}