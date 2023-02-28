using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientLoading.API {
    public class LobbyLoadScreenComponent : MonoBehaviour, Component, NoScaleScreen {
        public TextMeshProUGUI initialization;

        public ResourcesLoadProgressBarComponent progressBar;

        public LoadingStatusView loadingStatus;

        void Awake() {
            GetComponent<LoadBundlesTaskProviderComponent>().OnDataChange = OnDataChange;
        }

        void OnDataChange(LoadBundlesTaskComponent loadBundlesTask) {
            progressBar.UpdateView(loadBundlesTask);
            initialization.gameObject.SetActive(loadBundlesTask.BytesToLoad <= loadBundlesTask.BytesLoaded);
            loadingStatus.UpdateView(loadBundlesTask);
        }
    }
}