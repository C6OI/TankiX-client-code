using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class AdminUserSystem : ElevatedAccessUserBaseSystem {
        [OnEventFire]
        public void InitAdminConsole(NodeAddedEvent e, AdminUserNode admin) {
            InitConsole(admin);
            SmartConsole.RegisterCommand("exception", "Throws NullReferenceException", ThrowNullPointer);
            SmartConsole.RegisterCommand("dropSupply", "dropSupply ARMOR", "Drops Supply in Current", DropSupply);
            SmartConsole.RegisterCommand("dropGold", "dropGold CRY", "Drops Gold in Current", DropGold);
            SmartConsole.RegisterCommand("blockUser", "blockUser User1 CHEATING", "Blocks user. Possible reasons: RULES_ABUSE, SABOTAGE, CHEATING, FRAUD...", BlockUser);
            SmartConsole.RegisterCommand("runCommand", "Run server console command", RunCommand);
            SmartConsole.RegisterCommand("createUserItem", "createUserItem -1816745725 3", "Create user item", CreateUserItem);
            SmartConsole.RegisterCommand("wipeUserItems", "wipeUserItems", "Wipe user items", WipeUserItems);
            SmartConsole.RegisterCommand("addBots", "addBots RED 2", "Add bots to battle", AddBotsToBattle);
        }

        public class AdminUserNode : SelfUserNode {
            public UserAdminComponent userAdmin;
        }
    }
}