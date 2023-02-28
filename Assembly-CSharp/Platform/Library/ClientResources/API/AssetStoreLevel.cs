using System.ComponentModel;

namespace Platform.Library.ClientResources.API {
    [DefaultValue(MANAGED)]
    public enum AssetStoreLevel {
        NONE = 0,
        MANAGED = 1,
        STATIC = 2
    }
}