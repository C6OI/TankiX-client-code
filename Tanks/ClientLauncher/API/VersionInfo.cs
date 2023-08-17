using System.IO;
using System.Xml;

namespace Tanks.ClientLauncher.API {
    public class VersionInfo {
        public string Version { get; set; }

        public string Executable { get; set; }

        public string DistributionURL { get; set; }

        public void Read(Stream stream) {
            using (XmlReader xmlReader = XmlReader.Create(stream)) {
                xmlReader.ReadToFollowing("version");
                Version = xmlReader.ReadElementContentAsString();
                xmlReader.ReadToFollowing("executable");
                Executable = xmlReader.ReadElementContentAsString();
                xmlReader.ReadToFollowing("distributeUrl");
                DistributionURL = xmlReader.ReadElementContentAsString();
            }
        }

        public void Write(Stream stream) {
            XmlWriterSettings xmlWriterSettings = new();
            xmlWriterSettings.Indent = true;
            XmlWriterSettings settings = xmlWriterSettings;

            using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings)) {
                xmlWriter.WriteStartElement("distribution");
                xmlWriter.WriteStartElement("version");
                xmlWriter.WriteString(Version);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("executable");
                xmlWriter.WriteString(Executable);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("distributeUrl");
                xmlWriter.WriteString(DistributionURL);
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
            }
        }
    }
}