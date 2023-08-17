using System;
using System.IO;
using System.Xml.Serialization;

namespace Tanks.ClientLauncher.Impl {
    [Serializable]
    public class UpdateReport {
        public UpdateReport() {
            IsSuccess = true;
            Error = string.Empty;
            StackTrace = string.Empty;
            UpdateVersion = string.Empty;
        }

        public bool IsSuccess { get; set; }

        public string UpdateVersion { get; set; }

        public string Error { get; set; }

        public string StackTrace { get; set; }

        public void Write(Stream stream) {
            XmlSerializer xmlSerializer = new(typeof(UpdateReport));
            xmlSerializer.Serialize(stream, this);
        }

        public void Read(Stream stream) {
            XmlSerializer xmlSerializer = new(typeof(UpdateReport));
            UpdateReport updateReport = (UpdateReport)xmlSerializer.Deserialize(stream);
            UpdateVersion = updateReport.UpdateVersion;
            IsSuccess = updateReport.IsSuccess;
            Error = updateReport.Error;
            StackTrace = updateReport.StackTrace;
        }
    }
}