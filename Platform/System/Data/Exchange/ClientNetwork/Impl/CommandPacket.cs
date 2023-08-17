using System.Collections.Generic;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CommandPacket {
        public CommandPacket() { }

        public CommandPacket(List<Command> commands) => Commands = commands;

        public List<Command> Commands { get; internal set; }
    }
}