using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System;
using System.Threading;
using xxdwunity.engine;
using xxdwunity.vo;
using xxdwunity.util;

namespace xxdwunity.warehouse
{
    public class GpsMap
    {
        public const int MIN_ZOOM_LEVEL = 0;
        public const int NORM_ZOOM_LEVEL = 1;
        public const int MAX_ZOOM_LEVEL = 15;
        public const int NONE_ZOOM_LEVEL = -1;

        private const int DEFAULT_GRID_CELL_PIXEL_NUM = 32;   // 缺省网格单元像素
        public const int UNIT_PIXEL_NUM = 100;

        private SafeStatus loadingStatus = new SafeStatus();
        public int LoadingStatus 
        {
            get {
                return this.loadingStatus.GetStatus();
            }
            set {
                this.loadingStatus.SetStatus(value);
            }
        }
        public bool IsInfoDataReady()
        {
            return this.loadingStatus.IsInfoDataReady();
        }
        public bool IsPathDataReady()
        {
            return this.loadingStatus.IsPathDataReady();
        }

        private Rect mapRange;
        public Rect MapRange
        {
            get
            {
                return this.mapRange;
            }
        }

        // 缩放正交尺寸
        public float GetZoomOrthographicSize(int zoomLevel)
        {
            float ratio = (float)zoomLevel / GpsMap.MAX_ZOOM_LEVEL;
            // 缩到最小时，高度恰一屏Screen.height; 放到最大时，恰为原始尺寸)Document.Instance.Map.PixelHeight
            return (float)Screen.height / UNIT_PIXEL_NUM / 2 + (this.MapRange.size.y / 2 - (float)Screen.height / 100 / 2) * ratio;
        }

        private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        private int sliceWidth;
        public int SliceWidth
        {
            get
            {
                if (this.sliceWidth == 0)
                {
                    this.sliceWidth = Parameters.SLICE_WIDTH;
                }
                return this.sliceWidth;
            }
        }

        private int sliceHeight;
        public int SliceHeight
        {
            get
            {
                if (this.sliceHeight == 0)
                {
                    this.sliceHeight = Parameters.SLICE_HEIGHT;
                }
                return this.sliceHeight;
            }
        }

        // 切片在世界坐标上的宽度
        public float WorldSliceWidth
        {
            get { return (float)this.SliceWidth / UNIT_PIXEL_NUM; }
        }
        // 切片在世界坐标上的高度
        public float WorldSliceHeight
        {
            get { return (float)this.SliceHeight / UNIT_PIXEL_NUM; }
        }

        public int SliceRowNum
        {
            get { return this.PixelHeight / this.SliceHeight; }
        }

        public int SliceColNum
        {
            get { return this.PixelWidth / this.SliceWidth; }
        }

        private int triggerDistance;
        public int TriggerDistance
        {
            get
            {
                if (this.triggerDistance == 0)
                {
                    this.triggerDistance = Parameters.GUIDE_VOICE_TRIGGER_DISTANCE;
                }
                return this.triggerDistance;
            }
            set
            {
                this.triggerDistance = value;
            }
        }

        private int pixelWidth;
        public int PixelWidth
        {
            get
            {
                return this.pixelWidth;
            }
        }

        private int pixelHeight;
        public int PixelHeight
        {
            get
            {
                return this.pixelHeight;
            }
        }

        private int gridCellPixelNum;
        public int GridCellPixelNum
        {
            get
            {
                return this.gridCellPixelNum;
            }
        }

        public int rolePositionIndex;
        public int RolePositionIndex
        {
            get
            {
                return this.rolePositionIndex;
            }
        }

        public Cell RolePosition
        {
            get
            {
                return new Cell(this.rolePositionIndex / this.ColNum, this.rolePositionIndex % this.ColNum);
            }
        }

        public float GridCellUnit
        {
            get
            {
                return (float)gridCellPixelNum / UNIT_PIXEL_NUM;
            }
        }

        private int rowNum;
        public int RowNum
        {
            get
            {
                return this.rowNum;
            }
        }

        private int colNum;
        public int ColNum
        {
            get
            {
                return this.colNum;
            }
        }

        public int CellNum
        {
            get
            {
                return this.rowNum * this.colNum;
            }
        }

