namespace Platform.Library.ClientProtocol.API {
    public interface CodecFactory {
        Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs);
    }
}