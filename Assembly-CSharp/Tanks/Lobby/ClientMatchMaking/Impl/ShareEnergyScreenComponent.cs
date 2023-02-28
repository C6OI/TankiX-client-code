using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class ShareEnergyScreenComponent : BehaviourComponent {
        [SerializeField] Button startButton;

        [SerializeField] Button exitButton;

        [SerializeField] Button hideButton;

        [SerializeField] TextMeshProUGUI readyPlayers;

        [SerializeField] LocalizedField notAllPlayersReady;

        [SerializeField] LocalizedField allPlayersReady;

        [SerializeField] CircleProgressBar teleportPriceProgressBar;

        public CircleProgressBar TeleportPriceProgressBar => teleportPriceProgressBar;

        public bool SelfPlayerIsSquadLeader {
            set {
                startButton.gameObject.SetActive(value);
                exitButton.gameObject.SetActive(value);
                hideButton.gameObject.SetActive(!value);
            }
        }

        void OnEnable() {
            hideButton.onClick.AddListener(MainScreenComponent.Instance.HideShareEnergyScreen);
        }

        void OnDisable() {
            hideButton.onClick.RemoveListener(MainScreenComponent.Instance.HideShareEnergyScreen);
        }

        public void ReadyPlayers(int ready, int allPlayers) {
            bool flag = allPlayers == ready;
            readyPlayers.text = !flag ? string.Format(notAllPlayersReady, ready, allPlayers) : allPlayersReady.Value;
        }

        public void BackClick(BaseEventData data) {
            ScheduleEvent<HideAllShareButtonsEvent>(new EntityStub());
        }
    }
}