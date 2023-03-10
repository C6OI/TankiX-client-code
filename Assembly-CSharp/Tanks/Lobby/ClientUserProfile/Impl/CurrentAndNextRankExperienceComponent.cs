using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class CurrentAndNextRankExperienceComponent : MonoBehaviour, Component {
        [SerializeField] Text text;

        public Text Text => text;
    }
}