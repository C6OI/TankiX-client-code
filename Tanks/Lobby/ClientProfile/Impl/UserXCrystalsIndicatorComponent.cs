using Lobby.ClientControls.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class UserXCrystalsIndicatorComponent : BehaviourComponent {
        [SerializeField] Text moneyText;

        Animator animator;

        long money;

        Animator Animator {
            get {
                if (animator == null) {
                    animator = GetComponent<Animator>();
                }

                return animator;
            }
        }

        public void SetMoneyImmediately(long value) {
            money = value;
            ApplyMoney();
        }

        public void SetMoneyAnimated(long value) {
            money = value;
            Animator.SetTrigger("flash");
        }

        void ApplyMoney() => moneyText.text = money.ToStringSeparatedByThousands();
    }
}