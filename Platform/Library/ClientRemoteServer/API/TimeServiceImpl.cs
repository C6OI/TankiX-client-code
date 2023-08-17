using System;
using log4net;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Platform.Library.ClientRemoteServer.API {
    public class TimeServiceImpl : TimeService {
        public static float SMOOTH_RATE = 0.1f;

        public static long MAX_DELTA_DIFF_TO_SERVER = 5000L;

        readonly ILog log;

        long absDeltaDiffToServer;

        long deltaDiffToServer;

        long diffToServer;

        long initServerTime;

        float initUnityRealTime;

        bool serverTimeInited;

        float smoothBeginTime;

        long smoothedDiffToServer;

        bool smoothing;

        public TimeServiceImpl() => log = LoggerProvider.GetLogger(this);

        [Inject] public static UnityTime UnityTime { get; set; }

        public long DiffToServer {
            get {
                CheckInited();
                UpdateSmoothing();
                return smoothedDiffToServer;
            }
            set {
                CheckInited();
                log.InfoFormat("SetDiffToServer: {0}", value);
                smoothing = false;
                diffToServer = value;
            }
        }

        public void InitServerTime(long serverTime) {
            initServerTime = serverTime;
            initUnityRealTime = UnityTime.realtimeSinceStartup;
            diffToServer = initServerTime - (long)(initUnityRealTime * 1000f);
            serverTimeInited = true;

            log.InfoFormat("InitServerTime: serverTime={0} unityRealTime={1} diffToServer={2}",
                serverTime,
                initUnityRealTime,
                diffToServer);
        }

        public void SetDiffToServerWithSmoothing(long newDiffToServer) {
            UpdateSmoothing();
            deltaDiffToServer = newDiffToServer - smoothedDiffToServer;
            absDeltaDiffToServer = Math.Abs(deltaDiffToServer);

            if (absDeltaDiffToServer > MAX_DELTA_DIFF_TO_SERVER) {
                log.ErrorFormat("Delta too large: {0}", absDeltaDiffToServer);
                deltaDiffToServer = deltaDiffToServer <= 0 ? -MAX_DELTA_DIFF_TO_SERVER : MAX_DELTA_DIFF_TO_SERVER;
                absDeltaDiffToServer = MAX_DELTA_DIFF_TO_SERVER;
            }

            diffToServer = smoothedDiffToServer + deltaDiffToServer;
            log.InfoFormat("Begin smoothing: deltaDiffToServer={0} wasSmoothing={1}", deltaDiffToServer, smoothing);

            if (deltaDiffToServer != 0L) {
                smoothing = true;
                smoothBeginTime = UnityTime.realtimeSinceStartup;
            }
        }

        void CheckInited() {
            if (!serverTimeInited) {
                throw new Exception("Server time not inited");
            }
        }

        public void UpdateSmoothing() {
            if (smoothing) {
                float num = UnityTime.realtimeSinceStartup - smoothBeginTime;
                float num2 = absDeltaDiffToServer / 1000f / SMOOTH_RATE;

                if (num >= num2) {
                    log.InfoFormat("End smoothing: diffToServer={0}", diffToServer);
                    smoothedDiffToServer = diffToServer;
                    smoothing = false;
                } else {
                    float num3 = num / num2;
                    long num4 = diffToServer - deltaDiffToServer;
                    smoothedDiffToServer = num4 + (long)(num3 * deltaDiffToServer);
                }
            } else {
                smoothedDiffToServer = diffToServer;
            }
        }
    }
}