using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class GraffitiDynamicDecalManager : DynamicDecalManager {
        public GraffitiDynamicDecalManager(GameObject root, int maxDecalCount, float decalLifeTimeKoeff, LinkedList<DecalEntry> decalsQueue)
            : base(root, maxDecalCount, decalLifeTimeKoeff, decalsQueue) { }

        protected override DecalEntryType DecalType => DecalEntryType.Graffiti;

        protected override string DecalMeshObjectName => "Graffiti Mesh";

        public GameObject AddGraffiti(Mesh decalMesh, Material material, Color color, float lifeTime) {
            TrimQueue();
            SetMeshColorAndLifeTime(decalMesh, color, lifeTime * decalLifeTimeKoeff);
            float timeToDestroy = Time.time + lifeTime * decalLifeTimeKoeff + 2f;
            DecalEntry value = CreateDecalEntry(decalMesh, material, timeToDestroy);
            value.material.renderQueue = 2200 + decalsQueue.Count;
            LinkedListNode<DecalEntry> linkedListNode = decalsQueue.AddLast(value);
            decalsCount++;
            return linkedListNode.Value.gameObject;
        }

        public void RemoveDecal(GameObject decalObject) {
            LinkedListNode<DecalEntry> linkedListNode = decalsQueue.First;
            int num = 0;

            while (linkedListNode != null) {
                LinkedListNode<DecalEntry> next = linkedListNode.Next;

                if (!linkedListNode.Value.gameObject) {
                    decalsQueue.Remove(linkedListNode);
                    decalsCount--;
                } else if (linkedListNode.Value.gameObject == decalObject) {
                    Object.Destroy(linkedListNode.Value.gameObject);
                    decalsQueue.Remove(linkedListNode);
                    decalsCount--;
                }

                linkedListNode.Value.material.renderQueue = 2200 + num;
                num++;
                linkedListNode = next;
            }
        }
    }
}