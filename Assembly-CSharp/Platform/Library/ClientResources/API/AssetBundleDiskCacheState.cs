namespace Platform.Library.ClientResources.API {
    public enum AssetBundleDiskCacheState {
        INIT = 0,
        START_LOAD_FROM_HTTP = 1,
        LOADING_FROM_HTTP = 2,
        LOADING_FROM_MEMORY = 3,
        START_LOAD_FROM_DISK = 4,
        START_WRITE_TO_DISK = 5,
        WRITE_TO_DISK = 6,
        START_LOAD_FROM_MEMORY = 7,
        CREATE_ASSETBUNDLE = 8,
        COMPLETE = 9
    }
}