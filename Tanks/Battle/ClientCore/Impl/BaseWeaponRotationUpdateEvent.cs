using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public abstract class BaseWeaponRotationUpdateEvent<T> : Event where T : BaseWeaponRotationUpdateEvent<T>, new() {
        public static T Instance { get; } = new();
    }
}