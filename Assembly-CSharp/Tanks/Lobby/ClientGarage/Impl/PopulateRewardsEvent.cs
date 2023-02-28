using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PopulateRewardsEvent : Event {
        public PopulateRewardsEvent() => RewardsItems = new List<Entity>();

        public List<Entity> RewardsItems { get; set; }
    }
}