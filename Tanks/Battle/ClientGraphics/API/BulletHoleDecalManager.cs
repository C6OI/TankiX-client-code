using System.Collections.Generic;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.API {
    public class BulletHoleDecalManager : DynamicDecalManager {
        public BulletHoleDecalManager(GameObject root, int maxDecalCount, float decalLifeTimeKoeff,
            LinkedList<DecalEntry> decalsQueue)
            : base(root, maxDecalCount, decalLifeTimeKoeff, decalsQueue) { }

        protected override DecalEntryType DecalType => DecalEntryType.BulletHole;

        protected override string DecalMeshObjectName => "Decal Mesh";

        public void AddDecal(Mesh decalMesh, Material material, Color color, float lifeTime) {
            TrimQueue();
            SetMeshColorAndLifeTime(decalMesh, color, lifeTime * decalLifeTimeKoeff);
            float timeToDestroy = Time.time + lifeTime * decalLifeTimeKoeff + 2f;
            decalsQueue.AddLast(CreateDecalEntry(decalMesh, material, timeToDestroy));
            decalsCount++;
            Optimize();
        }

        public void Optimize() {
            LinkedListNode<DecalEntry> linkedListNode = decalsQueue.First;
            int num = 0;

            while (linkedListNode != null) {
                LinkedListNode<DecalEntry> next = linkedListNode.Next;

                if (Time.time > linkedListNode.Value.timeToDestroy) {
                    Object.Destroy(linkedListNode.Value.gameObject);
                    decalsQueue.Remove(linkedListNode);
                    decalsCount--;
                }

                linkedListNode.Value.material.renderQueue = 2100 + num;
                num++;
                linkedListNode = next;
            }
        }
    }
}