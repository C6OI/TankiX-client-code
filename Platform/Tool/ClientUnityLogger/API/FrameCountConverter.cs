using System.IO;
using log4net.Util;
using UnityEngine;

namespace Platform.Tool.ClientUnityLogger.API {
    public class FrameCountConverter : PatternConverter {
        public const string KEY = "frameCount";

        protected override void Convert(TextWriter writer, object state) => writer.Write(Time.frameCount);
    }
}