using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public class SkipLog {
        readonly ICollection<Entity> contextEntities;

        readonly Handler handler;

        public SkipLog(ICollection<Entity> contextEntities, Handler handler) {
            this.contextEntities = contextEntities;
            this.handler = handler;
        }

        public override string ToString() {
            StringBuilder stringBuilder = new();
            PrintReason(stringBuilder);
            PrintContextEntities(stringBuilder);
            PrintStackTrace(stringBuilder);
            return stringBuilder.ToString();
        }

        void PrintReason(StringBuilder o) {
            NewLine(o);
            List<LogPart> list = new();
            ICollection<Entity> leftEntities = contextEntities;

            foreach (HandlerArgument handlerArgument in handler.HandlerArgumentsDescription.HandlerArguments) {
                leftEntities = PrintReasonForHandlerArgument(handlerArgument, leftEntities, list);
            }

            o.Append(BuildLog(list));
        }

        ICollection<Entity> PrintReasonForHandlerArgument(HandlerArgument handlerArgument, ICollection<Entity> leftEntities, ICollection<LogPart> parts) {
            if (handlerArgument.Context) {
                parts.Add(new HandlerArgumentLogPart(handlerArgument, contextEntities));

                if (handlerArgument.JoinType.IsPresent()) {
                    parts.Add(new CheckGroupComponentLogPart(handlerArgument, contextEntities));
                }
            } else if (handlerArgument.JoinType.IsPresent()) {
                if (handlerArgument.JoinType.Get() is JoinAllType) {
                    ICollection<Entity> entities = Flow.Current.NodeCollector.GetEntities(handlerArgument.NodeDescription);
                    parts.Add(new JoinAllLogPart(handlerArgument, entities));
                    return entities;
                }

                if (handlerArgument.JoinType.Get().ContextComponent.IsPresent()) {
                    Type groupComponent = handlerArgument.JoinType.Get().ContextComponent.Get();
                    ICollection<Entity> collection = leftEntities.Where(e => e.HasComponent(groupComponent)).ToList();
                    HashSet<Entity> hashSet = new();

                    foreach (Entity item in collection) {
                        foreach (Entity groupMember in ((GroupComponent)item.GetComponent(groupComponent)).GetGroupMembers(handlerArgument.NodeDescription)) {
                            hashSet.Add(groupMember);
                        }
                    }

                    parts.Add(new HandlerArgumentLogPart(handlerArgument, hashSet));
                    return hashSet;
                }
            }

            return leftEntities;
        }

        void PrintContextEntities(StringBuilder o) {
            o.Append("\tContext entities:");

            foreach (Entity contextEntity in contextEntities) {
                NewLine(o);
                o.Append("\t" + EcsToStringUtil.ToStringWithComponents((EntityInternal)contextEntity));
            }
        }

        void PrintStackTrace(StringBuilder o) {
            TextWriter textWriter = new StringWriter(o);
            textWriter.WriteLine("ECS Stack Trace");
        }

        string BuildLog(ICollection<LogPart> logParts) {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(GetLogHeader());

            foreach (LogPart logPart in logParts) {
                Optional<string> skipReason = logPart.GetSkipReason();

                if (skipReason.IsPresent()) {
                    stringBuilder.Append(skipReason.Get());
                    stringBuilder.Append("\n");
                }
            }

            return stringBuilder.ToString();
        }

        protected string GetLogHeader() => "\nSkipped: " + EcsToStringUtil.ToString(handler) + "\n";

        static void NewLine(StringBuilder o) {
            o.Append("\n\t");
        }
    }
}