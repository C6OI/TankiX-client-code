using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public static class MineUtil {
        public static readonly float TANK_MINE_RAYCAST_DISTANCE = 10000f;

        [Inject] public static EngineService EngineService { get; set; }

        public static void ExecuteSplashExplosion(Entity mine, Entity tank, Vector3 splashCenter) {
            List<HitTarget> directTargets = new();
            StaticHit staticHit = new();
            staticHit.Normal = Vector3.up;
            staticHit.Position = splashCenter;
            StaticHit staticHit2 = staticHit;
            SplashHitData splashHitData = SplashHitData.CreateSplashHitData(directTargets, staticHit2, mine);
            splashHitData.ExcludedEntityForSplashHit = new HashSet<Entity> { tank };
            EngineService.Engine.ScheduleEvent<SendTankMovementEvent>(tank);
            EngineService.Engine.ScheduleEvent(new CollectSplashTargetsEvent(splashHitData), mine);
        }
    }
}