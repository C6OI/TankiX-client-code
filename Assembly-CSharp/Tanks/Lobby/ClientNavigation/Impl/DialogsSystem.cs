using System;
using System.Collections;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientNavigation.Impl {
    public class DialogsSystem : ECSSystem {
        [Inject] public new static EngineServiceInternal EngineService { get; set; }

        [OnEventFire]
        public void CloseDialog(NodeRemoveEvent e, SingleNode<ActiveScreenComponent> screen, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.CloseAll(string.Empty);
        }

        [OnEventFire]
        public void AddScreenLockClickListener(NodeAddedEvent e, SingleNode<ScreenLockComponent> screenLock) {
            screenLock.component.gameObject.AddComponent<DialogsOuterClickListener>().ClickAction = OnClick;
        }

        void OnClick(PointerEventData eventData) {
            EngineService.Engine.ScheduleEvent(new DialogsOuterClickEvent {
                EventData = eventData
            }, EngineService.EntityStub);
        }

        [OnEventFire]
        public void SendCancelEventToDialogs(DialogsOuterClickEvent e, Node node, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            IEnumerator enumerator = dialogs.component.transform.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Transform transform = (Transform)enumerator.Current;
                    ExecuteEvents.Execute(transform.gameObject, e.EventData, ExecuteEvents.cancelHandler);
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }
        }

        [OnEventComplete]
        public void MergeDialogs(NodeAddedEvent e, SingleNode<Dialogs60Component> newDialogs, [JoinAll] [Combine] SingleNode<Dialogs60Component> dialogs) {
            if (newDialogs.Entity != dialogs.Entity) {
                while (newDialogs.component.transform.childCount > 0) {
                    Transform child = newDialogs.component.transform.GetChild(0);
                    child.SetParent(dialogs.component.transform, false);
                }

                Object.Destroy(newDialogs.component.gameObject);
            }
        }
    }
}