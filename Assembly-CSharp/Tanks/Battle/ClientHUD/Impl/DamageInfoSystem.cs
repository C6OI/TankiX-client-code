using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientProfile.API;
using TMPro;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class DamageInfoSystem : ECSSystem {
        [OnEventFire]
        public void ShowDamage(DamageInfoEvent e, SingleNode<TankVisualRootComponent> tank, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD,
            [JoinAll] SingleNode<GameTankSettingsComponent> gameSettings) {
            if (gameSettings.component.DamageInfoEnabled) {
                Vector3 hitPoint = e.HitPoint;

                if (hitPoint == Vector3.zero) {
                    hitPoint.y += 1f;
                }

                Transform transform = tank.component.transform;
                Vector3 position = transform.TransformPoint(hitPoint);
                GameObject gameObject = Object.Instantiate(worldSpaceHUD.component.DamageInfoPrefab, worldSpaceHUD.component.gameObject.transform);
                gameObject.transform.position = position;
                gameObject.transform.rotation = Camera.main.transform.rotation;
                TextMeshProUGUI componentInChildren = gameObject.GetComponentInChildren<TextMeshProUGUI>();
                componentInChildren.text = ((int)e.Damage).ToString();

                if (e.BackHit) {
                    componentInChildren.fontStyle = FontStyles.Bold;
                    gameObject.GetComponent<Animator>().SetTrigger("Critical");
                }

                if (e.HealHit) {
                    gameObject.GetComponent<Animator>().SetTrigger("Healing");
                }
            }
        }

        [OnEventFire]
        public void UpdateDamageTransform(UpdateEvent e, SingleNode<DamageInfoComponent> damageInfo, [JoinAll] SingleNode<HUDWorldSpaceCanvas> worldSpaceHUD) {
            DamageInfoComponent component = damageInfo.component;
            Transform transform = component.transform;
            transform.rotation = component.CachedCamera.transform.rotation;
            WorldSpaceHUDUtil.ScaleToRealSize(worldSpaceHUD.component.canvas.transform, transform, component.CachedCamera);
        }
    }
}