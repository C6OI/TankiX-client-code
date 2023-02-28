using System.Collections.Generic;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class MainHUDComponent : BehaviourComponent {
        [SerializeField] HPBar hpBar;

        [SerializeField] HPBarGlow hpBar2;

        [SerializeField] EnergyBar energyBar;

        [SerializeField] EnergyBarGlow energyBar2;

        public BattleHudRootComponent battleHudRoot;

        [SerializeField] TextAnimation message;

        [SerializeField] GameObject battleLog;

        [SerializeField] GameObject inventory;

        bool activated;

        bool isShow;

        readonly SortedList<int, string> messages = new();

        public float CurrentHpValue {
            set {
                hpBar.CurrentValue = value;
                hpBar2.CurrentValue = value;
            }
        }

        public float MaxHpValue {
            set {
                hpBar.MaxValue = value;
                hpBar2.MaxValue = value;
            }
        }

        public float CurrentEnergyValue {
            get => energyBar.CurrentValue;
            set {
                energyBar.CurrentValue = value;
                energyBar2.CurrentValue = value;
            }
        }

        public float MaxEnergyValue {
            get => energyBar.MaxValue;
            set {
                energyBar.MaxValue = value;
                energyBar2.MaxValue = value;
            }
        }

        public float EnergyAmountPerSegment {
            set {
                energyBar.AmountPerSegment = value;
                energyBar2.AmountPerSegment = value;
            }
        }

        public bool EnergyBarEnabled {
            set {
                energyBar.gameObject.SetActive(value);
                energyBar2.gameObject.SetActive(value);
            }
        }

        public long HullId {
            set => hpBar.HullId = value;
        }

        public long TurretId {
            set => energyBar.TurretId = value;
        }

        void Update() {
            bool flag = Input.GetKey(KeyCode.RightAlt) || Input.GetKey(KeyCode.LeftAlt);

            if (Input.GetKeyDown(KeyCode.Slash) && flag) {
                if (isShow) {
                    Hide();
                } else {
                    Activate();
                }
            }
        }

        void OnDisable() {
            CancelInvoke();
            messages.Clear();
            activated = false;
            battleLog.SetActive(false);
        }

        public void EnergyBlink(bool value) {
            energyBar.Blink(value);
            energyBar2.Blink(value);
        }

        public void EnergyInjectionBlink(bool value) {
            energyBar.EnergyInjectionBlink(value);
            energyBar2.EnergyInjectionBlink(value);
        }

        public void StopEnergyBlink() {
            energyBar.StopBlinking();
            energyBar2.StopBlinking();
        }

        public void SetMessageTDMPosition() {
            message.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -12f);
        }

        public void SetMessageCTFPosition() {
            message.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -51.5f);
        }

        public void ShowMessageWithPriority(string message, int priority = 0) {
            if (messages.ContainsKey(priority)) {
                messages[priority] = message;
            } else {
                messages.Add(priority, message);
            }

            if (activated) {
                this.message.Text = messages.Values[messages.Count - 1];
            }
        }

        public void RemoveMessageByPriority(int priority) {
            messages.Remove(priority);
        }

        public void Activate() {
            Canvas componentInParent = GetComponentInParent<Canvas>();
            componentInParent.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
            componentInParent.planeDistance = 10f;
            GetComponent<Animator>().SetTrigger("Show");
        }

        public void Hide() {
            GetComponent<Animator>().SetTrigger("Hide");
            isShow = false;
        }

        void AfterActivation() {
            isShow = true;
            activated = true;

            if (messages.Count > 0) {
                message.Text = messages.Values[messages.Count - 1];
            }

            Invoke("EnableBattleLog", 1f);
        }

        void EnableBattleLog() {
            battleLog.SetActive(true);
        }

        public void SetSpecatatorMode() {
            hpBar.gameObject.SetActive(false);
            hpBar2.gameObject.SetActive(false);
            EnergyBarEnabled = false;
            inventory.SetActive(false);
            MainHUDVersionSwitcher component = GetComponent<MainHUDVersionSwitcher>();
            component.specMode = true;
            component.SetCurrentHud();
        }

        public void SetTankMode() {
            hpBar.gameObject.SetActive(true);
            hpBar2.gameObject.SetActive(true);
            EnergyBarEnabled = true;
            inventory.SetActive(true);
            MainHUDVersionSwitcher component = GetComponent<MainHUDVersionSwitcher>();
            component.specMode = false;
            component.SetCurrentHud();
        }
    }
}