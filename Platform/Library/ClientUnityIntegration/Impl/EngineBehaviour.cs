using System.Collections;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Platform.Library.ClientUnityIntegration.Impl {
    public class EngineBehaviour : MonoBehaviour {
        static readonly ApplicationFocusEvent applicationFocusEvent = new();

        static readonly ViewportResizeEvent viewportResizeEvent = new();

        float lastDeltaTime;

        float lastTime;

        bool sendAfterFixedUpdateEvent;

        [Inject] public static UnityTime UnityTime { get; set; }

        [Inject] public static EngineServiceInternal EngineServiceInternal { get; set; }

        void Awake() {
            lastTime = Time.time;
            lastDeltaTime = Time.deltaTime;
            InternalPreciseTime.Init(UnityTime.time, UnityTime.fixedTime);
        }

        void Update() {
            StartCoroutine(OnFrameEnd());

            if (lastTime != Time.time || lastDeltaTime != Time.deltaTime) {
                lastTime = Time.time;
                lastDeltaTime = Time.deltaTime;
                SendAfterFixedUpdateEventIfNeed();
                InternalPreciseTime.Update(UnityTime.deltaTime);
                UpdateEvent instance = UpdateEvent.Instance;
                float deltaTime = UnityTime.deltaTime;
                TimeUpdateEvent.Instance.DeltaTime = deltaTime;
                deltaTime = deltaTime;
                EarlyUpdateEvent.Instance.DeltaTime = deltaTime;
                instance.DeltaTime = deltaTime;
                Flow flow = EngineServiceInternal.NewFlow();

                flow.StartWith(delegate {
                    flow.TryInvoke(TimeUpdateEvent.Instance, typeof(TimeUpdateFireHandler));
                    flow.TryInvoke(TimeUpdateEvent.Instance, typeof(TimeUpdateCompleteHandler));
                    flow.TryInvoke(EarlyUpdateEvent.Instance, typeof(EarlyUpdateFireHandler));
                    flow.TryInvoke(EarlyUpdateEvent.Instance, typeof(EarlyUpdateCompleteHandler));
                    flow.TryInvoke(UpdateEvent.Instance, typeof(UpdateEventFireHandler));
                    flow.TryInvoke(UpdateEvent.Instance, typeof(UpdateEventCompleteHandler));
                });

                EngineServiceInternal.ExecuteFlow(flow);
                EngineServiceInternal.DelayedEventManager.Update(PreciseTime.Time);
            }
        }

        void FixedUpdate() {
            SendAfterFixedUpdateEventIfNeed();
            InternalPreciseTime.FixedUpdate(UnityTime.fixedDeltaTime);
            FixedUpdateEvent.Instance.DeltaTime = Time.fixedDeltaTime;
            Flow flow = EngineServiceInternal.NewFlow();

            flow.StartWith(delegate {
                flow.TryInvoke(FixedUpdateEvent.Instance, typeof(FixedUpdateEventFireHandler));
                flow.TryInvoke(FixedUpdateEvent.Instance, typeof(FixedUpdateEventCompleteHandler));
            });

            EngineServiceInternal.ExecuteFlow(flow);
            sendAfterFixedUpdateEvent = true;
        }

        void OnApplicationFocus(bool isFocused) {
            applicationFocusEvent.IsFocused = isFocused;
            EngineEventSender.SendEventIntoEngine(EngineServiceInternal, applicationFocusEvent);
        }

        void OnApplicationQuit() => Debug.Log("Application ending after " + Time.time + " seconds");

        void OnRectTransformDimensionsChange() =>
            EngineEventSender.SendEventIntoEngine(EngineServiceInternal, viewportResizeEvent);

        IEnumerator OnFrameEnd() {
            yield return new WaitForEndOfFrame();

            EngineServiceInternal.GetFlow().Clean();
        }

        void SendAfterFixedUpdateEventIfNeed() {
            if (sendAfterFixedUpdateEvent) {
                InternalPreciseTime.AfterFixedUpdate();
                Flow flow = EngineServiceInternal.NewFlow();

                flow.StartWith(delegate {
                    flow.TryInvoke(AfterFixedUpdateEvent.Instance, typeof(AfterFixedUpdateEventFireHandler));
                    flow.TryInvoke(AfterFixedUpdateEvent.Instance, typeof(AfterFixedUpdateEventCompleteHandler));
                });

                EngineServiceInternal.ExecuteFlow(flow);
                sendAfterFixedUpdateEvent = false;
            }
        }
    }
}