using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class DroppedItem {
        public Entity marketItemEntity { get; set; }

        public int Amount { get; set; }

        public override string ToString() => string.Format("marketItemEntity: {0}, Amount: {1}", marketItemEntity, Amount);
    }
}