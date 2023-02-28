using System.Collections;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;
using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
using UnityEngine;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class MVPModulesInfoComponent : MonoBehaviour {
        [SerializeField] MVPModuleContainer moduleContainerPrefab;

        [SerializeField] float moduleAnimationDelay = 0.2f;

        bool animated;

        readonly float moduleSize = 160f;

        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [ContextMenu("Animate cards")]
        public void AnimateCards() {
            StartCoroutine(AnimateCards(GetComponentsInChildren<MVPModuleContainer>()));
        }

        IEnumerator AnimateCards(MVPModuleContainer[] cards) {
            foreach (MVPModuleContainer module in cards) {
                if (module == null) {
                    yield break;
                }

                module.GetComponent<Animator>().SetBool("active", true);
                yield return new WaitForSeconds(moduleAnimationDelay);
            }

            yield return null;
        }

        public void Set(List<ModuleInfo> modules) {
            animated = false;

            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }

            List<ModuleInfo> modulesByBehaviourType = GetModulesByBehaviourType(modules, ModuleBehaviourType.ACTIVE);
            List<ModuleInfo> modulesByBehaviourType2 = GetModulesByBehaviourType(modules, ModuleBehaviourType.PASSIVE);

            modulesByBehaviourType.ForEach(delegate(ModuleInfo m) {
                addModule(m);
            });

            modulesByBehaviourType2.ForEach(delegate(ModuleInfo m) {
                addModule(m);
            });
        }

        void addModule(ModuleInfo m) {
            ModuleItem item = GarageItemsRegistry.GetItem<ModuleItem>(m.ModuleId);

            if (item.IsMutable()) {
                MVPModuleContainer mVPModuleContainer = Instantiate(moduleContainerPrefab, transform);
                mVPModuleContainer.SetupModuleCard(m, moduleSize);
            }
        }

        List<ModuleInfo> GetModulesByBehaviourType(List<ModuleInfo> modules, ModuleBehaviourType type) {
            List<ModuleInfo> res = new();

            modules.ForEach(delegate(ModuleInfo m) {
                ModuleItem item = GarageItemsRegistry.GetItem<ModuleItem>(m.ModuleId);

                if (item.ModuleBehaviourType == type) {
                    res.Add(m);
                }
            });

            res.Sort(new ModuleComparer());
            return res;
        }

        class ModuleComparer : IComparer<ModuleInfo> {
            public int Compare(ModuleInfo x, ModuleInfo y) {
                ModuleItem item = GarageItemsRegistry.GetItem<ModuleItem>(x.ModuleId);
                ModuleItem item2 = GarageItemsRegistry.GetItem<ModuleItem>(y.ModuleId);

                if (item.TankPartModuleType == item2.TankPartModuleType) {
                    return 0;
                }

                if (item.TankPartModuleType == TankPartModuleType.WEAPON) {
                    return -1;
                }

                return 1;
            }
        }
    }
}