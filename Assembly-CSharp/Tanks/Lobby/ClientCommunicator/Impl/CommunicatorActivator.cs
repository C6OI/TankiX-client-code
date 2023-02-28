using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientCommunicator.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class CommunicatorActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new ChatSystem());
            EngineService.RegisterSystem(new ChatScreenSystem());
            EngineService.RegisterSystem(new SendMessageSystem());
            EngineService.RegisterSystem(new ReceiveMessageSystem());
            EngineService.RegisterSystem(new LobbyChatUISystem());
            EngineService.RegisterSystem(new CreateChatSystem());
            TemplateRegistry.Register<ChatTemplate>();
            TemplateRegistry.Register<GeneralChatTemplate>();
            TemplateRegistry.Register<PersonalChatTemplate>();
            TemplateRegistry.Register<CustomChatTemplate>();
            TemplateRegistry.Register<SquadChatTemplate>();
        }
    }
}