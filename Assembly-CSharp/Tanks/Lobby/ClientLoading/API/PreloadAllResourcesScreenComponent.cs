using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientLoading.API {
    public class PreloadAllResourcesScreenComponent : MonoBehaviour, Component, NoScaleScreen {
        public ResourcesLoadProgressBarComponent progressBar;

        public LoadingStatusView loadingStatusView;

        void Awake() {
            GetComponent<LoadBundlesTaskProviderComponent>().OnDataChange = OnDataChange;
        }

        void OnDataChange(LoadBundlesTaskComponent loadBundlesTask) {
            progressBar.UpdateView(loadBundlesTask);
            loadingStatusView.UpdateView(loadBundlesTask);
        }
    }
}