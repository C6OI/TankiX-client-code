using Tanks.Lobby.ClientGarage.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    [RequireComponent(typeof(Button))]
    public class GoToShopButton : MonoBehaviour {
        [SerializeField] int tab;

        [SerializeField] BaseDialogComponent _callDialog;

        public int DesiredShopTab {
            get => tab;
            set => tab = value;
        }

        public BaseDialogComponent CallDialog {
            get => _callDialog;
            set => _callDialog = value;
        }

        void Awake() {
            GetComponent<Button>().onClick.AddListener(Go);
        }

        void Go() {
            MainScreenComponent.Instance.ShowShopIfNotVisible();
            ShopTabManager.shopTabIndex = tab;

            if (!(_callDialog == null)) {
                _callDialog.Hide();
            }
        }
    }
}