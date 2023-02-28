using System;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientMatchMaking.Impl {
    public class BuyEnergyDialogComponent : ConfirmDialogComponent {
        [SerializeField] TextMeshProUGUI message;

        [SerializeField] TextMeshProUGUI buyButtonText;

        [SerializeField] LocalizedField messageLoc;

        [SerializeField] LocalizedField buyButtonLoc;

        [SerializeField] LocalizedField chargesAmountSingularText;

        [SerializeField] LocalizedField chargesAmountPlural1Text;

        [SerializeField] LocalizedField chargesAmountPlural2Text;

        public long EnergyCount { get; private set; }

        public long Price { get; private set; }

        public void Show(long count, long price) {
            EnergyCount = count;
            message.text = string.Format(messageLoc, Pluralize((int)EnergyCount));
            Price = price;
            buyButtonText.text = string.Format(buyButtonLoc, price);
            Show();
        }

        string Pluralize(int amount) {
            switch (CasesUtil.GetCase(amount)) {
                case CaseType.DEFAULT:
                    return string.Format(chargesAmountPlural1Text.Value, amount);

                case CaseType.ONE:
                    return string.Format(chargesAmountSingularText.Value, amount);

                case CaseType.TWO:
                    return string.Format(chargesAmountPlural2Text.Value, amount);

                default:
                    throw new Exception("ivnalid case");
            }
        }

        public override void Hide() {
            base.Hide();
            MainScreenComponent.Instance.OverrideOnBack(delegate { });
        }
    }
}