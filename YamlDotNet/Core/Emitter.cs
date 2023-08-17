using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.Tokens;
using AnchorAlias = YamlDotNet.Core.Events.AnchorAlias;
using Comment = YamlDotNet.Core.Events.Comment;
using DocumentEnd = YamlDotNet.Core.Events.DocumentEnd;
using DocumentStart = YamlDotNet.Core.Events.DocumentStart;
using Scalar = YamlDotNet.Core.Events.Scalar;
using StreamEnd = YamlDotNet.Core.Events.StreamEnd;
using StreamStart = YamlDotNet.Core.Events.StreamStart;

namespace YamlDotNet.Core {
    public class Emitter : IEmitter {
        const int MinBestIndent = 2;

        const int MaxBestIndent = 9;

        const int MaxAliasLength = 128;

        static readonly Regex uriReplacer = new("[^0-9A-Za-z_\\-;?@=$~\\\\\\)\\]/:&+,\\.\\*\\(\\[!]",
            RegexOptions.Compiled | RegexOptions.Singleline);

        readonly AnchorData anchorData = new();

        readonly int bestIndent;

        readonly int bestWidth;

        readonly Queue<ParsingEvent> events = new();

        readonly Stack<int> indents = new();

        readonly bool isCanonical;

        readonly TextWriter output;

        readonly bool outputUsesUnicodeEncoding;

        readonly ScalarData scalarData = new();

        readonly Stack<EmitterState> states = new();

        readonly TagData tagData = new();

        readonly TagDirectiveCollection tagDirectives = new();

        int column;

        int flowLevel;

        int indent;

        bool isDocumentEndWritten;

        bool isIndentation;

        bool isMappingContext;

        bool isOpenEnded;

        bool isRootContext;

        bool isSimpleKeyContext;

        bool isWhitespace;

        EmitterState state;

        public Emitter(TextWriter output)
            : this(output, 2) { }

        public Emitter(TextWriter output, int bestIndent)
            : this(output, bestIndent, int.MaxValue) { }

        public Emitter(TextWriter output, int bestIndent, int bestWidth)
            : this(output, bestIndent, bestWidth, false) { }

        public Emitter(TextWriter output, int bestIndent, int bestWidth, bool isCanonical) {
            if (bestIndent < 2 || bestIndent > 9) {
                throw new ArgumentOutOfRangeException("bestIndent",
                    string.Format(CultureInfo.InvariantCulture,
                        "The bestIndent parameter must be between {0} and {1}.",
                        2,
                        9));
            }

            this.bestIndent = bestIndent;

            if (bestWidth <= bestIndent * 2) {
                throw new ArgumentOutOfRangeException("bestWidth",
                    "The bestWidth parameter must be greater than bestIndent * 2.");
            }

            this.bestWidth = bestWidth;
            this.isCanonical = isCanonical;
            this.output = output;
            outputUsesUnicodeEncoding = IsUnicode(output.Encoding);
        }

        public void Emit(ParsingEvent @event) {
            events.Enqueue(@event);

            while (!NeedMoreEvents()) {
                ParsingEvent evt = events.Peek();

                try {
                    AnalyzeEvent(evt);
                    StateMachine(evt);
                } finally {
                    events.Dequeue();
                }
            }
        }

        bool NeedMoreEvents() {
            if (events.Count == 0) {
                return true;
            }

            int num;

            switch (events.Peek().Type) {
                case EventType.DocumentStart:
                    num = 1;
                    break;

                case EventType.SequenceStart:
                    num = 2;
                    break;

                case EventType.MappingStart:
                    num = 3;
                    break;

                default:
                    return false;
            }

            if (events.Count > num) {
                return false;
            }

            int num2 = 0;

            foreach (ParsingEvent @event in events) {
                switch (@event.Type) {
                    case EventType.DocumentStart:
                    case EventType.SequenceStart:
                    case EventType.MappingStart:
                        num2++;
                        break;

                    case EventType.DocumentEnd:
                    case EventType.SequenceEnd:
                    case EventType.MappingEnd:
                        num2--;
                        break;
                }

                if (num2 == 0) {
                    return false;
                }
            }

            return true;
        }

        void AnalyzeEvent(ParsingEvent evt) {
            anchorData.anchor = null;
            tagData.handle = null;
            tagData.suffix = null;
            AnchorAlias anchorAlias = evt as AnchorAlias;

            if (anchorAlias != null) {
                AnalyzeAnchor(anchorAlias.Value, true);
                return;
            }

            NodeEvent nodeEvent = evt as NodeEvent;

            if (nodeEvent != null) {
                Scalar scalar = evt as Scalar;

                if (scalar != null) {
                    AnalyzeScalar(scalar);
                }

                AnalyzeAnchor(nodeEvent.Anchor, false);

                if (!string.IsNullOrEmpty(nodeEvent.Tag) && (isCanonical || nodeEvent.IsCanonical)) {
                    AnalyzeTag(nodeEvent.Tag);
                }
            }
        }

