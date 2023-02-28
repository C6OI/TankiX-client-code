using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PlayerInfoUI : MonoBehaviour {
        [SerializeField] TextMeshProUGUI uid;

        [SerializeField] Text containerLeftMultiplicator;

        [SerializeField] ImageListSkin rank;

        [SerializeField] Text containerRightMultiplicator;

        [SerializeField] TextMeshProUGUI kills;

        [SerializeField] TextMeshProUGUI deaths;

        [SerializeField] TextMeshProUGUI score;

        [SerializeField] TextMeshProUGUI hull;

        [SerializeField] TextMeshProUGUI turret;

        [SerializeField] Graphic background;

        [SerializeField] Button interactionsButton;

        [HideInInspector] public long ownerId;

        [HideInInspector] public long battleId;

        public void Init(long battleId, int position, int rank, string uid, int kills, int score, int deaths, Color color, long hull, long turret, long ownerId, bool isSelf,
            bool containerLeft, bool containerRight = false) {
            Debug.LogError("sad", gameObject);
            background.color = color;
            this.rank.SelectSprite(rank.ToString());
            this.uid.text = uid;
            this.kills.text = kills.ToString();
            this.deaths.text = deaths.ToString();
            this.hull.text = "<sprite name=\"" + hull + "\">";
            this.turret.text = "<sprite name=\"" + turret + "\">";
            this.ownerId = ownerId;
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