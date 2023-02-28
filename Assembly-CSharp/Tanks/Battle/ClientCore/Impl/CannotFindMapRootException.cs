using System;

namespace Tanks.Battle.ClientCore.Impl {
    public class CannotFindMapRootException : Exception {
        public CannotFindMapRootException(string mapname)
            : base(string.Format("mapname={0}", mapname)) { }
    }
}