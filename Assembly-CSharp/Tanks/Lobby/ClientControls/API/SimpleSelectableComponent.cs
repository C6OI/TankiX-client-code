using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class SimpleSelectableComponent : ECSBehaviour, Component, AttachToEntityListener {
        [SerializeField] GameObject selection;

        Entity entity;

        bool hasSelected;

        public void Awake() {
            selection.SetActive(false);
        }

        void OnDestroy() {
            destroyEvent(this);
        }

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        event Action<SimpleSelectableComponent, bool> selectEvent = delegate { };

        event Action<SimpleSelectableComponent> destroyEvent = delegate { };

        public void ChangeState() {
            Select(true);
        }

        public void Select(bool selected) {
            if (hasSelected != selected) {
                selectEvent(this, selected);

                if (selected) {
                    ScheduleEvent<ListItemSelectedEvent>(entity);
                } else {
                    ScheduleEvent<ListItemDeselectedEvent>(entity);
                }

                hasSelected = selected;
                selection.SetActive(selected);
            }
        }

        public void AddHandler(Action<SimpleSelectableComponent, bool> handler) {
            selectEvent += handler;
        }

        public void AddDestroyHandler(Action<SimpleSelectableComponent> handler) {
            destroyEvent += handler;
        }
    }
}