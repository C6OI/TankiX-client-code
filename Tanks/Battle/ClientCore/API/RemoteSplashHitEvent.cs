using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-2203330189936241204L)]
    [Shared]
    public class RemoteSplashHitEvent : RemoteHitEvent {
        [ProtocolOptional] public List<HitTarget> SplashTargets { get; set; }
    }
}