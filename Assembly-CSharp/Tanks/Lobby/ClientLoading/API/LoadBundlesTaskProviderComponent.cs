using System;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientLoading.API {
    public class LoadBundlesTaskProviderComponent : MonoBehaviour, Component {
        public Action<LoadBundlesTaskComponent> OnDataChange;

        public LoadBundlesTaskComponent LoadBundlesTask { get; private set; }

        public void UpdateData(LoadBundlesTaskComponent loadBundlesTask) {
            LoadBundlesTask = loadBundlesTask;

            if (OnDataChange != null) {
                OnDataChange(loadBundlesTask);
            }
        }
    }
}