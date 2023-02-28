using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientPayment.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1474365603636L)]
    public interface ChangeUIDTemplate : Template {
        [AutoAdded]
        ChangeUIDComponent changeUid();

        [AutoAdded]
        [PersistentConfig]
        GoodsXPriceComponent goodsXPrice();

        [AutoAdded]
        [PersistentConfig]
        BuyButtonConfirmWithPriceLocalizedTextComponent buttonWithPriceLocalizedText();
    }
}