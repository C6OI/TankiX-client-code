using System;

namespace Tanks.Battle.ClientCore.Impl {
    public class CannotFindHangarRootException : Exception {
        public CannotFindHangarRootException(string mapname)
            : base(string.Format("mapname={0}", mapname)) { }
    }
}