        private float gridCellLongitudeDiff;
        public float GridCellLongitudeDiff
        {
            get
            {
                return this.gridCellLongitudeDiff;
            }
        }
        private float gridCellLatitudeDiff;
        public float GridCellLatitudeDiff
        {
            get
            {
                return this.gridCellLatitudeDiff;
            }
        }
        private float gridCellDistance;
        public float GridCellDistance
        {
            get
            {
                return this.gridCellDistance;
            }
        }
        private float gridCellDiagonalDistance;
        public float GridCellDiagonalDistance
        {
            get
            {
                return this.gridCellDiagonalDistance;
            }
        }
        private float zeroLongitude;
        public float ZeroLongitude
        {
            get
            {
                return this.zeroLongitude;
            }
        }
        private float zeroLatitude;
        public float ZeroLatitude
        {
            get
            {
                return this.zeroLatitude;
            }
        }
        private int baseIndex1;
        public int BaseIndex1
        {
            get
            {
                return this.baseIndex1;
            }
        }
        private int baseIndex2;
        public int BaseIndex2
        {
            get
            {
                return this.baseIndex2;
            }
        }
        private float basePoint1Longitude;
        public float BasePoint1Longitude
        {
            get
            {
                return this.basePoint1Longitude;
            }
        }
        private float basePoint1Latitude;
        public float BasePoint1Latitude
        {
            get
            {
                return this.basePoint1Latitude;
            }
        }
        private float basePoint2Longitude;
        public float BasePoint2Longitude
        {
            get
            {
                return this.basePoint2Longitude;
            }
        }
        private float basePoint2Latitude;
        public float BasePoint2Latitude
        {
            get
            {
                return this.basePoint2Latitude;
            }
        }


        private GpsMapCell[,] grid;

        public GpsMapCell GetCell(int index)
        {
            return GetCell(index / this.colNum, index % this.colNum);
        }

        public GpsMapCell GetCell(int row, int col)
        {
            if (row < 0 || col < 0 || row >= this.RowNum || col >= this.ColNum) return null;

            if (this.grid[row, col] == null)
            {
                this.grid[row, col] = new GpsMapCell(row * this.colNum + col, row, col, GpsMapCell.EType.EN_SELECTION, "");
            }
            return this.grid[row, col];
        }

        public GpsMapCell GetCell(Vector3 position)
        {
            return GetCell(new Vector2(position.x, position.y));
        }

        public GpsMapCell GetCell(Vector2 position)
        {
            // 映射到左上角（0,0）的像素坐标
            int x = (int)((position.x + this.mapRange.size.x / 2) * UNIT_PIXEL_NUM);
            int y = (int)(-(position.y - this.mapRange.size.y / 2) * UNIT_PIXEL_NUM);

            int index = (y / this.gridCellPixelNum) % this.rowNum * this.colNum + (x / this.gridCellPixelNum) % this.colNum;
            //XxdwDebugger.Log("world ("  + position.x + "," + position.y + ") = pixel (" + x + "," + y + ")" + " = index: " + index);
            return this.GetCell(index);
        }

        // 根据经纬度取格子
        public GpsMapCell GetCell(float longitude, float latitude)
        {
            if (latitude > this.basePoint1Latitude || longitude < this.basePoint1Longitude
                || latitude < this.basePoint2Latitude || longitude > this.basePoint2Longitude)
            {
                return null;
            }

            float x = longitude - this.zeroLongitude;
            float y = this.zeroLatitude - latitude;

            int c = (int)(x / this.gridCellLongitudeDiff);
            int r = (int)(y / this.gridCellLatitudeDiff);

            if (r >= this.rowNum || c >= this.colNum)
            {
                return null;
            }

            return this.GetCell(r, c);
        }

        public Vector3 GetCellPosition(GpsMapCell gmc)
        {
            return GetCellPosition(gmc.Index);
        }

        public Vector3 GetCellPosition(Cell cell)
        {
            return GetCellPosition(cell.Row, cell.Col);
        }

        public Vector3 GetCellPosition(int cellIndex)
        {
            int r = cellIndex / this.colNum;
            int c = cellIndex % this.colNum;
            return GetCellPosition(r, c);
        }

        public Vector3 GetCellPosition(int row, int col)
        {
            int x = col * this.gridCellPixelNum;
            int y = row * this.gridCellPixelNum;
            Vector3 position;
            position.x = (float)x / UNIT_PIXEL_NUM - this.mapRange.size.x / 2 + (float)this.gridCellPixelNum / UNIT_PIXEL_NUM / 2;
            position.y = -(float)y / UNIT_PIXEL_NUM + this.mapRange.size.y / 2 - (float)this.gridCellPixelNum / UNIT_PIXEL_NUM / 2;
            position.z = 0;
            return position;
        }

        // 通过世界坐标取其对应的地图分片所在行/列
        public Vector2 GetSliceRowCol(Vector3 position)
        {
            // 映射到左上角（0,0）的像素坐标
            int x = (int)((position.x + this.mapRange.size.x / 2) * UNIT_PIXEL_NUM);
            int y = (int)(-(position.y - this.mapRange.size.y / 2) * UNIT_PIXEL_NUM);

            Vector2 v;
            v.x = (y / this.SliceHeight) % this.SliceRowNum;
            v.y = (x / this.SliceWidth) % this.SliceColNum;
            return v;
        }

