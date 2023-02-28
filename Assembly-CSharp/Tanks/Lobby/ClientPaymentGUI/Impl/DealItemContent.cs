using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class DealItemContent : LocalizedControl, Component, ListItemContent, ContentWithOrder {
        public TextMeshProUGUI title;

        public TextMeshProUGUI description;

        public ImageSkin banner;

        public TextMeshProUGUI price;

        public int order = 100;

        public bool canFillBigRow;

        public bool canFillSmallRow = true;

        protected Date EndDate = new(float.PositiveInfinity);

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        public Entity Entity { get; private set; }

        void OnEnable() {
            GetComponent<TextTimerComponent>().EndDate = EndDate;
        }

        public int Order => order;

        public bool CanFillBigRow => canFillBigRow;

        public bool CanFillSmallRow => canFillSmallRow;

        public virtual void SetParent(Transform parent) {
            transform.SetParent(parent, false);
        }

        public void SetDataProvider(object dataProvider) {
            if (Entity != dataProvider) {
                Entity = (Entity)dataProvider;
                FillFromEntity(Entity);
            }
        }

        public void Select() {
            if (!Entity.HasComponent<SelectedListItemComponent>()) {
                Entity.AddComponent<SelectedListItemComponent>();
            }

            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(Entity);
        }

        protected virtual void FillFromEntity(Entity entity) { }
    }
}