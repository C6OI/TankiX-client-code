using System;
using System.Collections.Generic;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class BattleResultSpecialOfferUiComponent : ItemContainerComponent {
        [SerializeField] TextMeshProUGUI titleText;

        [SerializeField] TextMeshProUGUI descriptionText;

        [SerializeField] GameObject smile;

        [SerializeField] SpecialOfferPriceButtonComponent priceButton;

        [SerializeField] SpecialOfferCrystalButtonComponent crystalButton;

        [SerializeField] SpecialOfferUseDiscountComponent useDiscountButton;

        [SerializeField] SpecialOfferTakeRewardButtonComponent takeRewardButton;

        [SerializeField] Button tutorialRewardButton;

        [SerializeField] SpecialOfferOpenContainerButton openButton;

        [SerializeField] SpecialOfferWorthItComponent worthIt;

        [SerializeField] Animator animator;

        [SerializeField] LocalizedField tutorialCongratulationLocalizedField;

        bool xBonus;

        void Awake() {
            tutorialRewardButton.onClick.AddListener(delegate {
                DeactivateAllButtons();
                ShowSmile(tutorialCongratulationLocalizedField.Value);
            });
        }

        public void Appear() {
            animator.SetTrigger("Appear");
        }

        public void CoolAppear() {
            animator.SetTrigger("CoolAppear");
        }

        public void Disappear() {
            animator.SetTrigger("Disappear");
        }

        public void ShowContent(string titleText, string descriptionText, List<SpecialOfferItem> items) {
            this.titleText.text = titleText;
            this.descriptionText.text = descriptionText;
            smile.SetActive(false);
            ClearItems();
            InstantiateItems(items);
        }

        public void ShowSmile(string titleText) {
            DeactivateAllButtons();
            ClearItems();
            worthIt.SetLabel(0);
            this.titleText.text = titleText;
            descriptionText.text = string.Empty;
            smile.SetActive(true);
        }

        public void SetPriceButton(int discount, double regularPrice, int labelPercentage, string currency) {
            DeactivateAllButtons();
            worthIt.SetLabel(labelPercentage);
            priceButton.gameObject.SetActive(true);
            priceButton.SetPrice(regularPrice, discount, currency);
        }

        public void SetCrystalButton(int discountPrice, int regularPrice, int labelPercentage, bool xCry) {
            DeactivateAllButtons();
            worthIt.SetLabel(labelPercentage);
            crystalButton.gameObject.SetActive(true);
            crystalButton.SetPrice(regularPrice, discountPrice, xCry);
        }

        public void SetUseDiscountButton() {
            DeactivateAllButtons();
            xBonus = true;
            worthIt.SetLabel(0);
            useDiscountButton.gameObject.SetActive(true);
        }

        public void HideDiscountButton() {
            useDiscountButton.gameObject.SetActive(false);
        }

        public void ShowDiscountButtonIfXBonus() {
            if (xBonus) {
                useDiscountButton.gameObject.SetActive(true);
            }
        }

        public void SetTakeRewardButton() {
            DeactivateAllButtons();
            worthIt.SetLabel(0);
            takeRewardButton.gameObject.SetActive(true);
        }

        public void SetTutorialRewardsButton() {
            DeactivateAllButtons();
            worthIt.SetLabel(0);
            tutorialRewardButton.gameObject.SetActive(true);
        }

        public void DeactivateAllButtons() {
            xBonus = false;
            priceButton.gameObject.SetActive(false);
            useDiscountButton.gameObject.SetActive(false);
            takeRewardButton.gameObject.SetActive(false);
            crystalButton.gameObject.SetActive(false);
            openButton.gameObject.SetActive(false);
            tutorialRewardButton.gameObject.SetActive(false);
            animator.SetTrigger("ButtonFlash");
        }

        public void SetOpenButton(long containerId, int quantity, Action onOpen) {
            DeactivateAllButtons();
            worthIt.SetLabel(0);
            openButton.containerId = containerId;
            openButton.quantity = quantity;
            openButton.onOpen = onOpen;
            openButton.gameObject.SetActive(true);
        }
    }
}