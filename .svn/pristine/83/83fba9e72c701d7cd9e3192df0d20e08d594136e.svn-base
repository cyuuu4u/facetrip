using UnityEngine;

namespace xxdwunity.vo
{
    public class PathPoint : GpsMapCell, System.IEquatable<PathPoint>
    {
        public Vector3 Vertex { get; set; }

        public PathPoint() { }
        public PathPoint(GpsMapCell gmc, Vector3 vertex)
            : base(gmc)
        {
            this.Vertex = vertex;
        }

        public bool Equals(PathPoint other)
        {
            return CompareTo(other) == 0;
        }
    }
}
