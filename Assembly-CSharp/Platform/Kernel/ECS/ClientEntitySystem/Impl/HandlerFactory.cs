using System.Reflection;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface HandlerFactory {
        Handler CreateHandler(MethodInfo method, ECSSystem system);
    }
}