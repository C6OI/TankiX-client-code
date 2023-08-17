using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class AmbientZoneSoundEffectSystem : ECSSystem {
        [OnEventFire]
        public void InitAmbientLevelSoundEffect(MapAmbientSoundPlayEvent evt, NonAmbientLevelSoundListenerNode listener,
            [JoinAll] SingleNode<AmbientZoneSoundEffectMarkerComponent> mapEffect) {
            AmbientZoneSoundEffect ambientZoneSoundEffect = mapEffect.component.AmbientZoneSoundEffect;
            AmbientZoneSoundEffect ambientZoneSoundEffect2 = Object.Instantiate(ambientZoneSoundEffect);
            Transform transform = ambientZoneSoundEffect2.transform;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            Transform transform2 = listener.soundListener.transform;
            ambientZoneSoundEffect2.Play(transform2);
            listener.Entity.AddComponent(new AmbientZoneSoundEffectComponent(ambientZoneSoundEffect2));
        }

        [OnEventFire]
        public void DisableZoneTransition(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> map,
            [JoinAll] AmbientLevelSoundListenerNode listener) =>
            listener.ambientZoneSoundEffect.AmbientZoneSoundEffect.DisableZoneTransition();

        [OnEventFire]
        public void FinalizeAmbientLevelSoundEffect(LobbyAmbientSoundPlayEvent evt, AmbientLevelSoundListenerNode listener) {
            AmbientZoneSoundEffect ambientZoneSoundEffect = listener.ambientZoneSoundEffect.AmbientZoneSoundEffect;
            ambientZoneSoundEffect.Stop();
            listener.Entity.RemoveComponent<AmbientZoneSoundEffectComponent>();
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        [Not(typeof(AmbientZoneSoundEffectComponent))]
        public class NonAmbientLevelSoundListenerNode : SoundListenerNode { }

        public class AmbientLevelSoundListenerNode : SoundListenerNode {
            public AmbientZoneSoundEffectComponent ambientZoneSoundEffect;
        }
    }
}