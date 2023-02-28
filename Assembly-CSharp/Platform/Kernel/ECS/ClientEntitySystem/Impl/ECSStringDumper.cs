using System.Collections.Generic;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public static class ECSStringDumper {
        [Inject] public static EngineServiceInternal EngineService { get; set; }

        public static string Build(bool detailInfo = false) {
            StringBuilder stringBuilder = new();
            ICollection<Entity> collection = EngineService == null ? new List<Entity>() : EngineService.EntityRegistry.GetAllEntities();

            foreach (Entity item in collection) {
                if (EngineService.EntityStub.Equals(item)) {
                    continue;
                }

                string value = string.Format("[Entity: Id={0}, Name={1}]\n", item.Id, item.Name);
                stringBuilder.Append(value);
                EntityInternal entityInternal = (EntityInternal)item;
                ICollection<Component> collection2 = !(entityInternal is EntityStub) ? entityInternal.Components : new List<Component>();

                foreach (Component item2 in collection2) {
                    stringBuilder.Append("[Component: ");
                    stringBuilder.Append(!detailInfo ? item2.GetType().Name : EcsToStringUtil.ToStringWithProperties(item2));
                    stringBuilder.Append("]\n");
                }
            }

            return stringBuilder.ToString();
        }
    }
}