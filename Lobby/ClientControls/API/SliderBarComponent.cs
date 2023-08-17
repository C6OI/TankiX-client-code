using Assets.lobby.modules.ClientControls.Scripts.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Slider))]
    [ExecuteInEditMode]
    public class SliderBarComponent : BehaviourComponent, ComponentLifecycle {
        Slider slider;

        public float Value {
            get => slider.value;
            set => slider.value = value;
        }

        public void OnDestroy() => slider.onValueChanged.RemoveAllListeners();

        public void AttachToEntity(Entity entity) {
            slider = GetComponent<Slider>();

            slider.onValueChanged.AddListener(delegate(float value) {
                ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine flow) {
                    if (slider.minValue.Equals(value)) {
                        flow.ScheduleEvent(new SliderBarSetToMinValueEvent(), entity);
                    } else {
                        flow.ScheduleEvent(new SliderBarValueChangedEvent(value), entity);
                    }
                });
            });
        }

        public void DetachFromEntity(Entity entity) { }
    }
}