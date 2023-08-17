using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Statics.ClientConfigurator.API;
using Platform.System.Data.Statics.ClientConfigurator.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;
using Random = System.Random;

namespace Platform.Library.ClientResources.Impl {
    public class ConfigurationActivator : UnityAwareActivator<ManuallyCompleting> {
        WWWLoader wwwLoader;

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        protected override void Activate() => StartCoroutine(Load());

        IEnumerator Load() {
            string configsUrl = InitConfiguration.Config.ConfigsUrl;
            string url = AddProfileToUrl(configsUrl);
            LoggerProvider.GetLogger(this).Debug("Load configs:" + url);
            string urlWithRandom = url + "?rnd=" + new Random().NextDouble();
            wwwLoader = new WWWLoader(new WWW(urlWithRandom));

            while (!wwwLoader.IsDone) {
                yield return null;
            }

            if (!string.IsNullOrEmpty(wwwLoader.Error)) {
                string errorMessage = string.Format("Configuration loading was failed. URL: {0}, Error: {1}",
                    wwwLoader.URL,
                    wwwLoader.Error);

                if (wwwLoader.Progress > 0f && wwwLoader.Progress < 1f) {
                    HandleError<ServerDisconnectedEvent>(errorMessage);
                } else {
                    HandleError<NoServerConnectionEvent>(errorMessage);
                }

                yield break;
            }

            if (wwwLoader.Bytes == null || wwwLoader.Bytes.Length == 0) {
                HandleError<InvalidLoadedDataErrorEvent>("Empty configuration data. URL: " + wwwLoader.URL);
                yield break;
            }

            ConfigTreeNodeImpl configNode;

            try {
                using (MemoryStream memoryStream = new(wwwLoader.Bytes)) {
                    TarImporter configImporter = new();
                    configNode = configImporter.ImportAll<ConfigTreeNodeImpl>(memoryStream);
                }
            } catch (Exception ex) {
                Exception e = ex;

                HandleError<InvalidLoadedDataErrorEvent>(string.Format("Invalid configuration data. URL: {0}, Error: {1}",
                        wwwLoader.URL,
                        e.Message),
                    e);

                yield break;
            }

            ConfigTreeNodeImpl rootConfig = LocalConfiguration.rootConfigNode;
            rootConfig.Add(configNode);
            ((ConfigurationServiceImpl)ConfigurationService).SetRootConfigNode(rootConfig);
            DisposeWWWLoader();
            Complete();
        }

        void HandleError<T>(string errorMessage) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            HandleError<T>();
        }

        void HandleError<T>(string errorMessage, Exception e) where T : Event, new() {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            HandleError<T>();
        }

        void HandleError<T>() where T : Event, new() {
            DisposeWWWLoader();

            ClientUnityIntegrationUtils.ExecuteInFlow(delegate(Engine engine) {
                Entity entity = engine.CreateEntity("RemoteConfigLoading");
                engine.ScheduleEvent<T>(entity);
            });
        }

        void DisposeWWWLoader() {
            wwwLoader.Dispose();
            wwwLoader = null;
        }

        string AddProfileToUrl(string url) {
            List<string> list = new();
            ConfigurationProfileElement[] components = GetComponents<ConfigurationProfileElement>();

            foreach (ConfigurationProfileElement configurationProfileElement in components) {
                list.Add(configurationProfileElement.ProfileElement);
            }

            list.Sort();
            url = url + "/" + InitConfiguration.Config.ConfigVersion + "/";

            foreach (string item in list) {
                url = url + item + "/";
            }

            url += "config.tar";
            return url;
        }
    }
}