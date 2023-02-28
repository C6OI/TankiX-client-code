using System;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientLoading.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ExitBattleToLobbyLoadScreenComponent : BehaviourComponent, NoScaleScreen {
        public ResourcesLoadProgressBarComponent progressBar;

        void Awake() {
            LoadBundlesTaskProviderComponent component = GetComponent<LoadBundlesTaskProviderComponent>();
            component.OnDataChange = (Action<LoadBundlesTaskComponent>)Delegate.Combine(component.OnDataChange, new Action<LoadBundlesTaskComponent>(OnDataChange));
        }

        void OnDataChange(LoadBundlesTaskComponent loadBundlesTask) {
            progressBar.UpdateView(loadBundlesTask);
        }
    }
}