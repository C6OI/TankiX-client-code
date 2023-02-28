using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultCommonUIComponent : UIBehaviour, Component {
        public Image tankPreviewImage1;

        public Image tankPreviewImage2;

        public TopPanelButtons topPanelButtons;

        public BottomPanelButtons bottomPanelButtons;

        [SerializeField] GameObject[] screenParts;

        ResultScreenParts currentPart;

        bool customBattle;

        bool enoughEnergy;

        bool spectator;

        bool squad;

        bool tutor;

        ResultScreenParts CurrentPart {
            get => currentPart;
            set {
                currentPart = value;
                GetComponent<Animator>().SetInteger("currentScreen", (int)value);
            }
        }

        new void OnDisable() {
            CurrentPart = ResultScreenParts.None;
            GameObject[] array = screenParts;

            foreach (GameObject gameObject in array) {
                gameObject.SetActive(false);
            }
        }

        public void ShowTopPanel() {
            GetComponent<Animator>().SetBool("showTopPanel", true);
        }

        public void HideTopPanel() {
            GetComponent<Animator>().SetBool("showTopPanel", false);
        }

        public void ShowBottomPanel() {
            GetComponent<Animator>().SetBool("showBottomPanel", true);
            bottomPanelButtons.BattleSeriesResult.SetActive(!spectator && !customBattle && !tutor);
            bottomPanelButtons.TryAgainButton.SetActive(!spectator && !customBattle && !tutor && !squad && enoughEnergy);
            bottomPanelButtons.MainScreenButton.gameObject.SetActive(spectator || !customBattle);
            bottomPanelButtons.ContinueButton.gameObject.SetActive(!spectator && customBattle);
        }

        public void HideBottomPanel() {
            GetComponent<Animator>().SetBool("showBottomPanel", false);
        }

        public void ShowScreen(bool customBattle, bool spectator, bool tutor, bool squad, bool enoughEnergy) {
            this.customBattle = customBattle;
            this.spectator = spectator;
            this.tutor = tutor;
            this.squad = squad;
            this.enoughEnergy = enoughEnergy;

            if (customBattle) {
                ShowStats();
                return;
            }

            ShowBestPlayer();
            MVPScreenUIComponent.ShowCounter = 0;
        }

        public void ShowBestPlayer() {
            HideTopPanel();
            HideBottomPanel();
            CurrentPart = ResultScreenParts.BestPlayer;
            topPanelButtons.ActivateButton(0);
            MVPScreenUIComponent.ShowCounter++;
        }

        public void ContinueAfterBestPlayer() {
            if (spectator) {
                ShowStats();
                HideTopPanel();
            } else {
                ShowAwards();
            }
        }

        public void ShowAwards() {
            CurrentPart = ResultScreenParts.Awards;
            topPanelButtons.ActivateButton(1);
        }

        public void ShowStats() {
            CurrentPart = ResultScreenParts.Stats;
            ShowBottomPanel();

            if (!customBattle) {
                ShowTopPanel();
                topPanelButtons.ActivateButton(2);
            }
        }

        enum ResultScreenParts {
            None = -1,
            BestPlayer = 0,
            Awards = 1,
            Stats = 2
        }
    }
}