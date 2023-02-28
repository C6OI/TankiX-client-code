namespace WebSocketSharp.Net {
    internal class HttpHeaderInfo {
        internal HttpHeaderInfo(string name, HttpHeaderType type) {
            Name = name;
            Type = type;
        }

        internal bool IsMultiValueInRequest => (Type & HttpHeaderType.MultiValueInRequest) == HttpHeaderType.MultiValueInRequest;

        internal bool IsMultiValueInResponse => (Type & HttpHeaderType.MultiValueInResponse) == HttpHeaderType.MultiValueInResponse;

        public bool IsRequest => (Type & HttpHeaderType.Request) == HttpHeaderType.Request;

        public bool IsResponse => (Type & HttpHeaderType.Response) == HttpHeaderType.Response;

        public string Name { get; }

        public HttpHeaderType Type { get; }

        public bool IsMultiValue(bool response) => (Type & HttpHeaderType.MultiValue) == HttpHeaderType.MultiValue ? !response ? IsRequest : IsResponse :
                                                   !response ? IsMultiValueInRequest : IsMultiValueInResponse;

        public bool IsRestricted(bool response) => (Type & HttpHeaderType.Restricted) == HttpHeaderType.Restricted && (!response ? IsRequest : IsResponse);
    }
}