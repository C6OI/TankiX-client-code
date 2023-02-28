using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class HammerDecalHitSystem : ECSSystem {
        [OnEventFire]
        public void DrawHammerHitDecalsSelf(SelfHammerShotEvent evt, PelletThrowingGraphicsNode weapon, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode,
            [JoinAll] SingleNode<DecalSettingsComponent> settings) {
            DrawHammerHitDecals(evt.ShotDirection, weapon, decalManagerNode, settings.component);
        }

        [OnEventFire]
        public void DrawHammerHitDecalsRemote(RemoteHammerShotEvent evt, PelletThrowingGraphicsNode weapon, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode,
            [JoinAll] SingleNode<DecalSettingsComponent> settings) {
            weapon.hammerPelletCone.ShotSeed = evt.RandomSeed;
            DrawHammerHitDecals(evt.ShotDirection, weapon, decalManagerNode, settings.component);
        }

        void DrawHammerHitDecals(Vector3 shotDirection, PelletThrowingGraphicsNode weapon, SingleNode<DecalManagerComponent> decalManagerNode, DecalSettingsComponent settings) {
            if (!settings.EnableDecals || settings.MaxDecalsForHammer <= 0) {
                return;
            }

            DecalMeshBuilder decalMeshBuilder = decalManagerNode.component.DecalMeshBuilder;
            BulletHoleDecalManager bulletHoleDecalManager = decalManagerNode.component.BulletHoleDecalManager;
            MuzzlePointComponent muzzlePoint = weapon.muzzlePoint;
            HammerDecalProjectorComponent hammerDecalProjector = weapon.hammerDecalProjector;
            Vector3 barrelOriginWorld = new MuzzleVisualAccessor(muzzlePoint).GetBarrelOriginWorld();
            decalMeshBuilder.Clean();
            DecalProjection decalProjection = new();
            decalProjection.HalfSize = hammerDecalProjector.CombineHalfSize;
            decalProjection.Distantion = hammerDecalProjector.Distance;
            decalProjection.Ray = new Ray(barrelOriginWorld - shotDirection, shotDirection);
            DecalProjection decalProjection2 = decalProjection;

            if (!decalMeshBuilder.CompleteProjectionByRaycast(decalProjection2) || !decalMeshBuilder.CollectPolygons(decalProjection2)) {
                return;
            }

            Vector3 localDirection = muzzlePoint.Current.InverseTransformVector(shotDirection);
            Vector3[] randomDirections = PelletDirectionsCalculator.GetRandomDirections(weapon.hammerPelletCone, muzzlePoint.Current.rotation, localDirection);
            List<Mesh> list = new(randomDirections.Length);

            for (int i = 0; i < Math.Min(randomDirections.Length, settings.MaxDecalsForHammer); i++) {
                Vector3 direction = randomDirections[i];
                decalProjection = new DecalProjection();
                decalProjection.AtlasHTilesCount = hammerDecalProjector.AtlasHTilesCount;
                decalProjection.AtlasVTilesCount = hammerDecalProjector.AtlasVTilesCount;
                decalProjection.SurfaceAtlasPositions = hammerDecalProjector.SurfaceAtlasPositions;
                decalProjection.HalfSize = hammerDecalProjector.HalfSize;
                decalProjection.Up = hammerDecalProjector.Up;
                decalProjection.Distantion = hammerDecalProjector.Distance;
                decalProjection.Ray = new Ray(barrelOriginWorld - shotDirection, direction);
                DecalProjection decalProjection3 = decalProjection;

                if (decalMeshBuilder.CompleteProjectionByRaycast(decalProjection3)) {
                    decalMeshBuilder.BuilldDecalFromCollectedPolygons(decalProjection3);
                    Mesh mesh = null;

                    if (decalMeshBuilder.GetResultToMesh(ref mesh)) {
                        list.Add(mesh);
                    }
                }
            }

            if (list.Count != 0) {
                CombineInstance[] array = new CombineInstance[list.Count];

                for (int j = 0; j < list.Count; j++) {
                    array[j].mesh = list[j];
                }

                Mesh mesh2 = new();
                mesh2.CombineMeshes(array, true, false);
                mesh2.RecalculateBounds();
                bulletHoleDecalManager.AddDecal(mesh2, hammerDecalProjector.Material, hammerDecalProjector.Color, hammerDecalProjector.LifeTime);
            }
        }

        public class PelletThrowingGraphicsNode : Node {
            public HammerDecalProjectorComponent hammerDecalProjector;
            public HammerPelletConeComponent hammerPelletCone;

            public MuzzlePointComponent muzzlePoint;

            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}