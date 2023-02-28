using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EventBuilder {
        EventBuilder Attach(Entity entity);

        EventBuilder Attach(Node node);

        EventBuilder Attach<T>(ICollection<T> nodes) where T : Node;

        EventBuilder AttachAll(ICollection<Entity> entities);

        EventBuilder AttachAll(params Entity[] entities);

        EventBuilder AttachAll(params Node[] nodes);

        void Schedule();

        ScheduledEvent ScheduleDelayed(float timeInSec);

        ScheduledEvent SchedulePeriodic(float timeInSec);
    }
}