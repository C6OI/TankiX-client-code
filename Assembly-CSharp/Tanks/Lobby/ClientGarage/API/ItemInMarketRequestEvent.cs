using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ItemInMarketRequestEvent : Event {
        public readonly Dictionary<long, string> marketItems = new();
    }
}