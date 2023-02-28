using Tanks.Lobby.ClientGarage.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ModuleCardsTierUI : MonoBehaviour {
        public void AddCard(ModuleCardItemUIComponent moduleCardItem) {
            moduleCardItem.transform.SetParent(transform, false);
            SortCards();
        }

        public void SortCards() {
            ModuleCardItemUIComponent[] componentsInChildren = GetComponentsInChildren<ModuleCardItemUIComponent>(false);
            ModuleCardItemUIComponent[] array = componentsInChildren;

            foreach (ModuleCardItemUIComponent moduleCardItemUIComponent in array) {
                if (moduleCardItemUIComponent.Type == ModuleBehaviourType.ACTIVE) {
                    moduleCardItemUIComponent.transform.SetAsFirstSibling();
                }
            }
        }

        public void Clear() {
            ModuleCardItemUIComponent[] componentsInChildren = GetComponentsInChildren<ModuleCardItemUIComponent>(true);

            for (int i = 0; i < componentsInChildren.Length; i++) {
                Destroy(componentsInChildren[i].gameObject);
            }
        }

        public ModuleCardItemUIComponent GetCard(long marketItemGroupId) {
            ModuleCardItemUIComponent[] componentsInChildren = GetComponentsInChildren<ModuleCardItemUIComponent>(true);
            ModuleCardItemUIComponent[] array = componentsInChildren;

            foreach (ModuleCardItemUIComponent moduleCardItemUIComponent in array) {
                if (moduleCardItemUIComponent.MarketGroupId == marketItemGroupId) {
                    return moduleCardItemUIComponent;
                }
            }

            return null;
        }
    }
}