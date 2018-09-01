using UnityEngine;
using System;
using System.Collections.Generic;
using xxdwunity.util;
using xxdwunity.vo;

namespace xxdwunity.warehouse
{
    /// <summary>
    /// 以邻接表+边表表示的地图道路图
    /// </summary>
    public class MapPathGraph
    {
        public class Edge
        {
            public int Cell1 { get; set; }         // 顶点地图格子索引
            public int Cell2 { get; set; }         // 顶点地图格子索引,应保证Cell1 < Cell2
            public float Weight { get; set; }      // 权值
            public int ID                          // 标识
            {
                get
                {
                    return Cell1 << 16 + Cell2;
                }
            }
            private string identifier;
            public string Identifer                // 标识串 
            {
                get
                {
                    if (this.identifier == "")
                        return "" + Cell1 + " -- " + Cell2;
                    else
                        return identifier;
                }
                set
                {
                    this.identifier = value;
                }
            }
            public int[] Spots { get; set; }
            public Edge(int cell1, int cell2, float weight, int[] spots, string identifer)
            {
                this.Cell1 = cell1;
                this.Cell2 = cell2;
                this.Weight = weight;
                this.Spots = spots;
                this.Identifer = identifer;
            }

            public void ReverseSpots()
            {
                int temp;
                temp = this.Cell1;
                this.Cell1 = this.Cell2;
                this.Cell2 = temp;

                int n = this.Spots.Length;
                for (int i = 0; i < n / 2; i++)
                {
                    temp = this.Spots[i];
                    this.Spots[i] = this.Spots[n - 1 - i];
                    this.Spots[n - 1 - i] = temp;
                }
            }
        };

        public class EdgeNode         //边表结点
        {
            public int AdjacentVertex { get; set; } //邻接点域，存储该顶点对应的下标
            public Edge EdgeLink { get; set; }     //用于存储具体的边的引用
            public EdgeNode Next { get; set; }     //链域，指向下一个邻接点
            public EdgeNode() { this.Next = null; }
            public EdgeNode(int adjacentVertex, Edge edge)
            {
                this.AdjacentVertex = adjacentVertex;
                this.EdgeLink = edge;
                this.Next = null;
            }
        };

        public class VertexNode       //顶点表结构
        {
            public int Cell { get; set; }               //顶点地图格子索引
            public EdgeNode FirstEdge { get; set; }     //边表头指针, 边按逆时针排列
            public int Degree { get; set; }
            public VertexNode() { this.FirstEdge = null; }
            public VertexNode(int cell)
            {
                this.Cell = cell;
                this.FirstEdge = null;
                this.Degree = 0;
            }
            public VertexNode(int cell, EdgeNode firstEdge)
            {
                this.Cell = cell;
                this.FirstEdge = firstEdge;
                this.Degree = 0;
            }
            public VertexNode(VertexNode vn)
            {
                this.Cell = vn.Cell;
                this.FirstEdge = vn.FirstEdge;
                this.Degree = 0;
            }
        }

        public class NavigateVertexNode : VertexNode    // 导航路径中的路口
        {
            public EdgeNode SelectedEdge { get; set; }  // 所选的路(按走向，过路口后要走的边)
            public int SelectedEdgeIndex { get; set; }  // 所选的路在顶点的边表中的序号
            public int NextSelectedEdgeIndex { get; set; }  // 过路口后的路在顶点的边表中的序号
            public NavigateVertexNode(VertexNode vn)
                : base(vn)
            {
                this.SelectedEdge = null;
                this.SelectedEdgeIndex = -1;
                this.NextSelectedEdgeIndex = -1;
            }
        }

        public class WeightVertexNode : VertexNode, IComparable
        {
            public int Weight { get; set; }
            public WeightVertexNode(VertexNode vn, int weight)
                : base(vn)
            {
                this.Weight = weight;
            }

            public int CompareTo(object obj)
            {
                int result = -1;
                try
                {
                    WeightVertexNode wvn = obj as WeightVertexNode;
                    if (wvn != null)
                    {
                        result = this.Weight - wvn.Weight;
                        if (result == 0)
                        {
                            return this.Cell - wvn.Cell;
                        }
                    }
                    else
                    {
                        result = this.Weight - (int)obj;
                    }
                }
                catch (Exception ex)
                {
                    XxdwDebugger.Log(ex.Message);
                }
                return result;
            }
        }

