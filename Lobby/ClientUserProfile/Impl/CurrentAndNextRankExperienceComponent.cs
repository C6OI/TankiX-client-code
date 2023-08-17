using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientUserProfile.Impl {
    public class CurrentAndNextRankExperienceComponent : MonoBehaviour, Component {
        [SerializeField] Text text;

        public Text Text => text;
    }
}