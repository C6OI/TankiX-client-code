using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using Tanks.Battle.ClientGraphics.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics {
    public class RailgunTrailSystem : ECSSystem {
        const int BIG_DISTANCE = 1000;

        const float TIPS_LENGTH = 2.5f;

        [OnEventFire]
        public void ShotTrail(BaseShotEvent evt, WeaponNode weapon) {
            RailgunTrailComponent railgunTrail = weapon.railgunTrail;
            Vector3 worldPosition = new MuzzleVisualAccessor(weapon.muzzlePoint).GetWorldPosition();
            Vector3 shotDirection = evt.ShotDirection;
            DirectionData directionData = weapon.targetCollector.Collect(worldPosition, shotDirection, 1000f, LayerMasks.VISUAL_STATIC);
            Vector3 hitPosition = !directionData.HasAnyHit() ? worldPosition + shotDirection * 1000f : directionData.FirstAnyHitPosition();
            DrawShotTrailEffect(worldPosition, hitPosition, railgunTrail.Prefab, railgunTrail.TipPrefab);
        }

        void DrawShotTrailEffect(Vector3 shotPosition, Vector3 hitPosition, GameObject prefab, GameObject tipPrefab) {
            float duration = tipPrefab.GetComponent<LineRendererEffectBehaviour>().duration;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = tipPrefab;
            getInstanceFromPoolEvent.AutoRecycleTime = duration;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            GameObject gameObject = getInstanceFromPoolEvent2.Instance.gameObject;
            CustomRenderQueue.SetQueue(gameObject, 3150);
            LineRendererEffectBehaviour component = gameObject.GetComponent<LineRendererEffectBehaviour>();
            gameObject.SetActive(true);
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            GameObject gameObject2 = getInstanceFromPoolEvent2.Instance.gameObject;
            LineRendererEffectBehaviour component2 = gameObject2.GetComponent<LineRendererEffectBehaviour>();
            gameObject2.SetActive(true);
            Vector3 vector = hitPosition - shotPosition;
            float magnitude = vector.magnitude;
            vector /= magnitude;

            if (magnitude > 5f) {
                float duration2 = prefab.GetComponent<LineRendererEffectBehaviour>().duration;
                getInstanceFromPoolEvent2.Prefab = prefab;
                getInstanceFromPoolEvent2.AutoRecycleTime = duration2;
                ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
                GameObject gameObject3 = getInstanceFromPoolEvent2.Instance.gameObject;
                LineRendererEffectBehaviour component3 = gameObject3.GetComponent<LineRendererEffectBehaviour>();
                gameObject3.SetActive(true);
                Vector3 vector2 = vector * 2.5f;
                Vector3 vector3 = shotPosition + vector2;
                Vector3 vector4 = hitPosition - vector2;
                component2.invertAlpha = true;
                component2.Init(component.LastScale, shotPosition, vector3);
                component3.Init(component2.LastScale, vector3, vector4);

                for (int i = 0; i < component2.LastScale.Length; i++) {
                    component2.LastScale[i] = component2.LastScale[i] + component3.LastScale[i];
                }

                component.Init(component2.LastScale, vector4, hitPosition);
            } else {
                Vector3 vector5 = Vector3.Lerp(shotPosition, hitPosition, 0.5f);
                component2.invertAlpha = true;
                component2.Init(component.LastScale, shotPosition, vector5);
                component.Init(component2.LastScale, vector5, hitPosition);
            }
        }

        public class WeaponNode : Node {
            public MuzzlePointComponent muzzlePoint;

            public RailgunTrailComponent railgunTrail;

            public TargetCollectorComponent targetCollector;
            public WeaponComponent weapon;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}