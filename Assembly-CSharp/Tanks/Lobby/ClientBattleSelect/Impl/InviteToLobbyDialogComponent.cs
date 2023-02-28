using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [RequireComponent(typeof(InviteDialogComponent))]
    public class InviteToLobbyDialogComponent : UIBehaviour, Component {
        public long lobbyId;

        public long engineId;

        public LocalizedField messageForSingleUser;

        public LocalizedField messageForSquadLeader;

        public LocalizedField messageForSquadMember;
    }
}