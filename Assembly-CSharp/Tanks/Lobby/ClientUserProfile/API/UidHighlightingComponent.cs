using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UidHighlightingComponent : MonoBehaviour, Component {
        public FontStyles friend;

        public FontStyles selfUser;

        public FontStyles normal;
    }
}