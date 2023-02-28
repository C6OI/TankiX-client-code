using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ShopTabManager : TabManager {
        static int _shopTabIndex;

        public static ShopTabManager Instance { get; private set; }

        public static int shopTabIndex {
            get => shopTabIndex;
            set {
                if (Instance == null) {
                    _shopTabIndex = value;
                } else {
                    Instance.Show(value);
                }
            }
        }

        void Awake() {
            Instance = this;
        }

        protected override void OnEnable() {
            Show(_shopTabIndex);
        }

        public override void Show(int newIndex) {
            _shopTabIndex = newIndex;
            base.Show(newIndex);
            LogScreen screen;

            switch (newIndex) {
                case 1:
                    screen = LogScreen.ShopBlueprints;
                    break;

                case 2:
                    screen = LogScreen.ShopContainers;
                    break;

                case 3:
                    screen = LogScreen.ShopXCry;
                    break;

                case 4:
                    screen = LogScreen.ShopCry;
                    break;

                case 5:
                    screen = LogScreen.ShopPrem;
                    break;

                case 6:
                    screen = LogScreen.GoldBoxes;
                    break;

                default:
                    screen = LogScreen.ShopDeals;
                    break;
            }

            MainScreenComponent.Instance.SendShowScreenStat(screen);
        }
    }
}