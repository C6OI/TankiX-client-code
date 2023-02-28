namespace Tanks.Lobby.ClientGarage.Impl {
    public enum PromoCodeCheckResult {
        VALID = 0,
        NOT_FOUND = 1,
        USED = 2,
        EXPIRED = 3,
        INVALID = 4,
        OWNED = 5
    }
}