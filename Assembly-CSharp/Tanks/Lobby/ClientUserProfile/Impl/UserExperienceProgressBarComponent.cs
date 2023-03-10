using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class UserExperienceProgressBarComponent : MonoBehaviour, Component {
        [SerializeField] ProgressBar progressBar;

        public void SetProgress(float progressValue) {
            progressBar.ProgressValue = progressValue;
        }
    }
}