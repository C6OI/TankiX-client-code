using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ActivateModuleTutorialStepValidator : ECSBehaviour, ITutorialShowStepValidator {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public bool ShowAllowed(long stepId) {
            GetChangeTurretTutorialValidationDataEvent getChangeTurretTutorialValidationDataEvent = new(stepId);
            ScheduleEvent(getChangeTurretTutorialValidationDataEvent, EngineService.EntityStub);
            return getChangeTurretTutorialValidationDataEvent.BattlesCount > 1;
        }

        public class UserStatisticsNode : Node {
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;

            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
            public SelfUserComponent selfUser;

            public UserStatisticsComponent userStatistics;
        }
    }
}