        private List<Edge> edgeList;        // 边表
        public List<Edge> EdgeList
        {
            get
            {
                return this.edgeList;
            }
        }
        private List<VertexNode> vertexList;// 邻接表
        public List<VertexNode> VertexList
        {
            get
            {
                return this.vertexList;
            }
        }

        public int EdgesNum                 // 边数
        {
            get
            {
                return this.edgeList.Count;
            }
        }
        public int VertexesNum              // 顶点数
        {
            get
            {
                return this.vertexList.Count;
            }
        }


        public MapPathGraph()
        {
            this.edgeList = new List<Edge>();
            this.vertexList = new List<VertexNode>();
        }

        public int FindEdge(int cell1, int cell2)
        {
            if (cell1 > cell2)
            {
                int temp = cell1;
                cell1 = cell2;
                cell2 = temp;
            }
            for (int i = 0; i < this.edgeList.Count; i++)
            {
                if (this.edgeList[i].Cell1 == cell1 && this.edgeList[i].Cell2 == cell2)
                {
                    return i;
                }
            }
            return -1;
        }

        public int FindVertex(int cell)
        {
            for (int i = 0; i < this.vertexList.Count; i++)
            {
                if (this.vertexList[i].Cell == cell)
                {
                    return i;
                }
            }
            return -1;
        }

        public VertexNode GetVertex(int cell)
        {
            int index = FindVertex(cell);
            return index >= 0 ? this.vertexList[index] : null;
        }

        /*public bool RemoveEdge(Edge edge)
        {
            return RemoveEdge(edge.Cell1, edge.Cell2);
        }

        public bool RemoveEdge(int cell1, int cell2)
        {
            if (cell1 > cell2)
            {
                int temp = cell1;
                cell1 = cell2;
                cell2 = temp;
            }

            int edgeSub = FindEdge(cell1, cell2);
            if (edgeSub >= 0)
            {
                this.edgeList.RemoveAt(edgeSub);
            }
            else
            {
                XxdwDebugger.Log("No edge: " + cell1 + "," + cell2 + " in RemoveEdge.");
                return false;
            }

            int index1 = FindVertex(cell1);
            int index2 = FindVertex(cell2);
            if (index1 < 0 || index2 < 0)
            {
                XxdwDebugger.Log("No vertex for : " + cell1 + "," + cell2 + " in RemoveEdge.");
                return false;
            }

            RemoveEdgeNode(index1, index2);
            RemoveEdgeNode(index2, index1);

            VertexNode vn1 = this.vertexList[index1];
            VertexNode vn2 = this.vertexList[index2];
            if (vn1.FirstEdge == null)
            {
                this.vertexList.Remove(vn1);
            }
            if (vn2.FirstEdge == null)
            {
                this.vertexList.Remove(vn2);
            }

            TraverseAllAdjacents();
            return true;
        }*/

        public delegate int FuncCompareEdgeByRadian(Edge e1, Edge e2);
        public bool AddEdge(int cell1, int cell2, float weight, int[] spots, string identifer, FuncCompareEdgeByRadian compare)
        {
            if (cell1 > cell2)
            {
                int temp = cell1;
                cell1 = cell2;
                cell2 = temp;
            }
            if (FindEdge(cell1, cell2) >= 0)
            {
                XxdwDebugger.Log("The edge already exists.");
                return false;
            }
            Edge edge = new Edge(cell1, cell2, weight, spots, identifer);
            this.edgeList.Add(edge);

            int index1 = FindVertex(cell1);
            if (index1 < 0)
            {
                index1 = this.vertexList.Count;
                this.vertexList.Add(new VertexNode(cell1));
            }

            int index2 = FindVertex(cell2);
            if (index2 < 0)
            {
                index2 = this.vertexList.Count;
                this.vertexList.Add(new VertexNode(cell2));
            }

            EdgeNode edgeNode1 = new EdgeNode(index2, edge);
            InsertEdgeNode(index1, edgeNode1, compare);
            EdgeNode edgeNode2 = new EdgeNode(index1, edge);
            InsertEdgeNode(index2, edgeNode2, compare);

            return true;
        }

