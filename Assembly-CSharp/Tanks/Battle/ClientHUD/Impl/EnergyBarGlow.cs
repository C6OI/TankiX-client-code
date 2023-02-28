using System.Collections;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class EnergyBarGlow : HUDBar {
        [SerializeField] Ruler fill;

        [SerializeField] Ruler glow;

        [SerializeField] Ruler energyInjectionGlow;

        [SerializeField] BarFillEnd barFillEnd;

        float amountPerSegment = 1f;

        bool isBlinking;

        public override float CurrentValue {
            get => currentValue;
            set {
                currentValue = Clamp(value);
                ApplyFill();
            }
        }

        public override float AmountPerSegment {
            get => amountPerSegment;
            set {
                if (amountPerSegment != value) {
                    amountPerSegment = value;
                    UpdateSegments();
                }
            }
        }

        void OnDisable() {
            StopBlinking();
        }

        public void Blink(bool canShoot) {
            GetComponent<Animator>().SetBool("CanShoot", canShoot);
            GetComponent<Animator>().SetTrigger("Blink");
        }

        public void EnergyInjectionBlink(bool canShoot) {
            GetComponent<Animator>().SetBool("CanShoot", canShoot);
            GetComponent<Animator>().SetTrigger("EnergyInjectionBlink");
        }

        public void StartBlinking(bool canShoot) {
            if (!isBlinking) {
                StartCoroutine(BlinkCoroutine(canShoot));
            }
        }

        public void StopBlinking() {
            isBlinking = false;
        }

        IEnumerator BlinkCoroutine(bool canShoot) {
            isBlinking = true;

            while (isBlinking) {
                Blink(canShoot);
                yield return new WaitForSeconds(0.5f);
            }
        }

        void ApplyFill() {
            fill.FillAmount = currentValue / maxValue;
            glow.FillAmount = fill.FillAmount;
            energyInjectionGlow.FillAmount = fill.FillAmount;
            barFillEnd.FillAmount = fill.FillAmount;
        }
    }
}