using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(-2043703779834243389L)]
    public interface BattleUserTemplate : Template {
        BattleUserComponent battleUser();

        UserGroupComponent userJoin();

        BattleGroupComponent battleJoin();

        UserInBattleAsTankComponent userInBattleAsTank();

        UserInBattleAsSpectatorComponent userInBattleAsSpectator();

        [AutoAdded]
        MouseControlStateHolderComponent mouseControlStateHolder();

        [PersistentConfig]
        [AutoAdded]
        IdleKickConfigComponent idleKickConfig();

        [PersistentConfig]
        [AutoAdded]
        UpsideDownConfigComponent upsideDownConfig();

        [PersistentConfig]
        [AutoAdded]
        SelfDestructionConfigComponent selfDestructionConfig();

        IdleCounterComponent idleCounter();

        [AutoAdded]
        IdleBeginTimeComponent idleBeginTime();

        [AutoAdded]
        IdleKickCheckLastTimeComponent IdleKickCheckLastTime();

        SelfBattleUserComponent selfBattleUser();
    }
}