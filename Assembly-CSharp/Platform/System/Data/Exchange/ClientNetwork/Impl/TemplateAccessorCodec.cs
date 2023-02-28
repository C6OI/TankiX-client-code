using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientProtocol.Impl;
using Platform.System.Data.Statics.ClientConfigurator.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class TemplateAccessorCodec : Codec {
        readonly TemplateRegistry templateRegistry;

        Codec longCodec;

        Codec stringCodec;

        public TemplateAccessorCodec(TemplateRegistry templateRegistry) => this.templateRegistry = templateRegistry;

        [Inject] public static ConfigurationService ConfigurationService { get; set; }

        public void Init(Protocol protocol) {
            longCodec = protocol.GetCodec(typeof(long));
            stringCodec = new OptionalCodec((NotOptionalCodec)protocol.GetCodec(typeof(string)));
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            throw new NotImplementedException();
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            long id = (long)longCodec.Decode(protocolBuffer);
            TemplateDescription templateInfo = templateRegistry.GetTemplateInfo(id);
            string configPath = (string)stringCodec.Decode(protocolBuffer);

            try {
                return new TemplateAccessor(templateInfo, configPath);
            } catch (Exception innerException) {
                throw new Exception("templateType = " + templateInfo, innerException);
            }
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance) {
            throw new NotImplementedException();
        }
    }
}