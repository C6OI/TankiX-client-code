namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public enum CommandCode : byte {
        SendEvent = 1,
        EntityShare = 2,
        EntityUnshare = 3,
        ComponentAdd = 4,
        ComponentRemove = 5,
        ComponentChange = 6,
        InitTime = 7,
        FlowBound = 8,
        Close = 9
    }
}