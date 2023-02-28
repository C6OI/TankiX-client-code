using System.Collections;
using System.Collections.Generic;

namespace Platform.Library.ClientDataStructures.API {
    public interface IPriorityQueue<T> : ICollection<T>, IEnumerable<T>, IEnumerable {
        void Enqueue(T item);

        T Dequeue();

        T Peek();
    }
}