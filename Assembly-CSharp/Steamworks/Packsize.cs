using System.Runtime.InteropServices;

namespace Steamworks {
    public static class Packsize {
        public const int value = 8;

        public static bool Test() {
            int num = Marshal.SizeOf(typeof(ValvePackingSentinel_t));
            int num2 = Marshal.SizeOf(typeof(RemoteStorageEnumerateUserSubscribedFilesResult_t));

            if (num != 32 || num2 != 616) {
                return false;
            }

            return true;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        struct ValvePackingSentinel_t {
            readonly uint m_u32;

            readonly ulong m_u64;

            readonly ushort m_u16;

            readonly double m_d;
        }
    }
}