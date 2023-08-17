using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Platform.Library.ClientResources.API {
    public interface LoadAssetFromBundleRequest {
        Object Asset { get; }

        bool IsDone { get; }

        AssetBundle Bundle { get; }

        string ObjectName { get; }

        Type ObjectType { get; }
    }
}