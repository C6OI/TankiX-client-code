using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class SelfUserAvatarComponent : BehaviourComponent {
        [SerializeField] ImageSkin avatar;

        [SerializeField] ImageListSkin border;

        [SerializeField] ImageListSkin rank;

        public void SetAvatar(string spriteUid) {
            if (avatar == null) {
                Debug.LogError(rank.gameObject.name + ":" + rank.transform.parent.name + ":" + rank.transform.parent.parent.name + ":" + rank.transform.parent.parent.parent.name);
            }

            avatar.SpriteUid = spriteUid;
        }

        public void SetRank() {
            SetRank(this.SendEvent<GetUserLevelInfoEvent>(SelfUserComponent.SelfUser).Info.Level + 1);
        }

        public void SetRank(int level) {
            rank.SelectSprite(level.ToString());
        }

        public void SetLeagueBorder(int league) {
            border.SelectSprite(league.ToString());
        }
    }
}