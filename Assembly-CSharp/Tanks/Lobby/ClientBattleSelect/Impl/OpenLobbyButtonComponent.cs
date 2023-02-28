using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class OpenLobbyButtonComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI _buttonText;

        [SerializeField] LocalizedField _openText;

        [SerializeField] LocalizedField _openTooltipText;

        [SerializeField] LocalizedField _shareTooltipText;

        [SerializeField] TooltipShowBehaviour _tooltip;

        [SerializeField] GaragePrice _price;

        long _lobbyId;

        public long LobbyId {
            get => _lobbyId;
            set {
                _lobbyId = value;
                _buttonText.text = _lobbyId.ToString();
                _tooltip.TipText = _shareTooltipText;
            }
        }

        public int Price {
            set {
                _price.transform.parent.gameObject.SetActive(value > 0);
                _price.SetPrice(0, value);
            }
        }

        public void ResetButtonText() {
            _buttonText.text = _openText;
            _tooltip.TipText = _openTooltipText;
        }
    }
}