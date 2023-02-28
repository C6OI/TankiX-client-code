using System.Runtime.InteropServices;

namespace Tanks.Battle.ClientCore.API {
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct TargetingCone {
        public float HAngle { get; set; }

        public float VAngleUp { get; set; }

        public float VAngleDown { get; set; }

        public float Distance { get; set; }
    }
}