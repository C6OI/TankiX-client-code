using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core.Events;
using YamlDotNet.Core.Tokens;
using AnchorAlias = YamlDotNet.Core.Tokens.AnchorAlias;
using Comment = YamlDotNet.Core.Tokens.Comment;
using DocumentEnd = YamlDotNet.Core.Tokens.DocumentEnd;
using DocumentStart = YamlDotNet.Core.Tokens.DocumentStart;
using Scalar = YamlDotNet.Core.Events.Scalar;
using StreamEnd = YamlDotNet.Core.Tokens.StreamEnd;
using StreamStart = YamlDotNet.Core.Tokens.StreamStart;

namespace YamlDotNet.Core {
    public class Parser : IParser {
        readonly Queue<ParsingEvent> pendingEvents = new();

        readonly IScanner scanner;
        readonly Stack<ParserState> states = new();

        readonly TagDirectiveCollection tagDirectives = new();

        Token currentToken;

        ParserState state;

        public Parser(TextReader input)
            : this(new Scanner(input)) { }

        public Parser(IScanner scanner) => this.scanner = scanner;

        public ParsingEvent Current { get; private set; }

        public bool MoveNext() {
            if (state == ParserState.StreamEnd) {
                Current = null;
                return false;
            }

            if (pendingEvents.Count == 0) {
                pendingEvents.Enqueue(StateMachine());
            }

            Current = pendingEvents.Dequeue();
            return true;
        }

        Token GetCurrentToken() {
            if (currentToken == null) {
                while (scanner.MoveNextWithoutConsuming()) {
                    currentToken = scanner.Current;
                    Comment comment = currentToken as Comment;

                    if (comment != null) {
                        pendingEvents.Enqueue(
                            new Events.Comment(comment.Value, comment.IsInline, comment.Start, comment.End));

                        continue;
                    }

                    break;
                }
            }

            return currentToken;
        }

        ParsingEvent StateMachine() {
            switch (state) {
                case ParserState.StreamStart:
                    return ParseStreamStart();

                case ParserState.ImplicitDocumentStart:
                    return ParseDocumentStart(true);

                case ParserState.DocumentStart:
                    return ParseDocumentStart(false);

                case ParserState.DocumentContent:
                    return ParseDocumentContent();

                case ParserState.DocumentEnd:
                    return ParseDocumentEnd();

                case ParserState.BlockNode:
                    return ParseNode(true, false);

                case ParserState.BlockNodeOrIndentlessSequence:
                    return ParseNode(true, true);

                case ParserState.FlowNode:
                    return ParseNode(false, false);

                case ParserState.BlockSequenceFirstEntry:
                    return ParseBlockSequenceEntry(true);

                case ParserState.BlockSequenceEntry:
                    return ParseBlockSequenceEntry(false);

                case ParserState.IndentlessSequenceEntry:
                    return ParseIndentlessSequenceEntry();

                case ParserState.BlockMappingFirstKey:
                    return ParseBlockMappingKey(true);

                case ParserState.BlockMappingKey:
                    return ParseBlockMappingKey(false);

                case ParserState.BlockMappingValue:
                    return ParseBlockMappingValue();

                case ParserState.FlowSequenceFirstEntry:
                    return ParseFlowSequenceEntry(true);

                case ParserState.FlowSequenceEntry:
                    return ParseFlowSequenceEntry(false);

                case ParserState.FlowSequenceEntryMappingKey:
                    return ParseFlowSequenceEntryMappingKey();

                case ParserState.FlowSequenceEntryMappingValue:
                    return ParseFlowSequenceEntryMappingValue();

                case ParserState.FlowSequenceEntryMappingEnd:
                    return ParseFlowSequenceEntryMappingEnd();

                case ParserState.FlowMappingFirstKey:
                    return ParseFlowMappingKey(true);

                case ParserState.FlowMappingKey:
                    return ParseFlowMappingKey(false);

                case ParserState.FlowMappingValue:
                    return ParseFlowMappingValue(false);

                case ParserState.FlowMappingEmptyValue:
                    return ParseFlowMappingValue(true);

                default:
                    throw new InvalidOperationException();
            }
        }

