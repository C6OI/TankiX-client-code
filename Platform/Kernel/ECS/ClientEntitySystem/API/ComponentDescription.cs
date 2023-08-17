using System;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface ComponentDescription {
        string FieldName { get; }

        Type ComponentType { get; }

        T GetInfo<T>() where T : ComponentInfo;

        bool IsInfoPresent(Type type);
    }
}