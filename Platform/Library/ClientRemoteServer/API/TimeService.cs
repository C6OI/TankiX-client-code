namespace Platform.Library.ClientRemoteServer.API {
    public interface TimeService {
        long DiffToServer { get; set; }

        void InitServerTime(long serverTime);

        void SetDiffToServerWithSmoothing(long newDiffToServer);
    }
}