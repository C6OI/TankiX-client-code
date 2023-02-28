using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GetPresetNameEvent : Event {
        public string Name { get; set; }
    }
}