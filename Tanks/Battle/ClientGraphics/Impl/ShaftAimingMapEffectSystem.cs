using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingMapEffectSystem : ECSSystem {
        const string HIDING_CENTER = "_HidingCenter";

        const string MIN_HIDING_RADIUS = "_MinHidingRadius";

        const string MAX_HIDING_RADIUS = "_MaxHidingRadius";

        const string HIDING_SPEED = "_HidingSpeed";

        const string HIDING_START_TIME = "_HidingStartTime";

        const string ENABLE_HIDING_KEYWORD = "ENABLE_HIDING";

        const int SHADER_TRANSPARENT_RENDER_QUEUE = 3500;

        const int SHADER_DEFAULT_RENDER_QUEUE = -1;

        [OnEventFire]
        public void StartHidingAnyNewBonus(NodeAddedEvent evt, BonusBoxNode bonus,
            [JoinByBattle] ShaftAimingMapWorkingNode weapon) {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            EnableHidingItem(bonus.material.Material, timeSinceLevelLoad, weapon);
        }

        [OnEventFire]
        public void StartHidingAnyNewParachute(NodeAddedEvent evt, BonusParachuteNode parachute,
            [JoinByBattle] ShaftAimingMapWorkingNode weapon) {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            EnableHidingItem(parachute.parachuteMaterial.Material, timeSinceLevelLoad, weapon);
        }

        [OnEventFire]
        public void StartHidingAnyNewFlag(NodeAddedEvent evt, FlagNode flag,
            [JoinByBattle] ShaftAimingMapWorkingNode weapon) {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;
            EnableHidingItem(flag.flagInstance.FlagInstance.GetComponent<Sprite3D>().material, timeSinceLevelLoad, weapon);
        }

        [OnEventFire]
        public void StartHiding(NodeAddedEvent evt, ShaftAimingMapWorkingNode weapon,
            [JoinByBattle] ICollection<BonusBoxNode> bonuses, [JoinAll] ICollection<BonusRegionNode> regions,
            ShaftAimingMapWorkingNode weaponToJoinParachutes, [JoinByBattle] ICollection<BonusParachuteNode> parachutes,
            ShaftAimingMapWorkingNode weaponToJoinFlags, [JoinByBattle] ICollection<FlagNode> flags,
            [JoinAll] ICollection<SingleNode<MapHidingGeometryComponent>> maps) {
            float timeSinceLevelLoad = Time.timeSinceLevelLoad;

            foreach (BonusBoxNode bonuse in bonuses) {
                EnableHidingItem(bonuse.material.Material, timeSinceLevelLoad, weapon);
            }

            foreach (BonusRegionNode region in regions) {
                EnableHidingItem(region.material.Material, timeSinceLevelLoad, weapon);
            }

            foreach (BonusParachuteNode parachute in parachutes) {
                EnableHidingItem(parachute.parachuteMaterial.Material, timeSinceLevelLoad, weapon);
            }

            foreach (FlagNode flag in flags) {
                EnableHidingItem(flag.flagInstance.FlagInstance.GetComponent<Sprite3D>().material,
                    timeSinceLevelLoad,
                    weapon);
            }

            ShaftAimingMapEffectComponent shaftAimingMapEffect = weapon.shaftAimingMapEffect;

            foreach (SingleNode<MapHidingGeometryComponent> map in maps) {
                Renderer[] hidingRenderers = map.component.hidingRenderers;

                foreach (Renderer renderer in hidingRenderers) {
                    renderer.receiveShadows = false;
                    Material[] materials = renderer.materials;

                    foreach (Material material in materials) {
                        if (material.shader == shaftAimingMapEffect.DefaultLeavesShader) {
                            EnableHidingItem(material,
                                weapon,
                                shaftAimingMapEffect.HidingLeavesShader,
                                3500,
                                timeSinceLevelLoad);
                        } else if (material.shader == shaftAimingMapEffect.DefaultBillboardTreesShader) {
                            EnableHidingItem(material,
                                weapon,
                                shaftAimingMapEffect.HidingBillboardTreesShader,
                                timeSinceLevelLoad);
                        }
                    }
                }
            }
        }

        void EnableHidingItem(Material material, ShaftAimingMapWorkingNode weapon, Shader targetShader,
            int targetRenderQueue, float startTime) {
            material.shader = targetShader;
            material.renderQueue = targetRenderQueue;
            EnableHidingItem(material, startTime, weapon);
        }

        void EnableHidingItem(Material material, ShaftAimingMapWorkingNode weapon, Shader targetShader, float startTime) {
            material.shader = targetShader;
            EnableHidingItem(material, startTime, weapon);
        }

        [OnEventFire]
        public void StopHiding(NodeAddedEvent evt, AimingMapIdleNode weapon,
            [JoinByBattle] ICollection<BonusBoxNode> bonuses, [JoinAll] ICollection<BonusRegionNode> regions,
            AimingMapIdleNode weaponToJoinParachutes, [JoinByBattle] ICollection<BonusParachuteNode> parachutes,
            AimingMapIdleNode weaponToJoinFlags, [JoinByBattle] ICollection<FlagNode> flags,
            [JoinAll] ICollection<SingleNode<MapHidingGeometryComponent>> maps) {
            foreach (BonusBoxNode bonuse in bonuses) {
                DisableMaterialHiding(bonuse.material.Material);
            }

            foreach (BonusRegionNode region in regions) {
                DisableMaterialHiding(region.material.Material);
            }

            foreach (BonusParachuteNode parachute in parachutes) {
                DisableMaterialHiding(parachute.parachuteMaterial.Material);
            }

            foreach (FlagNode flag in flags) {
                DisableMaterialHiding(flag.flagInstance.FlagInstance.GetComponent<Sprite3D>().material);
            }

            ShaftAimingMapEffectComponent shaftAimingMapEffect = weapon.shaftAimingMapEffect;

            foreach (SingleNode<MapHidingGeometryComponent> map in maps) {
                Renderer[] hidingRenderers = map.component.hidingRenderers;

                foreach (Renderer renderer in hidingRenderers) {
                    renderer.receiveShadows = true;
                    Material[] materials = renderer.materials;

                    foreach (Material material in materials) {
                        if (material.shader == shaftAimingMapEffect.HidingLeavesShader) {
                            DisableMaterialHiding(material, shaftAimingMapEffect.DefaultLeavesShader, -1);
                        } else if (material.shader == shaftAimingMapEffect.HidingBillboardTreesShader) {
                            DisableMaterialHiding(material, shaftAimingMapEffect.DefaultBillboardTreesShader);
                        }
                    }
                }
            }
        }

        void DisableMaterialHiding(Material material, Shader targetShader, int targetRenderQueue) {
            material.shader = targetShader;
            material.renderQueue = targetRenderQueue;
            DisableMaterialHiding(material);
        }

        void DisableMaterialHiding(Material material, Shader targetShader) {
            material.shader = targetShader;
            DisableMaterialHiding(material);
        }

        void EnableHidingItem(Material item, float startTime, ShaftAimingMapWorkingNode weapon) {
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(weapon.muzzlePoint).GetBarrelOriginWorld();
            ShaftAimingMapEffectComponent shaftAimingMapEffect = weapon.shaftAimingMapEffect;
            float initialEnergy = weapon.shaftAimingWorkingState.InitialEnergy;
            float exhaustedEnergy = weapon.shaftAimingWorkingState.ExhaustedEnergy;
            float t = exhaustedEnergy / initialEnergy;
            float shrubsHidingRadiusMin = shaftAimingMapEffect.ShrubsHidingRadiusMin;
            float num = shaftAimingMapEffect.ShrubsHidingRadiusMax * initialEnergy;
            shrubsHidingRadiusMin = Mathf.Lerp(shrubsHidingRadiusMin, num, t);
            Vector4 hidingCenter = new(barrelOriginWorld.x, barrelOriginWorld.y, barrelOriginWorld.z, 0f);
            float unloadAimingEnergyPerSec = weapon.shaftEnergy.UnloadAimingEnergyPerSec;
            EnableMaterialHiding(item, hidingCenter, unloadAimingEnergyPerSec, num, shrubsHidingRadiusMin, startTime);
        }

        void EnableMaterialHiding(Material material, Vector4 hidingCenter, float speed, float maxRadius, float minRadius,
            float startTime) {
            material.renderQueue = 3500;
            material.EnableKeyword("ENABLE_HIDING");
            material.SetVector("_HidingCenter", hidingCenter);
            material.SetFloat("_MaxHidingRadius", maxRadius);
            material.SetFloat("_MinHidingRadius", minRadius);
            material.SetFloat("_HidingSpeed", speed);
            material.SetFloat("_HidingStartTime", startTime);
        }

        void DisableMaterialHiding(Material material) {
            material.SetVector("_HidingCenter", Vector4.zero);
            material.SetFloat("_MaxHidingRadius", 0f);
            material.SetFloat("_MinHidingRadius", 0f);
            material.SetFloat("_HidingSpeed", 0f);
            material.SetFloat("_HidingStartTime", 0f);
            material.DisableKeyword("ENABLE_HIDING");
            material.renderQueue = -1;
        }

        public class ShaftAimingMapWorkingNode : Node {
            public BattleGroupComponent battleGroup;

            public MuzzlePointComponent muzzlePoint;
            public ShaftAimingMapEffectComponent shaftAimingMapEffect;

            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;

            public ShaftEnergyComponent shaftEnergy;

            public ShaftStateControllerComponent shaftStateController;
        }

        public class AimingMapIdleNode : Node {
            public BattleGroupComponent battleGroup;
            public ShaftAimingMapEffectComponent shaftAimingMapEffect;

            public ShaftIdleStateComponent shaftIdleState;

            public ShaftStateControllerComponent shaftStateController;
        }

        public class BonusBoxNode : Node {
            public BattleGroupComponent battleGroup;

            public BonusBoxInstanceComponent bonusBoxInstance;
            public MaterialComponent material;
        }

        public class BonusRegionNode : Node {
            public BonusRegionInstanceComponent bonusRegionInstance;
            public MaterialComponent material;
        }

        public class BonusParachuteNode : Node {
            public BattleGroupComponent battleGroup;

            public BonusParachuteInstanceComponent bonusParachuteInstance;
            public ParachuteMaterialComponent parachuteMaterial;
        }

        public class FlagNode : Node {
            public BattleGroupComponent battleGroup;
            public FlagInstanceComponent flagInstance;
        }
    }
}