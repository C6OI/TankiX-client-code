using System.Collections.Generic;
using Assets.platform.library.ClientResources.Scripts.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Platform.Library.ClientResources.Impl {
    public class UrlLoadSystem : ECSSystem {
        [OnEventFire]
        public void CreateLoader(NodeAddedEvent e, SingleNode<UrlComponent> node) =>
            node.Entity.AddComponent(new UrlLoaderComponent(CreateWWWLoader(node.component)));

        WWWLoader CreateWWWLoader(UrlComponent urlComponent) {
            WWW www = !urlComponent.Caching || !DiskCaching.Enabled ? new WWW(urlComponent.Url)
                          : WWW.LoadFromCacheOrDownload(urlComponent.Url, urlComponent.Hash, urlComponent.CRC);

            return new WWWLoader(www);
        }

        [OnEventComplete]
        public void CheckWWWIsDone(UpdateEvent e, SingleNode<UrlLoaderComponent> loaderNode) {
            Loader loader = loaderNode.component.Loader;

            if (loader.IsDone) {
                if (!string.IsNullOrEmpty(loader.Error)) {
                    string errorMessage = string.Format("URL: {0}, Error: {1}", loader.URL, loader.Error);
                    HandleError(loaderNode, loader, errorMessage);
                } else {
                    ScheduleEvent<LoadCompleteEvent>(loaderNode);
                }
            }
        }

        [OnEventComplete]
        public void DisposeLoader(LoadCompleteEvent e, SingleNode<UrlLoaderComponent> node) => DisposeLoader(node);

        [OnEventComplete]
        public void DisposeLoader(NoServerConnectionEvent e, SingleNode<UrlLoaderComponent> node) => DisposeLoader(node);

        [OnEventComplete]
        public void DisposeLoader(ServerDisconnectedEvent e, SingleNode<UrlLoaderComponent> node) => DisposeLoader(node);

        [OnEventComplete]
        public void DisposeLoader(InvalidLoadedDataErrorEvent e, SingleNode<UrlLoaderComponent> node) => DisposeLoader(node);

        [OnEventFire]
        public void DisposeLoader(DisposeUrlLoadersEvent e, Node node,
            [JoinAll] ICollection<SingleNode<UrlLoaderComponent>> loaderList) {
            foreach (SingleNode<UrlLoaderComponent> loader in loaderList) {
                DisposeLoader(loader);
            }
        }

        void DisposeLoader(SingleNode<UrlLoaderComponent> node) {
            Loader loader = node.component.Loader;
            loader.Dispose();
            node.Entity.RemoveComponent<UrlLoaderComponent>();
        }

        void HandleError(SingleNode<UrlLoaderComponent> loaderNode, Loader loader, string errorMessage) {
            if (loader.Progress > 0f && loader.Progress < 1f) {
                SheduleErrorEvent<ServerDisconnectedEvent>(loaderNode.Entity, errorMessage);
            } else {
                SheduleErrorEvent<NoServerConnectionEvent>(loaderNode.Entity, errorMessage);
            }
        }

        void SheduleErrorEvent<T>(Entity entity, string errorMessage) where T : Event, new() {
            Log.Error(errorMessage);
            ScheduleEvent<T>(entity);
        }
    }
}