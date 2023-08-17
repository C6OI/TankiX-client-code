namespace Lobby.ClientPayment.API {
    public enum PaymentStatisticsAction : byte {
        OPEN_PAYMENT = 0,
        COUNTRY_SELECT = 1,
        ITEM_SELECT = 2,
        MODE_SELECT = 3,
        PROCEED = 4,
        CLOSE_PAYMENT = 5,
        OPEN_EXCHANGE = 6
    }
}