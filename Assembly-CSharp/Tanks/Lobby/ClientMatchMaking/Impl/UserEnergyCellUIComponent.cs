using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class UserEnergyCellUIComponent : BehaviourComponent, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler {
        [SerializeField] TextMeshProUGUI nickname;

        [SerializeField] TextMeshProUGUI energyValue;

        [SerializeField] TextMeshProUGUI energyGiftText;

        [SerializeField] Color notEnoughColor;

        [SerializeField] Image borederImage;

        [SerializeField] GameObject enoughView;

        [SerializeField] GameObject notEnoughView;

        [SerializeField] TextMeshProUGUI text;

        [SerializeField] LocalizedField shareEnergyText;

        [SerializeField] LocalizedField buyEnergyText;

        [SerializeField] GameObject shareButton;

        [SerializeField] GameObject line;

        [SerializeField] LocalizedField chargesAmountSingularText;

        [SerializeField] LocalizedField chargesAmountPlural1Text;

        [SerializeField] LocalizedField chargesAmountPlural2Text;

        [SerializeField] LocalizedField fromText;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        bool shareButtonIsOpen => GetComponent<Animator>().GetBool("showShareButton");

        public bool CellIsFirst {
            set {
                line.SetActive(!value);
                GetComponent<LayoutElement>().preferredWidth = !value ? 150 : 70;
            }
        }

        public bool Ready { get; private set; }

        public long ShareEnergyValue { get; private set; }

        public bool Buy { get; private set; }

        public void OnPointerEnter(PointerEventData eventData) {
            Entity entity = GetComponent<EntityBehaviour>().Entity;
            entity.RemoveComponentIfPresent<AdditionalTeleportEnergyPreviewComponent>();
            entity.AddComponent<AdditionalTeleportEnergyPreviewComponent>();
        }

        public void OnPointerExit(PointerEventData eventData) {
            Entity entity = GetComponent<EntityBehaviour>().Entity;
            entity.RemoveComponentIfPresent<AdditionalTeleportEnergyPreviewComponent>();
        }

        public void SetShareEnergyText(long value, bool buy) {
            ShareEnergyValue = value;
            Buy = buy;
            string arg = Pluralize((int)value);
            text.text = string.Format(!buy ? shareEnergyText : buyEnergyText, arg);
        }

        public void Setup(string nickname, long energyValue, long energyCost) {
            this.nickname.text = nickname;
            string text = string.Format("{0}/{1}", Mathf.Min(energyCost, energyValue), energyCost);

            if (this.energyValue.text != text) {
                this.energyValue.text = text;
                HideShareButton();
            }

            Ready = energyValue >= energyCost;
            this.energyValue.color = !Ready ? notEnoughColor : Color.white;
            borederImage.color = this.energyValue.color;
            enoughView.SetActive(Ready);
            notEnoughView.SetActive(!Ready);
        }

        public void SetGiftValue(long value, List<string> uids = null) {
            if (value == 0) {
                energyGiftText.text = string.Empty;
                return;
            }

            energyGiftText.text = string.Format("{0} {1}\n", Pluralize((int)value), fromText.Value);

            if (uids == null) {
                return;
            }

            for (int i = 0; i < uids.Count; i++) {
                string text = uids[i];
                energyGiftText.text += text;

                if (i != uids.Count - 1) {
                    energyGiftText.text += ", ";
                }
            }
        }

        public void ShowShareButton() {
            if (shareButtonIsOpen) {
                HideShareButton();
                return;
            }

            EngineService.Engine.ScheduleEvent<HideAllShareButtonsEvent>(new EntityStub());
            GetComponent<Animator>().SetBool("showShareButton", true);
        }

        public void HideShareButton() {
            GetComponent<Animator>().SetBool("showShareButton", false);
        }

        string Pluralize(int amount) {
            CaseType @case = CasesUtil.GetCase(amount);
            string arg = "<color=#84F6F6FF>" + amount + "</color>";

            switch (@case) {
                case CaseType.DEFAULT:
                    return string.Format(chargesAmountPlural1Text.Value, arg);

                case CaseType.ONE:
                    return string.Format(chargesAmountSingularText.Value, arg);

                case CaseType.TWO:
                    return string.Format(chargesAmountPlural2Text.Value, arg);

                default:
                    throw new Exception("ivnalid case");
            }
        }
    }
}