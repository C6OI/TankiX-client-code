using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientMapSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitAmbientMapSoundEffect(MapAmbientSoundPlayEvent evt, NonAmbientMapSoundListenerNode listener,
            [JoinAll] SingleNode<AmbientMapSoundEffectMarkerComponent> mapEffect) {
            AmbientSoundFilter ambientSoundFilter = Object.Instantiate(mapEffect.component.AmbientSoundFilter);
            Transform transform = ambientSoundFilter.transform;
            transform.parent = listener.soundListener.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            ambientSoundFilter.Play();
            listener.Entity.AddComponent(new AmbientMapSoundEffectComponent(ambientSoundFilter));
        }

        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, AmbientMapSoundListenerNode listener) {
            AmbientSoundFilter ambientMapSound = listener.ambientMapSoundEffect.AmbientMapSound;
            ambientMapSound.Stop();
            listener.Entity.RemoveComponent<AmbientMapSoundEffectComponent>();
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        [Not(typeof(AmbientMapSoundEffectComponent))]
        public class NonAmbientMapSoundListenerNode : SoundListenerNode { }

        public class AmbientMapSoundListenerNode : SoundListenerNode {
            public AmbientMapSoundEffectComponent ambientMapSoundEffect;
        }
    }
}