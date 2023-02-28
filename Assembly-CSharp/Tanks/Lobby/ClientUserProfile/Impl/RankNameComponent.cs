using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    public class RankNameComponent : MonoBehaviour, Component {
        [SerializeField] Text rankNameText;

        public string RankName {
            get => rankNameText.text;
            set => rankNameText.text = value;
        }
    }
}