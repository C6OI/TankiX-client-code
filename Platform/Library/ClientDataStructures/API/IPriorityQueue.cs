using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public interface IPriorityQueue<T> : IEnumerable, ICollection<T>, IEnumerable<T> {
        void Enqueue(T item);

        T Dequeue();

        T Peek();
    }
}