        public void TraverseAllAdjacents()
        {
            if (this.vertexList.Count == 0)
            {
                XxdwDebugger.Log("No vertex!");
                return;
            }

            foreach (VertexNode vn in this.vertexList)
            {
                string str = "Vertex: (" + (vn.Cell % 144) + "," + (vn.Cell / 144) + ") -> (";

                EdgeNode p = vn.FirstEdge;
                while (p != null)
                {
                    str += (this.vertexList[p.AdjacentVertex].Cell % 144) + "," + this.vertexList[p.AdjacentVertex].Cell / 144 + " -> ";
                    /*Edge edge = p.EdgeLink;
                    foreach (int e in edge.Spots)
                    {
                        XxdwDebugger.Log("" + e + ",");
                    }*/
                    p = p.Next;
                }

                XxdwDebugger.Log(str + "\n");
            }
        }

        // 按逆时针排序插入， compare(a, b) < 0 表示a -> b为逆时针
        private void InsertEdgeNode(int vertexIndex, EdgeNode edgeNode, FuncCompareEdgeByRadian compare)
        {
            EdgeNode p = this.vertexList[vertexIndex].FirstEdge, q = null;
            //string str = "compare: (" + (this.vertexList[vertexIndex].Cell % 144) + "," + (this.vertexList[vertexIndex].Cell / 144) + ") ->";
            //str += "(" + (this.vertexList[edgeNode.AdjacentVertex].Cell % 144) + "," + (this.vertexList[edgeNode.AdjacentVertex].Cell / 144) + ") with ";
            while (p != null)
            {
                int rs = compare(edgeNode.EdgeLink, p.EdgeLink);
                //str += "(" + (this.vertexList[p.AdjacentVertex].Cell % 144) + "," + (this.vertexList[p.AdjacentVertex].Cell / 144) + ")" + "rs=" + rs;
                if (rs > 0) break;

                q = p;
                p = p.Next;
            }
            //if (this.vertexList[vertexIndex].Cell == 11038)
            //{
            //    XxdwDebugger.Log(str);
            //}
            if (q == null)
            {
                this.vertexList[vertexIndex].FirstEdge = edgeNode;
            }
            else
            {
                q.Next = edgeNode;
            }
            edgeNode.Next = p;

            this.vertexList[vertexIndex].Degree++;
        }

        /// <summary>
        /// 获取边在顶点所有边中的序号
        /// </summary>
        /// <param name="vn">顶点</param>
        /// <param name="e">边</param>
        public int GetEdgeNumber(VertexNode vn, Edge e)
        {
            EdgeNode p = vn.FirstEdge;
            int which = 0;
            while (p != null && p.EdgeLink != e)
            {
                p = p.Next;
                which++;
            }
            return which;
        }

        // 返回被删除的边的引用
        /*private Edge RemoveEdgeNode(int vertexIndex, int adjacentIndex)
        {
            EdgeNode p = this.vertexList[vertexIndex].FirstEdge, q = null;
            while (p != null && p.AdjacentVertex != adjacentIndex)
            {
                q = p;
                p = p.Next;
            }

            if (p != null) // 找到
            {
                if (q == null)  // 首条边
                {
                    this.vertexList[vertexIndex].FirstEdge = p.Next;
                }
                else
                {
                    q.Next = p.Next;
                }

                return p.EdgeLink;
            }

            return null;
        }*/

