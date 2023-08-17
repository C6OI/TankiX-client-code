using System;
using System.IO;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;

namespace Platform.Library.ClientResources.Impl {
    public class AssetBundleDatabaseLoadSystem : ECSSystem {
        [OnEventFire]
        public void LoadAssetBundleDatabaseFromBundle(LoadCompleteEvent e, AssetDatabaseNode node) {
            node.Entity.RemoveComponent<AssetBundleDatabaseLoadingComponent>();
            byte[] bytes = node.urlLoader.Loader.Bytes;
            string text = string.Empty;

            if (false) {
                using (GZipStream gZipStream = new(new MemoryStream(bytes), CompressionMode.Decompress)) {
                    int num = 10485760;
                    byte[] array = new byte[num];
                    int num2 = gZipStream.Read(array, 0, num);

                    if (num2 < bytes.Length || num2 == num) {
                        throw new Exception("Decompress failed. read=" + num2);
                    }

                    text = new UTF8Encoding().GetString(array, 0, num2);
                }
            }

            text = Encoding.UTF8.GetString(bytes);

            if (string.IsNullOrEmpty(text)) {
                throw new Exception("AssetBundleDatabase data is empty");
            }

            AssetBundleDatabase assetBundleDatabase = new();
            assetBundleDatabase.Deserialize(text);
            AssetBundleDatabaseComponent assetBundleDatabaseComponent = new();
            assetBundleDatabaseComponent.AssetBundleDatabase = assetBundleDatabase;
            node.Entity.AddComponent(assetBundleDatabaseComponent);
        }

        public class AssetDatabaseNode : Node {
            public AssetBundleDatabaseLoadingComponent assetBundleDatabaseLoading;
            public UrlLoaderComponent urlLoader;
        }
    }
}