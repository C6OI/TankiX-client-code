namespace log4net.Core {
    public enum ErrorCode {
        GenericFailure = 0,
        WriteFailure = 1,
        FlushFailure = 2,
        CloseFailure = 3,
        FileOpenFailure = 4,
        MissingLayout = 5,
        AddressParseFailure = 6
    }
}