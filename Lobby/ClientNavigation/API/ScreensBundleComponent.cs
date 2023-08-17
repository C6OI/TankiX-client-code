using System.Collections.Generic;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientNavigation.API {
    public class ScreensBundleComponent : MonoBehaviour, Component {
        [HideInInspector] ScreenComponent[] screens;

        public IEnumerable<ScreenComponent> Screens {
            get {
                if (screens == null) {
                    screens = GetComponentsInChildren<ScreenComponent>(true);
                }

                return screens;
            }
        }

        public DialogsComponent Dialogs => GetComponentInChildren<DialogsComponent>(true);

        void Awake() {
            foreach (ScreenComponent screen in Screens) {
                if (screen.gameObject.activeSelf) {
                    Debug.LogError("Screen is Active " + screen.name + ". Disable it in scene!");
                    screen.gameObject.SetActive(false);
                }
            }
        }
    }
}