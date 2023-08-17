using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class NameplateOpacitySystem : ECSSystem {
        [OnEventFire]
        public void ToConcealmentState(TimeUpdateEvent e, NameplateAppearanceNode nameplate) {
            if (!nameplate.nameplate.alwaysVisible &&
                nameplate.nameplatePosition.sqrDistance > nameplate.nameplateOpacity.sqrConcealmentDistance) {
                nameplate.Entity.RemoveComponent<NameplateAppearanceStateComponent>();
                nameplate.Entity.AddComponent<NameplateConcealmentStateComponent>();
            }
        }

        [OnEventFire]
        public void ToAppearanceState(TimeUpdateEvent e, NameplateConclealmentNode nameplate) {
            if (nameplate.nameplatePosition.sqrDistance < nameplate.nameplateOpacity.sqrConcealmentDistance) {
                nameplate.Entity.AddComponent<NameplateAppearanceStateComponent>();
                nameplate.Entity.RemoveComponent<NameplateConcealmentStateComponent>();
            }
        }

        [OnEventFire]
        public void HideNameplate(NodeAddedEvent e, NameplateNode nameplate) {
            nameplate.nameplate.Alpha = 0f;
            nameplate.Entity.AddComponent<NameplateAppearanceStateComponent>();
        }

        [OnEventFire]
        public void RevealNameplate(TimeUpdateEvent e, NameplateAppearanceNode nameplate) {
            NameplateComponent nameplate2 = nameplate.nameplate;
            IncreaseAlpha(nameplate2, e.DeltaTime);
        }

        [OnEventFire]
        public void HideNameplate(TimeUpdateEvent e, NameplateConclealmentNode nameplate) {
            NameplateComponent nameplate2 = nameplate.nameplate;

            if (nameplate2.Alpha > 0f) {
                DecreaseAlpha(nameplate2, e.DeltaTime);
            }
        }

        void IncreaseAlpha(NameplateComponent nameplateComponent, float dt) {
            float deltaAlpha = nameplateComponent.appearanceSpeed * dt;

            if (nameplateComponent.Alpha < 1f) {
                ChangeAlpha(nameplateComponent, deltaAlpha);
            }
        }

        void DecreaseAlpha(NameplateComponent nameplateComponent, float dt) {
            float deltaAlpha = (0f - nameplateComponent.disappearanceSpeed) * dt;
            ChangeAlpha(nameplateComponent, deltaAlpha);
        }

        [OnEventFire]
        public void StopOpacityChange(NodeAddedEvent e, NameplateDeletionNode nameplate) =>
            nameplate.Entity.RemoveComponent<NameplateOpacityComponent>();

        [OnEventFire]
        public void DeleteNameplate(TimeUpdateEvent e, NameplateDeletionNode nameplate) {
            NameplateComponent nameplate2 = nameplate.nameplate;
            DecreaseAlpha(nameplate2, e.DeltaTime);

            if (nameplate2.Alpha <= 0f) {
                Object.Destroy(nameplate2.gameObject);
            }
        }

        void ChangeAlpha(NameplateComponent nameplate, float deltaAlpha) =>
            nameplate.Alpha = Mathf.Clamp01(nameplate.Alpha + deltaAlpha);

        public class NameplateNode : Node {
            public NameplateComponent nameplate;

            public NameplateOpacityComponent nameplateOpacity;

            public NameplatePositionComponent nameplatePosition;
        }

        public class NameplateAppearanceNode : Node {
            public NameplateComponent nameplate;
            public NameplateAppearanceStateComponent nameplateAppearanceState;

            public NameplateOpacityComponent nameplateOpacity;

            public NameplatePositionComponent nameplatePosition;
        }

        public class NameplateConclealmentNode : Node {
            public NameplateComponent nameplate;
            public NameplateConcealmentStateComponent nameplateConcealmentState;

            public NameplateOpacityComponent nameplateOpacity;

            public NameplatePositionComponent nameplatePosition;
        }

        public class NameplateDeletionNode : Node {
            public NameplateComponent nameplate;
            public NameplateDeletionStateComponent nameplateDeletionState;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public TeamGroupComponent teamGroup;
        }
    }
}