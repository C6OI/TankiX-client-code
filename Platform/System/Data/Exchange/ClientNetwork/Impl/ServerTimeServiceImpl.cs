using System;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ServerTimeServiceImpl : ServerTimeService, ServerTimeServiceInternal {
        long initialServerTime;

        public long InitialServerTime {
            get => initialServerTime;
            set {
                initialServerTime = value;

                if (OnInitServerTime != null) {
                    OnInitServerTime(value);
                }
            }
        }

        public event Action<long> OnInitServerTime;
    }
}