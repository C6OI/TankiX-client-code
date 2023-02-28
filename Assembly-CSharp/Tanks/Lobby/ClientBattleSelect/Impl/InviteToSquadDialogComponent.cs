using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [RequireComponent(typeof(InviteDialogComponent))]
    public class InviteToSquadDialogComponent : UIBehaviour, Component {
        public long FromUserId;

        public long SquadId;

        public long EngineId;

        public bool invite;

        [SerializeField] LocalizedField inviteMessageLocalizedField;

        [SerializeField] LocalizedField requestMessageLocalizedField;

        public void Show(string userUid, bool inBattle, bool invite) {
            this.invite = invite;

            GetComponent<InviteDialogComponent>()
                .Show(
                    !invite ? string.Format(requestMessageLocalizedField.Value, "<color=orange>" + userUid + "</color>", "\n")
                        : string.Format(inviteMessageLocalizedField.Value, "<color=orange>" + userUid + "</color>", "\n"), inBattle);
        }

        public void Hide() {
            GetComponent<InviteDialogComponent>().OnNo();
        }
    }
}