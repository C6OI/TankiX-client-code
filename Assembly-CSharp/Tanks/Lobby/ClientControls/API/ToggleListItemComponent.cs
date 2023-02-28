using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    [RequireComponent(typeof(Toggle))]
    public class ToggleListItemComponent : MonoBehaviour, Component, AttachToEntityListener {
        public Entity Entity { get; private set; }

        public Toggle Toggle => GetComponent<Toggle>();

        void Start() {
            AttachToParentToggleGroup();
        }

        void OnDisable() {
            if (Toggle.isOn) {
                Toggle.isOn = false;
            }
        }

        public void AttachedToEntity(Entity entity) {
            Entity = entity;
        }

        public event Action<bool> onValueChanged = delegate { };

        public void AttachToParentToggleGroup() {
            Toggle.group = GetComponentInParent<ToggleGroup>();
        }

        public void OnValueChangedListener() {
            if (Toggle.isOn) {
                if (Entity.HasComponent<ToggleListSelectedItemComponent>()) {
                    Entity.RemoveComponent<ToggleListSelectedItemComponent>();
                }

                Entity.AddComponent<ToggleListSelectedItemComponent>();
            } else if (Entity.HasComponent<ToggleListSelectedItemComponent>()) {
                Entity.RemoveComponent<ToggleListSelectedItemComponent>();
            }

            onValueChanged(Toggle.isOn);
        }
    }
}