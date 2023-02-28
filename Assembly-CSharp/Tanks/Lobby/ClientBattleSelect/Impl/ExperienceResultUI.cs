using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class ExperienceResultUI : ProgressResultUI {
        public void SetProgress(float expReward, int[] levels, BattleResultsTextTemplatesComponent textTemplates) {
            LevelInfo info = this.SendEvent<GetUserLevelInfoEvent>(SelfUserComponent.SelfUser).Info;
            SetProgress(expReward, levels, info, textTemplates);
        }

        public void SetNewLevel() {
            SetResidualProgress();
        }
    }
}