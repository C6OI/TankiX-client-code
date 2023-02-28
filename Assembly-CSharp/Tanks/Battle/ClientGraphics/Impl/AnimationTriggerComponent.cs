using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class AnimationTriggerComponent : ECSBehaviour, Component {
        Entity entity;

        public Entity Entity {
            get => entity;
            set {
                entity = value;
                enabled = true;
            }
        }

        void Awake() {
            enabled = false;
        }

        void SendEvent<T>() where T : Event, new() {
            NewEvent<T>().Attach(Entity).Schedule();
        }

        void AddComponentIfNeeded<T>() where T : Component, new() {
            if (!Entity.HasComponent<T>()) {
                Entity.AddComponent<T>();
            }
        }

        void RemoveComponentIfNeeded<T>() where T : Component, new() {
            if (Entity.HasComponent<T>()) {
                Entity.RemoveComponent<T>();
            }
        }

        protected void AddComponent<T>() where T : Component, new() {
            if (enabled) {
                AddComponentIfNeeded<T>();
            }
        }

        protected void RemoveComponent<T>() where T : Component, new() {
            if (enabled) {
                RemoveComponentIfNeeded<T>();
            }
        }

        protected void ProvideEvent<T>() where T : Event, new() {
            if (enabled) {
                SendEvent<T>();
            }
        }
    }
}