        /// <summary>
        /// 景点列表(含入口信息)
        /// </summary>
        private List<GpsMapScenicSpot> listScenicSpots;

        public GpsMapScenicSpot GetScenicSpot(string id)
        {
            int index = this.listScenicSpots.BinarySearch(new GpsMapScenicSpot(id));
            if (index >= 0)
            {
                return this.listScenicSpots[index];
            }
            return null;
        }

        /// <summary>
        /// 触发点列表(含所属景点和所处格子信息)
        /// </summary>
        private List<GpsMapMonitorSpot> listMonitorSpots;    // 导游解说触发点

        public List<GpsMapMonitorSpot> GetMonitorSpots()
        {
            return this.listMonitorSpots;
        }

        public GpsMapMonitorSpot FindMonitorSpotNearestTo(GpsMapCell gmc)
        {
            GpsMapMonitorSpot rs = null;

            int min = (int)Parameters.POSITIVE_INFINITE;
            foreach (GpsMapMonitorSpot gmms in this.listMonitorSpots)
            {
                int w = this.CalcSquareDistanceWeight(gmms.Cell, gmc.Index);
                if (w < min)
                {
                    min = w;
                    rs = gmms;
                }
            }

            return rs;
        }

        public GpsMapMonitorSpot GetMonitorSpot(string id)
        {
            int index = this.listMonitorSpots.BinarySearch(new GpsMapMonitorSpot(id));
            if (index >= 0)
            {
                return this.listMonitorSpots[index];
            }
            return null;
        }

        private MapPathGraph pathGraph;                 // 道路图    
        private string mapXmlText;                      // 地图数据XML文本
        Thread dataThread;                              // 声明线程

        /// <summary>
        /// 创建GpsMap.
        /// </summary>
        public GpsMap()
        {
            this.LoadingStatus = SafeStatus.LS_NULL;
            this.pixelWidth = 0;
            this.pixelHeight = 0;
            this.gridCellPixelNum = DEFAULT_GRID_CELL_PIXEL_NUM;
            this.rowNum = 0;
            this.colNum = 1;
            this.grid = null;
            this.pathGraph = new MapPathGraph();
            this.listScenicSpots = new List<GpsMapScenicSpot>();
            this.listMonitorSpots = new List<GpsMapMonitorSpot>();
        }

        public void LoadData(string mapXmlText, bool force = false)
        {
            if (this.LoadingStatus == SafeStatus.LS_NULL || (force && this.LoadingStatus == SafeStatus.LS_PATH_DATA))
            {
                this.LoadingStatus = SafeStatus.LS_NULL;

                this.mapXmlText = mapXmlText;
                this.dataThread = new Thread(new ThreadStart(DataThreadFunc));
                XxdwDebugger.Log("GpsMap: LoadData() -> start dataThread");
                this.dataThread.Start();
            }
            else
            {
                XxdwDebugger.Log("GpsMap: LoadData() do nothing in status : " + this.LoadingStatus);
            }
        }

