using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MultipleBonusView : MonoBehaviour {
        public TextMeshProUGUI countText;

        public TextMeshProUGUI text;

        public GameObject back;

        public void UpdateView(long amount) {
            countText.text = "x" + amount;
            back.SetActive(true);
            text.text = text.text.ToUpper();
        }
    }
}