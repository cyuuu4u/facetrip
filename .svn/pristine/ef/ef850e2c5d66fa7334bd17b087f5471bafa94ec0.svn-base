using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

namespace xxdwunity.controller
{
    public class BackgroundController : MonoBehaviour
    {
        public GameObject monitorSpotPrefab;
        private GpsMap gpsMap;
        private List<GameObject> triggerPoints;
        private bool[,] sliceLoaded;
        
        // 在此注册通知
        void Awake()
        {
            XxdwDebugger.Log("BackgroundController -> Awake()");
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_MAP_SWITCHED);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_MAP_INFO_READY);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_MAP_DATA_READY);            
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_VIEW_PORT_CHANGED);
        }

        // Use this for initialization
        void Start()
        {

        }

        /*private Rect TestBackgroundRange()
        {
            float minX = TypeAndParameter.MAX_COORDINATE, minY = TypeAndParameter.MAX_COORDINATE;
            float maxX = TypeAndParameter.MIN_COORDINATE, maxY = TypeAndParameter.MIN_COORDINATE;

            foreach (Transform child in transform)
            {
                string name = child.gameObject.name;
                Vector3 min = child.gameObject.GetComponent<Renderer>().bounds.min;
                Vector3 max = child.gameObject.GetComponent<Renderer>().bounds.max;
                XxdwDebugger.Log(name + " @ (" + min.x + "," + min.y + "),(" + max.x + "," + max.y + ")");
                if (min.x < minX) minX = min.x;
                if (min.y < minY) minY = min.y;
                if (max.x > maxX) maxX = max.x;
                if (max.y > maxY) maxY = max.y;
            }

            Rect rect = new Rect(minX, minY, maxX - minX, maxY - minY);
            XxdwDebugger.Log("bg bounds: (" + rect.min.x + "," + rect.min.y + "),(" +
                      rect.max.x + "," + rect.max.y + ")");
            return rect;
        }*/

        public void OnNotiMapSwitched(Notification noti)
        {
            XxdwDebugger.Log("BackgroundController -> OnNotiMapSwitched()");
            this.gpsMap = (GpsMap)noti.Data;            
        }

        private void OnNotiMapInfoReady(Notification noti)
        {
            XxdwDebugger.Log("BackgroundController -> OnNotiMapInfoReady()");
            if (this.gpsMap == null) return;

            // 销毁原先的背景
            foreach (Transform child in transform)
            {
                DestroyObject(child.gameObject);
            }

            //------------------------------------------------------
            this.sliceLoaded = new bool[this.gpsMap.SliceRowNum, this.gpsMap.SliceColNum];

            // 通知相机地图背景OK，可进行缩放
            //Document.Instance.Map.CheckMapRange(TestBackgroundRange());
            NotificationCenter.Instance.SendNotification(this, Notification.NOTI_BACKGROUND_READY);
            //}
        }

        private void OnNotiMapDataReady(Notification noti)
        {
            XxdwDebugger.Log("BackgroundController -> OnNotiMapDataReady()");
            if (this.gpsMap == null) return;

            if (this.gpsMap.IsPathDataReady())
            {
                if (this.triggerPoints == null)
                {
                    this.triggerPoints = new List<GameObject>();
                }
                else if (this.triggerPoints.Count > 0)
                {
                    // 清除原先的触发点
                    foreach (GameObject go in this.triggerPoints)
                    {
                        DestroyObject(go);
                    }
                    this.triggerPoints.Clear();
                }

                // 添加新的触发点
                List<GpsMapMonitorSpot> spots = this.gpsMap.GetMonitorSpots();
                XxdwDebugger.Log("MonitorPoint Number:" + spots.Count);
                if (this.monitorSpotPrefab != null)
                {
                    foreach (GpsMapMonitorSpot gmms in spots)
                    {
                        XxdwDebugger.Log("MonitorPoint At:" + gmms.Cell);
                        Vector3 v = this.gpsMap.GetCellPosition(gmms.Cell);
                        GameObject go = (GameObject)Instantiate(this.monitorSpotPrefab, v, Quaternion.identity);
                        go.name = Parameters.STR_PREFIX_MONITOR_POINT + gmms.Id;
                        this.triggerPoints.Add(go);
                    }
                }
            }
        }

        private void OnNotiViewPortChanged(Notification noti)
        {
            float viewPortHeight = (float)noti.Data;
            Vector2 pos = (Vector2)noti.ExtraData;
            LoadSlices(viewPortHeight, (int)pos.x, (int)pos.y);
        }

        private void LoadSlice(string mapName, int rowNum, int colNum, float sliceWorldWidth, float sliceWorldHeight, int i, int j)
        {
            string picFilename = string.Format("{0:d2}_{1:d2}", i + 1, j + 1);
            Texture2D texture = Resources.Load(mapName + "/bg/" + picFilename, typeof(Texture2D)) as Texture2D;
            if (texture == null)
            {
                XxdwDebugger.LogError("Fail to load texture: " + mapName + "/bg/" + picFilename);
            }
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));   // 以左上角为pivot
            if (sprite == null)
            {
                XxdwDebugger.LogError("Fail to load bg picture " + picFilename);
            }
            GameObject go = new GameObject(picFilename);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            go.transform.SetParent(this.transform);

            // 计算世界坐标
            float r = (float)rowNum / 2 - 1 - i;
            float c = -(float)colNum / 2 + j;
            go.transform.position = new Vector3(c * sliceWorldWidth, r * sliceWorldHeight);
        }

        private void LoadSlices(float viewPortHeight, int centerRow, int centerCol)
        {
            string mapName = this.gpsMap.Name;

            int screenWidth = (int)(viewPortHeight * Camera.main.aspect * GpsMap.UNIT_PIXEL_NUM);
            int screenHeight = (int)(viewPortHeight * GpsMap.UNIT_PIXEL_NUM);

            const int PERIPHERAL_SIZE = 4;
            int rowNum = this.gpsMap.SliceRowNum;
            int colNum = this.gpsMap.SliceColNum;
            int showRowNum = screenHeight / this.gpsMap.SliceHeight + PERIPHERAL_SIZE;
            int showColNum = screenWidth / this.gpsMap.SliceWidth + PERIPHERAL_SIZE;
            showRowNum = (int)Mathf.Clamp(showRowNum, 2, rowNum);
            showColNum = (int)Mathf.Clamp(showColNum, 2, colNum);

            int beginRow = centerRow - showRowNum / 2;
            int endRow = centerRow + showRowNum / 2;
            int beginCol = centerCol - showColNum / 2;
            int endCol = centerCol + showColNum / 2;
            if (beginRow < 0)
            {
                beginRow = 0;
                endRow = (int)Mathf.Min(endRow + 1, rowNum - 1);
            }
            if (beginCol < 0)
            {
                beginCol = 0;
                endCol = (int)Mathf.Min(endCol + 1, colNum - 1);
            }

            for (int i = beginRow; i <= endRow && i < rowNum; i++)
            {
                for (int j = beginCol; j <= endCol && j < colNum; j++)
                {
                    if (!this.sliceLoaded[i, j])
                    {
                        LoadSlice(mapName, rowNum, colNum, this.gpsMap.WorldSliceWidth, this.gpsMap.WorldSliceHeight, i, j);
                        this.sliceLoaded[i, j] = true;
                    }
                }
            }
        }
    }
}
