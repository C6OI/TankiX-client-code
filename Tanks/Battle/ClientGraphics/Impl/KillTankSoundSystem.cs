using Lobby.ClientEntrance.API;
using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class KillTankSoundSystem : ECSSystem {
        [OnEventFire]
        public void PlayKillSound(KillEvent e, SelfBattleUserNode battleUser, [JoinByUser] TankNode tank,
            [JoinAll] BattleSoundListenerNode soundListener) {
            Transform transform = soundListener.soundListener.transform;
            KillTankSoundEffectBehaviour effectPrefab = tank.killTankSoundEffect.EffectPrefab;
            KillTankSoundEffectBehaviour killTankSoundEffectBehaviour = Object.Instantiate(effectPrefab);
            Transform transform2 = killTankSoundEffectBehaviour.transform;
            transform2.SetParent(transform);
            transform2.localPosition = Vector3.zero;
            transform2.localRotation = Quaternion.identity;
            transform2.localScale = Vector3.one;
            killTankSoundEffectBehaviour.Play();
        }

        public class BattleSoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerBattleStateComponent soundListenerBattleState;
        }

        public class SelfBattleUserNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserGroupComponent userGroup;
        }

        public class TankNode : Node {
            public KillTankSoundEffectComponent killTankSoundEffect;

            public UserGroupComponent userGroup;
        }
    }
}