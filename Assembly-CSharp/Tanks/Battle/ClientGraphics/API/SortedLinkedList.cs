using System;
using System.Collections.Generic;

namespace Tanks.Battle.ClientGraphics.API {
    public class SortedLinkedList<T> : LinkedList<T> where T : IComparable {
        public void AddOrdered(T value) {
            for (LinkedListNode<T> next = First; next != null; next = next.Next) {
                if (next.Value.CompareTo(value) < 0) {
                    AddBefore(next, new LinkedListNode<T>(value));
                    return;
                }
            }

            AddLast(new LinkedListNode<T>(value));
        }
    }
}