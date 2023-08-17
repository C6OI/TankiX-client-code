using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class ErrorScreenTextComponent : Component {
        public string ErrorText { get; set; }

        public string ButtonLabel { get; set; }

        public string ReportButtonLabel { get; set; }

        public string ReportUrl { get; set; }
    }
}