        void Skip() {
            if (currentToken != null) {
                currentToken = null;
                scanner.ConsumeCurrent();
            }
        }

        ParsingEvent ParseStreamStart() {
            StreamStart streamStart = GetCurrentToken() as StreamStart;

            if (streamStart == null) {
                Token token = GetCurrentToken();
                throw new SemanticErrorException(token.Start, token.End, "Did not find expected <stream-start>.");
            }

            Skip();
            state = ParserState.ImplicitDocumentStart;
            return new Events.StreamStart(streamStart.Start, streamStart.End);
        }

        ParsingEvent ParseDocumentStart(bool isImplicit) {
            if (!isImplicit) {
                while (GetCurrentToken() is DocumentEnd) {
                    Skip();
                }
            }

            if (isImplicit &&
                !(GetCurrentToken() is VersionDirective) &&
                !(GetCurrentToken() is TagDirective) &&
                !(GetCurrentToken() is DocumentStart) &&
                !(GetCurrentToken() is StreamEnd)) {
                TagDirectiveCollection tags = new();
                ProcessDirectives(tags);
                states.Push(ParserState.DocumentEnd);
                state = ParserState.BlockNode;
                return new Events.DocumentStart(null, tags, true, GetCurrentToken().Start, GetCurrentToken().End);
            }

            if (!(GetCurrentToken() is StreamEnd)) {
                Mark start = GetCurrentToken().Start;
                TagDirectiveCollection tags2 = new();
                VersionDirective version = ProcessDirectives(tags2);
                Token token = GetCurrentToken();

                if (!(token is DocumentStart)) {
                    throw new SemanticErrorException(token.Start, token.End, "Did not find expected <document start>.");
                }

                states.Push(ParserState.DocumentEnd);
                state = ParserState.DocumentContent;
                ParsingEvent result = new Events.DocumentStart(version, tags2, false, start, token.End);
                Skip();
                return result;
            }

            state = ParserState.StreamEnd;
            ParsingEvent result2 = new Events.StreamEnd(GetCurrentToken().Start, GetCurrentToken().End);

            if (scanner.MoveNextWithoutConsuming()) {
                throw new InvalidOperationException("The scanner should contain no more tokens.");
            }

            return result2;
        }

        VersionDirective ProcessDirectives(TagDirectiveCollection tags) {
            VersionDirective versionDirective = null;
            bool flag = false;

            while (true) {
                VersionDirective versionDirective2;

                if ((versionDirective2 = GetCurrentToken() as VersionDirective) != null) {
                    if (versionDirective != null) {
                        throw new SemanticErrorException(versionDirective2.Start,
                            versionDirective2.End,
                            "Found duplicate %YAML directive.");
                    }

                    if (versionDirective2.Version.Major != 1 || versionDirective2.Version.Minor != 1) {
                        throw new SemanticErrorException(versionDirective2.Start,
                            versionDirective2.End,
                            "Found incompatible YAML document.");
                    }

                    versionDirective = versionDirective2;
                    flag = true;
                } else {
                    TagDirective tagDirective;

                    if ((tagDirective = GetCurrentToken() as TagDirective) == null) {
                        break;
                    }

                    if (tags.Contains(tagDirective.Handle)) {
                        throw new SemanticErrorException(tagDirective.Start,
                            tagDirective.End,
                            "Found duplicate %TAG directive.");
                    }

                    tags.Add(tagDirective);
                    flag = true;
                }

                Skip();
            }

            AddTagDirectives(tags, Constants.DefaultTagDirectives);

            if (flag) {
                tagDirectives.Clear();
            }

            AddTagDirectives(tagDirectives, tags);
            return versionDirective;
        }

        static void AddTagDirectives(TagDirectiveCollection directives, IEnumerable<TagDirective> source) {
            foreach (TagDirective item in source) {
                if (!directives.Contains(item)) {
                    directives.Add(item);
                }
            }
        }

        ParsingEvent ParseDocumentContent() {
            if (GetCurrentToken() is VersionDirective ||
                GetCurrentToken() is TagDirective ||
                GetCurrentToken() is DocumentStart ||
                GetCurrentToken() is DocumentEnd ||
                GetCurrentToken() is StreamEnd) {
                state = states.Pop();
                return ProcessEmptyScalar(scanner.CurrentPosition);
            }

            return ParseNode(true, false);
        }

