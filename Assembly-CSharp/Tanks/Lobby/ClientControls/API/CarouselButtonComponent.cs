using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientControls.API {
    public abstract class CarouselButtonComponent : BehaviourComponent {
        [SerializeField] EntityBehaviour entityBehaviour;

        public long CarouselEntity { get; private set; }

        public void Build(Entity btnEntity, long carouselEntity) {
            CarouselEntity = carouselEntity;
            entityBehaviour.BuildEntity(btnEntity);
        }

        public void DestroyButton() {
            entityBehaviour.DestroyEntity();
        }
    }
}