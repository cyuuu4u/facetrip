using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace xxdwunity.vo
{
    public class ScenicSpot
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string AudioUrl { get; set; }
        private float[] audioSegments;
        public string AudioSegments
        {
            set
            {
                string[] segments = value.Split(';');
                this.audioSegments = new float[segments.Length];
                for (int i = 0; i < segments.Length; i++)
                {
                    try
                    {
                        this.audioSegments[i] = float.Parse(segments[i].Trim());
                    }
                    catch (System.Exception)
                    {
                        this.audioSegments[i] = 0;
                    }
                }
            }
        }
        public float[] AllAudioSegments
        {
            get
            {
                return audioSegments;
            }
        }
        public string PictureUrl { get; set; }
        public string StarLevel { get; set; }
        public string Kind { get; set; }
        public string KindName 
        {
            get {
                return GetKindName(this.Kind);
            }
        }

        public static string GetKindName(string kind)
        {
            string rs = kind;
            switch(kind)
            {
                case "MJ":
                    rs = "美景";
                    break;
                case "MS":
                    rs = "美食";
                    break;
                case "XX":
                    rs = "学习";
                    break;
                case "XZ":
                    rs = "行政";
                    break;
                case "JX":
                    rs = "教学";
                    break;
                case "YL":
                    rs = "娱乐";
                    break;
                case "SH":
                    rs = "生活";
                    break;
                case "YD":
                    rs = "运动";
                    break;
                case "ZS":
                    rs = "住宿";
                    break;
                case "QT":
                    rs = "其他";
                    break;
            }
            return rs;
        }
    }
}
