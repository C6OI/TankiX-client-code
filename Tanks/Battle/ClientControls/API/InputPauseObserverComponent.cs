using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientControls.API {
    [RequireComponent(typeof(InputField))]
    public class InputPauseObserverComponent : MonoBehaviour, Component, ComponentLifecycle {
        [SerializeField] float delayInSec;

        Entity entity;
        bool inputChanged;

        float lastChangeTime;

        [Inject] public static UnityTime UnityTime { get; set; }

        public void Update() {
            if (inputChanged && UnityTime.time - lastChangeTime > delayInSec) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine flow) {
                    flow.ScheduleEvent<InputPausedEvent>(entity);
                });

                inputChanged = false;
            }
        }

        public void AttachToEntity(Entity entity) {
            inputChanged = false;
            this.entity = entity;
            InputField component = GetComponent<InputField>();
            component.onValueChange.AddListener(OnInputValueChange);
        }

        public void DetachFromEntity(Entity entity) { }

        void OnInputValueChange(string arg0) {
            inputChanged = true;
            lastChangeTime = UnityTime.time;
        }
    }
}