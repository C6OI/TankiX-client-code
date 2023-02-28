using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class PreciseTimeBehaviour : MonoBehaviour {
        bool sendAfterFixedUpdateEvent;

        [Inject] public static EngineServiceInternal EngineServiceInternal { get; set; }

        void Update() {
            SendAfterFixedUpdateEventIfNeed();
            InternalPreciseTime.Update(Time.deltaTime);
        }

        void FixedUpdate() {
            SendAfterFixedUpdateEventIfNeed();
            InternalPreciseTime.FixedUpdate(Time.fixedDeltaTime);
            sendAfterFixedUpdateEvent = true;
        }

        void SendAfterFixedUpdateEventIfNeed() {
            if (sendAfterFixedUpdateEvent) {
                InternalPreciseTime.AfterFixedUpdate();
                Flow flow = EngineServiceInternal.GetFlow();
                Invoke();
                sendAfterFixedUpdateEvent = false;
            }
        }

        static void Invoke() {
            Flow current = Flow.Current;
            current.TryInvoke(AfterFixedUpdateEvent.Instance, typeof(AfterFixedUpdateEventFireHandler));
            current.TryInvoke(AfterFixedUpdateEvent.Instance, typeof(AfterFixedUpdateEventCompleteHandler));
        }
    }
}