        private void DataThreadFunc()
        {
            XxdwDebugger.Log("GpsMap: DataThreadFunc() entering...");
            try
            {
                this.LoadingStatus = SafeStatus.LS_NULL;

                int pos = this.mapXmlText.IndexOf("<cells>");
                string strMapData = this.mapXmlText.Substring(0, pos);
                strMapData += "</grid>";
                string strCellAndPathData = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<grid>\n";
                strCellAndPathData += this.mapXmlText.Substring(pos);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(new StringReader(strMapData));

                XmlNode node = xmlDoc.SelectSingleNode("/grid/mapName");
                this.name = (node == null ? "" : node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/sliceWidth");
                this.sliceWidth = (node == null ? 0 : int.Parse(node.InnerText.Trim()));
                node = xmlDoc.SelectSingleNode("/grid/sliceHeight");
                this.sliceHeight = (node == null ? 0 : int.Parse(node.InnerText.Trim()));
                node = xmlDoc.SelectSingleNode("/grid/triggerDistance");
                this.triggerDistance = (node == null ? 0 : int.Parse(node.InnerText.Trim()));

                node = xmlDoc.SelectSingleNode("/grid/pixelWidth");
                this.pixelWidth = int.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/pixelHeight");
                this.pixelHeight = int.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/gridCellPixelNum");
                this.gridCellPixelNum = int.Parse(node.InnerText.Trim());
                this.rowNum = this.pixelHeight / this.gridCellPixelNum;
                this.colNum = this.pixelWidth / this.gridCellPixelNum;

                this.mapRange = new Rect(-(float)this.PixelWidth / 2 / UNIT_PIXEL_NUM,
                                        -(float)this.pixelHeight / 2 / UNIT_PIXEL_NUM,
                                        (float)this.PixelWidth / UNIT_PIXEL_NUM,
                                        (float)this.PixelHeight / UNIT_PIXEL_NUM);
                XxdwDebugger.Log("rowNum=" + this.rowNum + "colNum=" + this.colNum 
                    + " mapRange:(" + this.mapRange.xMin + "," + this.mapRange.yMin + "),(" + this.mapRange.xMax + "," + this.mapRange.yMax + ")");

                node = xmlDoc.SelectSingleNode("/grid/rolePositionIndex");
                if (node != null)
                {
                    this.rolePositionIndex = int.Parse(node.InnerText.Trim());
                }

                node = xmlDoc.SelectSingleNode("/grid/gridCellLongitudeDiff");
                this.gridCellLongitudeDiff = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/gridCellLatitudeDiff");
                this.gridCellLatitudeDiff = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/gridCellDistance");
                this.gridCellDistance = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/gridCellDiagonalDistance");
                this.gridCellDiagonalDistance = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/zeroLongitude");
                this.zeroLongitude = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/zeroLatitude");
                this.zeroLatitude = float.Parse(node.InnerText.Trim());

                node = xmlDoc.SelectSingleNode("/grid/baseIndex1");
                this.baseIndex1 = int.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/baseIndex2");
                this.baseIndex2 = int.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/basePoint1Longitude");
                this.basePoint1Longitude = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/basePoint1Latitude");
                this.basePoint1Latitude = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/basePoint2Longitude");
                this.basePoint2Longitude = float.Parse(node.InnerText.Trim());
                node = xmlDoc.SelectSingleNode("/grid/basePoint2Latitude");
                this.basePoint2Latitude = float.Parse(node.InnerText.Trim());

                this.LoadingStatus |= SafeStatus.LS_INFO_DATA;
                XxdwDebugger.Log("GpsMap: DataThreadFunc(), post NOTI_MAP_INFO_READY");
                AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_INFO_READY, this.LoadingStatus, this.name);

                //--------------------------------------------------------------------------------------
                xmlDoc.Load(new StringReader(strCellAndPathData));

                // read cells of the grid
                this.grid = new GpsMapCell[this.rowNum, this.colNum];

                string xmlPathPattern = "/grid/cells/r";
                XmlNodeList rows = xmlDoc.SelectNodes(xmlPathPattern);
                if (rows != null && rows.Count > 0)
                {
                    foreach (XmlNode xn in rows)
                    {
                        int i = int.Parse(xn.Attributes["v"].InnerText);
                        XmlNodeList cols = xn.SelectNodes("./c");
                        foreach (XmlNode cell in cols)
                        {
                            int j = int.Parse(cell.Attributes["v"].InnerText);
                            if (!(i >= 0 && i < this.rowNum && j >= 0 && j < this.colNum)) continue;

                            node = cell.SelectSingleNode("./t");
                            int type = int.Parse(node.InnerText.Trim());

                            node = cell.SelectSingleNode("./d");
                            string data = (node == null ? "" : node.InnerText.Trim());

                            int entryNumber = 0;
                            if ((type & (int)GpsMapCell.EType.EN_CROSS) != 0)
                            {
                                node = cell.SelectSingleNode("./n");
                                if (node != null)
                                {
                                    entryNumber = int.Parse(node.InnerText.Trim());
                                }
                            }

                            this.grid[i, j] = new GpsMapCell(i * this.colNum + j, i, j, (GpsMapCell.EType)type, data, entryNumber);

                            //---------------------------------------------------------------------------------
                            // 触发点，以触发点编号排序插入
                            if ((type & (int)GpsMapCell.EType.EN_MONITOR_POINT) != 0)
                            {
                                node = cell.SelectSingleNode("./d2");
                                if (node != null)
                                {
                                    string data2 = node.InnerText.Trim();
                                    string[] ssid_number = data2.Split(new char[] { '_' });
                                    if (ssid_number.Length == 2)
                                    {
                                        GpsMapMonitorSpot ms = new GpsMapMonitorSpot(data2, ssid_number[0], i * this.colNum + j);
                                        int index = this.listMonitorSpots.BinarySearch(ms);
                                        if (index < 0)
                                        {
                                            this.listMonitorSpots.Insert(~index, ms);
                                        }
                                    }
                                    else
                                    {
                                        XxdwDebugger.LogWarning("Invalid monitor spot data at: " + (i * this.colNum + j));
                                    }
                                }
                                else
                                {
                                    XxdwDebugger.LogWarning("Monitor spot without data at: " + (i * this.colNum + j));
                                }
                            }
                        }
                    }
                }

                this.LoadingStatus |= SafeStatus.LS_CELL_DATA;
                XxdwDebugger.Log("GpsMap: DataThreadFunc(), post NOTI_MAP_DATA_READY cell_data ok");
                AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_DATA_READY, this.LoadingStatus, this.name);

                //--------------------------------------------------------------------------------------
                // read the road data of the grid, which will shape a Data Structure of Graph.
                xmlPathPattern = "/grid/edges/e";
                XmlNodeList edges = xmlDoc.SelectNodes(xmlPathPattern);

                if (edges != null && edges.Count > 0)
                {
                    for (int i = 0; i < edges.Count; i++)
                    {
                        node = edges[i].SelectSingleNode("./cl1");
                        int cell1 = int.Parse(node.InnerText.Trim());
                        node = edges[i].SelectSingleNode("./cl2");
                        int cell2 = int.Parse(node.InnerText.Trim());
                        node = edges[i].SelectSingleNode("./w");
                        float weight = float.Parse(node.InnerText.Trim());
                        node = edges[i].SelectSingleNode("./ir");
                        string identifier = node.InnerText.Trim();
                        node = edges[i].SelectSingleNode("./s");
                        string strSpots = node.InnerText.Trim();
                        string[] sSpots = strSpots.Split(new char[] { ',' });
                        if (sSpots.Length == 0) continue;
                        int[] spots = new int[sSpots.Length];
                        for (int j = 0; j < sSpots.Length; j++)
                        {
                            spots[j] = int.Parse(sSpots[j]);
                        }

                        this.pathGraph.AddEdge(cell1, cell2, weight, spots, identifier, CompareEdgeByRadian);
                    }
                }

                //--------------------------------------------------------------------------------------
                // read the scenic spot data of the grid
                xmlPathPattern = "/grid/scenic_spot/ss";
                XmlNodeList scenicSpots = xmlDoc.SelectNodes(xmlPathPattern);
                this.listScenicSpots.Clear();
                if (scenicSpots != null)
                {
                    foreach (XmlNode xn in scenicSpots)
                    {
                        node = xn.SelectSingleNode("./ir");
                        string identifier = node.InnerText.Trim();
                        node = xn.SelectSingleNode("./en");
                        string cells = node.InnerText.Trim();

                        GpsMapScenicSpot ss = new GpsMapScenicSpot(identifier, cells);
                        int index = listScenicSpots.BinarySearch(ss);
                        if (index < 0)
                        {
                            this.listScenicSpots.Insert(~index, ss);
                        }
                    }
                }

                //------------------------------------------------------
                this.LoadingStatus |= SafeStatus.LS_PATH_DATA;
                XxdwDebugger.Log("GpsMap: DataThreadFunc(), post NOTI_MAP_DATA_READY path_data ok");
                AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_DATA_READY, this.LoadingStatus, this.name);
            }
            catch (System.Exception ex)
            {
                XxdwDebugger.LogError(ex.Message);
                this.LoadingStatus = SafeStatus.LS_NULL;
            }