        void AnalyzeAnchor(string anchor, bool isAlias) {
            anchorData.anchor = anchor;
            anchorData.isAlias = isAlias;
        }

        void AnalyzeScalar(Scalar scalar) {
            string value = scalar.Value;
            scalarData.value = value;

            if (value.Length == 0) {
                if (scalar.Tag == "tag:yaml.org,2002:null") {
                    scalarData.isMultiline = false;
                    scalarData.isFlowPlainAllowed = false;
                    scalarData.isBlockPlainAllowed = true;
                    scalarData.isSingleQuotedAllowed = false;
                    scalarData.isBlockAllowed = false;
                } else {
                    scalarData.isMultiline = false;
                    scalarData.isFlowPlainAllowed = false;
                    scalarData.isBlockPlainAllowed = false;
                    scalarData.isSingleQuotedAllowed = true;
                    scalarData.isBlockAllowed = false;
                }

                return;
            }

            bool flag = false;
            bool flag2 = false;

            if (value.StartsWith("---", StringComparison.Ordinal) || value.StartsWith("...", StringComparison.Ordinal)) {
                flag = true;
                flag2 = true;
            }

            CharacterAnalyzer<StringLookAheadBuffer> characterAnalyzer = new(new StringLookAheadBuffer(value));
            bool flag3 = true;
            bool flag4 = characterAnalyzer.IsWhiteBreakOrZero(1);
            bool flag5 = false;
            bool flag6 = false;
            bool flag7 = false;
            bool flag8 = false;
            bool flag9 = false;
            bool flag10 = false;
            bool flag11 = false;
            bool flag12 = false;
            bool flag13 = false;
            bool flag14 = !ValueIsRepresentableInOutputEncoding(value);
            bool flag15 = true;

            while (!characterAnalyzer.EndOfInput) {
                if (flag15) {
                    if (characterAnalyzer.Check("#,[]{}&*!|>\\\"%@`")) {
                        flag = true;
                        flag2 = true;
                    }

                    if (characterAnalyzer.Check("?:")) {
                        flag = true;

                        if (flag4) {
                            flag2 = true;
                        }
                    }

                    if (characterAnalyzer.Check('-') && flag4) {
                        flag = true;
                        flag2 = true;
                    }
                } else {
                    if (characterAnalyzer.Check(",?[]{}")) {
                        flag = true;
                    }

                    if (characterAnalyzer.Check(':')) {
                        flag = true;

                        if (flag4) {
                            flag2 = true;
                        }
                    }

                    if (characterAnalyzer.Check('#') && flag3) {
                        flag = true;
                        flag2 = true;
                    }
                }

                if (!flag14 && !characterAnalyzer.IsPrintable()) {
                    flag14 = true;
                }

                if (characterAnalyzer.IsBreak()) {
                    flag13 = true;
                }

                if (characterAnalyzer.IsSpace()) {
                    if (flag15) {
                        flag5 = true;
                    }

                    if (characterAnalyzer.Buffer.Position >= characterAnalyzer.Buffer.Length - 1) {
                        flag7 = true;
                    }

                    if (flag12) {
                        flag9 = true;
                    }

                    flag11 = true;
                    flag12 = false;
                } else if (characterAnalyzer.IsBreak()) {
                    if (flag15) {
                        flag6 = true;
                    }

                    if (characterAnalyzer.Buffer.Position >= characterAnalyzer.Buffer.Length - 1) {
                        flag8 = true;
                    }

                    if (flag11) {
                        flag10 = true;
                    }

                    flag11 = false;
                    flag12 = true;
                } else {
                    flag11 = false;
                    flag12 = false;
                }

                flag3 = characterAnalyzer.IsWhiteBreakOrZero();
                characterAnalyzer.Skip(1);

                if (!characterAnalyzer.EndOfInput) {
                    flag4 = characterAnalyzer.IsWhiteBreakOrZero(1);
                }

                flag15 = false;
            }

            scalarData.isFlowPlainAllowed = true;
            scalarData.isBlockPlainAllowed = true;
            scalarData.isSingleQuotedAllowed = true;
            scalarData.isBlockAllowed = true;

            if (flag5 || flag6 || flag7 || flag8) {
                scalarData.isFlowPlainAllowed = false;
                scalarData.isBlockPlainAllowed = false;
            }

            if (flag7) {
                scalarData.isBlockAllowed = false;
            }

            if (flag9) {
                scalarData.isFlowPlainAllowed = false;
                scalarData.isBlockPlainAllowed = false;
                scalarData.isSingleQuotedAllowed = false;
            }

            if (flag10 || flag14) {
                scalarData.isFlowPlainAllowed = false;
                scalarData.isBlockPlainAllowed = false;
                scalarData.isSingleQuotedAllowed = false;
                scalarData.isBlockAllowed = false;
            }

            scalarData.isMultiline = flag13;

            if (flag13) {
                scalarData.isFlowPlainAllowed = false;
                scalarData.isBlockPlainAllowed = false;
            }

            if (flag) {
                scalarData.isFlowPlainAllowed = false;
            }

            if (flag2) {
                scalarData.isBlockPlainAllowed = false;
            }
        }

