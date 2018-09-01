using System;

namespace xxdwunity.vo
{
    public class TriggerPoint : GpsMapCell, System.IEquatable<TriggerPoint>
    {
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Name { get; set; }

        public TriggerPoint() { }
        public TriggerPoint(GpsMapCell gmc, float longitude, float latitude, string name)
            : base(gmc)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Name = name;
        }

        public bool Equals(TriggerPoint other)
        {
            return CompareTo(other) == 0;
        }
    }
}