        static ParsingEvent ProcessEmptyScalar(Mark position) => new Scalar(null,
            null,
            string.Empty,
            ScalarStyle.Plain,
            true,
            false,
            position,
            position);

        ParsingEvent ParseNode(bool isBlock, bool isIndentlessSequence) {
            AnchorAlias anchorAlias = GetCurrentToken() as AnchorAlias;

            if (anchorAlias != null) {
                state = states.Pop();
                ParsingEvent result = new Events.AnchorAlias(anchorAlias.Value, anchorAlias.Start, anchorAlias.End);
                Skip();
                return result;
            }

            Mark start = GetCurrentToken().Start;
            Anchor anchor = null;
            Tag tag = null;

            while (true) {
                if (anchor == null && (anchor = GetCurrentToken() as Anchor) != null) {
                    Skip();
                    continue;
                }

                if (tag == null && (tag = GetCurrentToken() as Tag) != null) {
                    Skip();
                    continue;
                }

                break;
            }

            string text = null;

            if (tag != null) {
                if (string.IsNullOrEmpty(tag.Handle)) {
                    text = tag.Suffix;
                } else {
                    if (!tagDirectives.Contains(tag.Handle)) {
                        throw new SemanticErrorException(tag.Start,
                            tag.End,
                            "While parsing a node, find undefined tag handle.");
                    }

                    text = tagDirectives[tag.Handle].Prefix + tag.Suffix;
                }
            }

            if (string.IsNullOrEmpty(text)) {
                text = null;
            }

            string text2 = anchor == null ? null : !string.IsNullOrEmpty(anchor.Value) ? anchor.Value : null;
            bool flag = string.IsNullOrEmpty(text);

            if (isIndentlessSequence && GetCurrentToken() is BlockEntry) {
                state = ParserState.IndentlessSequenceEntry;
                return new SequenceStart(text2, text, flag, SequenceStyle.Block, start, GetCurrentToken().End);
            }

            Tokens.Scalar scalar = GetCurrentToken() as Tokens.Scalar;

            if (scalar != null) {
                bool isPlainImplicit = false;
                bool isQuotedImplicit = false;

                if (scalar.Style == ScalarStyle.Plain && text == null || text == "!") {
                    isPlainImplicit = true;
                } else if (text == null) {
                    isQuotedImplicit = true;
                }

                state = states.Pop();

                ParsingEvent result2 = new Scalar(text2,
                    text,
                    scalar.Value,
                    scalar.Style,
                    isPlainImplicit,
                    isQuotedImplicit,
                    start,
                    scalar.End);

                Skip();
                return result2;
            }

            FlowSequenceStart flowSequenceStart = GetCurrentToken() as FlowSequenceStart;

            if (flowSequenceStart != null) {
                state = ParserState.FlowSequenceFirstEntry;
                return new SequenceStart(text2, text, flag, SequenceStyle.Flow, start, flowSequenceStart.End);
            }

            FlowMappingStart flowMappingStart = GetCurrentToken() as FlowMappingStart;

            if (flowMappingStart != null) {
                state = ParserState.FlowMappingFirstKey;
                return new MappingStart(text2, text, flag, MappingStyle.Flow, start, flowMappingStart.End);
            }

            if (isBlock) {
                BlockSequenceStart blockSequenceStart = GetCurrentToken() as BlockSequenceStart;

                if (blockSequenceStart != null) {
                    state = ParserState.BlockSequenceFirstEntry;
                    return new SequenceStart(text2, text, flag, SequenceStyle.Block, start, blockSequenceStart.End);
                }

                BlockMappingStart blockMappingStart = GetCurrentToken() as BlockMappingStart;

                if (blockMappingStart != null) {
                    state = ParserState.BlockMappingFirstKey;
                    return new MappingStart(text2, text, flag, MappingStyle.Block, start, GetCurrentToken().End);
                }
            }

            if (text2 != null || tag != null) {
                state = states.Pop();
                return new Scalar(text2, text, string.Empty, ScalarStyle.Plain, flag, false, start, GetCurrentToken().End);
            }

            Token token = GetCurrentToken();

            throw new SemanticErrorException(token.Start,
                token.End,
                "While parsing a node, did not find expected node content.");
        }

