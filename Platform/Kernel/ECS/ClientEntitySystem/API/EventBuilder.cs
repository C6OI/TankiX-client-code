using System.Collections.Generic;

namespace Platform.Kernel.ECS.ClientEntitySystem.API {
    public interface EventBuilder {
        EventBuilder Attach(Entity entity);

        EventBuilder Attach(Node node);

        EventBuilder Attach(ICollection<Node> nodes);

        EventBuilder AttachAll(ICollection<Entity> entities);

        EventBuilder AttachAll(params Entity[] entities);

        EventBuilder AttachAll(params Node[] nodes);

        void Schedule();

        ScheduledEvent ScheduleDelayed(float timeInSec);

        ScheduledEvent SchedulePeriodic(float timeInSec);
    }
}