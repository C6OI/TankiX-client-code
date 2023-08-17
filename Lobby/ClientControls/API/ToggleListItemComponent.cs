using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Toggle))]
    public class ToggleListItemComponent : MonoBehaviour, Component, ComponentLifecycle {
        Entity entity;

        public Toggle Toggle => GetComponent<Toggle>();

        void Awake() => AttachToParentToggleGroup();

        public void AttachToEntity(Entity entity) => this.entity = entity;

        public void DetachFromEntity(Entity entity) { }

        public event Action<bool> onValueChanged = delegate { };

        public void AttachToParentToggleGroup() => Toggle.group = GetComponentInParent<ToggleGroup>();

        public void OnValueChangedListener() {
            if (Toggle.isOn) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
                    e.ScheduleEvent<ListItemSelectedEvent>(entity);
                });
            } else {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine e) {
                    e.ScheduleEvent<ListItemDeselectedEvent>(entity);
                });
            }

            onValueChanged(Toggle.isOn);
        }
    }
}