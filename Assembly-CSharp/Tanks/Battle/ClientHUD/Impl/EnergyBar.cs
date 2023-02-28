using System.Collections;
using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class EnergyBar : HUDBar {
        [SerializeField] Ruler stroke;

        [SerializeField] Ruler fill;

        [SerializeField] Ruler glow;

        [SerializeField] Ruler energyInjectionGlow;

        [SerializeField] TankPartItemIcon turretIcon;

        float amountPerSegment = 1f;

        bool isBlinking;

        public long TurretId {
            set => turretIcon.SetIconWithName(value.ToString());
        }

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
            Animator component = GetComponent<Animator>();

            if (component.isActiveAndEnabled) {
                component.SetBool("CanShoot", canShoot);
                component.SetTrigger("Blink");
            }
        }

        public void EnergyInjectionBlink(bool canShoot) {
            Animator component = GetComponent<Animator>();

            if (component.isActiveAndEnabled) {
                component.SetBool("CanShoot", canShoot);
                component.SetTrigger("EnergyInjectionBlink");
            }
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
            stroke.RectTransform.anchoredPosition = new Vector2(fill.RectTransform.rect.width * fill.FillAmount, stroke.RectTransform.anchoredPosition.y);
            stroke.FillAmount = 1f - fill.FillAmount;
            glow.FillAmount = fill.FillAmount;
            energyInjectionGlow.FillAmount = fill.FillAmount;
        }
    }
}