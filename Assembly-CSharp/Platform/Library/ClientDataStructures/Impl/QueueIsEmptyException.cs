using System;

namespace Platform.Library.ClientDataStructures.Impl {
    public class QueueIsEmptyException : InvalidOperationException {
        public QueueIsEmptyException()
            : base("Queue is empty") { }
    }
}