using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ExitBattleWindow : LocalizedControl {
        [SerializeField] TextMeshProUGUI yesText;

        [SerializeField] TextMeshProUGUI noText;

        [SerializeField] TextMeshProUGUI headerText;

        [SerializeField] TextMeshProUGUI firstLineText;

        [SerializeField] TextMeshProUGUI secondLineText;

        [SerializeField] TextMeshProUGUI thirdLineText;

        [SerializeField] TextMeshProUGUI warningText;

        [SerializeField] Button yes;

        [SerializeField] Button no;

        [SerializeField] Color newbieHeaderColor;

        [SerializeField] Color regularHeaderColor;

        [SerializeField] Image warningSign;

        bool alive;

        Entity battleUser;

        bool igoreFirstEscape;

        bool savedCursorVisible;

        CursorLockMode savedLockMode;

        Entity screen;

        [Inject] public static InputManager InputManager { get; set; }

        public string YesText { get; set; }

        public string CustomYesText { get; set; }

        public string NoText {
            set => noText.text = value;
        }

        public string FirstLineText { get; set; }

        public string SecondLineText { get; set; }

        public string CustomBattleSecondLineText { get; set; }

        public string ThirdLineText { get; set; }

        public string CustomThirdLineText { get; set; }

        public string WarningText {
            set => warningText.text = value;
        }

        public string RegularHeaderText { get; set; }

        public string NewbieExitText { get; set; }

        public string NewbieSecondLineText { get; set; }

        public string NewbieThirdLineText { get; set; }

        void Start() {
            alive = true;
            yes.onClick.AddListener(OnYes);
            no.onClick.AddListener(OnNo);
        }

        void Update() {
            if (InputMapping.Cancel) {
                if (igoreFirstEscape) {
                    igoreFirstEscape = false;
                    return;
                }

                OnNo();
            }

            if (!yes.interactable) {
                yes.interactable = true;
            }

            if (!no.interactable) {
                no.interactable = true;
            }
        }

        void OnDisable() {
            if (alive) {
                InputManager.Resume();

                if (screen.HasComponent<LockedScreenComponent>()) {
                    screen.RemoveComponent<LockedScreenComponent>();
                }

                EngineService.Engine.ScheduleEvent<BattleInputContextSystem.CheckMouseEvent>(battleUser);
            }
        }

        void OnApplicationQuit() {
            alive = false;
        }

        void SaveCursorStateAndShow() {
            savedLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
            savedCursorVisible = Cursor.visible;
            Cursor.visible = true;
        }

        void RestoreCurorState() {
            Cursor.lockState = savedLockMode;
            Cursor.visible = savedCursorVisible;
        }

        void OnYes() {
            EngineService.Engine.ScheduleEvent<GoBackFromBattleEvent>(screen);
            Hide();
        }

        void OnNo() {
            Hide();
        }

        public void Hide() {
            if (gameObject.activeInHierarchy) {
                GetComponent<Animator>().SetBool("Visible", false);
            }

            RestoreCurorState();
        }

        public void Show(Entity screen, Entity battleUser, bool customBattle, bool isDeserter, bool isNewbieBattle) {
            this.screen = screen;
            this.battleUser = battleUser;
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            SaveCursorStateAndShow();
            InputManager.Suspend();

            if (InputMapping.Cancel) {
                igoreFirstEscape = true;
            }

            gameObject.SetActive(true);
            no.GetComponent<Animator>().ResetTrigger("Normal");
            no.GetComponent<Animator>().SetTrigger("Highlighted");
            no.Select();

            if (!screen.HasComponent<LockedScreenComponent>()) {
                screen.AddComponent<LockedScreenComponent>();
            }

            bool flag = !battleUser.HasComponent<UserInBattleAsTankComponent>();
            firstLineText.gameObject.SetActive(false);

            if (!isNewbieBattle) {
                warningSign.gameObject.SetActive(true);
                headerText.color = regularHeaderColor;
                headerText.text = RegularHeaderText;
                secondLineText.gameObject.SetActive(!flag);
                secondLineText.text = !customBattle ? SecondLineText : CustomBattleSecondLineText;
                thirdLineText.text = !customBattle && !flag ? ThirdLineText : CustomThirdLineText;
                yesText.text = !customBattle && !flag && isDeserter ? YesText : CustomYesText;
                warningText.gameObject.SetActive(isDeserter && !customBattle && !flag);
            } else {
                warningSign.gameObject.SetActive(false);
                headerText.color = newbieHeaderColor;
                headerText.text = NewbieExitText;
                secondLineText.text = NewbieSecondLineText;
                thirdLineText.text = NewbieThirdLineText;
                warningText.gameObject.SetActive(false);
                yesText.text = CustomYesText;
            }
        }
    }
}