        /// <summary>
        /// 采用A*算法寻最短路
        /// A*算法 (F=G+H)
        ///   1，把起始格添加到开启列表。
        ///   2，重复如下的工作：
        ///      a) 寻找开启列表中F值最低的格子。我们称它为当前格。
        ///      b) 把它切换到关闭列表。
        ///      c) 对相邻的格中的每一个
        ///          * 如果它不可通过或者已经在关闭列表中，略过它。反之如下。
        ///          * 如果它不在开启列表中，把它添加进去。把当前格作为这一格的父节点。记录这一格的F,G,和H值。
        ///          * 如果它已经在开启列表中，用G值为参考检查新的路径是否更好。更低的G值意味着更好的路径。
        ///          *     如果是这样，就把这一格的父节点改成当前格，并且重新计算这一格的G和F值。
        ///          *     如果要保持的开启列表按F值排序，改变之后可能需要重新对开启列表排序。
        ///      d) 停止，当
        ///          * 把目标格添加进了关闭列表，这时候路径被找到，或者
        ///          * 没有找到目标格，开启列表已经空了。这时候，路径不存在。
        ///   3.保存路径。从目标格开始，沿着每一格的父节点移动直到回到起始格。这就是找到的路径。
        /// </summary>
        /// <param name="vnFrom">起点</param>
        /// <param name="vnTo">终点</param>
        /// <param name="listNavigateVertexNode">顶点表示的路径,每个元素记录的是边对应结点的入边, "并未"设置出边的序号</param>
        /// <returns>路径的总权值, 负数表示没找到</returns>
        public float FindShortestPaths(AStarHelper ash, VertexNode vnFrom, VertexNode vnTo, List<NavigateVertexNode> listNavigateVertexNode)
        {
            listNavigateVertexNode.Clear();

            AStarHelper.SeekingNode me = new AStarHelper.SeekingNode(-1, 0.0f, 0.0f, null, vnFrom, null);
            ash.InsertIntoOpening(me);

            while (ash.OpeningLength > 0)
            {
                me = ash.PopLowestWeightInOpening();
                VertexNode vn = me.Data as VertexNode;
                if (vn.Cell == vnTo.Cell)
                    break; // 找到

                // 遍历邻接点
                EdgeNode p = vn.FirstEdge;
                int whichAdjacent = 0;
                while (p != null)
                {
                    if (ash.IsInClosed(p.EdgeLink.ID))
                    {
                        p = p.Next;
                        whichAdjacent++;
                        continue;
                    }

                    NavigateVertexNode nvn = new NavigateVertexNode(this.vertexList[p.AdjacentVertex] as VertexNode);
                    nvn.SelectedEdgeIndex = whichAdjacent;
                    nvn.SelectedEdge = p;
                    float h = ash.hFunc(this.vertexList[p.AdjacentVertex].Cell, vnTo.Cell);
                    //string info = string.Format("edge={0:d}<->{1:d}, g={2:f}, h={3:f}", 
                    //    vn.Cell, this.vertexList[p.AdjacentVertex].Cell, me.G + p.EdgeLink.Weight, h);
                    //XxdwDebugger.Log(info);
                    AStarHelper.SeekingNode next = new AStarHelper.SeekingNode(
                        p.EdgeLink.ID,
                        me.G + p.EdgeLink.Weight,
                        h,
                        me,
                        this.vertexList[p.AdjacentVertex],
                        nvn);
                    AStarHelper.SeekingNode pre = ash.FindInOpening(p.EdgeLink.ID);
                    if (pre == null)
                    {
                        ash.InsertIntoOpening(next);
                    }
                    else if (next.F < pre.F)
                    {
                        ash.RemoveFromOpening(pre);
                        ash.InsertIntoOpening(next);
                    }

                    p = p.Next;
                    whichAdjacent++;
                }
            } // while

            float weight = 0.0f;
            if ((me.Data as VertexNode).Cell == vnTo.Cell) // 找到
            {
                NavigateVertexNode nvn;

                AStarHelper.SeekingNode sn = me;
                while (sn != null && sn.ExtraData != null)
                {
                    nvn = sn.ExtraData as NavigateVertexNode;
                    listNavigateVertexNode.Insert(0, nvn);
                    weight += nvn.SelectedEdge.EdgeLink.Weight;
                    sn = sn.Parent;
                }
                listNavigateVertexNode.Insert(0, new NavigateVertexNode(vnFrom)); //加入开始节点

                return weight;
            }
            else
            {
                return -1.0f;
            }
        }

        /// <summary>
        /// 设置出边序号，调整边中格子的顺序
        /// </summary>
        /// <param name="listNavigateVertexNode">输入并输出参数, 顶点表示的路径,每个元素记录的是边对应结点的入边, 在函数内设置出边的序号</param>
        /// <param name="listEdge">输出参数，路径上的边，顶点数减一条，其中的各格子按路过的顺序排列</param>

        public void NormalizeVertexAndSpotsOrder(List<NavigateVertexNode> listNavigateVertexNode, List<Edge> listEdge)
        {
            listEdge.Clear();
            NavigateVertexNode nvn, nvnPrevious = listNavigateVertexNode[0];
            for (int i = 1; i < listNavigateVertexNode.Count; i++)
            {
                nvn = listNavigateVertexNode[i];
                if (nvn.SelectedEdge.EdgeLink.Cell1 == nvn.Cell) // 顺序相反, 反转
                {
                    nvn.SelectedEdge.EdgeLink.ReverseSpots();
                }
                listEdge.Add(nvn.SelectedEdge.EdgeLink);

                // 设置前一路口之----过路口之后的边在该路口边表中的位置
                nvnPrevious.NextSelectedEdgeIndex = this.GetEdgeNumber(nvnPrevious, nvn.SelectedEdge.EdgeLink);

                nvnPrevious = nvn;
            }
        }
    }
}
