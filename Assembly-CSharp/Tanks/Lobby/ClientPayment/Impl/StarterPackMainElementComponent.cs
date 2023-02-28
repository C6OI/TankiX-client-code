using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientPayment.Impl {
    public class StarterPackMainElementComponent : Component {
        public long ItemId { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public string Description { get; set; }
    }
}