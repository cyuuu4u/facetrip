using UnityEngine;
using System.Collections;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

namespace xxdwunity.controller
{
    public class CameraController : MonoBehaviour
    {
        private int zoomLevel;
        private Rect cameraRange; // 相机坐标范围
        private Vector3 cameraTarget; // 移动相机的目的坐标
        private GpsMap gpsMap;
        public void SetGpsMap(GpsMap gm)
        {
            this.gpsMap = gm;
        }

        // 在此注册通知
        void Awake()
        {
            XxdwDebugger.Log("CameraController -> Awake()");
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_MAP_SWITCHED);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_BACKGROUND_READY);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_DRAG, Observer.NOTI_PRIORITY_HIGHEST);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_ZOOM_IN);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_ZOOM_OUT);
            NotificationCenter.Instance.AddObserver(this, Notification.NOTI_MOVE_CAMERA);
        }

        // Use this for initialization
        void Start()
        {
            XxdwDebugger.Log("CameraController -> Start()");
            this.zoomLevel = GpsMap.NORM_ZOOM_LEVEL;
            this.cameraRange = new Rect(0, 0, 0, 0);
        }

        public void OnNotiMapSwitched(Notification noti)
        {
            XxdwDebugger.Log("CameraController -> OnNotiMapSwitched()");
            this.gpsMap = (GpsMap)noti.Data;
        }

        public void OnNotiBackgroundReady(Notification noti)
        {
            XxdwDebugger.Log("Camera: Notification noti OnBackgroundReady");
            this.zoomLevel = GpsMap.NORM_ZOOM_LEVEL;
            Zoom();
        }

        private void OnNotiDrag(Notification noti)
        {
            XxdwDebugger.Log("OnNotiDrag in CameraController");
            Vector3 pre = (Vector3)noti.ExtraData;
            Vector3 cur = (Vector3)noti.Data;

            Vector3 preWorld = this.GetComponent<Camera>().ScreenToWorldPoint(pre);
            Vector3 curWorld = this.GetComponent<Camera>().ScreenToWorldPoint(cur);

            Vector3 v = this.GetComponent<Camera>().transform.position;
            v.x -= curWorld.x - preWorld.x;
            v.y -= curWorld.y - preWorld.y;

            this.cameraTarget = LimitRange(v);
            MoveCamera();
        }

        private void OnNotiZoomIn(Notification noti)
        {
            if (this.zoomLevel > GpsMap.MIN_ZOOM_LEVEL)
            {
                this.zoomLevel--;
                Zoom();
            }
            else
            {
                NotificationCenter.Instance.SendNotification(this, Notification.NOTI_SET_HINT, Parameters.STR_REACH_MAX_SIZE);
            }
        }

        private void OnNotiZoomOut(Notification noti)
        {
            if (this.zoomLevel < GpsMap.MAX_ZOOM_LEVEL)
            {
                this.zoomLevel++;
                Zoom();
            }
            else
            {
                NotificationCenter.Instance.SendNotification(this, Notification.NOTI_SET_HINT, Parameters.STR_REACH_MIN_SIZE);
            }
        }

        private void OnNotiMoveCamera(Notification noti)
        {
            Vector3 target = (Vector3)noti.Data;
            float delay = 0.0f;
            if (noti.ExtraData != null)
            {
                delay = (float)noti.ExtraData;
            }

            target.z = this.GetComponent<Camera>().transform.position.z;
            this.cameraTarget = LimitRange(target);

            if (delay > 0)
            {
                Invoke("MoveCamera", delay);
            }
            else
            {
                MoveCamera();
            }
        }

        private Vector3 LimitRange(Vector3 v)
        {
            if (v.x < this.cameraRange.xMin)
                v.x = this.cameraRange.xMin;
            else if (v.x > this.cameraRange.xMax)
                v.x = this.cameraRange.xMax;
            if (v.y < this.cameraRange.yMin)
                v.y = this.cameraRange.yMin;
            else if (v.y > this.cameraRange.yMax)
                v.y = this.cameraRange.yMax;
            return v;
        }

        private void Zoom()
        {
            if (this.gpsMap == null) return;
            
            float orthographicSize = this.gpsMap.GetZoomOrthographicSize(this.zoomLevel);

            // this.camera.orthographicSize 为相机视口纵幅的一半
            // this.camera.orthographicSize * this.camera.aspect 为相机视口横幅的一半
            this.cameraRange.xMin = orthographicSize * this.GetComponent<Camera>().aspect + this.gpsMap.MapRange.xMin;
            this.cameraRange.xMax = -orthographicSize * this.GetComponent<Camera>().aspect + this.gpsMap.MapRange.xMax;
            this.cameraRange.yMin = orthographicSize + this.gpsMap.MapRange.yMin;
            this.cameraRange.yMax = -orthographicSize + this.gpsMap.MapRange.yMax;
            XxdwDebugger.Log("cameraRange:(" + this.cameraRange.xMin + "," + this.cameraRange.yMin + "),(" + this.cameraRange.xMax + "," + this.cameraRange.yMax + ")");

            Vector3 v = LimitRange(this.GetComponent<Camera>().transform.position);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
            this.cameraTarget = v;
            MoveCamera();
        }

        private void MoveCamera()
        {
            // 通知视口改变以便加载地图切片
            Vector2 sliceRowCol = this.gpsMap.GetSliceRowCol(this.cameraTarget);
            NotificationCenter.Instance.SendNotification(this, Notification.NOTI_VIEW_PORT_CHANGED,
                    this.GetComponent<Camera>().orthographicSize * 2, sliceRowCol);

            this.GetComponent<Camera>().transform.position = this.cameraTarget;
            XxdwDebugger.Log("current position:(" + this.cameraTarget.x + "," + this.cameraTarget.y + ")");
        }
    }
}
