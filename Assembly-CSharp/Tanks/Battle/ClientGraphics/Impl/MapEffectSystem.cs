using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitMapEffect(NodeAddedEvent e, MapNode map) {
            Entity entity = CreateEntity("MapEffect");
            entity.AddComponent<MapEffectComponent>();
            map.mapGroup.Attach(entity);
            GameObject sceneRoot = map.mapInstance.SceneRoot;
            MapEffectReferenceBehaviour component = sceneRoot.GetComponent<MapEffectReferenceBehaviour>();
            entity.AddComponent(new AssetReferenceComponent(component.MapEffect));
            entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void RemoveMapEffect(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map, [JoinByMap] SingleNode<MapEffectComponent> mapEffect) {
            DeleteEntity(mapEffect.Entity);
        }

        [OnEventFire]
        public void RemoveMapEffect(NodeRemoveEvent e, SingleNode<MapEffectInstanceComponent> mapEffect) {
            Object.DestroyObject(mapEffect.component.Instance);
        }

        [OnEventFire]
        public void PrepareMapEffect(NodeAddedEvent e, MapEffectLoadedNode mapEffect) {
            GameObject original = (GameObject)mapEffect.resourceData.Data;
            GameObject gameObject = Object.Instantiate(original);
            CommonMapEffectBehaviour component = gameObject.GetComponent<CommonMapEffectBehaviour>();
            GameObject gameObject2 = Object.Instantiate(component.CommonMapEffectPrefab);
            gameObject2.transform.SetParent(gameObject.transform);
            gameObject2.transform.localPosition = Vector3.zero;
            gameObject2.transform.localRotation = Quaternion.identity;
            gameObject.GetComponent<EntityBehaviour>().BuildEntity(mapEffect.Entity);
            mapEffect.Entity.AddComponent(new MapEffectInstanceComponent(gameObject));
        }

        [OnEventFire]
        public void PrepareMapEffect(NodeAddedEvent e, ReadyMapEffectNode mapEffect) {
            mapEffect.Entity.AddComponent<MapEffectAssembledComponent>();
        }

        public class MapNode : Node {
            public MapGroupComponent mapGroup;
            public MapInstanceComponent mapInstance;
        }

        public class MapEffectLoadedNode : Node {
            public MapEffectComponent mapEffect;

            public MapGroupComponent mapGroup;

            public ResourceDataComponent resourceData;
        }

        public class ReadyMapEffectNode : MapEffectLoadedNode {
            public MapEffectInstanceComponent mapEffectInstance;

            public PreloadedModuleEffectsComponent preloadedModuleEffects;
            public PreloadingModuleEffectsComponent preloadingModuleEffects;
        }
    }
}