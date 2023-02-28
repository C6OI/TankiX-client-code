using System.Runtime.InteropServices;

namespace Tanks.Battle.ClientCore.Impl {
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct DiscreteTankControl {
        static readonly int BIT_LEFT;

        static readonly int BIT_RIGHT = 1;

        static readonly int BIT_DOWN = 2;

        static readonly int BIT_UP = 3;

        static readonly int BIT_WEAPON_LEFT = 4;

        static readonly int BIT_WEAPON_RIGHT = 5;

        public byte Control { get; set; }

        public int MoveAxis {
            get => GetBit(BIT_UP) - GetBit(BIT_DOWN);
            set => SetDiscreteControl(value, BIT_DOWN, BIT_UP);
        }

        public int TurnAxis {
            get => GetBit(BIT_RIGHT) - GetBit(BIT_LEFT);
            set => SetDiscreteControl(value, BIT_LEFT, BIT_RIGHT);
        }

        public int WeaponControl {
            get => GetBit(BIT_WEAPON_RIGHT) - GetBit(BIT_WEAPON_LEFT);
            set => SetDiscreteControl(value, BIT_WEAPON_LEFT, BIT_WEAPON_RIGHT);
        }

        int GetBit(int bitNumber) => Control >> bitNumber & 1;

        void SetBit(int bitNumber, int value) {
            int num = ~(1 << bitNumber);
            Control = (byte)(Control & num | (value & 1) << bitNumber);
        }

        void SetDiscreteControl(int value, int negativeBit, int positiveBit) {
            SetBit(negativeBit, 0);
            SetBit(positiveBit, 0);

            if (value > 0) {
                SetBit(positiveBit, 1);
            } else if (value < 0) {
                SetBit(negativeBit, 1);
            }
        }
    }
}