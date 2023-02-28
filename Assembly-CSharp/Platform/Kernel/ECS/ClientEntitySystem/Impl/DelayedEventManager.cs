using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class DelayedEventManager {
        readonly EngineServiceInternal engineService;

        readonly LinkedList<DelayedEventTask> delayedTasks = new();

        readonly LinkedList<PeriodicEventTask> periodicTasks = new();

        public DelayedEventManager(EngineServiceInternal engine) => engineService = engine;

        public ScheduleManager SchedulePeriodicEvent(Event e, ICollection<Entity> entities, float timeInSec) {
            PeriodicEventTask periodicEventTask = new(e, engineService, entities, timeInSec);
            periodicTasks.AddLast(periodicEventTask);
            return periodicEventTask;
        }

        public ScheduleManager ScheduleDelayedEvent(Event e, ICollection<Entity> entities, float timeInSec) {
            DelayedEventTask delayedEventTask = new(e, entities, engineService, Time.time + timeInSec);
            delayedTasks.AddLast(delayedEventTask);
            return delayedEventTask;
        }

        public void Update(double time) {
            UpdatePeriodicTasks(time);
            UpdateDelayedTasks(time);
        }

        void UpdateDelayedTasks(double time) {
            LinkedListNode<DelayedEventTask> linkedListNode = delayedTasks.First;

            while (linkedListNode != null) {
                DelayedEventTask value = linkedListNode.Value;
                LinkedListNode<DelayedEventTask> next = linkedListNode.Next;

                if (value.IsCanceled()) {
                    delayedTasks.Remove(value);
                } else {
                    TryUpdate(time, value);
                }

                linkedListNode = next;
            }
        }

        void TryUpdate(double time, DelayedEventTask task) {
            try {
                if (task.Update(time)) {
                    delayedTasks.Remove(task);
                }
            } catch {
                delayedTasks.Remove(task);
                throw;
            }
        }

        void UpdatePeriodicTasks(double time) {
            LinkedListNode<PeriodicEventTask> linkedListNode = periodicTasks.First;

            while (linkedListNode != null) {
                PeriodicEventTask value = linkedListNode.Value;
                LinkedListNode<PeriodicEventTask> next = linkedListNode.Next;

                if (value.IsCanceled()) {
                    periodicTasks.Remove(linkedListNode);
                } else {
                    value.Update(time);
                }

                linkedListNode = next;
            }
        }
    }
}