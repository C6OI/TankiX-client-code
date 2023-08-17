using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [SerialVersionUID(635824351093325226L)]
    public class BattleSelectScreenComponent : MonoBehaviour, Component {
        [SerializeField] EntityBehaviour itemContentPrefab;

        [SerializeField] GameObject prevBattlesButton;

        [SerializeField] GameObject nextBattlesButton;

        [SerializeField] GameObject enterBattleDMButton;

        [SerializeField] GameObject enterBattleRedButton;

        [SerializeField] GameObject enterBattleBlueButton;

        [SerializeField] GameObject enterAsSpectatorButton;

        [SerializeField] RectTransform battleInfoPanelsContainer;

        [SerializeField] GameObject dmInfoPanel;

        [SerializeField] GameObject tdmInfoPanel;

        [SerializeField] GameObject entrancePanel;

        [SerializeField] GameObject friendsPanel;

        [SerializeField] Text enterBattleDMButtonText;

        [SerializeField] Text enterBattleRedButtonText;

        [SerializeField] Text enterBattleBlueButtonText;

        [SerializeField] Text enterAsSpectatorButtonText;

        public Text EnterAsSpectatorButtonText => enterAsSpectatorButtonText;

        public Text EnterBattleBlueButtonText => enterBattleBlueButtonText;

        public Text EnterBattleRedButtonText => enterBattleRedButtonText;

        public Text EnterBattleDmButtonText => enterBattleDMButtonText;

        public EntityBehaviour ItemContentPrefab => itemContentPrefab;

        public GameObject PrevBattlesButton => prevBattlesButton;

        public GameObject NextBattlesButton => nextBattlesButton;

        public GameObject EnterBattleDMButton => enterBattleDMButton;

        public GameObject EnterBattleRedButton => enterBattleRedButton;

        public GameObject EnterBattleBlueButton => enterBattleBlueButton;

        public GameObject EnterBattleAsSpectatorButton => enterAsSpectatorButton;

        public RectTransform BattleInfoPanelsContainer => battleInfoPanelsContainer;

        public GameObject DMInfoPanel => dmInfoPanel;

        public GameObject TDMInfoPanel => tdmInfoPanel;

        public GameObject EntrancePanel => entrancePanel;

        public GameObject FriendsPanel => friendsPanel;

        public bool DebugEnabled { get; set; }

        void Awake() {
            DMInfoPanel.SetActive(false);
            TDMInfoPanel.SetActive(false);
        }
    }
}