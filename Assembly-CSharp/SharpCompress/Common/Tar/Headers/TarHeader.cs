using System;
using System.IO;
using System.Net;
using System.Text;

namespace SharpCompress.Common.Tar.Headers {
    internal class TarHeader {
        internal static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0);

        internal string Name { get; set; }

        internal long Size { get; set; }

        internal DateTime LastModifiedTime { get; set; }

        internal EntryType EntryType { get; set; }

        internal Stream PackedStream { get; set; }

        public long? DataStartPosition { get; set; }

        public string Magic { get; set; }

        internal void Write(Stream output) {
            if (Name.Length > 255) {
                throw new InvalidFormatException("UsTar fileName can not be longer thatn 255 chars");
            }

            byte[] array = new byte[512];
            string text = Name;

            if (text.Length > 100) {
                text = Name.Substring(0, 100);
            }

            WriteStringBytes(text, array, 0, 100);
            WriteOctalBytes(511L, array, 100, 8);
            WriteOctalBytes(0L, array, 108, 8);
            WriteOctalBytes(0L, array, 116, 8);
            WriteOctalBytes(Size, array, 124, 12);
            long value = (long)(LastModifiedTime - Epoch).TotalSeconds;
            WriteOctalBytes(value, array, 136, 12);
            array[156] = (byte)EntryType;

            if (Name.Length > 100) {
                text = Name.Substring(101, Name.Length);
                ArchiveEncoding.Default.GetBytes(text).CopyTo(array, 345);
            }

            if (Size >= 8589934591L) {
                byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Size));
                byte[] array2 = new byte[12];
                bytes.CopyTo(array2, 12 - bytes.Length);
                array2[0] |= 128;
                array2.CopyTo(array, 124);
            }

            int num = RecalculateChecksum(array);
            WriteOctalBytes(num, array, 148, 8);
            output.Write(array, 0, array.Length);
        }

        internal bool Read(BinaryReader reader) {
            byte[] array = reader.ReadBytes(512);

            if (array.Length == 0) {
                return false;
            }

            if (array.Length < 512) {
                throw new InvalidOperationException();
            }

            Name = ArchiveEncoding.Default.GetString(array, 0, 100).TrimNulls();
            EntryType = (EntryType)array[156];

            if ((array[124] & 0x80) == 128) {
                long network = BitConverter.ToInt64(array, 128);
                Size = IPAddress.NetworkToHostOrder(network);
            } else {
                Size = ReadASCIIInt64Base8(array, 124, 11);
            }

            long num = ReadASCIIInt64Base8(array, 136, 11);
            LastModifiedTime = Epoch.AddSeconds(num);
            Magic = ArchiveEncoding.Default.GetString(array, 257, 6).TrimNulls();

            if (!string.IsNullOrEmpty(Magic) && "ustar ".Equals(Magic)) {
                string @string = ArchiveEncoding.Default.GetString(array, 345, 157);
                @string = @string.TrimNulls();

                if (!string.IsNullOrEmpty(@string)) {
                    Name = @string + "/" + Name;
                }
            }

            if (EntryType != EntryType.LongName && Name.Length == 0) {
                return false;
            }

            return true;
        }

        static void WriteStringBytes(string name, byte[] buffer, int offset, int length) {
            int i;

            for (i = 0; i < length - 1 && i < name.Length; i++) {
                buffer[offset + i] = (byte)name[i];
            }

            for (; i < length; i++) {
                buffer[offset + i] = 0;
            }
        }

        static void WriteOctalBytes(long value, byte[] buffer, int offset, int length) {
            string text = Convert.ToString(value, 8);
            int num = length - text.Length - 1;

            for (int i = 0; i < num; i++) {
                buffer[offset + i] = 32;
            }

            for (int j = 0; j < text.Length; j++) {
                buffer[offset + j + num] = (byte)text[j];
            }

            buffer[offset + length] = 0;
        }

        static int ReadASCIIInt32Base8(byte[] buffer, int offset, int count) {
            string value = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();

            if (string.IsNullOrEmpty(value)) {
                return 0;
            }

            return Convert.ToInt32(value, 8);
        }

        static long ReadASCIIInt64Base8(byte[] buffer, int offset, int count) {
            string value = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();

            if (string.IsNullOrEmpty(value)) {
                return 0L;
            }

            return Convert.ToInt64(value, 8);
        }

        static long ReadASCIIInt64(byte[] buffer, int offset, int count) {
            string value = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();

            if (string.IsNullOrEmpty(value)) {
                return 0L;
            }

            return Convert.ToInt64(value);
        }

        internal static int RecalculateChecksum(byte[] buf) {
            Encoding.UTF8.GetBytes("        ").CopyTo(buf, 148);
            int num = 0;

            foreach (byte b in buf) {
                num += b;
            }

            return num;
        }

        internal static int RecalculateAltChecksum(byte[] buf) {
            Encoding.UTF8.GetBytes("        ").CopyTo(buf, 148);
            int num = 0;

            foreach (byte b in buf) {
                num = (b & 0x80) != 128 ? num + b : num - (b ^ 0x80);
            }

            return num;
        }
    }
}