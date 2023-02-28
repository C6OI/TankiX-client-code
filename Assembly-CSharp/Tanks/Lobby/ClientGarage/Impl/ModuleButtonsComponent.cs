using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleButtonsComponent : LocalizedControl, Component {
        [SerializeField] Button mountButton;

        [SerializeField] Button unmountButton;

        [SerializeField] Button assembleButton;

        [SerializeField] Button addResButton;

        [SerializeField] Text assembleText;

        [SerializeField] Text mountText;

        [SerializeField] Text unmountText;

        Entity selectedModule;

        Entity selectedSlot;

        [Inject] public new static EngineService EngineService { get; set; }

        public Button MountButton => mountButton;

        public Button UnmountButton => unmountButton;

        public Button AssembleButton => assembleButton;

        public Button AddResButton => addResButton;

        public Entity SelectedModule {
            set => selectedModule = value;
        }

        public Entity SelectedSlot {
            set => selectedSlot = value;
        }

        public string AssembleText {
            get => assembleText.text;
            set => assembleText.text = value;
        }

        public string MountText {
            get => mountText.text;
            set => mountText.text = value;
        }

        public string UnmountText {
            get => unmountText.text;
            set => unmountText.text = value;
        }

        protected override void Awake() {
            base.Awake();
            mountButton.onClick.AddListener(ScheduleForModuleAndSlotEvent<ModuleMountEvent>);
            unmountButton.onClick.AddListener(ScheduleForModuleAndSlotEvent<UnmountModuleFromSlotEvent>);
            assembleButton.onClick.AddListener(ScheduleForModuleEvent<RequestModuleAssembleEvent>);
            addResButton.onClick.AddListener(AddResources);
        }

        void AddResources() {
            ScheduleEvent(new ShowGarageItemEvent {
                Item = Flow.Current.EntityRegistry.GetEntity(-370755132L)
            }, selectedModule);
        }

        void ScheduleForModuleEvent<T>() where T : Event, new() {
            ScheduleEvent<T>(selectedModule);
        }

        void ScheduleForModuleAndSlotEvent<T>() where T : Event, new() {
            NewEvent<T>().AttachAll(selectedModule, selectedSlot).Schedule();
        }
    }
}