using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Lobby.ClientControls.API {
    public abstract class ListItemEntityContent : MonoBehaviour, ListItemContent {
        Entity entity;

        [Inject] public static EngineServiceInternal EngineService { get; set; }

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

        protected abstract void FillFromEntity(Entity entity);
    }
}