using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class GetChangeTurretTutorialValidationDataEvent : Event {
        public GetChangeTurretTutorialValidationDataEvent(long stepId, long itemId = 0L) {
            StepId = stepId;
            ItemId = itemId;
        }

        public long ItemId { get; set; }

        public long StepId { get; set; }

        public long BattlesCount { get; set; }

        public bool TutorialItemAlreadyMounted { get; set; }

        public bool TutorialItemAlreadyBought { get; set; }

        public long MountedWeaponId { get; set; }
    }
}