using System.IO;
using log4net.Util;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.API {
    public class BuildGuidConverter : PatternConverter {
        public const string KEY = "buildGUID";

        protected override void Convert(TextWriter writer, object state) {
            writer.Write(Application.buildGUID);
        }
    }
}