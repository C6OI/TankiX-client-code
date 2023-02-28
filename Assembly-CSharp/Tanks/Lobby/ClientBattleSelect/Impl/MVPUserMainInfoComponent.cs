using Tanks.Battle.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientUserProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPUserMainInfoComponent : MonoBehaviour {
        [SerializeField] TextMeshProUGUI nickname;

        [SerializeField] ImageSkin avatar;

        [SerializeField] ImageListSkin league;

        [SerializeField] RankIconComponent rank;

        public void Set(UserResult mvp) {
            nickname.SetText(mvp.Uid.Replace("botxz_", string.Empty));

            if (mvp.League != null) {
                league.SelectedSpriteIndex = mvp.League.GetComponent<LeagueConfigComponent>().LeagueIndex;
            } else {
                league.SelectedSpriteIndex = 0;
            }

            rank.SetRank(mvp.Rank);
            avatar.SpriteUid = mvp.AvatarId;
        }
    }
}