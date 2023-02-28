using Assets.lobby.modules.ClientControls.Scripts.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientControls.API {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Slider))]
    public class SliderBarComponent : BehaviourComponent, AttachToEntityListener {
        Slider slider;

        public float Value {
            get => slider.value;
            set => slider.value = value;
        }

        public void OnDestroy() {
            slider.onValueChanged.RemoveAllListeners();
        }

        public void AttachedToEntity(Entity entity) {
            slider = GetComponent<Slider>();

            slider.onValueChanged.AddListener(delegate(float value) {
                if (slider.minValue.Equals(value)) {
                    EngineService.Engine.ScheduleEvent(new SliderBarSetToMinValueEvent(), entity);
                } else {
                    EngineService.Engine.ScheduleEvent(new SliderBarValueChangedEvent(value), entity);
                }
            });
        }
    }
}