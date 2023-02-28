using System.Collections.Generic;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TargetSectorUtils {
        public static void CutSectorsByOverlap(LinkedList<TargetSector> targetSectors, float border) {
            if (targetSectors.Count < 2) {
                return;
            }

            LinkedListNode<TargetSector> linkedListNode = targetSectors.First;
            LinkedListNode<TargetSector> linkedListNode2 = linkedListNode.Next;

            while (linkedListNode2 != null) {
                LinkedListNode<TargetSector> next = linkedListNode.Next;
                LinkedListNode<TargetSector> linkedListNode3 = linkedListNode2.Next;
                CutSectorsNodesByOverlap(linkedListNode, linkedListNode2, border);

                if (linkedListNode2.Value.Length() < float.Epsilon) {
                    targetSectors.Remove(linkedListNode2);
                } else if (linkedListNode.Value.Length() < float.Epsilon) {
                    targetSectors.Remove(linkedListNode);
                    linkedListNode3 = null;
                }

                linkedListNode2 = linkedListNode3;

                if (linkedListNode2 == null && next != null) {
                    linkedListNode = next;
                    linkedListNode2 = linkedListNode.Next;
                }
            }
        }

        static void CutSectorsNodesByOverlap(LinkedListNode<TargetSector> nodeA, LinkedListNode<TargetSector> nodeB, float border) {
            TargetSector value = nodeA.Value;
            TargetSector value2 = nodeB.Value;

            if (value.Distance > value2.Distance) {
                nodeA.Value = SubstractSectors(value, value2, border);
            } else {
                nodeB.Value = SubstractSectors(value2, value, border);
            }
        }

        static TargetSector SubstractSectors(TargetSector sectorA, TargetSector sectorB, float border) {
            float num = sectorB.Down - border;
            float num2 = sectorB.Up + border;
            bool flag = Between(sectorA.Down, num, num2);
            bool flag2 = Between(sectorA.Up, num, num2);

            if (flag && flag2) {
                float down = sectorA.Up = 0f;
                sectorA.Down = down;
            } else if (flag) {
                sectorA.Down = num2;
            } else if (flag2) {
                sectorA.Up = num;
            }

            return sectorA;
        }

        static bool Between(float position, float left, float right) => position >= left && position <= right;
    }
}