using System.Collections.Generic;
using UnityEngine;

namespace Lobby.ClientNavigation.API {
    public abstract class ScreenHistoryCleaner : MonoBehaviour {
        public abstract void ClearHistory(Stack<ShowScreenData> history);
    }
}