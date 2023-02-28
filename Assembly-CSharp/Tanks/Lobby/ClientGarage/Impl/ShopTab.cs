using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ShopTab : Tab {
        [SerializeField] bool showBackground = true;

        [SerializeField] Animator backgroundAnimator;

        protected override void OnEnable() {
            base.OnEnable();
            ContainersUI component = gameObject.GetComponent<ContainersUI>();

            if (component != null) {
                component.OnEnable();
            }

            backgroundAnimator.SetBool("Background", showBackground);
        }

        public override void Hide() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            base.Hide();
        }
    }
}