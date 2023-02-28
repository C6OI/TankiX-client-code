using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(1446805577605L)]
    public interface BuyButtonWithPriceTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        BuyButtonConfirmWithPriceLocalizedTextComponent buttonWithPriceLocalizedText();
    }
}