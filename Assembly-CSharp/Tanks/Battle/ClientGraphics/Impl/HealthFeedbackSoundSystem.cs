using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class HealthFeedbackSoundSystem : ECSSystem {
        [OnEventFire]
        public void InitSoundListener(NodeAddedEvent e, SingleNode<HealthFeedbackMapEffectMaterialComponent> mapEffect, SingleNode<HealthFeedbackSoundListenerComponent> listener,
            SingleNode<GameTankSettingsComponent> settings) {
            if (!settings.component.HealthFeedbackEnabled) {
                listener.Entity.RemoveComponentIfPresent<HealthFeedbackSoundEffectEnabledComponent>();
            } else {
                listener.Entity.AddComponentIfAbsent<HealthFeedbackSoundEffectEnabledComponent>();
            }
        }

        [OnEventFire]
        public void DisableHealthFilterOnEnterLobby(LobbyAmbientSoundPlayEvent evt, SoundListenerNode listener) {
            listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
        }

        [OnEventFire]
        public void DisableHealthFilter(NodeAddedEvent evt, SelfTankNode tank, SoundListenerNode listener) {
            listener.healthFeedbackSoundListener.ResetHealthFeedbackData();
        }

        [OnEventFire]
        public void DisableHealthFilter(NodeRemoveEvent evt, SelfTankNode tank, [JoinAll] SoundListenerNode listener) {
            listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
        }

        [OnEventFire]
        public void SwitchToNormalMode(NodeAddedEvent evt, SelfDeadTankNode tank, SoundListenerNode listener) {
            listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
        }

        [OnEventFire]
        public void SwitchToLowHealthMode(HealthChangedEvent evt, SelfActiveTankNode tank, [JoinAll] SoundListenerNode listener) {
            float currentHealth = tank.health.CurrentHealth;
            float maxHealth = tank.health.MaxHealth;
            float num = currentHealth / maxHealth;

            if (num > listener.healthFeedbackSoundListener.MaxHealthPercentForSound) {
                listener.healthFeedbackSoundListener.SwitchToNormalHealthMode();
            } else {
                listener.healthFeedbackSoundListener.SwitchToLowHealthMode();
            }
        }

        public class SoundListenerNode : Node {
            public HealthFeedbackSoundEffectEnabledComponent healthFeedbackSoundEffectEnabled;
            public HealthFeedbackSoundListenerComponent healthFeedbackSoundListener;
        }

        public class SelfTankNode : Node {
            public HealthComponent health;
            public SelfTankComponent selfTank;
        }

        public class SelfDeadTankNode : SelfTankNode {
            public TankDeadStateComponent tankDeadState;
        }

        public class SelfActiveTankNode : SelfTankNode {
            public TankActiveStateComponent tankActiveState;
        }

        public class HealthFeedbackSoundEffectEnabledComponent : Component { }
    }
}