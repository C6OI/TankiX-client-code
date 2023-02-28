using System.Collections.Generic;
using Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PresetsDropDownList : DropDownListComponent {
        public void UpdateList(List<PresetItem> items) {
            dataProvider.ClearItems();
            PresetItem presetItem = null;

            foreach (PresetItem item in items) {
                if (item.isSelected && AllowSelectPresetItem(item, presetItem)) {
                    presetItem = item;
                }
            }

            dataProvider.Init(items, presetItem);
            listTitle.text = presetItem.Name;
        }

        protected override void OnItemSelect(ListItem item) {
            base.OnItemSelect(item);
            listTitle.text = ((PresetItem)item.Data).Name;
        }

        bool AllowSelectPresetItem(PresetItem item, PresetItem selected) {
            if (selected == null) {
                return true;
            }

            long key = selected.presetEntity.GetComponent<UserGroupComponent>().Key;
            long key2 = item.presetEntity.GetComponent<UserGroupComponent>().Key;
            long id = SelfUserComponent.SelfUser.Id;

            if (key2 != id) {
                if (SelfUserComponent.SelfUser.HasComponent<UserUseItemsPrototypeComponent>()) {
                    return SelfUserComponent.SelfUser.GetComponent<UserUseItemsPrototypeComponent>().Preset.Id == item.presetEntity.Id;
                }

                return false;
            }

            return key == id;
        }
    }
}