using Platform.Library.ClientProtocol.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface CommandsCodec : Codec {
        void RegisterCommand<T>(CommandCode code) where T : Command;
    }
}