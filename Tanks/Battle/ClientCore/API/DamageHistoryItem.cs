using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Tanks.Battle.ClientCore.API {
    public class DamageHistoryItem {
        public float Damage;

        public Entity DamagerUser;
        public Date TimeOfDamage;
    }
}