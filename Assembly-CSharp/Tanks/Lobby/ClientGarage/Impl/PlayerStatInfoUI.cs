using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PlayerStatInfoUI : MonoBehaviour {
        [SerializeField] ImageListSkin imageListSkin;

        [SerializeField] TextMeshProUGUI uid;

        [SerializeField] ImageListSkin league;

        [SerializeField] ImageSkin avatar;

        [SerializeField] Text containerLeftMultiplicator;

        [SerializeField] TextMeshProUGUI hull;

        [SerializeField] TextMeshProUGUI turret;

        [SerializeField] TextMeshProUGUI kills;

        [SerializeField] TextMeshProUGUI score;

        [SerializeField] Button interactionsButton;

        [HideInInspector] public long userId;

        [HideInInspector] public long battleId;

        [SerializeField] Text containerRightMultiplicator;

        public void Init(int leagueIndex, string uid, int kills, int deaths, int assists, int score, Color uidColor, long hullId, long turretId, long userId, long battleId,
            string avatarId, bool isSelf, bool isDm, bool isFriend, bool containerLeft, bool containerRight = false) {
            this.uid.color = uidColor;
            avatar.SpriteUid = avatarId;
            this.uid.text = uid.Replace("botxz_", string.Empty);

            if (isFriend) {
                this.uid.text = "<b>" + this.uid.text + "</b>";
            }

            league.SelectedSpriteIndex = leagueIndex;

            if (isDm) {
                this.kills.text = string.Format("{0}/{1}", kills, deaths);
            } else {
                this.kills.text = string.Format("{0}/{1}/{2}", kills, assists, deaths);
            }

            hull.text = "<sprite name=\"" + hullId + "\">";
            turret.text = "<sprite name=\"" + turretId + "\">";
            this.userId = userId;
            this.battleId = battleId;

            if (this.score != null) {
                this.score.text = score.ToStringSeparatedByThousands();
            }

            SetButtonState(isSelf);
        }

        void SetButtonState(bool isSelf) {
            if (interactionsButton != null) {
                interactionsButton.interactable = !isSelf;
            } else {
                Debug.LogError("Button reference wasn't set in " + gameObject.name);
            }
        }
    }
}