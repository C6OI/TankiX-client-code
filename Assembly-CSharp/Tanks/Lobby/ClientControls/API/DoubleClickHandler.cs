using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Lobby.ClientControls.API {
    public class DoubleClickHandler : ECSBehaviour, Component, IPointerDownHandler, AttachToEntityListener, IEventSystemHandler {
        public FirstClickEvent FirstClick = new();

        readonly float delta = 0.2f;

        Entity entity;

        float time;

        void Update() {
            if (time != 0f && Time.realtimeSinceStartup - time > delta) {
                time = 0f;
                FirstClick.Invoke();
            }
        }

        public void AttachedToEntity(Entity entity) {
            this.entity = entity;
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (Time.realtimeSinceStartup - time < delta) {
                    ScheduleEvent<DoubleClickEvent>(entity);
                    time = 0f;
                } else {
                    time = Time.realtimeSinceStartup;
                }
            }
        }

        [Serializable]
        public class FirstClickEvent : UnityEvent { }
    }
}