using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class CTFHUDMessagesComponent : LocalizedControl, Component {
        public string CaptureFlagMessage { get; set; }

        public string ProtectFlagCarrierMessage { get; set; }

        public string DeliverFlagMessage { get; set; }

        public string ReturnFlagMessage { get; set; }

        public string EnemyBase { get; set; }

        public string EnemyFlag { get; set; }

        public string YourBase { get; set; }

        public string YourFlag { get; set; }
    }
}