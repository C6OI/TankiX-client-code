using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientGraphics.Impl {
    public abstract class AnimationTriggerComponent : MonoBehaviour, Component {
        Entity entity;

        public Entity Entity {
            get => entity;
            set {
                entity = value;
                enabled = true;
            }
        }

        void Awake() => enabled = false;

        void SendEvent<T>(Engine engine) where T : Event, new() => engine.NewEvent<T>().Attach(Entity).Schedule();

        void AddComponentIfNeeded<T>(Engine engine) where T : Component, new() {
            if (!Entity.HasComponent<T>()) {
                Entity.AddComponent<T>();
            }
        }

        void RemoveComponentIfNeeded<T>(Engine engine) where T : Component, new() {
            if (Entity.HasComponent<T>()) {
                Entity.RemoveComponent<T>();
            }
        }

        protected void AddComponent<T>() where T : Component, new() {
            if (enabled) {
                ClientUnityIntegrationUtils.ExecuteInFlow(AddComponentIfNeeded<T>);
            }
        }

        protected void RemoveComponent<T>() where T : Component, new() {
            if (enabled) {
                ClientUnityIntegrationUtils.ExecuteInFlow(RemoveComponentIfNeeded<T>);
            }
        }

        protected void ProvideEvent<T>() where T : Event, new() {
            if (enabled) {
                ClientUnityIntegrationUtils.ExecuteInFlow(SendEvent<T>);
            }
        }
    }
}