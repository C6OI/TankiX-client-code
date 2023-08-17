using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpCompress.Archive;
using SharpCompress.Common;
using SharpCompress.IO;

namespace SharpCompress {
    static class Utility {
        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> items) => new(items.ToList());

        public static int URShift(int number, int bits) {
            if (number >= 0) {
                return number >> bits;
            }

            return (number >> bits) + (2 << ~bits);
        }

        public static int URShift(int number, long bits) => URShift(number, (int)bits);

        public static long URShift(long number, int bits) {
            if (number >= 0) {
                return number >> bits;
            }

            return (number >> bits) + (2L << ~bits);
        }

        public static long URShift(long number, long bits) => URShift(number, (int)bits);

        public static void Fill<T>(T[] array, int fromindex, int toindex, T val) where T : struct {
            if (array.Length == 0) {
                throw new NullReferenceException();
            }

            if (fromindex > toindex) {
                throw new ArgumentException();
            }

            if (fromindex < 0 || array.Length < toindex) {
                throw new IndexOutOfRangeException();
            }

            for (int i = fromindex <= 0 ? fromindex : fromindex--; i < toindex; i++) {
                array[i] = val;
            }
        }

        public static void Fill<T>(T[] array, T val) where T : struct => Fill(array, 0, array.Length, val);

        public static void SetSize(this List<byte> list, int count) {
            if (count > list.Count) {
                for (int i = list.Count; i < count; i++) {
                    list.Add(0);
                }
            } else {
                byte[] array = new byte[count];
                list.CopyTo(array, 0);
                list.Clear();
                list.AddRange(array);
            }
        }

        public static int readIntBigEndian(byte[] array, int pos) {
            int num = 0;
            num |= array[pos] & 0xFF;
            num <<= 8;
            num |= array[pos + 1] & 0xFF;
            num <<= 8;
            num |= array[pos + 2] & 0xFF;
            num <<= 8;
            return num | array[pos + 3] & 0xFF;
        }

        public static short readShortLittleEndian(byte[] array, int pos) => BitConverter.ToInt16(array, pos);

        public static int readIntLittleEndian(byte[] array, int pos) => BitConverter.ToInt32(array, pos);

        public static void writeIntBigEndian(byte[] array, int pos, int value) {
            array[pos] = (byte)((uint)URShift(value, 24) & 0xFFu);
            array[pos + 1] = (byte)((uint)URShift(value, 16) & 0xFFu);
            array[pos + 2] = (byte)((uint)URShift(value, 8) & 0xFFu);
            array[pos + 3] = (byte)((uint)value & 0xFFu);
        }

