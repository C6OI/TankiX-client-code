namespace Platform.Library.ClientProtocol.API {
    public interface IOptionalMap {
        void Add(bool optional);

        bool GetLast();

        bool Get();

        bool Has();

        void Flip();

        void Clear();

        IOptionalMap Duplicate();

        int GetSize();

        void Concat(IOptionalMap optionalMap);

        void Fill(byte[] map, int size);
    }
}