using System;
using System.Collections.Generic;

namespace YamlDotNet.Core {
    public class FakeList<T> {
        readonly IEnumerator<T> collection;

        int currentIndex = -1;

        public FakeList(IEnumerator<T> collection) => this.collection = collection;

        public FakeList(IEnumerable<T> collection)
            : this(collection.GetEnumerator()) { }

        public T this[int index] {
            get {
                if (index < currentIndex) {
                    collection.Reset();
                    currentIndex = -1;
                }

                while (currentIndex < index) {
                    if (!collection.MoveNext()) {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    currentIndex++;
                }

                return collection.Current;
            }
        }
    }
}