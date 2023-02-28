using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientHUD.API {
    public class CombatLogDMMessagesComponent : Component {
        public string BattleStartMessage { get; set; }
    }
}