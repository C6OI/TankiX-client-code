using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.Impl {
    public class PasswordErrorsComponent : LocalizedControl, Component {
        public string PasswordContainsRestrictedSymbols { get; set; }

        public string PasswordIsTooSimple { get; set; }

        public string PasswordIsTooShort { get; set; }

        public string PasswordIsTooLong { get; set; }

        public string PasswordsDoNotMatch { get; set; }

        public string PasswordEqualsLogin { get; set; }
    }
}