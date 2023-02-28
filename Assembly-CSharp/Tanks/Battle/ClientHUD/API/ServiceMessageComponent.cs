using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientHUD.API {
    public class ServiceMessageComponent : MonoBehaviour, Component {
        public Animator animator;

        public Text MessageText;
    }
}