using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ExitBattleWindow : LocalizedControl {
        [SerializeField] Text yesText;

        [SerializeField] Text noText;

        [SerializeField] Text headerText;

        [SerializeField] Text footerText;

        [SerializeField] Button yes;

        [SerializeField] Button no;

        Entity battleUser;

        bool igoreFirstEscape;

        CursorLockMode savedLockMode;

        Entity screen;

        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static InputManager InputManager { get; set; }

        public string YesText {
            set => yesText.text = value;
        }

        public string NoText {
            set => noText.text = value;
        }

        public string HeaderText {
            set => headerText.text = value;
        }

        public string FooterText {
            set => footerText.text = value;
        }

        void Start() {
            yes.onClick.AddListener(OnYes);
            no.onClick.AddListener(OnNo);
        }

        void Update() {
            if (InputMapping.Cancel) {
                if (igoreFirstEscape) {
                    igoreFirstEscape = false;
                } else {
                    OnNo();
                }
            }
        }

        void OnDisable() {
            InputManager.Resume();

            EngineService.ExecuteInFlow(delegate(Engine e) {
                if (screen.HasComponent<LockedScreenComponent>()) {
                    screen.RemoveComponent<LockedScreenComponent>();
                }

                e.ScheduleEvent<BattleInputContextSystem.CheckMouseEvent>(battleUser);
            });
        }

        void OnYes() {
            EngineService.ExecuteInFlow(delegate(Engine e) {
                e.ScheduleEvent<GoBackFromBattleEvent>(screen);
            });

            Hide();
        }

        void OnNo() => Hide();

        void Hide() {
            GetComponent<Animator>().SetBool("Visible", false);
            Cursor.lockState = savedLockMode;

            if (battleUser.HasComponent<MouseControlStateHolderComponent>()) {
                Cursor.visible = !battleUser.GetComponent<MouseControlStateHolderComponent>().MouseControlAllowed;
            } else {
                Cursor.visible = false;
            }
        }

        public void Show(Entity screen, Entity battleUser) {
            this.screen = screen;
            this.battleUser = battleUser;
            InputManager.DeactivateContext(BasicContexts.MOUSE_CONTEXT);
            savedLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InputManager.Suspend();

            if (InputMapping.Cancel) {
                igoreFirstEscape = true;
            }

            gameObject.SetActive(true);
            no.GetComponent<Animator>().ResetTrigger("Normal");
            no.GetComponent<Animator>().SetTrigger("Highlighted");
            no.Select();

            EngineService.ExecuteInFlow(delegate {
                if (!screen.HasComponent<LockedScreenComponent>()) {
                    screen.AddComponent<LockedScreenComponent>();
                }
            });
        }
    }
}