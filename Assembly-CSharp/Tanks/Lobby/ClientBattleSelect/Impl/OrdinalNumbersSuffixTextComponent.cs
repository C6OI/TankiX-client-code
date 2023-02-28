using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class OrdinalNumbersSuffixTextComponent : LocalizedControl {
        public string StSuffix { get; set; }

        public string NdSuffix { get; set; }

        public string RdSuffix { get; set; }

        public string ThSuffix { get; set; }

        public string GetSuffix(int number) {
            int num = number % 100;
            int num2 = num / 10;
            int num3 = num % 10;

            if (num2 == 1) {
                return ThSuffix;
            }

            switch (num3) {
                case 1:
                    return StSuffix;

                case 2:
                    return NdSuffix;

                case 3:
                    return RdSuffix;

                default:
                    return ThSuffix;
            }
        }
    }
}