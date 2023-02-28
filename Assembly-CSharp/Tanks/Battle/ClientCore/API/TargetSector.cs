using System.Runtime.InteropServices;

namespace Tanks.Battle.ClientCore.API {
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct TargetSector {
        public float Down { get; set; }

        public float Up { get; set; }

        public float Distance { get; set; }

        public float Length() => Up - Down;
    }
}