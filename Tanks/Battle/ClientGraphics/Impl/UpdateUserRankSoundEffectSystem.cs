using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class UpdateUserRankSoundEffectSystem : ECSSystem {
        const float DESTROY_DELAY = 0.3f;

        [OnEventFire]
        public void PlaySelfUserRankSoundEffect(UpdateUserRankEffectEvent evt, SelfTankNode tank,
            [JoinAll] SingleNode<MapInstanceComponent> map) {
            AudioSource audioSource = Object.Instantiate(tank.updateUserRankSoundEffectAsset.SelfUserRankSource);
            Entity entity = CreateEntity("UpdateUserRankSoundEffect");
            entity.AddComponent(new SelfUserRankSoundEffectInstanceComponent(audioSource));
            audioSource.transform.SetParent(map.component.SceneRoot.transform, true);
            audioSource.Play();
            NewEvent<RemoveSelfUserRankSoundEffectEvent>().Attach(entity).ScheduleDelayed(audioSource.clip.length);
        }

        [OnEventFire]
        public void PlayRemoteUserRankSoundEffect(UpdateUserRankEffectEvent evt, RemoteTankNode tank,
            [JoinAll] SingleNode<MapInstanceComponent> map) {
            AudioSource audioSource = Object.Instantiate(tank.updateUserRankSoundEffectAsset.RemoteUserRankSource);
            audioSource.transform.position = tank.tankSoundRoot.SoundRootTransform.position;
            audioSource.transform.rotation = tank.tankSoundRoot.SoundRootTransform.rotation;
            audioSource.transform.SetParent(map.component.SceneRoot.transform, true);
            audioSource.Play();
            Object.DestroyObject(audioSource.gameObject, audioSource.clip.length + 0.3f);
        }

        [OnEventFire]
        public void RemoveSelfUserRankSoundEffect(RemoveSelfUserRankSoundEffectEvent e,
            SingleNode<SelfUserRankSoundEffectInstanceComponent> effect) {
            if (effect.component.Source == null) {
                DeleteEntity(effect.Entity);
                return;
            }

            Object.DestroyObject(effect.component.Source.gameObject, 0.3f);
            DeleteEntity(effect.Entity);
        }

        public class TankNode : Node {
            public TankSoundRootComponent tankSoundRoot;

            public UpdateUserRankSoundEffectAssetComponent updateUserRankSoundEffectAsset;
        }

        public class SelfTankNode : TankNode {
            public SelfTankComponent selfTank;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }
    }
}