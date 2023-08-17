using Lobby.ClientControls.API;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientPayment.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class CrystalPackageContent : LocalizedControl, ListItemContent {
        [SerializeField] Text priceText;

        [SerializeField] Text crystalsText;

        [SerializeField] Text bonusText;

        [SerializeField] GameObject bonusContainer;

        [SerializeField] Text bonusHint;

        Entity entity;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public virtual string PriceText { get; set; }

        public virtual string BonusHint {
            set => bonusHint.text = value;
        }

        public void SetDataProvider(object dataProvider) {
            if (entity != dataProvider) {
                entity = (Entity)dataProvider;
                FillFromEntity(entity);
            }
        }

        public void Select() => EngineService.ExecuteInFlow(delegate(Engine e) {
            if (!entity.HasComponent<SelectedListItemComponent>()) {
                entity.AddComponent<SelectedListItemComponent>();
            }

            e.ScheduleEvent<ListItemSelectedEvent>(entity);
        });

        void SetPrice(double price, string currency) => priceText.text =
                                                            PriceText.Replace("{PRICE}",
                                                                    price.ToStringSeparatedByThousands())
                                                                .Replace("{CURRENCY}", currency);

        void SetBonus(long bonus) {
            bonusText.text = "+ " + bonus.ToStringSeparatedByThousands();
            bonusContainer.SetActive(bonus > 0);
        }

        void FillFromEntity(Entity entity) {
            GoodsPriceComponent component = entity.GetComponent<GoodsPriceComponent>();
            SetPrice(component.Price, component.Currency);
            XCrystalsPackComponent component2 = entity.GetComponent<XCrystalsPackComponent>();
            crystalsText.text = component2.Amount.ToStringSeparatedByThousands();
            SetBonus(component2.Bonus);
        }
    }
}