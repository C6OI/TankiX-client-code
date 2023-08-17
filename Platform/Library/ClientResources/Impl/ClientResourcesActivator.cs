using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.System.Data.Statics.ClientConfigurator.API;
using UnityEngine;

namespace Platform.Library.ClientResources.Impl {
    public class ClientResourcesActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineServiceInternal Engine { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        public void RegisterSystemsAndTemplates() => RegisterSystems();

        protected override void Activate() {
            string baseUrl = InitConfiguration.Config.ResourcesUrl + "/" + BuildTargetName.GetName();
            Entity entity = null;

            Flow flow = Engine.NewFlow().StartWith(delegate(Engine engine) {
                entity = engine.CreateEntity("AssetBundleDatabase");
            });

            Engine.ExecuteFlow(flow);

            flow = Engine.NewFlow().StartWith(delegate {
                baseUrl = baseUrl.Replace("{DataPath}", Application.dataPath);

                string text = !"LATEST".Equals(InitConfiguration.Config.BundleDbVersion)
                                  ? "-" + InitConfiguration.Config.BundleDbVersion : string.Empty;

                string assetBundleUrl = AssetBundleNaming.GetAssetBundleUrl(baseUrl, AssetBundleNaming.DB_PATH + text);

                UrlComponent component = new(assetBundleUrl, default, 0u) {
                    Caching = false
                };

                entity.AddComponent(component);
                entity.AddComponent<AssetBundleDatabaseLoadingComponent>();

                BaseUrlComponent component2 = new() {
                    Url = baseUrl + "/"
                };

                entity.AddComponent(component2);
            });

            Engine.ExecuteFlow(flow);
        }

        void RegisterSystems() {
            Engine.RegisterSystem(new AssetStorageSystem());
            Engine.RegisterSystem(new AssetAsyncLoaderSystem());
            Engine.RegisterSystem(new AssetBundleLoadSystem());
            Engine.RegisterSystem(new AssetBundleStorageSystem());
            Engine.RegisterSystem(new ResourceLoadStatSystem());
            Engine.RegisterSystem(new UrlLoadSystem());
            Engine.RegisterSystem(new AssetBundleDatabaseLoadSystem());
            Engine.RegisterSystem(new AssetBundlePreloadSystem());
            Engine.RegisterSystem(new ResourceLoadHelperSystem());
            Engine.RegisterSystem(new AssetBundleDiskCacheSystem());
        }
    }
}