        bool ValueIsRepresentableInOutputEncoding(string value) {
            if (outputUsesUnicodeEncoding) {
                return true;
            }

            try {
                byte[] bytes = output.Encoding.GetBytes(value);
                string @string = output.Encoding.GetString(bytes, 0, bytes.Length);
                return @string.Equals(value);
            } catch (EncoderFallbackException) {
                return false;
            } catch (ArgumentOutOfRangeException) {
                return false;
            }
        }

        bool IsUnicode(Encoding encoding) => encoding is UTF8Encoding ||
                                             encoding is UnicodeEncoding ||
                                             encoding is UTF7Encoding ||
                                             encoding is UTF8Encoding;

        void AnalyzeTag(string tag) {
            tagData.handle = tag;

            foreach (TagDirective tagDirective in tagDirectives) {
                if (tag.StartsWith(tagDirective.Prefix, StringComparison.Ordinal)) {
                    tagData.handle = tagDirective.Handle;
                    tagData.suffix = tag.Substring(tagDirective.Prefix.Length);
                    break;
                }
            }
        }

        void StateMachine(ParsingEvent evt) {
            Comment comment = evt as Comment;

            if (comment != null) {
                EmitComment(comment);
                return;
            }

            switch (state) {
                case EmitterState.StreamStart:
                    EmitStreamStart(evt);
                    break;

                case EmitterState.FirstDocumentStart:
                    EmitDocumentStart(evt, true);
                    break;

                case EmitterState.DocumentStart:
                    EmitDocumentStart(evt, false);
                    break;

                case EmitterState.DocumentContent:
                    EmitDocumentContent(evt);
                    break;

                case EmitterState.DocumentEnd:
                    EmitDocumentEnd(evt);
                    break;

                case EmitterState.FlowSequenceFirstItem:
                    EmitFlowSequenceItem(evt, true);
                    break;

                case EmitterState.FlowSequenceItem:
                    EmitFlowSequenceItem(evt, false);
                    break;

                case EmitterState.FlowMappingFirstKey:
                    EmitFlowMappingKey(evt, true);
                    break;

                case EmitterState.FlowMappingKey:
                    EmitFlowMappingKey(evt, false);
                    break;

                case EmitterState.FlowMappingSimpleValue:
                    EmitFlowMappingValue(evt, true);
                    break;

                case EmitterState.FlowMappingValue:
                    EmitFlowMappingValue(evt, false);
                    break;

                case EmitterState.BlockSequenceFirstItem:
                    EmitBlockSequenceItem(evt, true);
                    break;

                case EmitterState.BlockSequenceItem:
                    EmitBlockSequenceItem(evt, false);
                    break;

                case EmitterState.BlockMappingFirstKey:
                    EmitBlockMappingKey(evt, true);
                    break;

                case EmitterState.BlockMappingKey:
                    EmitBlockMappingKey(evt, false);
                    break;

                case EmitterState.BlockMappingSimpleValue:
                    EmitBlockMappingValue(evt, true);
                    break;

                case EmitterState.BlockMappingValue:
                    EmitBlockMappingValue(evt, false);
                    break;

                case EmitterState.StreamEnd:
                    throw new YamlException("Expected nothing after STREAM-END");

                default:
                    throw new InvalidOperationException();
            }
        }

        void EmitComment(Comment comment) {
            if (comment.IsInline) {
                Write(' ');
            } else {
                WriteBreak();
            }

            Write("# ");
            Write(comment.Value);
            isIndentation = true;
        }

        void EmitStreamStart(ParsingEvent evt) {
            if (!(evt is StreamStart)) {
                throw new ArgumentException("Expected STREAM-START.", "evt");
            }

            indent = -1;
            column = 0;
            isWhitespace = true;
            isIndentation = true;
            state = EmitterState.FirstDocumentStart;
        }

