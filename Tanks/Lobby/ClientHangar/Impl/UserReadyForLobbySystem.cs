using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientHangar.Impl.Builder;

namespace Tanks.Lobby.ClientHangar.Impl {
    public class UserReadyForLobbySystem : ECSSystem {
        [OnEventFire]
        public void MarkUserAsReadyForLobby(HangarTankBuildedEvent e, Node hangar, [JoinAll] UserNode selfUser) =>
            selfUser.Entity.AddComponent<UserReadyForLobbyComponent>();

        [OnEventFire]
        public void UnmarkUserAsReadyForLobby(NodeRemoveEvent e, SingleNode<HangarInstanceComponent> hangar,
            [JoinAll] SingleNode<SelfUserComponent> selfUser) =>
            selfUser.Entity.RemoveComponent<UserReadyForLobbyComponent>();

        [Not(typeof(UserReadyForLobbyComponent))]
        public class UserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}