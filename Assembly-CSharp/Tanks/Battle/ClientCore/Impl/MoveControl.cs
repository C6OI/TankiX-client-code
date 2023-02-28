using System.Runtime.InteropServices;

namespace Tanks.Battle.ClientCore.Impl {
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct MoveControl {
        public float MoveAxis { get; set; }

        public float TurnAxis { get; set; }

        public override string ToString() => string.Format("[MoveControl MoveAxis={0} TurnAxis={1}]", MoveAxis, TurnAxis);
    }
}