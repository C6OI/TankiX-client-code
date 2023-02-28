using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientCore.Impl {
    public class SplashHitData {
        const string TARGETS_ARRAY_DELIMETER = ", ";

        const string LOG_FORMAT = "Splash Hit Data: direct targets = [{0}] ; static hit = {1} ; splash center = {2} splash targets = [{3}]";

        Vector3 splashCenter;

        SplashHitData() => SplashCenterInitialized = false;

        public List<HitTarget> DirectTargets { get; private set; }

        public StaticHit StaticHit { get; private set; }

        public Entity WeaponHitEntity { get; private set; }

        public List<HitTarget> SplashTargets { get; private set; }

        public List<GameObject> ExclusionGameObjectForSplashRaycast { get; private set; }

        public Vector3 SplashCenter {
            get => splashCenter;
            set {
                SplashCenterInitialized = true;
                splashCenter = value;
            }
        }

        public HashSet<Entity> ExcludedEntityForSplashHit { get; set; }

        public bool SplashCenterInitialized { get; private set; }

        public static SplashHitData CreateSplashHitData(List<HitTarget> directTargets, StaticHit staticHit, Entity weaponHitEntity) {
            SplashHitData splashHitData = new();
            splashHitData.DirectTargets = directTargets;
            splashHitData.StaticHit = staticHit;
            splashHitData.WeaponHitEntity = weaponHitEntity;
            splashHitData.SplashTargets = new List<HitTarget>();
            splashHitData.ExclusionGameObjectForSplashRaycast = new List<GameObject>();
            splashHitData.splashCenter = Vector3.zero;
            splashHitData.ExcludedEntityForSplashHit = null;
            return splashHitData;
        }

        public override string ToString() => string.Format("Splash Hit Data: direct targets = [{0}] ; static hit = {1} ; splash center = {2} splash targets = [{3}]",
            ConvertTargetsCollectionToString(DirectTargets), StaticHit != null ? StaticHit.ToString() : string.Empty, splashCenter.ToString(),
            ConvertTargetsCollectionToString(SplashTargets));

        string ConvertTargetsCollectionToString(List<HitTarget> targets) {
            return string.Join(", ", targets.Select(i => i.ToString()).ToArray());
        }
    }
}