        void EmitDocumentStart(ParsingEvent evt, bool isFirst) {
            DocumentStart documentStart = evt as DocumentStart;

            if (documentStart != null) {
                bool flag = documentStart.IsImplicit && isFirst && !isCanonical;
                TagDirectiveCollection tagDirectiveCollection = NonDefaultTagsAmong(documentStart.Tags);

                if (!isFirst &&
                    !isDocumentEndWritten &&
                    (documentStart.Version != null || tagDirectiveCollection.Count > 0)) {
                    isDocumentEndWritten = false;
                    WriteIndicator("...", true, false, false);
                    WriteIndent();
                }

                if (documentStart.Version != null) {
                    AnalyzeVersionDirective(documentStart.Version);
                    flag = false;
                    WriteIndicator("%YAML", true, false, false);
                    WriteIndicator(string.Format(CultureInfo.InvariantCulture, "{0}.{1}", 1, 1), true, false, false);
                    WriteIndent();
                }

                foreach (TagDirective item in tagDirectiveCollection) {
                    AppendTagDirectiveTo(item, false, tagDirectives);
                }

                TagDirective[] defaultTagDirectives = Constants.DefaultTagDirectives;

                foreach (TagDirective value in defaultTagDirectives) {
                    AppendTagDirectiveTo(value, true, tagDirectives);
                }

                if (tagDirectiveCollection.Count > 0) {
                    flag = false;
                    TagDirective[] defaultTagDirectives2 = Constants.DefaultTagDirectives;

                    foreach (TagDirective value2 in defaultTagDirectives2) {
                        AppendTagDirectiveTo(value2, true, tagDirectiveCollection);
                    }

                    foreach (TagDirective item2 in tagDirectiveCollection) {
                        WriteIndicator("%TAG", true, false, false);
                        WriteTagHandle(item2.Handle);
                        WriteTagContent(item2.Prefix, true);
                        WriteIndent();
                    }
                }

                if (CheckEmptyDocument()) {
                    flag = false;
                }

                if (!flag) {
                    WriteIndent();
                    WriteIndicator("---", true, false, false);

                    if (isCanonical) {
                        WriteIndent();
                    }
                }

                state = EmitterState.DocumentContent;
            } else {
                if (!(evt is StreamEnd)) {
                    throw new YamlException("Expected DOCUMENT-START or STREAM-END");
                }

                if (isOpenEnded) {
                    WriteIndicator("...", true, false, false);
                    WriteIndent();
                }

                state = EmitterState.StreamEnd;
            }
        }

        TagDirectiveCollection NonDefaultTagsAmong(IEnumerable<TagDirective> tagCollection) {
            TagDirectiveCollection tagDirectiveCollection = new();

            if (tagCollection == null) {
                return tagDirectiveCollection;
            }

            foreach (TagDirective item2 in tagCollection) {
                AppendTagDirectiveTo(item2, false, tagDirectiveCollection);
            }

            TagDirective[] defaultTagDirectives = Constants.DefaultTagDirectives;

            foreach (TagDirective item in defaultTagDirectives) {
                tagDirectiveCollection.Remove(item);
            }

            return tagDirectiveCollection;
        }

        void AnalyzeVersionDirective(VersionDirective versionDirective) {
            if (versionDirective.Version.Major != 1 || versionDirective.Version.Minor != 1) {
                throw new YamlException("Incompatible %YAML directive");
            }
        }

        void AppendTagDirectiveTo(TagDirective value, bool allowDuplicates, TagDirectiveCollection tagDirectives) {
            if (tagDirectives.Contains(value)) {
                if (!allowDuplicates) {
                    throw new YamlException("Duplicate %TAG directive.");
                }
            } else {
                tagDirectives.Add(value);
            }
        }

        void EmitDocumentContent(ParsingEvent evt) {
            states.Push(EmitterState.DocumentEnd);
            EmitNode(evt, true, false, false);
        }

        void EmitNode(ParsingEvent evt, bool isRoot, bool isMapping, bool isSimpleKey) {
            isRootContext = isRoot;
            isMappingContext = isMapping;
            isSimpleKeyContext = isSimpleKey;

            switch (evt.Type) {
                case EventType.Alias:
                    EmitAlias();
                    break;

                case EventType.Scalar:
                    EmitScalar(evt);
                    break;

                case EventType.SequenceStart:
                    EmitSequenceStart(evt);
                    break;

                case EventType.MappingStart:
                    EmitMappingStart(evt);
                    break;

                default:
                    throw new YamlException(
                        string.Format("Expected SCALAR, SEQUENCE-START, MAPPING-START, or ALIAS, got {0}", evt.Type));
            }
        }

        void EmitAlias() {
            ProcessAnchor();
            state = states.Pop();
        }

        void EmitScalar(ParsingEvent evt) {
            SelectScalarStyle(evt);
            ProcessAnchor();
            ProcessTag();
            IncreaseIndent(true, false);
            ProcessScalar();
            indent = indents.Pop();
            state = states.Pop();
        }

