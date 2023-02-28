using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(2457453672954365966L)]
    public interface RicochetBulletTemplate : Template {
        BulletComponent bullet();

        [PersistentConfig]
        BulletConfigComponent bulletConfig();

        RicochetBulletComponent ricochetBullet();

        BulletTargetingComponent barrelTargeting();

        TargetCollectorComponent targetCollector();
    }
}