        ParsingEvent ParseDocumentEnd() {
            bool isImplicit = true;
            Mark start = GetCurrentToken().Start;
            Mark end = start;

            if (GetCurrentToken() is DocumentEnd) {
                end = GetCurrentToken().End;
                Skip();
                isImplicit = false;
            }

            state = ParserState.DocumentStart;
            return new Events.DocumentEnd(isImplicit, start, end);
        }

        ParsingEvent ParseBlockSequenceEntry(bool isFirst) {
            if (isFirst) {
                GetCurrentToken();
                Skip();
            }

            if (GetCurrentToken() is BlockEntry) {
                Mark end = GetCurrentToken().End;
                Skip();

                if (!(GetCurrentToken() is BlockEntry) && !(GetCurrentToken() is BlockEnd)) {
                    states.Push(ParserState.BlockSequenceEntry);
                    return ParseNode(true, false);
                }

                state = ParserState.BlockSequenceEntry;
                return ProcessEmptyScalar(end);
            }

            if (GetCurrentToken() is BlockEnd) {
                state = states.Pop();
                ParsingEvent result = new SequenceEnd(GetCurrentToken().Start, GetCurrentToken().End);
                Skip();
                return result;
            }

            Token token = GetCurrentToken();

            throw new SemanticErrorException(token.Start,
                token.End,
                "While parsing a block collection, did not find expected '-' indicator.");
        }

        ParsingEvent ParseIndentlessSequenceEntry() {
            if (GetCurrentToken() is BlockEntry) {
                Mark end = GetCurrentToken().End;
                Skip();

                if (!(GetCurrentToken() is BlockEntry) &&
                    !(GetCurrentToken() is Key) &&
                    !(GetCurrentToken() is Value) &&
                    !(GetCurrentToken() is BlockEnd)) {
                    states.Push(ParserState.IndentlessSequenceEntry);
                    return ParseNode(true, false);
                }

                state = ParserState.IndentlessSequenceEntry;
                return ProcessEmptyScalar(end);
            }

            state = states.Pop();
            return new SequenceEnd(GetCurrentToken().Start, GetCurrentToken().End);
        }

        ParsingEvent ParseBlockMappingKey(bool isFirst) {
            if (isFirst) {
                GetCurrentToken();
                Skip();
            }

            if (GetCurrentToken() is Key) {
                Mark end = GetCurrentToken().End;
                Skip();

                if (!(GetCurrentToken() is Key) && !(GetCurrentToken() is Value) && !(GetCurrentToken() is BlockEnd)) {
                    states.Push(ParserState.BlockMappingValue);
                    return ParseNode(true, true);
                }

                state = ParserState.BlockMappingValue;
                return ProcessEmptyScalar(end);
            }

            if (GetCurrentToken() is BlockEnd) {
                state = states.Pop();
                ParsingEvent result = new MappingEnd(GetCurrentToken().Start, GetCurrentToken().End);
                Skip();
                return result;
            }

            Token token = GetCurrentToken();

            throw new SemanticErrorException(token.Start,
                token.End,
                "While parsing a block mapping, did not find expected key.");
        }

        ParsingEvent ParseBlockMappingValue() {
            if (GetCurrentToken() is Value) {
                Mark end = GetCurrentToken().End;
                Skip();

                if (!(GetCurrentToken() is Key) && !(GetCurrentToken() is Value) && !(GetCurrentToken() is BlockEnd)) {
                    states.Push(ParserState.BlockMappingKey);
                    return ParseNode(true, true);
                }

                state = ParserState.BlockMappingKey;
                return ProcessEmptyScalar(end);
            }

            state = ParserState.BlockMappingKey;
            return ProcessEmptyScalar(GetCurrentToken().Start);
        }

