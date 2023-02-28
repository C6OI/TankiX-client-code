using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapReverbZoneSystem : ECSSystem {
        [OnEventFire]
        public void InitReverbZones(NodeAddedEvent evt, NonMapReverbZoneListenerNode listener, MapNode map, [JoinByMap] [Context] MapEffectNode mapEffect) {
            Transform transform = map.mapInstance.SceneRoot.transform;
            GameObject mapReverbZonesRoot = mapEffect.mapReverbZoneAsset.MapReverbZonesRoot;
            GameObject gameObject = Object.Instantiate(mapReverbZonesRoot);
            Object.DontDestroyOnLoad(gameObject.gameObject);
            Transform transform2 = gameObject.transform;
            transform2.position = transform.position;
            transform2.rotation = transform.rotation;
            transform2.localScale = Vector3.one;
            listener.Entity.AddComponent(new MapReverbZoneComponent(gameObject));
        }

        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, MapReverbZoneListenerNode listener) {
            Object.DestroyObject(listener.mapReverbZone.ReverbZoneRoot);
            listener.Entity.RemoveComponent<MapReverbZoneComponent>();
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        [Not(typeof(MapReverbZoneComponent))]
        public class NonMapReverbZoneListenerNode : SoundListenerNode { }

        public class MapReverbZoneListenerNode : SoundListenerNode {
            public MapReverbZoneComponent mapReverbZone;
        }

        public class MapEffectNode : Node {
            public MapGroupComponent mapGroup;
            public MapReverbZoneAssetComponent mapReverbZoneAsset;
        }

        public class MapNode : Node {
            public MapGroupComponent mapGroup;
            public MapInstanceComponent mapInstance;
        }
    }
}