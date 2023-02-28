using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LeagueTitleUIComponent : BehaviourComponent {
        [SerializeField] new TextMeshProUGUI name;

        [SerializeField] ImageSkin icon;

        public Entity LeagueEntity { get; private set; }

        public string Name {
            set => name.text = value;
        }

        public string Icon {
            set => icon.SpriteUid = value;
        }

        void OnDestroy() {
            if (ClientUnityIntegrationUtils.HasEngine()) {
                RemoveFromEntity();
            }
        }

        public void Init(Entity entity) {
            if (entity.HasComponent<LeagueTitleUIComponent>()) {
                entity.RemoveComponent<LeagueTitleUIComponent>();
            }

            entity.AddComponent(this);
            LeagueEntity = entity;
        }

        void RemoveFromEntity() {
            if (LeagueEntity != null && LeagueEntity.HasComponent<LeagueTitleUIComponent>()) {
                LeagueEntity.RemoveComponent<LeagueTitleUIComponent>();
            }
        }
    }
}