        ParsingEvent ParseFlowSequenceEntry(bool isFirst) {
            if (isFirst) {
                GetCurrentToken();
                Skip();
            }

            ParsingEvent result;

            if (!(GetCurrentToken() is FlowSequenceEnd)) {
                if (!isFirst) {
                    if (!(GetCurrentToken() is FlowEntry)) {
                        Token token = GetCurrentToken();

                        throw new SemanticErrorException(token.Start,
                            token.End,
                            "While parsing a flow sequence, did not find expected ',' or ']'.");
                    }

                    Skip();
                }

                if (GetCurrentToken() is Key) {
                    state = ParserState.FlowSequenceEntryMappingKey;
                    result = new MappingStart(null, null, true, MappingStyle.Flow);
                    Skip();
                    return result;
                }

                if (!(GetCurrentToken() is FlowSequenceEnd)) {
                    states.Push(ParserState.FlowSequenceEntry);
                    return ParseNode(false, false);
                }
            }

            state = states.Pop();
            result = new SequenceEnd(GetCurrentToken().Start, GetCurrentToken().End);
            Skip();
            return result;
        }

        ParsingEvent ParseFlowSequenceEntryMappingKey() {
            if (!(GetCurrentToken() is Value) &&
                !(GetCurrentToken() is FlowEntry) &&
                !(GetCurrentToken() is FlowSequenceEnd)) {
                states.Push(ParserState.FlowSequenceEntryMappingValue);
                return ParseNode(false, false);
            }

            Mark end = GetCurrentToken().End;
            Skip();
            state = ParserState.FlowSequenceEntryMappingValue;
            return ProcessEmptyScalar(end);
        }

        ParsingEvent ParseFlowSequenceEntryMappingValue() {
            if (GetCurrentToken() is Value) {
                Skip();

                if (!(GetCurrentToken() is FlowEntry) && !(GetCurrentToken() is FlowSequenceEnd)) {
                    states.Push(ParserState.FlowSequenceEntryMappingEnd);
                    return ParseNode(false, false);
                }
            }

            state = ParserState.FlowSequenceEntryMappingEnd;
            return ProcessEmptyScalar(GetCurrentToken().Start);
        }

        ParsingEvent ParseFlowSequenceEntryMappingEnd() {
            state = ParserState.FlowSequenceEntry;
            return new MappingEnd(GetCurrentToken().Start, GetCurrentToken().End);
        }

        ParsingEvent ParseFlowMappingKey(bool isFirst) {
            if (isFirst) {
                GetCurrentToken();
                Skip();
            }

            if (!(GetCurrentToken() is FlowMappingEnd)) {
                if (!isFirst) {
                    if (!(GetCurrentToken() is FlowEntry)) {
                        Token token = GetCurrentToken();

                        throw new SemanticErrorException(token.Start,
                            token.End,
                            "While parsing a flow mapping,  did not find expected ',' or '}'.");
                    }

                    Skip();
                }

                if (GetCurrentToken() is Key) {
                    Skip();

                    if (!(GetCurrentToken() is Value) &&
                        !(GetCurrentToken() is FlowEntry) &&
                        !(GetCurrentToken() is FlowMappingEnd)) {
                        states.Push(ParserState.FlowMappingValue);
                        return ParseNode(false, false);
                    }

                    state = ParserState.FlowMappingValue;
                    return ProcessEmptyScalar(GetCurrentToken().Start);
                }

                if (!(GetCurrentToken() is FlowMappingEnd)) {
                    states.Push(ParserState.FlowMappingEmptyValue);
                    return ParseNode(false, false);
                }
            }

            state = states.Pop();
            ParsingEvent result = new MappingEnd(GetCurrentToken().Start, GetCurrentToken().End);
            Skip();
            return result;
        }

        ParsingEvent ParseFlowMappingValue(bool isEmpty) {
            if (isEmpty) {
                state = ParserState.FlowMappingKey;
                return ProcessEmptyScalar(GetCurrentToken().Start);
            }

            if (GetCurrentToken() is Value) {
                Skip();

                if (!(GetCurrentToken() is FlowEntry) && !(GetCurrentToken() is FlowMappingEnd)) {
                    states.Push(ParserState.FlowMappingKey);
                    return ParseNode(false, false);
                }
            }

            state = ParserState.FlowMappingKey;
            return ProcessEmptyScalar(GetCurrentToken().Start);
        }
    }
}