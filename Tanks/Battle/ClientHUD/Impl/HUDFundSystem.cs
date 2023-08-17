using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HUDFundSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<HUDFundComponent> fund,
            [Combine] SingleNode<RoundFundComponent> roundFund) => SetFundText(fund.component, roundFund);

        [OnEventFire]
        public void UpdateFund(RoundFundUpdatedEvent e, SingleNode<RoundFundComponent> roundFund,
            [JoinAll] FundUIElementNode fund) {
            SetFundText(fund.hudFund, roundFund);
            ScheduleEvent(new StartVisiblePeriodEvent(fund.visibilityInterval.intervalInSec), fund);
        }

        void SetFundText(HUDFundComponent fund, SingleNode<RoundFundComponent> roundFund) =>
            fund.fundText.text = ((int)roundFund.component.Fund).ToString();

        public class FundUIElementNode : Node {
            public HUDFundComponent hudFund;

            public VisibilityIntervalComponent visibilityInterval;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }
    }
}