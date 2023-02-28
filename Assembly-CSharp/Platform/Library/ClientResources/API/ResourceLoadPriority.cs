using System.ComponentModel;

namespace Platform.Library.ClientResources.API {
    [DefaultValue(USUAL)]
    public enum ResourceLoadPriority {
        LOW = -100,
        USUAL = 0,
        HIGH = 100
    }
}