using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1949198098578360952L)]
    [Shared]
    public class HealthComponent : SharedChangeableComponent {
        float currentHealth;

        float maxHealth;

        public float CurrentHealth {
            get => currentHealth;
            set {
                currentHealth = value;
                OnChange();
            }
        }

        public float MaxHealth {
            get => maxHealth;
            set {
                maxHealth = value;
                OnChange();
            }
        }
    }
}