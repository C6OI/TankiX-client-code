using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public abstract class ListItemEntityContent : MonoBehaviour, ListItemContent {
        Entity entity;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public void SetDataProvider(object dataProvider) {
            if (entity != dataProvider) {
                entity = (Entity)dataProvider;
                FillFromEntity(entity);
            }
        }

        public void Select() {
            if (!entity.HasComponent<SelectedListItemComponent>()) {
                entity.AddComponent<SelectedListItemComponent>();
            }

            EngineService.Engine.ScheduleEvent<ListItemSelectedEvent>(entity);
        }

        protected abstract void FillFromEntity(Entity entity);
    }
}