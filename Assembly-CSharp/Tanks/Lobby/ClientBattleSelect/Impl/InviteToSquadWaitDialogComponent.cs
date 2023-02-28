using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [RequireComponent(typeof(WaitDialogComponent))]
    public class InviteToSquadWaitDialogComponent : UIBehaviour, Component { }
}