using Platform.Kernel.ECS.ClientEntitySystem.API;

public class SquadTeammateInteractionTooltipContentData {
    public Entity teammateEntity { get; set; }

    public bool ShowProfileButton { get; set; }

    public bool ShowLeaveSquadButton { get; set; }

    public bool ShowRemoveFromSquadButton { get; set; }

    public bool ActiveRemoveFromSquadButton { get; set; }

    public bool ShowGiveLeaderButton { get; set; }

    public bool ActiveGiveLeaderButton { get; set; }

    public bool ShowAddFriendButton { get; set; }

    public bool ShowFriendRequestSentButton { get; set; }
}