using Lobby.ClientSettings.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SoundListenerStateSystem : ECSSystem {
        [OnEventFire]
        public void InitSoundListenerESM(NodeAddedEvent evt, SoundListenerNode listener) {
            SoundListenerESMComponent soundListenerESMComponent = new();
            EntityStateMachine esm = soundListenerESMComponent.Esm;
            esm.AddState<SoundListenerStates.SoundListenerSpawnState>();
            esm.AddState<SoundListenerStates.SoundListenerBattleState>();
            esm.AddState<SoundListenerStates.SoundListenerLobbyState>();
            listener.Entity.AddComponent(soundListenerESMComponent);
        }

        [OnEventFire]
        public void SwitchSoundListenerToLobbyState(LobbyAmbientSoundPlayEvent evt, SoundListenerESMNode soundListener) =>
            ScheduleEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerLobbyState>>(soundListener);

        [OnEventFire]
        public void SwitchSoundListenerToEnterBattleState(MapAmbientSoundPlayEvent evt, SoundListenerESMNode soundListener) {
            soundListener.soundListenerEsm.Esm.ChangeState<SoundListenerStates.SoundListenerSpawnState>();

            NewEvent<SwitchSoundListenerStateEvent<SoundListenerStates.SoundListenerBattleState>>().Attach(soundListener)
                .ScheduleDelayed(soundListener.soundListener.DelayForBattleState);
        }

        [OnEventFire]
        public void
            SwitchSoundListenerToBattleState(SwitchSoundListenerStateEvent evt, SoundListenerESMNode soundListener) =>
            soundListener.soundListenerEsm.Esm.ChangeState(evt.StateType);

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;
        }

        public class SoundListenerESMNode : SoundListenerNode {
            public SoundListenerESMComponent soundListenerEsm;
        }
    }
}