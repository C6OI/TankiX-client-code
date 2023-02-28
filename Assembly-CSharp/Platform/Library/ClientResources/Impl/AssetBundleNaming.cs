namespace Platform.Library.ClientResources.Impl {
    public static class AssetBundleNaming {
        public static readonly string DB_PATH = "/db/db.json";

        public static readonly string DB_DIR_PATH = "/db";

        public static readonly string DB_FILENAME = "db.json";

        public static readonly string EMBEDDED_BUNDLES_FILENAME = "embedded_bundles.txt";

        public static string GetAssetBundleUrl(string baseUrl, string assetBundleName) => string.Format("{0}{1}", baseUrl, assetBundleName).Replace('\\', '/');

        public static string AddCrcToBundleName(string assetBundleName, uint crc) => string.Format("{0}_{1:x8}.bundle", assetBundleName, crc);
    }
}