using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(3245747854367689034L)]
    public interface TwinsBulletTemplate : Template {
        BulletComponent bullet();

        [PersistentConfig]
        BulletConfigComponent bulletConfig();

        TwinsBulletComponent twinsBullet();

        BulletTargetingComponent barrelTargeting();

        TargetCollectorComponent targetCollector();
    }
}