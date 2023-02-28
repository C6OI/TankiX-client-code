using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LoginRewardProgressBar : MonoBehaviour {
        public enum FillType {
            Empty = 0,
            Half = 1,
            Full = 2
        }

        [SerializeField] Image middleIcon;

        [SerializeField] Image leftLine;

        [SerializeField] Image rightLine;

        [SerializeField] Color fillColor;

        [SerializeField] Color emptyColor;

        public GameObject LeftLine => leftLine.gameObject;

        public GameObject RightLine => rightLine.gameObject;

        public void Fill(FillType type) {
            middleIcon.color = type != FillType.Half && type != FillType.Full ? emptyColor : fillColor;
            leftLine.color = type != FillType.Half && type != FillType.Full ? emptyColor : fillColor;
            rightLine.color = type != FillType.Full ? emptyColor : fillColor;
        }
    }
}