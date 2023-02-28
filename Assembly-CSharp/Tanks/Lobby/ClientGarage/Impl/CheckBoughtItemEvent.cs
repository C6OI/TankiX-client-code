using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CheckBoughtItemEvent : Event {
        public CheckBoughtItemEvent(long itemId) => ItemId = itemId;

        public long ItemId { get; set; }

        public bool TutorialItemAlreadyBought { get; set; }
    }
}