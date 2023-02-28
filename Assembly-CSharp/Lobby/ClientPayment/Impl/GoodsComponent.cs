using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientPayment.Impl;

namespace Lobby.ClientPayment.Impl {
    public class GoodsComponent : Component {
        public SaleState SaleState = new();
    }
}