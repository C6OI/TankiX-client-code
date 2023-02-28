using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class TooltipDataRequestComponent : Component {
        public Vector3 MousePosition { get; set; }

        public GameObject TooltipPrefab { get; set; }

        public InteractionSource InteractionSource { get; set; }

        public long idOfRequestedUser { get; internal set; }

        public long InteractableSourceId { get; set; }
    }
}