            //this.pathGraph.TraverseAllAdjacents();
            XxdwDebugger.Log("GpsMap: DataThreadFunc() leaving...");
        }

        // e1,e2在屏幕坐标系统下，返回值小于0表示e1 -> e2为逆时针
        private int CompareEdgeByRadian(MapPathGraph.Edge e1, MapPathGraph.Edge e2)
        {
            int v, v1, v2;
            if (e1.Cell1 == e2.Cell1)
            {
                v = e1.Cell1;
                v1 = e1.Cell2;
                v2 = e2.Cell2;
            }
            else if (e1.Cell1 == e2.Cell2)
            {
                v = e1.Cell1;
                v1 = e1.Cell2;
                v2 = e2.Cell1;
            }
            else if (e1.Cell2 == e2.Cell1)
            {
                v = e1.Cell2;
                v1 = e1.Cell1;
                v2 = e2.Cell2;
            }
            else if (e1.Cell2 == e2.Cell2)
            {
                v = e1.Cell2;
                v1 = e1.Cell1;
                v2 = e2.Cell1;
            }
            else
            {
                throw new Exception("无相同的顶点");
            }

            int x = v % this.colNum;
            int y = v / this.colNum;
            int x1 = v1 % this.colNum;
            int y1 = v1 / this.colNum;
            int x2 = v2 % this.colNum;
            int y2 = v2 / this.colNum;
            Vector3 ve = new Vector3(x, y, 0);
            Vector3 ve1 = new Vector3(x1, y1, 0);
            Vector3 ve2 = new Vector3(x2, y2, 0);
            Vector3 vs = Vector3.Cross((ve1 - ve).normalized, (ve2 - ve).normalized);

            return vs.z > 0 ? 1 : -1;
        }

        // 找与建筑相连的路口
        public GpsMapCell FindCrossCellOfBuilding(GpsMapCell gmcBuilding)
        {
            List<GpsMapCell> listGmc = new List<GpsMapCell>();
            List<GpsMapCell> listGmcClosed = new List<GpsMapCell>();
            listGmc.Add(gmcBuilding);

            while (listGmc.Count > 0)
            {
                GpsMapCell gmc = listGmc[0];
                listGmc.RemoveAt(0);
                int index = listGmcClosed.BinarySearch(gmc);
                if (index < 0)
                {
                    listGmcClosed.Insert(~index, gmc);
                }

                for (int n = 0; n < (int)Parameters.EDirection.DIR_NUM; n++)
                {
                    GpsMapCell neighbor = Neighbor(gmc, (Parameters.EDirection)n);
                    if (neighbor != null)
                    {
                        if (neighbor.IsCross())
                        {
                            return neighbor;
                        }
                        else if (neighbor.IsBuilding()
                            && listGmcClosed.BinarySearch(neighbor) < 0 && !listGmc.Contains(neighbor))
                        {
                            listGmc.Add(neighbor);
                        }

                        if (Math.Abs(neighbor.Row - gmcBuilding.Row) > Parameters.MAX_SEARCH_CELL_RADIUS
                            || Math.Abs(neighbor.Col - gmcBuilding.Col) > Parameters.MAX_SEARCH_CELL_RADIUS) break;
                    }
                }
            }

            return null;
        }

        public bool IsNeighbor(GpsMapCell gmc, int cellIndex, int distance = 1)
        {
            bool rs = false;

            for (int d = 0; d < distance; d++)
            {
                for (int n = 0; n < (int)Parameters.EDirection.DIR_NUM; n++)
                {
                    GpsMapCell neighbor = Neighbor(gmc, (Parameters.EDirection)n, d);
                    if (neighbor != null && neighbor.Index == cellIndex)
                    {
                        rs = true;
                        goto LBL_END;
                    }
                }
            }

        LBL_END:
            return rs;
        }

        private GpsMapCell Neighbor(GpsMapCell gmc, Parameters.EDirection dir, int distance = 1)
        {
            int r = gmc.Row, c = gmc.Col;
            switch (dir)
            {
                case Parameters.EDirection.DIR_R:
                    if (c < this.colNum - distance)
                    {
                        c += distance;
                    }
                    break;
                case Parameters.EDirection.DIR_RD:
                    if (r < this.rowNum - distance && c < this.colNum - distance)
                    {
                        r += distance;
                        c += distance;
                    }
                    break;
                case Parameters.EDirection.DIR_D:
                    if (r < this.rowNum - distance)
                    {
                        r += distance;
                    }
                    break;
                case Parameters.EDirection.DIR_LD:
                    if (r < this.rowNum - distance && c > distance - 1)
                    {
                        r += distance;
                        c -= distance;
                    }
                    break;
                case Parameters.EDirection.DIR_L:
                    if (c > distance - 1)
                    {
                        c -= distance;
                    }
                    break;
                case Parameters.EDirection.DIR_LU:
                    if (r > distance - 1 && c > distance - 1)
                    {
                        r -= distance;
                        c -= distance;
                    }
                    break;
                case Parameters.EDirection.DIR_U:
                    if (r > distance - 1)
                    {
                        r -= distance;
                    }
                    break;
                case Parameters.EDirection.DIR_RU:
                    if (r > distance - 1 && c < this.colNum - distance)
                    {
                        r -= distance;
                        c += distance;
                    }
                    break;
                default:
                    break;
            }

            if (r != gmc.Row || c != gmc.Col)
            {
                //return this.grid[r, c];
                return this.GetCell(r, c);
            }

            return null;
        }

        // 输出的顶点数和边数相同
        public bool FindCell2CrossPath(GpsMapCell from, GpsMapCell cross,
            out List<MapPathGraph.NavigateVertexNode> listNavigateVertexNode, out List<MapPathGraph.Edge> listEdge)
        {
            listNavigateVertexNode = null;
            listEdge = null;

            from = FindNearestCellOf(from, GpsMapCell.EType.EN_ROAD);
            if (from == null)
            {
                XxdwDebugger.Log("Road nearby not found. Strange!");
                return false;
            }

            int crossVertexIndex = this.pathGraph.FindVertex(cross.Index);
            if (crossVertexIndex < 0)
            {
                XxdwDebugger.Log("Target cross not found. Strange!");
                return false;
            }
            MapPathGraph.VertexNode vnTo = this.pathGraph.VertexList[crossVertexIndex];

            MapPathGraph.VertexNode vn1, vn2;
            MapPathGraph.Edge e;
            int spotIndex = FindEdgeOfRoadCell(from.Index, out vn1, out vn2, out e);
            if (spotIndex < 0)
            {
                XxdwDebugger.Log("Cross of road not found. Strange!");
                return false;
            }
            float w1 = CalcDetailWeight(e.Spots, 0, spotIndex + 1);
            float w2 = CalcDetailWeight(e.Spots, spotIndex, e.Spots.Length);
            int[] spots1 = new int[spotIndex + 1];
            Array.Copy(e.Spots, 0, spots1, 0, spotIndex + 1);
            int[] spots2 = new int[e.Spots.Length - spotIndex];
            Array.Copy(e.Spots, spotIndex, spots2, 0, e.Spots.Length - spotIndex);

            MapPathGraph.Edge e1, e2;
            if (e.Spots[0] == vn1.Cell)
            {
                e1 = new MapPathGraph.Edge(vn1.Cell, e.Spots[spotIndex], w1, spots1, "");
                e1.ReverseSpots();
                e2 = new MapPathGraph.Edge(vn2.Cell, e.Spots[spotIndex], w2, spots2, "");
            }
            else
            {
                e1 = new MapPathGraph.Edge(vn1.Cell, e.Spots[spotIndex], w2, spots2, "");
                e2 = new MapPathGraph.Edge(vn2.Cell, e.Spots[spotIndex], w1, spots1, "");
                e2.ReverseSpots();
            }


            List<MapPathGraph.NavigateVertexNode> listNavigateVertexNode1 = new List<MapPathGraph.NavigateVertexNode>();
            List<MapPathGraph.NavigateVertexNode> listNavigateVertexNode2 = new List<MapPathGraph.NavigateVertexNode>();
            AStarHelper ash = new AStarHelper(this.CalcDistanceWeight);
            ash.Clear();
            float weight1 = this.pathGraph.FindShortestPaths(ash, vn1, vnTo, listNavigateVertexNode1);
            weight1 += weight1 >= 0 ? w1 : Parameters.MAX_EDGE_WEIGHT;
            ash.Clear();
            float weight2 = this.pathGraph.FindShortestPaths(ash, vn2, vnTo, listNavigateVertexNode2);
            weight2 += weight2 >= 0 ? w2 : Parameters.MAX_EDGE_WEIGHT;
            if (listNavigateVertexNode1.Count == 0 && listNavigateVertexNode2.Count == 0) return false;
            
            listEdge = new List<MapPathGraph.Edge>();
            if (weight1 < weight2)
            {
                listNavigateVertexNode1[0].SelectedEdgeIndex = this.pathGraph.GetEdgeNumber(vn1, e);
                listNavigateVertexNode = listNavigateVertexNode1;
                this.pathGraph.NormalizeVertexAndSpotsOrder(listNavigateVertexNode, listEdge);
                listEdge.Insert(0, e1);
            }
            else
            {
                listNavigateVertexNode2[0].SelectedEdgeIndex = this.pathGraph.GetEdgeNumber(vn2, e);
                listNavigateVertexNode = listNavigateVertexNode2;
                this.pathGraph.NormalizeVertexAndSpotsOrder(listNavigateVertexNode, listEdge);
                listEdge.Insert(0, e2);
            }

            return true;
        }

        // 输出的顶点数和边数相同
        public bool FindCross2CrossPaths(GpsMapCell fromCross, GpsMapCell toCross,
            out List<MapPathGraph.NavigateVertexNode> listNavigateVertexNode, out List<MapPathGraph.Edge> listEdge, AStarHelper ash = null)
        {
            if (ash == null)
            {
                ash = new AStarHelper(this.CalcDistanceWeight);
            }
            ash.Clear();

            listNavigateVertexNode = new List<MapPathGraph.NavigateVertexNode>();
            listEdge = new List<MapPathGraph.Edge>();
            float weight = this.pathGraph.FindShortestPaths(ash,
                this.pathGraph.GetVertex(fromCross.Index),
                this.pathGraph.GetVertex(toCross.Index),
                listNavigateVertexNode);
            this.pathGraph.NormalizeVertexAndSpotsOrder(listNavigateVertexNode, listEdge);

            listNavigateVertexNode.RemoveAt(0); // 去掉首结点，因其不指导导航路径，与FindCell2CrossPath一致
            return weight >= 0;
        }

        private GpsMapCell FindNearestCellOf(GpsMapCell gmc, GpsMapCell.EType type)
        {
            if (gmc.IsTypeOf(type)) return gmc;
            for (int k = 1; k <= Parameters.MAX_SEARCH_CELL_RADIUS; k++)
            {
                for (int n = 0; n < (int)Parameters.EDirection.DIR_NUM; n++)
                {
                    GpsMapCell neighbor = Neighbor(gmc, (Parameters.EDirection)n, k);
                    if (neighbor != null && neighbor.IsTypeOf(type))
                    {
                        return neighbor;
                    }
                }
            }
            return null;
        }

        private float CalcDistanceWeight(object cell1Index, object cell2Index)
        {
            return (float)Math.Sqrt(CalcSquareDistanceWeight((int)cell1Index, (int)cell2Index));
        }

        public int CalcSquareDistanceWeight(int cell1Index, int cell2Index)
        {
            int r1 = cell1Index / this.colNum;
            int c1 = cell1Index % this.colNum;
            int r2 = cell2Index / this.colNum;
            int c2 = cell2Index % this.colNum;

            return (r1 - r2) * (r1 - r2) + (c1 - c2) * (c1 - c2);
        }

        private float CalcDetailWeight(int[] spots, int from, int to)
        {
            double weight = 0.0f;
            int preCell = spots[from];
            for (int i = from + 1; i < to; i++)
            {
                weight += IsDiagonalNeighbor(preCell, spots[i]) ? Parameters.SQRT_TWO : 1.0;
            }

            return (float)weight;
        }

        // 判断是否对角方向的邻格
        private bool IsDiagonalNeighbor(int cell1Index, int cell2Index)
        {
            return CalcSquareDistanceWeight(cell1Index, cell2Index) > 1;
        }

        private int CalcSquareDistanceWeight(GpsMapCell gmc1, GpsMapCell gmc2)
        {
            int x = gmc1.Row - gmc2.Row;
            int y = gmc1.Col - gmc2.Col;
            return x * x + y * y;
        }

        // 查找最近的顶点，按顶点远近排序返回
        private List<MapPathGraph.WeightVertexNode> FindNearestVertex(int cellIndex)
        {
            List<MapPathGraph.WeightVertexNode> listVertex = new List<MapPathGraph.WeightVertexNode>();
            foreach (MapPathGraph.VertexNode vn in this.pathGraph.VertexList)
            {
                int weight = CalcSquareDistanceWeight(vn.Cell, cellIndex);
                MapPathGraph.WeightVertexNode wvn = new MapPathGraph.WeightVertexNode(vn, weight);

                int index = listVertex.BinarySearch(wvn);
                if (index < 0)
                {
                    listVertex.Insert(~index, wvn);
                }
                if (weight == (int)Parameters.MIN_EDGE_WEIGHT) break;
            }

            return listVertex;
        }

        // 返回路格在边中的位置（数组下标）
        public int FindEdgeOfRoadCell(int cellIndex, out MapPathGraph.VertexNode vn1, out MapPathGraph.VertexNode vn2, out MapPathGraph.Edge e)
        {
            vn1 = vn2 = null;
            e = null;

            List<MapPathGraph.WeightVertexNode> listVertex = FindNearestVertex(cellIndex);
            foreach (MapPathGraph.WeightVertexNode vn in listVertex)
            {
                MapPathGraph.EdgeNode p = vn.FirstEdge;
                while (p != null)
                {
                    MapPathGraph.Edge edge = p.EdgeLink;
                    for (int i = 0; i < edge.Spots.Length; i++)
                    {
                        if (edge.Spots[i] == cellIndex)
                        {
                            vn1 = vn;
                            vn2 = this.pathGraph.VertexList[p.AdjacentVertex];
                            e = edge;
                            return i;
                        }
                    }
                    p = p.Next;
                }
            }

            return -1;
        }

        public class SafeStatus
        {
            public const int LS_NULL = 0;
            public const int LS_INFO_DATA = 1;
            public const int LS_CELL_DATA = 2;
            public const int LS_PATH_DATA = 4;
            private object theLock = new object();
            private int status = LS_NULL;

            public void SetStatus(int status)
            {
                Monitor.Enter(theLock);
                this.status = status;
                Monitor.Exit(theLock);
            }

            public int GetStatus()
            {
                int s;
                Monitor.Enter(theLock);
                s = this.status;
                Monitor.Exit(theLock);
                return s;
            }

            public bool IsJustInfoDataReady()
            {
                return GetStatus() == LS_INFO_DATA;
            }
            public bool IsInfoDataReady()
            {
                int s = GetStatus();
                return (s & LS_INFO_DATA) != 0;
            }
            public bool IsCellDataReady()
            {
                int s = GetStatus();
                return (s & LS_CELL_DATA) != 0;
            }
            public bool IsPathDataReady()
            {
                int s = GetStatus();
                return (s & LS_PATH_DATA) != 0;
            }
        };
    }
}
