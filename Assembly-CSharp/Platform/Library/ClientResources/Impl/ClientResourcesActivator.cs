using System;
using System.IO;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;
using UnityEngine;
using Activator = Platform.Kernel.OSGi.ClientCore.API.Activator;

namespace Platform.Library.ClientResources.Impl {
    public class ClientResourcesActivator : UnityAwareActivator<ManuallyCompleting>, ECSActivator, Activator {
        Entity dbEntity;
        WWWLoader dbLoader;

        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        void Update() {
            if (dbLoader != null && dbLoader.IsDone) {
                Engine engine = EngineService.Engine;
                enabled = false;

                if (!string.IsNullOrEmpty(dbLoader.Error)) {
                    string message = string.Format("AssetBundleDatabase loading was failed. URL: {0}, Error: {1}", dbLoader.URL, dbLoader.Error);
                    LoggerProvider.GetLogger(this).Error(message);
                    dbLoader.Dispose();
                    dbLoader = null;
                    Entity entity = engine.CreateEntity("RemoteConfigLoading");
                    engine.ScheduleEvent<InvalidGameDataErrorEvent>(entity);
                } else {
                    AssetBundleDatabase assetBundleDatabase = DeserializeDatabase(dbLoader.Bytes);
                    dbLoader.Dispose();
                    dbLoader = null;

                    dbEntity.AddComponent(new AssetBundleDatabaseComponent {
                        AssetBundleDatabase = assetBundleDatabase
                    });

                    Complete();
                }
            }
        }

        public void RegisterSystemsAndTemplates() {
            RegisterSystems();
        }

        protected override void Activate() {
            string text = InitConfiguration.Config.ResourcesUrl + "/" + BuildTargetName.GetName();
            Engine engine = EngineService.Engine;
            dbEntity = engine.CreateEntity("AssetBundleDatabase");
            bool flag = true;
            text = text.Replace("{DataPath}", Application.dataPath);
            string text2 = !"LATEST".Equals(InitConfiguration.Config.BundleDbVersion) ? "-" + InitConfiguration.Config.BundleDbVersion : string.Empty;

            string url = !flag ? AssetBundleNaming.GetAssetBundleUrl(text, AssetBundleNaming.DB_PATH + text2)
                             : (Application.platform != RuntimePlatform.WindowsPlayer ? "file://" : "file:///") + Application.dataPath + "/" + AssetBundleNaming.DB_FILENAME;

            dbLoader = new WWWLoader(new WWW(url)) {
                MaxRestartAttempts = 0
            };

            BaseUrlComponent baseUrlComponent = new();
            baseUrlComponent.Url = text + "/";
            dbEntity.AddComponent(baseUrlComponent);
        }

        void RegisterSystems() {
            EngineService.RegisterSystem(new AssetStorageSystem());
            EngineService.RegisterSystem(new AssetAsyncLoaderSystem());
            EngineService.RegisterSystem(new AssetBundleLoadSystem());
            EngineService.RegisterSystem(new AssetBundleStorageSystem());
            EngineService.RegisterSystem(new AssetLoadStatSystem());
            EngineService.RegisterSystem(new UrlLoadSystem());
            EngineService.RegisterSystem(new AssetBundlePreloadSystem());
            EngineService.RegisterSystem(new AssetLoadByEventSystem());
            EngineService.RegisterSystem(new AssetBundleDiskCacheSystem());
        }

        public AssetBundleDatabase DeserializeDatabase(byte[] bytes) {
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
            return assetBundleDatabase;
        }
    }
}