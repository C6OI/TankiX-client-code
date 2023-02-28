using System;

namespace SharpCompress.Common {
    internal static class FlagUtility {
        public static bool HasFlag<T>(long bitField, T flag) where T : struct, IConvertible => HasFlag(bitField, flag.ToInt64(null));

        public static bool HasFlag<T>(ulong bitField, T flag) where T : struct, IConvertible => HasFlag(bitField, flag.ToUInt64(null));

        public static bool HasFlag(ulong bitField, ulong flag) => (bitField & flag) == flag;

        public static bool HasFlag(short bitField, short flag) => (bitField & flag) == flag;

        public static bool HasFlag(this Enum enumVal, Enum flag) => (Convert.ToInt32(enumVal) & Convert.ToInt32(flag)) == Convert.ToInt32(flag);

        public static bool HasFlag<T>(T bitField, T flag) where T : struct => HasFlag(Convert.ToInt64(bitField), Convert.ToInt64(flag));

        public static bool HasFlag(long bitField, long flag) => (bitField & flag) == flag;

        public static long SetFlag<T>(long bitField, T flag, bool on) where T : struct, IConvertible {
            if (on) {
                return bitField | flag.ToInt64(null);
            }

            return bitField & ~flag.ToInt64(null);
        }

        public static long SetFlag<T>(T bitField, T flag, bool on) where T : struct, IConvertible => SetFlag(bitField.ToInt64(null), flag, on);
    }
}