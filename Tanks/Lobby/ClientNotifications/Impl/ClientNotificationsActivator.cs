using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration;
using Tanks.Lobby.ClientNotifications.API;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class ClientNotificationsActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator {
        [Inject] public static EngineService EngineService { get; set; }

        [Inject] public static TemplateRegistry TemplateRegistry { get; set; }

        public void RegisterSystemsAndTemplates() {
            TemplateRegistry.Register<ItemNotificationTemplate>();
            TemplateRegistry.Register<NewItemNotificationTemplate>();
            TemplateRegistry.Register<UIDChangedNotificationTemplate>();
            EngineService.RegisterSystem(new ItemNotificationsSystem());
            EngineService.RegisterSystem(new NewItemNotificationsSystems());
            EngineService.RegisterSystem(new UIDChangedNotificationSystem());
        }
    }
}