using System;
using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Library.ClientResources.API {
    [Serializable]
    public class AssetReferenceListHelper {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public static void Load(List<AssetReference> refereneces) => Load(refereneces, 0);

        public static void Load(List<AssetReference> refereneces, int priority) {
            Flow flow = EngineService.NewFlow();

            flow.StartWith(delegate(Engine engine) {
                Entity entity = engine.CreateEntity("Loader");
                entity.AddComponent(new AssetReferenceListComponent(refereneces));

                entity.AddComponent(new AssetRequestComponent {
                    Priority = priority
                });
            });

            EngineService.ExecuteFlow(flow);
        }
    }
}