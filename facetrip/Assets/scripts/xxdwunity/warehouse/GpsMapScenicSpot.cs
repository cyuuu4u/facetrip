using UnityEngine;
using System;
using System.Collections.Generic;
using xxdwunity.util;

namespace xxdwunity.warehouse
{
    public class GpsMapScenicSpot : IComparable
    {
        private string id;
        public string Id
        {
            get
            {
                return this.id;
            }
        }

        private int[] entryCells;      // 入口格子编号，0-based
        public int[] EntryCells
        {
            get
            {
                return this.entryCells;
            }
        }

        public GpsMapScenicSpot(string id)
        {
            this.id = id;
            this.entryCells = null;
        }

        public GpsMapScenicSpot(string id, string entryCells)
        {
            this.id = id;
            string[] cells = entryCells.Split(new char[] { ',' });
            this.entryCells = new int[cells.Length];
            for (int i = 0; i < cells.Length; i++)
            {
                try
                {
                    this.entryCells[i] = int.Parse(cells[i]);
                }
                catch (System.Exception)
                {
                    this.entryCells[i] = 0;
                }
            }

        }

        public int CompareTo(object obj)
        {
            int result = -1;
            try
            {
                GpsMapScenicSpot ss = obj as GpsMapScenicSpot;
                if (ss != null)
                {
                    result = this.Id.CompareTo(ss.Id);
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
