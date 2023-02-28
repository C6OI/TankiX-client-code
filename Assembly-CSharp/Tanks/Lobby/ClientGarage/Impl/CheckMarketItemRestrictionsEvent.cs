using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class CheckMarketItemRestrictionsEvent : Event {
        public bool RestrictedByRank { get; private set; }

        public bool RestrictedByUpgradeLevel { get; private set; }

        public bool MountWillBeRestrictedByUpgradeLevel { get; private set; }

        public void RestrictByRank(bool value) {
            RestrictedByRank = RestrictedByRank || value;
        }

        public void RestrictByUpgradeLevel(bool value) {
            RestrictedByUpgradeLevel = RestrictedByUpgradeLevel || value;
        }

        public void MountRestrictByUpgradeLevel(bool value) {
            MountWillBeRestrictedByUpgradeLevel = MountWillBeRestrictedByUpgradeLevel || value;
        }
    }
}