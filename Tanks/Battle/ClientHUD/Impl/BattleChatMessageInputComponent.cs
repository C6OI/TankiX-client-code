using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatMessageInputComponent : MonoBehaviour, Component {
        void Start() {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}