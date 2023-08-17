using YamlDotNet.Core.Tokens;

namespace YamlDotNet.Core {
    static class Constants {
        public const int MajorVersion = 1;

        public const int MinorVersion = 1;

        public const char HandleCharacter = '!';

        public const string DefaultHandle = "!";

        public static readonly TagDirective[] DefaultTagDirectives = new TagDirective[2] {
            new("!", "!"),
            new("!!", "tag:yaml.org,2002:")
        };
    }
}