        public static void WriteLittleEndian(byte[] array, int pos, short value) {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, array, pos, bytes.Length);
        }

        public static void incShortLittleEndian(byte[] array, int pos, short incrementValue) {
            short num = BitConverter.ToInt16(array, pos);
            num += incrementValue;
            WriteLittleEndian(array, pos, num);
        }

        public static void WriteLittleEndian(byte[] array, int pos, int value) {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, array, pos, bytes.Length);
        }

        public static void Initialize<T>(this T[] array, Func<T> func) {
            for (int i = 0; i < array.Length; i++) {
                array[i] = func();
            }
        }

        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source) {
            foreach (T item in source) {
                destination.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action) {
            foreach (T item in items) {
                action(item);
            }
        }

        public static IEnumerable<T> AsEnumerable<T>(this T item) {
            yield return item;
        }

        public static void CheckNotNull(this object obj, string name) {
            if (obj == null) {
                throw new ArgumentNullException(name);
            }
        }

        public static void CheckNotNullOrEmpty(this string obj, string name) {
            obj.CheckNotNull(name);

            if (obj.Length == 0) {
                throw new ArgumentException("String is empty.");
            }
        }

        public static void Skip(this Stream source, long advanceAmount) {
            byte[] array = new byte[32768];
            int num = 0;
            int num2 = 0;

            do {
                num2 = array.Length;

                if (num2 > advanceAmount) {
                    num2 = (int)advanceAmount;
                }

                num = source.Read(array, 0, num2);

                if (num < 0) {
                    break;
                }

                advanceAmount -= num;
            } while (advanceAmount != 0L);
        }

        public static void SkipAll(this Stream source) {
            byte[] array = new byte[32768];

            while (source.Read(array, 0, array.Length) == array.Length) { }
        }

        public static byte[] UInt32ToBigEndianBytes(uint x) => new byte[4] {
            (byte)(x >> 24 & 0xFFu),
            (byte)(x >> 16 & 0xFFu),
            (byte)(x >> 8 & 0xFFu),
            (byte)(x & 0xFFu)
        };

        public static DateTime DosDateToDateTime(ushort iDate, ushort iTime) {
            int year = iDate / 512 + 1980;
            int num = iDate % 512 / 32;
            int num2 = iDate % 512 % 32;
            int hour = iTime / 2048;
            int minute = iTime % 2048 / 32;
            int second = iTime % 2048 % 32 * 2;

            if (iDate == ushort.MaxValue || num == 0 || num2 == 0) {
                year = 1980;
                num = 1;
                num2 = 1;
            }

            if (iTime == ushort.MaxValue) {
                hour = minute = second = 0;
            }

            DateTime result;

            try {
                result = new DateTime(year, num, num2, hour, minute, second);
            } catch {
                result = default;
            }

            return result;
        }

        public static uint DateTimeToDosTime(this DateTime? dateTime) {
            if (!dateTime.HasValue) {
                return 0u;
            }

            return (uint)(dateTime.Value.Second / 2 |
                          dateTime.Value.Minute << 5 |
                          dateTime.Value.Hour << 11 |
                          dateTime.Value.Day << 16 |
                          dateTime.Value.Month << 21 |
                          dateTime.Value.Year - 1980 << 25);
        }

        public static DateTime DosDateToDateTime(uint iTime) =>
            DosDateToDateTime((ushort)(iTime / 65536), (ushort)(iTime % 65536));

        public static DateTime DosDateToDateTime(int iTime) => DosDateToDateTime((uint)iTime);

        public static long TransferTo(this Stream source, Stream destination) {
            byte[] array = new byte[4096];
            long num = 0L;
            int num2;

            while ((num2 = source.Read(array, 0, array.Length)) != 0) {
                num += num2;
                destination.Write(array, 0, num2);
            }

            return num;
        }

        public static bool ReadFully(this Stream stream, byte[] buffer) {
            int num = 0;
            int num2;

            while ((num2 = stream.Read(buffer, num, buffer.Length - num)) > 0) {
                num += num2;

                if (num >= buffer.Length) {
                    return true;
                }
            }

            return num >= buffer.Length;
        }

        public static string TrimNulls(this string source) => source.Replace('\0', ' ').Trim();

        public static bool BinaryEquals(this byte[] source, byte[] target) {
            if (source.Length != target.Length) {
                return false;
            }

            for (int i = 0; i < source.Length; i++) {
                if (source[i] != target[i]) {
                    return false;
                }
            }

            return true;
        }

        internal static void Extract<TEntry, TVolume>(this TEntry entry, AbstractArchive<TEntry, TVolume> archive,
            Stream streamToWriteTo) where TEntry : IArchiveEntry where TVolume : IVolume {
            if (entry.IsDirectory) {
                throw new ExtractionException("Entry is a file directory and cannot be extracted.");
            }

            archive.EnsureEntriesLoaded();
            archive.FireEntryExtractionBegin(entry);
            ((IStreamListener)archive).FireFilePartExtractionBegin(entry.FilePath, entry.Size, entry.CompressedSize);

            using (Stream source = new ListeningStream(archive, entry.OpenEntryStream())) {
                source.TransferTo(streamToWriteTo);
            }

            archive.FireEntryExtractionEnd(entry);
        }
    }
}