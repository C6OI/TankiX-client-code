using System.ComponentModel;

namespace Platform.Library.ClientResources.API {
    [DefaultValue(MANAGED)]
    public enum ResourceStoreLevel {
        NONE = 0,
        MANAGED = 1,
        STATIC = 2
    }
}