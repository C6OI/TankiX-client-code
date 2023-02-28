using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    [RequireComponent(typeof(Animator))]
    public class ItemButtonComponent : BehaviourComponent {
        [SerializeField] CooldownAnimation cooldown;

        [SerializeField] PaletteColorField epicColor;

        [SerializeField] PaletteColorField exceptionalColor;

        [SerializeField] ImageSkin icon;

        [SerializeField] TextMeshProUGUI keyBind;

        [SerializeField] Animator lockByEMPAnimator;

        [SerializeField] Color CDColor;

        [SerializeField] Color FullCDColor = new(1f, 0.15f, 0f, 0.74f);

        [SerializeField] Color LowCDColor = new(0.3f, 0.9f, 0.5f, 0.74f);

        [SerializeField] Color RageCDColor;

        [SerializeField] Image CDFill;

        [SerializeField] GameObject barRoot;

        [SerializeField] AmmunitionBar ammunitionBarPrefab;

        [SerializeField] AmmunitionBar[] ammunitionBars;

        public bool ammunitionCountWasIncreased;

        public bool isRage;

        public float CooldownCoeff = 1f;

        float _cooldownMultiplier;
        Animator animator;

        float cooldownTime;

        float cooldownTimer;

        int currentAmmunitionItemIndex;

        bool inCooldown;

        int maxItemAmmunitionCount;

        Animator Animator {
            get {
                if (animator == null) {
                    animator = GetComponent<Animator>();
                }

                return animator;
            }
        }

        public string Icon {
            set => icon.SpriteUid = value;
        }

        public string KeyBind {
            set => keyBind.text = value;
        }

        public int MaxItemAmmunitionCount {
            get => maxItemAmmunitionCount;
            set {
                maxItemAmmunitionCount = value;

                if (value > 1) {
                    ammunitionBars = new AmmunitionBar[value];

                    for (int i = 0; i < value; i++) {
                        AmmunitionBar ammunitionBar = Instantiate(ammunitionBarPrefab, barRoot.transform);
                        ammunitionBars[i] = ammunitionBar;
                    }
                }
            }
        }

        public int ItemAmmunitionCount {
            get => currentAmmunitionItemIndex;
            set {
                ammunitionCountWasIncreased = currentAmmunitionItemIndex < value;
                currentAmmunitionItemIndex = value;
                Animator.SetInteger("AmmunitionCount", value);

                for (int i = 0; i < ammunitionBars.Length; i++) {
                    AmmunitionBar ammunitionBar = ammunitionBars[i];
                    ammunitionBar.FillValue = 1f;

                    if (i < value) {
                        ammunitionBar.Activate();
                    } else {
                        ammunitionBar.Deactivate();
                    }
                }
            }
        }

        public bool TabPressed {
            set => Animator.SetBool("IsTab", value);
        }

        void Update() {
            if (inCooldown) {
                cooldownTimer += CooldownCoeff * Time.deltaTime;
                float num = cooldownTimer / cooldownTime;
                float num2 = Mathf.Clamp01(1f - num);

                if (maxItemAmmunitionCount > 0 && currentAmmunitionItemIndex < ammunitionBars.Length && Mathf.Abs(CDFill.fillAmount - num2) > 0f) {
                    ammunitionBars[currentAmmunitionItemIndex].FillValue = num;
                }

                if (!isRage) {
                    CDFill.color = Color.Lerp(FullCDColor, LowCDColor, num);
                }

                CDFill.fillAmount = num2;

                if (cooldownTimer >= cooldownTime) {
                    inCooldown = false;
                }
            }
        }

        public void Activate() {
            Animator.SetTrigger("Activate");
        }

        public void SetEpic() {
            icon.GetComponent<Image>().color = epicColor;
        }

        public void SetExceptional() {
            icon.GetComponent<Image>().color = exceptionalColor;
        }

        public void FinishCooldown() {
            Animator.ResetTrigger("StartCooldown");
            Animator.ResetTrigger("StartRageCooldown");
            Animator.SetTrigger("FinishCooldown");
        }

        public void Enable() {
            Animator.SetBool("Enabled", true);
        }

        public void Disable() {
            Animator.SetBool("Enabled", false);
        }

        public void PressedWhenDisable() {
            Animator.SetTrigger("PressedWhenDisable");
        }

        public void Passive() {
            Animator.SetBool("Passive", true);
        }

        public void RageMode() {
            Animator.SetBool("Rage", isRage);
        }

        public void SetCooldownCoeff(float coeff) {
            CooldownCoeff = coeff;
        }

        public void StartCooldown(float timeInSec, bool slotEnabled) {
            CDFill.color = CDColor;
            Animator.ResetTrigger("FinishCooldown");
            Animator.SetTrigger("StartCooldown");
            Animator.SetBool("Enabled", slotEnabled);
            _cooldownMultiplier = 1f / timeInSec;
            Animator.SetFloat("CooldownMultiplier", _cooldownMultiplier);
            cooldown.Cooldown = timeInSec;
            cooldownTime = timeInSec;
            cooldownTimer = 0f;
            inCooldown = true;
        }

        public void StartRageCooldown(float timeInSec, bool slotEnabled) {
            CDFill.color = RageCDColor;
            Animator.ResetTrigger("FinishCooldown");
            Animator.SetTrigger("StartCooldown");
            Animator.SetBool("Enabled", slotEnabled);
            _cooldownMultiplier = 1f / (timeInSec / CooldownCoeff);
            Animator.SetFloat("CooldownMultiplier", _cooldownMultiplier);
            cooldown.Cooldown = timeInSec;
            cooldownTime = timeInSec;
            cooldownTimer = 0f;
            inCooldown = true;
            isRage = true;
        }

        public void ChangeCooldown(float time, float coeff, bool slotEnabled) {
            SetCooldownCoeff(coeff);

            if (isRage) {
                _cooldownMultiplier = 1f / cooldown.Cooldown;
                CDFill.color = CDColor;
            } else {
                _cooldownMultiplier = 1f / (cooldown.Cooldown / CooldownCoeff);
                CDFill.color = RageCDColor;
            }

            Animator.SetFloat("CooldownMultiplier", _cooldownMultiplier);
        }

        public void CutCooldown(float cutTime) {
            cooldown.Cooldown = cutTime;
            Animator.SetTrigger("Bloodlust");
            Animator.SetFloat("CooldownMultiplier", 1f / cutTime);
        }

        public void LockByEMP(bool isLock) {
            lockByEMPAnimator.SetBool("Locked", isLock);
        }
    }
}