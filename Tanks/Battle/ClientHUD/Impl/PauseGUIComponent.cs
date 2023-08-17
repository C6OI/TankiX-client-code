using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class PauseGUIComponent : MonoBehaviour, Component {
        public bool ShowMessage { get; set; }

        public string MessageText { get; set; }

        void OnGUI() {
            if (ShowMessage) {
                Vector3 vector = new(500f, 200f);

                Rect screenRect = new(Screen.width / 2f - vector.x / 2f,
                    Screen.height / 2f - vector.y / 2f,
                    vector.x,
                    vector.y);

                GUILayout.BeginArea(screenRect);
                GUILayout.BeginVertical();
                GUILayout.Label(MessageText, ClientGraphicsActivator.guiStyle);
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }
    }
}