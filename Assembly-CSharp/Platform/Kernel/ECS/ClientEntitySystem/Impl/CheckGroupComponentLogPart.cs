using System;
using System.Collections.Generic;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class CheckGroupComponentLogPart : LogPart {
        readonly IList<Entity> entitiesWithMissingGroupComponentByEntity = Collections.EmptyList<Entity>();
        readonly Type groupComponent;

        public CheckGroupComponentLogPart(HandlerArgument handlerArgument, ICollection<Entity> entities) {
            Optional<Type> contextComponent = handlerArgument.JoinType.Get().ContextComponent;

            if (!contextComponent.IsPresent()) {
                return;
            }

            groupComponent = contextComponent.Get();
            entitiesWithMissingGroupComponentByEntity = new List<Entity>();

            foreach (Entity entity in entities) {
                if (entity.HasComponent(groupComponent)) {
                    break;
                }

                entitiesWithMissingGroupComponentByEntity.Add(entity);
            }
        }

        public Optional<string> GetSkipReason() => entitiesWithMissingGroupComponentByEntity.Count != 0 ? Optional<string>.of(GetMessageForGroupComponent()) : Optional<string>.empty();

        string GetMessageForGroupComponent() {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(string.Format("\tMissing group component={0}\n\t", groupComponent.Name));

            foreach (Entity item in entitiesWithMissingGroupComponentByEntity) {
                stringBuilder.Append(string.Format("\tEntity={0}", EcsToStringUtil.ToString(item)));
                stringBuilder.Append("\n\t");
            }

            return stringBuilder.ToString();
        }
    }
}