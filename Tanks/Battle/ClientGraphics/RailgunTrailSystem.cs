using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
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
            RaycastHit hitInfo;

            Vector3 hitPosition =
                !Physics.Raycast(worldPosition, shotDirection, out hitInfo, 1000f, LayerMasks.VISUAL_STATIC)
                    ? worldPosition + shotDirection * 1000f : hitInfo.point;

            DrawShotTrailEffect(worldPosition, hitPosition, railgunTrail.Prefab, railgunTrail.TipPrefab);
        }

        void DrawShotTrailEffect(Vector3 shotPosition, Vector3 hitPosition, GameObject prefab, GameObject tipPrefab) {
            GameObject gameObject = Object.Instantiate(tipPrefab);
            LineRendererEffectBehaviour component = gameObject.GetComponent<LineRendererEffectBehaviour>();
            Object.Destroy(gameObject, component.duration);
            GameObject gameObject2 = Object.Instantiate(tipPrefab);
            LineRendererEffectBehaviour component2 = gameObject2.GetComponent<LineRendererEffectBehaviour>();
            Object.Destroy(gameObject2, component2.duration);
            Vector3 vector = hitPosition - shotPosition;
            float magnitude = vector.magnitude;
            vector /= magnitude;

            if (magnitude > 5f) {
                GameObject gameObject3 = Object.Instantiate(prefab);
                LineRendererEffectBehaviour component3 = gameObject3.GetComponent<LineRendererEffectBehaviour>();
                Object.Destroy(gameObject3, component3.duration);
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
            public WeaponComponent weapon;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}