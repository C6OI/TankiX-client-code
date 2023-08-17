using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Lobby.ClientNavigation.API {
    public abstract class AbstractBackButtonComponent<T> : MonoBehaviour, Component, ComponentLifecycle
        where T : Event, new() {
        Entity entity;

        public bool Disabled { get; set; }

        void Update() {
            if (entity != null &&
                !Disabled &&
                (EventSystem.current.currentSelectedGameObject == null ||
                 EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() == null) &&
                InputMapping.Cancel) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine flow) {
                    flow.ScheduleEvent<T>(entity);
                });
            }
        }

        void OnEnable() => Disabled = false;

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) => this.entity = entity;
    }
}