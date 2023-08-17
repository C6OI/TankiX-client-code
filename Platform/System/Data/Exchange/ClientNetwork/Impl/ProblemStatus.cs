namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public enum ProblemStatus {
        ClosedByClient = 0,
        ClosedByServer = 1,
        SocketMethodInvokeError = 2,
        ReceiveError = 3,
        DecodePacketError = 4,
        DecodeCommandError = 5,
        EncodeError = 6,
        SendError = 7
    }
}