using Lobby.ClientNavigation.API.Screens;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientNavigation.API {
    [SerialVersionUID(635774962688845913L)]
    public interface ErrorScreenTemplate : ScreenTemplate, Template {
        [AutoAdded]
        [PersistentConfig]
        ErrorScreenTextComponent errorScreenText();

        ErrorScreenActionComponent errorScreenAction();
    }
}