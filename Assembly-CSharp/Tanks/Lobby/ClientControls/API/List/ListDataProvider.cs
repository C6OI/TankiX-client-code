using System;
using System.Collections.Generic;

namespace Tanks.Lobby.ClientControls.API.List {
    public interface ListDataProvider {
        IList<object> Data { get; }

        object Selected { get; set; }

        event Action<ListDataProvider> DataChanged;
    }
}