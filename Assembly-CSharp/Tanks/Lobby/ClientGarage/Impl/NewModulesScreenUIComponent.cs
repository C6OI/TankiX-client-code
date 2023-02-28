using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientProfile.Impl;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class NewModulesScreenUIComponent : BehaviourComponent {
        public static float OVER_SCREEN_Z_OFFSET = -0.054f;

        public static Dictionary<ModuleItem, SlotItemView> slotItems = new();

        public static ModuleScreenSelection selection;

        [SerializeField] LocalizedField hullHealth;

        [SerializeField] LocalizedField turretDamage;

        [SerializeField] LocalizedField level;

        [SerializeField] PresetsDropDownList presetsDropDownList;

        public XCrystalsIndicatorComponent xCrystalButton;

        public CrystalsIndicatorComponent crystalButton;

        public TankPartCollectionView turretCollectionView;

        public TankPartCollectionView hullCollectionView;

        public CollectionView collectionView;

        public Button backButton;

        public SelectedModuleView selectedModuleView;

        public GameObject background;

        public DragAndDropController dragAndDropController;

        public GameObject slotItemPrefab;

        public bool showAnimationFinished;

        public List<List<int>> level2PowerByTier;

        bool needUpdate;

        public Action OnShowAnimationFinishedAction;

        public TankPartModeController tankPartModeController;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        public TankPartItem Weapon { get; set; }

        public TankPartItem Tank { get; set; }

        public string HullHealth => hullHealth.Value;

        public string TurretDamage => turretDamage.Value;

        public string Level => level.Value;

        public ModuleItem SelectedModule { get; private set; }

        public void Awake() {
            ModuleScreenSelection moduleScreenSelection = new();
            moduleScreenSelection.onSelectionChange = OnSelectionChange;
            selection = moduleScreenSelection;
            tankPartModeController = new TankPartModeController(turretCollectionView, hullCollectionView, collectionView);
            tankPartModeController.onModeChange = OnTankPartModeChange;

            if (CollectionView.slots != null) {
                CollectionView.slots.Clear();
                CollectionView.slots = null;
            }

            if (slotItems != null) {
                slotItems.Clear();
            }

            collectionView.UpdateView();
            Dictionary<ModuleItem, CollectionSlotView>.ValueCollection values = CollectionView.slots.Values;

            foreach (CollectionSlotView item in values) {
                item.onDoubleClick = (Action<CollectionSlotView>)Delegate.Combine(item.onDoubleClick, new Action<CollectionSlotView>(OnCollectionSlotDoubleClick));
            }

            backButton.onClick.AddListener(Hide);
            SelectedModule = null;
        }

        void Update() {
            if (needUpdate) {
                UpdateView();
                needUpdate = false;
            }
        }

        void OnEnable() {
            PresetsDropDownList obj = presetsDropDownList;
            obj.onDropDownListItemSelected = (OnDropDownListItemSelected)Delegate.Combine(obj.onDropDownListItemSelected, new OnDropDownListItemSelected(OnPresetSelected));
        }

        void OnDisable() {
            PresetsDropDownList obj = presetsDropDownList;
            obj.onDropDownListItemSelected = (OnDropDownListItemSelected)Delegate.Remove(obj.onDropDownListItemSelected, new OnDropDownListItemSelected(OnPresetSelected));
        }

        void OnTankPartModeChange() {
            selection.Clear();
        }

        public void Show(TankPartModuleType tankPartModuleType) {
            MainScreenComponent.Instance.OverrideOnBack(Hide);
            showAnimationFinished = false;
            gameObject.SetActive(true);
            tankPartModeController.SetMode(tankPartModuleType);
        }

        public void Hide() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetBool("hide", true);
        }

        public void OnHideAnimationFinished() {
            gameObject.SetActive(false);
        }

        public void OnShowAnimationFinished() {
            showAnimationFinished = true;

            if (OnShowAnimationFinishedAction != null) {
                OnShowAnimationFinishedAction();
            }
        }

        void OnSelectionChange(ModuleItem item) {
            SelectedModule = item;
            selectedModuleView.UpdateView(item, level2PowerByTier, Tank, Weapon);
        }

        public void UpdateView() {
            hullCollectionView.UpdateView(Tank);
            turretCollectionView.UpdateView(Weapon);
            collectionView.UpdateView();
            CreateSlotItems();
            PlaceSlotItems();
            UpdateSlotItems();
            tankPartModeController.UpdateView();
            UpdateLineCollectionView();
            selection.Update(CollectionView.slots, slotItems);
            SelectedModule = selection.GetSelectedModuleItem();

            if (SelectedModule != null) {
                selectedModuleView.UpdateView(SelectedModule, level2PowerByTier, Tank, Weapon);
            }
        }

        public void UpdateLineCollectionView() {
            hullCollectionView.lineCollectionView.slot1.SetActive(hullCollectionView.activeSlot.HasItem());
            hullCollectionView.lineCollectionView.slot2.SetActive(hullCollectionView.activeSlot2.HasItem());
            hullCollectionView.lineCollectionView.slot3.SetActive(hullCollectionView.passiveSlot.HasItem());
            turretCollectionView.lineCollectionView.slot1.SetActive(turretCollectionView.activeSlot.HasItem());
            turretCollectionView.lineCollectionView.slot2.SetActive(turretCollectionView.activeSlot2.HasItem());
            turretCollectionView.lineCollectionView.slot3.SetActive(turretCollectionView.passiveSlot.HasItem());
        }

        public void UpdateViewInNextFrame() {
            needUpdate = true;
        }

        public void InitSlots(ICollection<NewModulesScreenSystem.SlotNode> slotNodes) {
            List<TankSlotView> list = new();
            list.Add(turretCollectionView.activeSlot);
            list.Add(turretCollectionView.activeSlot2);
            list.Add(turretCollectionView.passiveSlot);
            list.Add(hullCollectionView.activeSlot);
            list.Add(hullCollectionView.activeSlot2);
            list.Add(hullCollectionView.passiveSlot);
            List<TankSlotView> list2 = list;

            if (slotNodes.Count != list2.Count) {
                throw new ArgumentException("Incorrect module slot entity count " + slotNodes.Count);
            }

            foreach (NewModulesScreenSystem.SlotNode slotNode in slotNodes) {
                TankSlotView tankSlotView = list2[(int)slotNode.slotUserItemInfo.Slot];
                tankSlotView.SlotNode = slotNode;
            }
        }

        void PlaceSlotItems() {
            List<ModuleItem> list = slotItems.Keys.ToList();
            list.Sort();

            for (int i = 0; i < list.Count; i++) {
                ModuleItem moduleItem = list[i];
                SlotItemView slotItemView = slotItems[moduleItem];

                if (moduleItem.Slot != null) {
                    AddItemToTankCollection(moduleItem, slotItemView);
                } else {
                    collectionView.AddSlotItem(moduleItem, slotItemView);
                }

                slotItemView.gameObject.SetActive(true);
            }
        }

        void AddItemToTankCollection(ModuleItem moduleItem, SlotItemView slotItemView) {
            TankPartCollectionView tankPartCollection = GetTankPartCollection(moduleItem);

            if (moduleItem.ModuleBehaviourType == ModuleBehaviourType.PASSIVE) {
                tankPartCollection.passiveSlot.SetItem(slotItemView);
            } else if (tankPartCollection.activeSlot.SlotNode.Entity.Equals(moduleItem.Slot)) {
                tankPartCollection.activeSlot.SetItem(slotItemView);
            } else {
                tankPartCollection.activeSlot2.SetItem(slotItemView);
            }
        }

        TankPartCollectionView GetTankPartCollection(ModuleItem moduleItem) => moduleItem.TankPartModuleType != 0 ? turretCollectionView : hullCollectionView;

        void CreateSlotItems() {
            List<ModuleItem> list = GarageItemsRegistry.Modules.Where(mi => mi.IsMutable()).ToList();

            foreach (ModuleItem item in list) {
                if (item.UserItem != null && !slotItems.ContainsKey(item)) {
                    GameObject gameObject = Instantiate(slotItemPrefab);
                    gameObject.SetActive(false);
                    SlotItemView component = gameObject.GetComponent<SlotItemView>();
                    component.UpdateView(item);
                    component.onDoubleClick = OnSlotItemDoubleClick;
                    slotItems.Add(item, component);
                }
            }
        }

        void UpdateSlotItems() {
            foreach (KeyValuePair<ModuleItem, SlotItemView> slotItem in slotItems) {
                SlotItemView value = slotItem.Value;
                value.UpdateView(slotItem.Key);
            }
        }

        void OnSlotItemDoubleClick(SlotItemView view) {
            ModuleItem moduleItem = view.ModuleItem;
            DragAndDropItem component = view.GetComponent<DragAndDropItem>();
            TankPartCollectionView tankPartCollection = GetTankPartCollection(moduleItem);
            DragAndDropCell component2 = CollectionView.slots[moduleItem].GetComponent<DragAndDropCell>();

            if (moduleItem.IsMounted) {
                TankSlotView slotBySlotEntity = tankPartCollection.GetSlotBySlotEntity(moduleItem.Slot);

                if (slotBySlotEntity == null) {
                    throw new Exception("Modules screen: couln't find tank slot for moduleItem slot entity " + moduleItem.Slot.Id);
                }

                dragAndDropController.OnDrop(slotBySlotEntity.GetComponent<DragAndDropCell>(), component2, component);
            } else {
                TankSlotView slotForDrop = tankPartCollection.GetSlotForDrop(moduleItem.ModuleBehaviourType);

                if (slotForDrop != null) {
                    dragAndDropController.OnDrop(component2, slotForDrop.GetComponent<DragAndDropCell>(), component);
                }
            }
        }

        void OnCollectionSlotDoubleClick(CollectionSlotView collectionSlotView) {
            ModuleItem moduleItem = collectionSlotView.ModuleItem;

            if (moduleItem.Slot != null) {
                TankPartCollectionView tankPartCollection = GetTankPartCollection(moduleItem);
                TankSlotView slotBySlotEntity = tankPartCollection.GetSlotBySlotEntity(moduleItem.Slot);

                if (slotBySlotEntity == null) {
                    throw new Exception("Modules screen: couldn't find tank slot");
                }

                DragAndDropItem component = slotBySlotEntity.GetItem().GetComponent<DragAndDropItem>();
                dragAndDropController.OnDrop(slotBySlotEntity.GetComponent<DragAndDropCell>(), collectionSlotView.GetComponent<DragAndDropCell>(), component);
            }
        }

        public void InitPresetsDropDown(List<PresetItem> items) {
            presetsDropDownList.UpdateList(items);
        }

        public void OnPresetSelected(ListItem item) {
            PresetItem presetItem = (PresetItem)item.Data;
            Mount(presetItem.presetEntity);
        }

        public void OnVisualItemSelected(ListItem item) {
            VisualItem visualItem = (VisualItem)item.Data;
            Mount(visualItem.UserItem);
        }

        void Mount(Entity entity) {
            EngineService.Engine.ScheduleEvent<MountItemEvent>(entity);
        }

        public void InitMoney(NewModulesScreenSystem.SelfUserMoneyNode money) {
            selectedModuleView.InitMoney(money);
            crystalButton.SetValueWithoutAnimation(money.userMoney.Money);
            xCrystalButton.SetValueWithoutAnimation(money.userXCrystals.Money);
        }
    }
}