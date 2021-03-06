using UnityEngine;
using System;
using System.Collections.Generic;
using xxdwunity.util;

namespace xxdwunity.warehouse
{
    public class GpsMapMonitorSpot : IComparable
    {
        private string id;              // 触发点编号
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        private string scenicSpotId;    // 对应的景点编号
        public string ScenicSpotId
        {
            get
            {
                return this.scenicSpotId;
            }
        }

        private int cell;               // 格子编号，0-based
        public int Cell
        {
            get
            {
                return this.cell;
            }
        }

        public GpsMapMonitorSpot(string id)
        {
            this.id = id;
        }

        public GpsMapMonitorSpot(string id, string scenicSpotId, int cell)
        {
            this.id = id;
            this.scenicSpotId = scenicSpotId;
            this.cell = cell;
        }

        public int CompareTo(object obj)
        {
            int result = -1;
            try
            {
                GpsMapMonitorSpot ms = obj as GpsMapMonitorSpot;
                if (ms != null)
                {
                    result = this.Id.CompareTo(ms.Id);
                }
                else
                {
                    result = this.Id.CompareTo(obj);
                }
            }
            catch (Exception ex)
            {
                XxdwDebugger.Log(ex.Message);
            }
            return result;
        }
    }
}