        void SelectScalarStyle(ParsingEvent evt) {
            Scalar scalar = (Scalar)evt;
            ScalarStyle scalarStyle = scalar.Style;
            bool flag = tagData.handle == null && tagData.suffix == null;

            if (flag && !scalar.IsPlainImplicit && !scalar.IsQuotedImplicit) {
                throw new YamlException("Neither tag nor isImplicit flags are specified.");
            }

            if (scalarStyle == ScalarStyle.Any) {
                scalarStyle = !scalarData.isMultiline ? ScalarStyle.Plain : ScalarStyle.Folded;
            }

            if (isCanonical) {
                scalarStyle = ScalarStyle.DoubleQuoted;
            }

            if (isSimpleKeyContext && scalarData.isMultiline) {
                scalarStyle = ScalarStyle.DoubleQuoted;
            }

            if (scalarStyle == ScalarStyle.Plain) {
                if (flowLevel != 0 && !scalarData.isFlowPlainAllowed || flowLevel == 0 && !scalarData.isBlockPlainAllowed) {
                    scalarStyle = ScalarStyle.SingleQuoted;
                }

                if (string.IsNullOrEmpty(scalarData.value) && (flowLevel != 0 || isSimpleKeyContext)) {
                    scalarStyle = ScalarStyle.SingleQuoted;
                }

                if (flag && !scalar.IsPlainImplicit) {
                    scalarStyle = ScalarStyle.SingleQuoted;
                }
            }

            if (scalarStyle == ScalarStyle.SingleQuoted && !scalarData.isSingleQuotedAllowed) {
                scalarStyle = ScalarStyle.DoubleQuoted;
            }

            if ((scalarStyle == ScalarStyle.Literal || scalarStyle == ScalarStyle.Folded) &&
                (!scalarData.isBlockAllowed || flowLevel != 0 || isSimpleKeyContext)) {
                scalarStyle = ScalarStyle.DoubleQuoted;
            }

            scalarData.style = scalarStyle;
        }

