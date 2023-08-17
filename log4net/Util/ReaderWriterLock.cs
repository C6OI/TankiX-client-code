namespace log4net.Util {
    public sealed class ReaderWriterLock {
        readonly System.Threading.ReaderWriterLock m_lock;

        public ReaderWriterLock() => m_lock = new System.Threading.ReaderWriterLock();

        public void AcquireReaderLock() => m_lock.AcquireReaderLock(-1);

        public void ReleaseReaderLock() => m_lock.ReleaseReaderLock();

        public void AcquireWriterLock() => m_lock.AcquireWriterLock(-1);

        public void ReleaseWriterLock() => m_lock.ReleaseWriterLock();
    }
}