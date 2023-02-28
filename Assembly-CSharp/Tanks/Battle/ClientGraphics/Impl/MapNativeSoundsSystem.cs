using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class MapNativeSoundsSystem : ECSSystem {
        [OnEventFire]
        public void InitAmbientMapSoundEffect(MapAmbientSoundPlayEvent evt, NonMapNativeSoundsListenerNode listener, [JoinAll] MapNode map, [JoinByMap] MapEffectNode mapEffect) {
            Transform transform = map.mapInstance.SceneRoot.transform;
            MapNativeSoundsBehaviour asset = mapEffect.mapNativeSoundsAsset.Asset;
            MapNativeSoundsBehaviour mapNativeSoundsBehaviour = Object.Instantiate(asset);
            Object.DontDestroyOnLoad(mapNativeSoundsBehaviour.gameObject);
            Transform transform2 = mapNativeSoundsBehaviour.transform;
            transform2.position = transform.position;
            transform2.rotation = transform.rotation;
            transform2.localScale = Vector3.one;
            listener.Entity.AddComponent(new MapNativeSoundsComponent(mapNativeSoundsBehaviour));
            mapNativeSoundsBehaviour.Play();
        }

        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, MapNativeSoundsListenerNode listener) {
            listener.mapNativeSounds.MapNativeSounds.Stop();
            listener.Entity.RemoveComponent<MapNativeSoundsComponent>();
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        [Not(typeof(MapNativeSoundsComponent))]
        public class NonMapNativeSoundsListenerNode : SoundListenerNode { }

        public class MapNativeSoundsListenerNode : SoundListenerNode {
            public MapNativeSoundsComponent mapNativeSounds;
        }

        public class MapEffectNode : Node {
            public MapGroupComponent mapGroup;
            public MapNativeSoundsAssetComponent mapNativeSoundsAsset;
        }

        public class MapNode : Node {
            public MapGroupComponent mapGroup;
            public MapInstanceComponent mapInstance;
        }
    }
}