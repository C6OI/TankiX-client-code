using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public abstract class AbstractDecalHitSystem : ECSSystem {
        protected void DrawHitDecal(HitEvent evt, DecalManagerComponent decalManager, DynamicDecalProjectorComponent decalProjector, MuzzlePointComponent muzzlePoint) {
            if (evt.StaticHit != null) {
                Vector3 barrelOriginWorld = new MuzzleVisualAccessor(muzzlePoint).GetBarrelOriginWorld();
                Vector3 normalized = (evt.StaticHit.Position - barrelOriginWorld).normalized;
                DrawHitDecal(decalManager, decalProjector, barrelOriginWorld, normalized);
            }
        }

        protected void DrawHitDecal(DecalManagerComponent managerComponent, DynamicDecalProjectorComponent decalProjector, Vector3 position, Vector3 direction) {
            DecalProjection decalProjection = new();
            decalProjection.AtlasHTilesCount = decalProjector.AtlasHTilesCount;
            decalProjection.AtlasVTilesCount = decalProjector.AtlasVTilesCount;
            decalProjection.SurfaceAtlasPositions = decalProjector.SurfaceAtlasPositions;
            decalProjection.HalfSize = decalProjector.HalfSize;
            decalProjection.Up = decalProjector.Up;
            decalProjection.Distantion = decalProjector.Distance;
            decalProjection.Ray = new Ray(position, direction);
            DecalProjection decalProjection2 = decalProjection;
            Mesh mesh = null;

            if (managerComponent.DecalMeshBuilder.Build(decalProjection2, ref mesh)) {
                managerComponent.BulletHoleDecalManager.AddDecal(mesh, decalProjector.Material, decalProjector.Color, decalProjector.LifeTime);
            }
        }
    }
}