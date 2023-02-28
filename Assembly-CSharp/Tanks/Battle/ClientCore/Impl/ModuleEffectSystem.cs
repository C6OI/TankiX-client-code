using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Tanks.Battle.ClientCore.Impl {
    public class ModuleEffectSystem : ECSSystem {
        static readonly float EFFECT_UNITYOBJECT_DELETION_DELAY = 1.5f;

        [OnEventFire]
        public void Destroy(RemoveEffectEvent e, SingleNode<EffectInstanceComponent> effect) {
            effect.component.GameObject.transform.SetParent(null, true);
        }

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, SingleNode<EffectInstanceComponent> effect) {
            ScheduleEvent<PrepareDestroyModuleEffectEvent>(effect);

            if (effect.Entity.HasComponent<EffectInstanceRemovableComponent>()) {
                GameObject gameObject = effect.component.GameObject;
                gameObject.AddComponent<DelayedSelfDestroyBehaviour>().Delay = EFFECT_UNITYOBJECT_DELETION_DELAY;

                gameObject.GetComponentsInChildren<Renderer>(true).ForEach(delegate(Renderer r) {
                    r.enabled = false;
                });
            }
        }

        [OnEventFire]
        public void CleanPreloadingEffects(NodeRemoveEvent evt, SingleNode<PreloadingModuleEffectsComponent> mapEffect) {
            mapEffect.component.EntitiesForEffectsLoading.ForEach(delegate(Entity e) {
                DeleteEntity(e);
            });

            mapEffect.component.EntitiesForEffectsLoading.Clear();
        }

        [OnEventFire]
        public void PreloadModuleEffects(NodeAddedEvent e, SingleNode<PreloadingModuleEffectsComponent> mapEffect) {
            PreloadingModuleEffectData[] preloadingModuleEffects = mapEffect.component.PreloadingModuleEffects;
            mapEffect.component.PreloadingBuffer = new Dictionary<string, GameObject>();
            mapEffect.component.EntitiesForEffectsLoading = new List<Entity>();

            preloadingModuleEffects.ForEach(delegate(PreloadingModuleEffectData i) {
                Entity entity = CreateEntity(string.Format("Preloading {0}", i.key));
                mapEffect.component.EntitiesForEffectsLoading.Add(entity);
                entity.AddComponent(new AssetReferenceComponent(i.asset));
                entity.AddComponent<AssetRequestComponent>();
                entity.AddComponent(new PreloadingModuleEffectKeyComponent(i.key));
            });
        }

        [OnEventFire]
        public void WarmUpModuleEffects(NodeAddedEvent e, SingleNode<PreloadingModuleEffectsComponent> mapEffect, [Combine] PreloadedModuleEffectNode preloadedModuleEffect) {
            Transform preloadedModuleEffectsRoot = mapEffect.component.PreloadedModuleEffectsRoot;

            GameObject gameObject = Object.Instantiate((GameObject)preloadedModuleEffect.resourceData.Data, preloadedModuleEffectsRoot.position, preloadedModuleEffectsRoot.rotation,
                preloadedModuleEffectsRoot);

            gameObject.GetComponentsInChildren<Collider>(true).ForEach(delegate(Collider c) {
                if (!c.enabled) {
                    c.enabled = true;
                    c.enabled = false;
                }
            });

            gameObject.SetActive(false);
            mapEffect.component.PreloadingBuffer.Add(preloadedModuleEffect.preloadingModuleEffectKey.Key, gameObject);

            if (mapEffect.component.PreloadingBuffer.Count == mapEffect.component.PreloadingModuleEffects.Length) {
                mapEffect.component.EntitiesForEffectsLoading.ForEach(delegate(Entity entity) {
                    DeleteEntity(entity);
                });

                mapEffect.component.EntitiesForEffectsLoading.Clear();
                mapEffect.Entity.AddComponent(new PreloadedModuleEffectsComponent(mapEffect.component.PreloadingBuffer));
            }
        }

        public class PreloadingModuleEffectKeyComponent : Component {
            public PreloadingModuleEffectKeyComponent(string key) => Key = key;

            public string Key { get; set; }
        }

        public class PreloadedModuleEffectNode : Node {
            public PreloadingModuleEffectKeyComponent preloadingModuleEffectKey;

            public ResourceDataComponent resourceData;
        }
    }
}