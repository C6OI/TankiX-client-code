using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class BorrowEntityNotResolvedException : Exception {
        public BorrowEntityNotResolvedException(long[] ids)
            : base("Entity id=" + string.Join(", ", Array.ConvertAll(ids, s => s.ToString()))) { }
    }
}