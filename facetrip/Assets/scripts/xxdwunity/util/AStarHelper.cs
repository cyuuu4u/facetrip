using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace xxdwunity.util
{
    public class AStarHelper
    {
        public class SeekingNode : IComparable
        {
            public int ID { get; set; }                 // 节点唯一标识，可排序
            public float G { get; set; }                // 与起点之间距离的权值
            public float H { get; set; }                // 与终点之间距离的权值
            public float F { get { return G + H; } }    // 总权值
            public object Data { get; set; }            // 节点数据
            public object ExtraData { get; set; }       // 节点附加数据
            public SeekingNode Parent { get; set; }
            public SeekingNode(int id)
            {
                this.ID = id;
            }
            public SeekingNode(int id, float g, float h, SeekingNode parent, object data)
            {
                this.ID = id;
                this.G = g;
                this.H = h;
                this.Parent = parent;
                this.Data = data;
            }
            public SeekingNode(int id, float g, float h, SeekingNode parent, object data, object extraData)
            {
                this.ID = id;
                this.G = g;
                this.H = h;
                this.Parent = parent;
                this.Data = data;
                this.ExtraData = extraData;
            }
            public int CompareTo(object obj)
            {
                int result = -1;
                try
                {
                    SeekingNode sn = obj as SeekingNode;
                    if (sn != null)
                    {
                        result = this.ID - sn.ID;
                    }
                    else
                    {
                        result = this.ID - (int)obj;
                    }
                }
                catch (Exception ex)
                {
                    XxdwDebugger.Log(ex.Message);
                }
                return result;
            }
        };

        public delegate float H_FUNC(object me, object target);
        public H_FUNC hFunc;
        private List<SeekingNode> opening;
        private List<SeekingNode> closed;

        public AStarHelper(H_FUNC hFunc)
        {
            this.hFunc = hFunc;
            this.opening = new List<SeekingNode>();
            this.closed = new List<SeekingNode>();
        }

        public int OpeningLength
        {
            get { return this.opening.Count; }
        }

        public int ClosedLength
        {
            get { return this.closed.Count; }
        }

        public void Clear()
        {
            this.opening.Clear();
            this.closed.Clear();
        }

        public bool IsInOpening(SeekingNode sn)
        {
            return this.opening.BinarySearch(sn) >= 0;
        }

        public bool IsInOpening(int id)
        {
            return this.opening.BinarySearch(new SeekingNode(id)) >= 0;
        }

        // 如已在opening集中，取得其f值
        public SeekingNode FindInOpening(int id)
        {
            int index = this.opening.BinarySearch(new SeekingNode(id));
            if (index >= 0)
            {
                return this.opening[index];
            }
            return null;
        }

        public int InsertIntoOpening(SeekingNode sn)
        {
            int index = this.opening.BinarySearch(sn);
            if (index < 0)
            {
                this.opening.Insert(~index, sn);
                return ~index;
            }
            return -1;
        }

        public int RemoveFromOpening(SeekingNode sn)
        {
            int index = this.opening.BinarySearch(sn);
            if (index >= 0)
            {
                this.opening.RemoveAt(index);
            }
            return index;
        }

        public int RemoveFromOpening(int sub)
        {
            this.opening.RemoveAt(sub);
            return sub;
        }

        public bool IsInClosed(SeekingNode sn)
        {
            return this.closed.BinarySearch(sn) >= 0;
        }

        public bool IsInClosed(int id)
        {
            return this.closed.BinarySearch(new SeekingNode(id)) >= 0;
        }

        public int InsertIntoClosed(SeekingNode sn)
        {
            int index = this.closed.BinarySearch(sn);
            if (index < 0)
            {
                this.closed.Insert(~index, sn);
                return ~index;
            }
            return -1;
        }

        public int RemoveFromClosed(int sub)
        {
            this.closed.RemoveAt(sub);
            return sub;
        }

        public int RemoveFromClosed(SeekingNode sn)
        {
            int index = this.closed.BinarySearch(sn);
            if (index >= 0)
            {
                this.closed.RemoveAt(index);
            }
            return index;
        }

        public SeekingNode PopLowestWeightInOpening()
        {
            int index = 0;
            for (int i = 1; i < this.opening.Count; i++)
            {
                if (this.opening[i].F < this.opening[index].F)
                {
                    index = i;
                }
            }
            SeekingNode sn = this.opening[index];
            InsertIntoClosed(sn);
            RemoveFromOpening(index);
            return sn;
        }
    }
}
