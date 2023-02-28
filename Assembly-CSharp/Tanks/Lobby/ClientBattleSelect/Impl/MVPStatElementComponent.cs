using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPStatElementComponent : MonoBehaviour {
        [SerializeField] TextMeshProUGUI count;

        [SerializeField] GameObject best;

        int _count;

        public int Count {
            get => _count;
            set {
                _count = value;
                count.text = _count.ToString();
            }
        }

        public void SetBest(bool isBest) {
            best.gameObject.SetActive(isBest);
        }

        public bool ShowIfCan() {
            gameObject.SetActive(_count > 0);
            return _count > 0;
        }

        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}