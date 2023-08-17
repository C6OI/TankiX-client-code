using Lobby.ClientCommunicator.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;

namespace Lobby.ClientCommunicator.Impl {
    public class CommunicatorActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            EngineService.RegisterSystem(new ChatRequestSystem());
            EngineService.RegisterSystem(new ChatSystem());
            EngineService.RegisterSystem(new PublicChatSystem());
            EngineService.RegisterSystem(new PrivateChatSystem());
            EngineService.RegisterSystem(new ActiveUsersSystem());
            EngineService.RegisterSystem(new SendMessageSystem());
            EngineService.RegisterSystem(new ReceiveMessageSystem());
            EngineService.RegisterSystem(new SectionListSystem());
            TemplateRegistry.Register<ChatDescriptionTemplate>();
            TemplateRegistry.Register<PrivateChatDescriptionTemplate>();
            TemplateRegistry.Register<PublicChatDescriptionTemplate>();
            TemplateRegistry.Register<ChatTemplate>();
            TemplateRegistry.Register<PublicChatTemplate>();
            TemplateRegistry.Register<PrivateChatTemplate>();
            TemplateRegistry.Register<ChatMessageTemplate>();
            TemplateRegistry.Register<SectionTemplate>();
        }
    }
}