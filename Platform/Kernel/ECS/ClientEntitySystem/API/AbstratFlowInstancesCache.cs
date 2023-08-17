using System;
using System.Collections.Generic;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientDataStructures.Impl.Cache;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public abstract class AbstratFlowInstancesCache : FlowListener {
        protected List<AbstractCache> caches = new();

        public AbstratFlowInstancesCache() => EngineService.AddFlowListener(this);

        [Inject] public static EngineService EngineService { get; set; }

        public virtual void OnFlowSuccess() { }

        public virtual void OnFlowFatalError() { }

        public virtual void OnFlowClean() => caches.ForEach(delegate(AbstractCache c) {
            c.FreeAll();
        });

        protected Cache<T> Register<T>() {
            CacheImpl<T> cacheImpl = new();
            caches.Add(cacheImpl);
            return cacheImpl;
        }

        protected Cache<T> Register<T>(Action<T> cleaner) {
            CacheImpl<T> cacheImpl = new(cleaner);
            caches.Add(cacheImpl);
            return cacheImpl;
        }
    }
}