using System.Collections;

namespace log4net.Repository.Hierarchy {
    sealed class ProvisionNode : ArrayList {
        internal ProvisionNode(Logger log) => Add(log);
    }
}