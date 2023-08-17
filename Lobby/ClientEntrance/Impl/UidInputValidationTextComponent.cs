using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.Impl {
    public class UidInputValidationTextComponent : LocalizedControl, Component {
        public string LoginContainsRestrictedSymbols { get; set; }

        public string LoginAlreadyInUse { get; set; }

        public string LoginIsTooShort { get; set; }

        public string LoginIsTooLong { get; set; }

        public string LoginContainsSpecialSymbolsInARow { get; set; }

        public string LoginBeginsWithSpecialSymbol { get; set; }

        public string LoginEndsWithSpecialSymbol { get; set; }
    }
}