        void ProcessScalar() {
            switch (scalarData.style) {
                case ScalarStyle.Plain:
                    WritePlainScalar(scalarData.value, !isSimpleKeyContext);
                    break;

                case ScalarStyle.SingleQuoted:
                    WriteSingleQuotedScalar(scalarData.value, !isSimpleKeyContext);
                    break;

                case ScalarStyle.DoubleQuoted:
                    WriteDoubleQuotedScalar(scalarData.value, !isSimpleKeyContext);
                    break;

                case ScalarStyle.Literal:
                    WriteLiteralScalar(scalarData.value);
                    break;

                case ScalarStyle.Folded:
                    WriteFoldedScalar(scalarData.value);
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        void WritePlainScalar(string value, bool allowBreaks) {
            if (!isWhitespace) {
                Write(' ');
            }

            bool flag = false;
            bool flag2 = false;

            for (int i = 0; i < value.Length; i++) {
                char c = value[i];

                if (IsSpace(c)) {
                    if (allowBreaks && !flag && column > bestWidth && i + 1 < value.Length && value[i + 1] != ' ') {
                        WriteIndent();
                    } else {
                        Write(c);
                    }

                    flag = true;
                    continue;
                }

                if (IsBreak(c)) {
                    if (!flag2 && c == '\n') {
                        WriteBreak();
                    }

                    WriteBreak();
                    isIndentation = true;
                    flag2 = true;
                    continue;
                }

                if (flag2) {
                    WriteIndent();
                }

                Write(c);
                isIndentation = false;
                flag = false;
                flag2 = false;
            }

            isWhitespace = false;
            isIndentation = false;

            if (isRootContext) {
                isOpenEnded = true;
            }
        }

        void WriteSingleQuotedScalar(string value, bool allowBreaks) {
            WriteIndicator("'", true, false, false);
            bool flag = false;
            bool flag2 = false;

            for (int i = 0; i < value.Length; i++) {
                char c = value[i];

                if (c == ' ') {
                    if (allowBreaks &&
                        !flag &&
                        column > bestWidth &&
                        i != 0 &&
                        i + 1 < value.Length &&
                        value[i + 1] != ' ') {
                        WriteIndent();
                    } else {
                        Write(c);
                    }

                    flag = true;
                    continue;
                }

                if (IsBreak(c)) {
                    if (!flag2 && c == '\n') {
                        WriteBreak();
                    }

                    WriteBreak();
                    isIndentation = true;
                    flag2 = true;
                    continue;
                }

                if (flag2) {
                    WriteIndent();
                }

                if (c == '\'') {
                    Write(c);
                }

                Write(c);
                isIndentation = false;
                flag = false;
                flag2 = false;
            }

            WriteIndicator("'", false, false, false);
            isWhitespace = false;
            isIndentation = false;
        }

        void WriteDoubleQuotedScalar(string value, bool allowBreaks) {
            WriteIndicator("\"", true, false, false);
            bool flag = false;

            for (int i = 0; i < value.Length; i++) {
                char c = value[i];

                if (IsPrintable(c) && !IsBreak(c)) {
                    switch (c) {
                        case '"':
                        case '\\':
                            break;

                        case ' ':
                            if (allowBreaks && !flag && column > bestWidth && i > 0 && i + 1 < value.Length) {
                                WriteIndent();

                                if (value[i + 1] == ' ') {
                                    Write('\\');
                                }
                            } else {
                                Write(c);
                            }

                            flag = true;
                            continue;

                        default:
                            Write(c);
                            flag = false;
                            continue;
                    }
                }

                Write('\\');

                switch (c) {
                    case '\0':
                        Write('0');
                        break;

                    case '\a':
                        Write('a');
                        break;

                    case '\b':
                        Write('b');
                        break;

                    case '\t':
                        Write('t');
                        break;

                    case '\n':
                        Write('n');
                        break;

                    case '\v':
                        Write('v');
                        break;

                    case '\f':
                        Write('f');
                        break;

                    case '\r':
                        Write('r');
                        break;

                    case '\u001b':
                        Write('e');
                        break;

                    case '"':
                        Write('"');
                        break;

                    case '\\':
                        Write('\\');
                        break;

                    case '\u0085':
                        Write('N');
                        break;

                    case '\u00a0':
                        Write('_');
                        break;

                    case '\u2028':
                        Write('L');
                        break;

                    case '\u2029':
                        Write('P');
                        break;

                    default: {
                        short num = (short)c;

                        if (num <= 255) {
                            Write('x');
                            Write(num.ToString("X02", CultureInfo.InvariantCulture));
                        } else {
                            Write('u');
                            Write(num.ToString("X04", CultureInfo.InvariantCulture));
                        }

                        break;
                    }
                }

                flag = false;
            }

            WriteIndicator("\"", false, false, false);
            isWhitespace = false;
            isIndentation = false;
        }

        void WriteLiteralScalar(string value) {
            bool flag = true;
            WriteIndicator("|", true, false, false);
            WriteBlockScalarHints(value);
            WriteBreak();
            isIndentation = true;
            isWhitespace = true;

            foreach (char c in value) {
                if (IsBreak(c)) {
                    WriteBreak();
                    isIndentation = true;
                    flag = true;
                    continue;
                }

                if (flag) {
                    WriteIndent();
                }

                Write(c);
                isIndentation = false;
                flag = false;
            }
        }

        void WriteFoldedScalar(string value) {
            bool flag = true;
            bool flag2 = true;
            WriteIndicator(">", true, false, false);
            WriteBlockScalarHints(value);
            WriteBreak();
            isIndentation = true;
            isWhitespace = true;

            for (int i = 0; i < value.Length; i++) {
                char c = value[i];

                if (IsBreak(c)) {
                    if (!flag && !flag2 && c == '\n') {
                        int j;

                        for (j = 0; i + j < value.Length && IsBreak(value[i + j]); j++) { }

                        if (i + j < value.Length && !IsBlank(value[i + j]) && !IsBreak(value[i + j])) {
                            WriteBreak();
                        }
                    }

                    WriteBreak();
                    isIndentation = true;
                    flag = true;
                } else {
                    if (flag) {
                        WriteIndent();
                        flag2 = IsBlank(c);
                    }

                    if (!flag && c == ' ' && i + 1 < value.Length && value[i + 1] != ' ' && column > bestWidth) {
                        WriteIndent();
                    } else {
                        Write(c);
                    }

                    isIndentation = false;
                    flag = false;
                }
            }
        }

        static bool IsSpace(char character) => character == ' ';

        static bool IsBreak(char character) => character == '\r' ||
                                               character == '\n' ||
                                               character == '\u0085' ||
                                               character == '\u2028' ||
                                               character == '\u2029';

        static bool IsBlank(char character) => character == ' ' || character == '\t';

        static bool IsPrintable(char character) => character == '\t' ||
                                                   character == '\n' ||
                                                   character == '\r' ||
                                                   character >= ' ' && character <= '~' ||
                                                   character == '\u0085' ||
                                                   character >= '\u00a0' && character <= '\ud7ff' ||
                                                   character >= '\ue000' && character <= '\ufffd';

        void EmitSequenceStart(ParsingEvent evt) {
            ProcessAnchor();
            ProcessTag();
            SequenceStart sequenceStart = (SequenceStart)evt;

            if (flowLevel != 0 || isCanonical || sequenceStart.Style == SequenceStyle.Flow || CheckEmptySequence()) {
                state = EmitterState.FlowSequenceFirstItem;
            } else {
                state = EmitterState.BlockSequenceFirstItem;
            }
        }

        void EmitMappingStart(ParsingEvent evt) {
            ProcessAnchor();
            ProcessTag();
            MappingStart mappingStart = (MappingStart)evt;

            if (flowLevel != 0 || isCanonical || mappingStart.Style == MappingStyle.Flow || CheckEmptyMapping()) {
                state = EmitterState.FlowMappingFirstKey;
            } else {
                state = EmitterState.BlockMappingFirstKey;
            }
        }

        void ProcessAnchor() {
            if (anchorData.anchor != null) {
                WriteIndicator(!anchorData.isAlias ? "&" : "*", true, false, false);
                WriteAnchor(anchorData.anchor);
            }
        }

        void ProcessTag() {
            if (tagData.handle == null && tagData.suffix == null) {
                return;
            }

            if (tagData.handle != null) {
                WriteTagHandle(tagData.handle);

                if (tagData.suffix != null) {
                    WriteTagContent(tagData.suffix, false);
                }
            } else {
                WriteIndicator("!<", true, false, false);
                WriteTagContent(tagData.suffix, false);
                WriteIndicator(">", false, false, false);
            }
        }

        void EmitDocumentEnd(ParsingEvent evt) {
            DocumentEnd documentEnd = evt as DocumentEnd;

            if (documentEnd != null) {
                WriteIndent();

                if (!documentEnd.IsImplicit) {
                    WriteIndicator("...", true, false, false);
                    WriteIndent();
                    isDocumentEndWritten = true;
                }

                state = EmitterState.DocumentStart;
                tagDirectives.Clear();
                return;
            }

            throw new YamlException("Expected DOCUMENT-END.");
        }

        void EmitFlowSequenceItem(ParsingEvent evt, bool isFirst) {
            if (isFirst) {
                WriteIndicator("[", true, true, false);
                IncreaseIndent(true, false);
                flowLevel++;
            }

            if (evt is SequenceEnd) {
                flowLevel--;
                indent = indents.Pop();

                if (isCanonical && !isFirst) {
                    WriteIndicator(",", false, false, false);
                    WriteIndent();
                }

                WriteIndicator("]", false, false, false);
                state = states.Pop();
            } else {
                if (!isFirst) {
                    WriteIndicator(",", false, false, false);
                }

                if (isCanonical || column > bestWidth) {
                    WriteIndent();
                }

                states.Push(EmitterState.FlowSequenceItem);
                EmitNode(evt, false, false, false);
            }
        }

        void EmitFlowMappingKey(ParsingEvent evt, bool isFirst) {
            if (isFirst) {
                WriteIndicator("{", true, true, false);
                IncreaseIndent(true, false);
                flowLevel++;
            }

            if (evt is MappingEnd) {
                flowLevel--;
                indent = indents.Pop();

                if (isCanonical && !isFirst) {
                    WriteIndicator(",", false, false, false);
                    WriteIndent();
                }

                WriteIndicator("}", false, false, false);
                state = states.Pop();
                return;
            }

            if (!isFirst) {
                WriteIndicator(",", false, false, false);
            }

            if (isCanonical || column > bestWidth) {
                WriteIndent();
            }

            if (!isCanonical && CheckSimpleKey()) {
                states.Push(EmitterState.FlowMappingSimpleValue);
                EmitNode(evt, false, true, true);
            } else {
                WriteIndicator("?", true, false, false);
                states.Push(EmitterState.FlowMappingValue);
                EmitNode(evt, false, true, false);
            }
        }

        void EmitFlowMappingValue(ParsingEvent evt, bool isSimple) {
            if (isSimple) {
                WriteIndicator(":", false, false, false);
            } else {
                if (isCanonical || column > bestWidth) {
                    WriteIndent();
                }

                WriteIndicator(":", true, false, false);
            }

            states.Push(EmitterState.FlowMappingKey);
            EmitNode(evt, false, true, false);
        }

        void EmitBlockSequenceItem(ParsingEvent evt, bool isFirst) {
            if (isFirst) {
                IncreaseIndent(false, isMappingContext && !isIndentation);
            }

            if (evt is SequenceEnd) {
                indent = indents.Pop();
                state = states.Pop();
                return;
            }

            WriteIndent();
            WriteIndicator("-", true, false, true);
            states.Push(EmitterState.BlockSequenceItem);
            EmitNode(evt, false, false, false);
        }

        void EmitBlockMappingKey(ParsingEvent evt, bool isFirst) {
            if (isFirst) {
                IncreaseIndent(false, false);
            }

            if (evt is MappingEnd) {
                indent = indents.Pop();
                state = states.Pop();
                return;
            }

            WriteIndent();

            if (CheckSimpleKey()) {
                states.Push(EmitterState.BlockMappingSimpleValue);
                EmitNode(evt, false, true, true);
            } else {
                WriteIndicator("?", true, false, true);
                states.Push(EmitterState.BlockMappingValue);
                EmitNode(evt, false, true, false);
            }
        }

        void EmitBlockMappingValue(ParsingEvent evt, bool isSimple) {
            if (isSimple) {
                WriteIndicator(":", false, false, false);
            } else {
                WriteIndent();
                WriteIndicator(":", true, false, true);
            }

            states.Push(EmitterState.BlockMappingKey);
            EmitNode(evt, false, true, false);
        }

        void IncreaseIndent(bool isFlow, bool isIndentless) {
            indents.Push(indent);

            if (indent < 0) {
                indent = isFlow ? bestIndent : 0;
            } else if (!isIndentless) {
                indent += bestIndent;
            }
        }

        bool CheckEmptyDocument() {
            int num = 0;

            foreach (ParsingEvent @event in events) {
                num++;

                if (num == 2) {
                    Scalar scalar = @event as Scalar;

                    if (scalar != null) {
                        return string.IsNullOrEmpty(scalar.Value);
                    }

                    break;
                }
            }

            return false;
        }

        bool CheckSimpleKey() {
            if (events.Count < 1) {
                return false;
            }

            int num;

            switch (events.Peek().Type) {
                case EventType.Alias:
                    num = SafeStringLength(anchorData.anchor);
                    break;

                case EventType.Scalar:
                    if (scalarData.isMultiline) {
                        return false;
                    }

                    num = SafeStringLength(anchorData.anchor) +
                          SafeStringLength(tagData.handle) +
                          SafeStringLength(tagData.suffix) +
                          SafeStringLength(scalarData.value);

                    break;

                case EventType.SequenceStart:
                    if (!CheckEmptySequence()) {
                        return false;
                    }

                    num = SafeStringLength(anchorData.anchor) +
                          SafeStringLength(tagData.handle) +
                          SafeStringLength(tagData.suffix);

                    break;

                case EventType.MappingStart:
                    if (!CheckEmptySequence()) {
                        return false;
                    }

                    num = SafeStringLength(anchorData.anchor) +
                          SafeStringLength(tagData.handle) +
                          SafeStringLength(tagData.suffix);

                    break;

                default:
                    return false;
            }

            return num <= 128;
        }

        int SafeStringLength(string value) => value != null ? value.Length : 0;

        bool CheckEmptySequence() {
            if (events.Count < 2) {
                return false;
            }

            FakeList<ParsingEvent> fakeList = new(events);
            return fakeList[0] is SequenceStart && fakeList[1] is SequenceEnd;
        }

        bool CheckEmptyMapping() {
            if (events.Count < 2) {
                return false;
            }

            FakeList<ParsingEvent> fakeList = new(events);
            return fakeList[0] is MappingStart && fakeList[1] is MappingEnd;
        }

        void WriteBlockScalarHints(string value) {
            CharacterAnalyzer<StringLookAheadBuffer> characterAnalyzer = new(new StringLookAheadBuffer(value));

            if (characterAnalyzer.IsSpace() || characterAnalyzer.IsBreak()) {
                string indicator = string.Format(CultureInfo.InvariantCulture, "{0}\0", bestIndent);
                WriteIndicator(indicator, false, false, false);
            }

            isOpenEnded = false;
            string text = null;

            if (value.Length == 0 || !characterAnalyzer.IsBreak(value.Length - 1)) {
                text = "-";
            } else if (value.Length >= 2 && characterAnalyzer.IsBreak(value.Length - 2)) {
                text = "+";
                isOpenEnded = true;
            }

            if (text != null) {
                WriteIndicator(text, false, false, false);
            }
        }

        void WriteIndicator(string indicator, bool needWhitespace, bool whitespace, bool indentation) {
            if (needWhitespace && !isWhitespace) {
                Write(' ');
            }

            Write(indicator);
            isWhitespace = whitespace;
            isIndentation &= indentation;
            isOpenEnded = false;
        }

        void WriteIndent() {
            int num = Math.Max(indent, 0);

            if (!isIndentation || column > num || column == num && !isWhitespace) {
                WriteBreak();
            }

            while (column < num) {
                Write(' ');
            }

            isWhitespace = true;
            isIndentation = true;
        }

        void WriteAnchor(string value) {
            Write(value);
            isWhitespace = false;
            isIndentation = false;
        }

        void WriteTagHandle(string value) {
            if (!isWhitespace) {
                Write(' ');
            }

            Write(value);
            isWhitespace = false;
            isIndentation = false;
        }

        void WriteTagContent(string value, bool needsWhitespace) {
            if (needsWhitespace && !isWhitespace) {
                Write(' ');
            }

            Write(UrlEncode(value));
            isWhitespace = false;
            isIndentation = false;
        }

        string UrlEncode(string text) => uriReplacer.Replace(text,
            delegate(Match match) {
                StringBuilder stringBuilder = new();
                byte[] bytes = Encoding.UTF8.GetBytes(match.Value);

                foreach (byte b in bytes) {
                    stringBuilder.AppendFormat("%{0:X02}", b);
                }

                return stringBuilder.ToString();
            });

        void Write(char value) {
            output.Write(value);
            column++;
        }

        void Write(string value) {
            output.Write(value);
            column += value.Length;
        }

        void WriteBreak() {
            output.WriteLine();
            column = 0;
        }

        class AnchorData {
            public string anchor;

            public bool isAlias;
        }

        class TagData {
            public string handle;

            public string suffix;
        }

        class ScalarData {
            public bool isBlockAllowed;

            public bool isBlockPlainAllowed;

            public bool isFlowPlainAllowed;

            public bool isMultiline;

            public bool isSingleQuotedAllowed;

            public ScalarStyle style;
            public string value;
        }
    }
}