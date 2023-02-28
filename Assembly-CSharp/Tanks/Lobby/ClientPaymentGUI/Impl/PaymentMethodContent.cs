using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentMethodContent : MonoBehaviour, ListItemContent {
        [SerializeField] ImageListSkin skin;

        [SerializeField] TextMeshProUGUI text;

        [SerializeField] GameObject saleItem;

        [SerializeField] GameObject saleItemLabelEmpty;

        [SerializeField] GameObject saleItemXtraLabelEmpty;

        [SerializeField] TextMeshProUGUI saleItemLabelText;

        Entity entity;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public void Select() {
            if (!entity.HasComponent<SelectedListItemComponent>()) {
                entity.AddComponent<SelectedListItemComponent>();
            }

            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(entity);
        }

        public void SetDataProvider(object dataProvider) {
            entity = (Entity)dataProvider;
            FillFromEntity(entity);
        }

        void SetMethodName(string name) {
            skin.SelectSprite(name);
        }

        void FillFromEntity(Entity entity) {
            if (entity.HasComponent<PaymentMethodComponent>()) {
                PaymentMethodComponent component = entity.GetComponent<PaymentMethodComponent>();
                SetMethodName(component.MethodName);
                text.text = component.ShownName;
                saleItem.SetActive(false);
                saleItemLabelEmpty.SetActive(false);
                saleItemXtraLabelEmpty.SetActive(false);
                saleItemLabelText.text = string.Empty;
            }
        }
    }
}