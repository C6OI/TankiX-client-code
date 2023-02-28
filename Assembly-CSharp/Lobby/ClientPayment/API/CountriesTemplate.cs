using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.API {
    [SerialVersionUID(635993398222494398L)]
    public interface CountriesTemplate : Template {
        [AutoAdded]
        [PersistentConfig]
        CountriesComponent countries();

        [AutoAdded]
        [PersistentConfig]
        PhoneCodesComponent phoneCodes();
    }
}