using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class CheckSquadEnergyEvent : Event {
        public bool HaveEnoughtEnergyForEntrance { get; set; }
    }
}