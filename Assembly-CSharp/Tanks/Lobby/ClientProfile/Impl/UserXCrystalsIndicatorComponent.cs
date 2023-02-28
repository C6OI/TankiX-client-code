using System.Collections;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class UserXCrystalsIndicatorComponent : BehaviourComponent {
        const float setMoneyAnimationSpeedPerSec = 100f;

        const float setMoneyAnimationMaxTime = 5f;

        [SerializeField] Text moneyText;

        Animator animator;

        long money;

        long moneyExpected;

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
            if (moneyExpected > 0 && !money.Equals(moneyExpected)) {
                StopAllCoroutines();
                money = moneyExpected;
                ApplyMoney();
            }

            moneyExpected = value;
            StartCoroutine(ShowAnimation(value));
        }

        IEnumerator ShowAnimation(long newMoneyValue) {
            float moneyDiff = newMoneyValue - money;
            float setMoneyAnimationTime = Mathf.Min(Mathf.Abs(moneyDiff) / 100f, 5f);
            long moneyDiffSign = (long)Mathf.Sign(moneyDiff);
            float delay = moneyDiff == 0f ? 0.01f : setMoneyAnimationTime / Mathf.Abs(moneyDiff);
            int step = Mathf.CeilToInt(0.02f / delay);

            while (Mathf.Abs(money - newMoneyValue) > step) {
                yield return new WaitForSeconds(delay);

                money += moneyDiffSign * step;
                ApplyMoney();
            }

            yield return new WaitForSeconds(delay);

            money = newMoneyValue;
            ApplyMoney();
            Animator.SetTrigger("flash");
        }

        void ApplyMoney() {
            moneyText.text = money.ToStringSeparatedByThousands();
        }
    }
}