using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using xxdwunity.util;
 
//    NotificationCenter is used for handling messages between GameObjects.
//    GameObjects can register to receive specific notifications.  When another objects sends a notification of that type, all GameObjects that registered for it and implement the appropriate message will receive that notification.
//    Observing GameObjects must register to receive notifications with the AddObserver function, and pass their selves, and the name of the notification.  Observing GameObjects can also unregister themselves with the RemoveObserver function.  GameObjects must request to receive and remove notification types on a type by type basis.
//    Posting notifications is done by creating a Notification object and passing it to SendNotification.  All receiving GameObjects will accept that Notification object.  The Notification object contains the sender, the notification type name, and an option hashtable containing data.
//    To use NotificationCenter, either create and manage a unique instance of it somewhere, or use the static NotificationCenter.
 
// We need a static method for objects to be able to obtain the default notification center.
// This default center is what all objects will use for most notifications.  We can of course create our own separate instances of NotificationCenter, but this is the static one used by all.
namespace xxdwunity.engine
{
    public class NotificationCenter : MonoBehaviour
    {
        private static NotificationCenter defaultCenter;
        public static NotificationCenter Instance
        {
            get
            {
                // If the defaultCenter doesn't already exist, we need to create it
                if (!defaultCenter)
                {
                    // Because the NotificationCenter is a component, we have to create a GameObject to attach it to.
                    GameObject notificationObject = new GameObject("Default Notification Center");
                    // Add the NotificationCenter component, and set it as the defaultCenter
                    defaultCenter = notificationObject.AddComponent<NotificationCenter>();
                    notificationObject.AddComponent<XxdwDebugger>();
                    DontDestroyOnLoad(notificationObject);
                }

                return defaultCenter;
            }
        }

        // 响应输入事件的Observer的队列
        private Queue<Component> inputResponseQueue = new Queue<Component>();
        public void EnqueueInput(Component obj)
        {
            this.inputResponseQueue.Enqueue(obj);
        }
        public void DequeueInput()
        {
            if (this.inputResponseQueue.Count > 0)
                this.inputResponseQueue.Dequeue();
        }

        // Our hashtable containing all the notifications.  Each notification in the hash table is an ArrayList that contains all the observers for that notification.
        Hashtable notifications = new Hashtable();

        // AddObserver includes a version where the observer can request to only receive notifications from a specific object.  We haven't implemented that yet, so the sender value is ignored for now.
        public void AddObserver(Component observer, string name) { AddObserver(observer, name, Observer.NOTI_PRIORITY_NORMAL, null); }
        public void AddObserver(Component observer, string name, int priority) { AddObserver(observer, name, priority, null); }
        public void AddObserver(Component observer, string name, int priority, object sender)
        {
            // If the name isn't good, then throw an error and return.
            if (name == null || name == "") { XxdwDebugger.Log("Null name specified for notification in AddObserver."); return; }
            // If this specific notification doens't exist yet, then create it.
            if (!notifications.ContainsKey(name))
            {
                notifications[name] = new List<Observer>();
            }

            List<Observer> notifyList = (List<Observer>)notifications[name];

            // If the list of observers doesn't already contain the one that's registering, then add it.
            Observer o = new Observer(observer, priority, sender);
            int index = notifyList.BinarySearch(o);
            if (index < 0)
            {
                notifyList.Insert(~index, o);
            }
        }

        // RemoveObserver removes the observer from the notification list for the specified notification type
        public void RemoveObserver(Component observer, string name)
        {
            List<Observer> notifyList = (List<Observer>)notifications[name]; //change from original

            // Assuming that this is a valid notification type, remove the observer from the list.
            // If the list of observers is now empty, then remove that notification type from the notifications hash.  This is for housekeeping purposes.
            if (notifyList != null)
            {
                //if (notifyList.Contains(observer)) { notifyList.Remove(observer); }
                int index = notifyList.BinarySearch(new Observer(observer));
                if (index >= 0)
                {
                    notifyList.RemoveAt(index);
                }
                if (notifyList.Count == 0) { notifications.Remove(name); }
            }
        }

        // SendNotification sends a notification object to all objects that have requested to receive this type of notification.
        // A notification can either be posted with a notification object or by just sending the individual components.
        public void SendNotification(object aSender, string aName) { SendNotification(aSender, aName, null); }
        public void SendNotification(object aSender, string aName, object aData) { SendNotification(new Notification(aSender, aName, aData)); }
        public void SendNotification(object aSender, string aName, object aData, object aExtraData)
        {
            SendNotification(new Notification(aSender, aName, aData, aExtraData));
        }
        public void SendNotification(Notification aNotification)
        {
            // First make sure that the name of the notification is valid.
            if (aNotification.Name == null || aNotification.Name == "") { XxdwDebugger.Log("Null name sent to SendNotification."); return; }
            // Obtain the notification list, and make sure that it is valid as well
            List<Observer> notifyList = (List<Observer>)notifications[aNotification.Name]; //change from original
            if (notifyList == null) { XxdwDebugger.Log("NotificationCenter Notify list not found -> " + aNotification.Name); return; }

            // Clone list, so there won't be an issue if an observer is added or removed while notifications are being sent
            notifyList = new List<Observer>(notifyList);

            // Create an array to keep track of invalid observers that we need to remove
            List<Observer> observersToRemove = new List<Observer>(); //change from original

            // Itterate through all the objects that have signed up to be notified by this type of notification.
            bool preNotiReturn = false;
            for (int i = notifyList.Count - 1; i >= 0; i--)
            {
                // If the observer isn't valid, then keep track of it so we can remove it later.
                // We can't remove it right now, or it will mess the for loop up.
                if (!notifyList[i].observer)
                {
                    observersToRemove.Add(notifyList[i]);
                }
                else if (!preNotiReturn && (notifyList[i].sender == null || notifyList[i].sender == aNotification.Sender))
                {
                    // If the observer is valid, then send it the notification.  The message that's sent is the name of the notification.
                    if (aNotification.IsInput
                        && this.inputResponseQueue.Count > 0
                        && notifyList[i].observer != this.inputResponseQueue.Peek())
                        continue;

                    notifyList[i].observer.SendMessage(aNotification.Name, aNotification, SendMessageOptions.DontRequireReceiver);
                    preNotiReturn = aNotification.Ret;
                }
            }

            // Remove all the invalid observers
            foreach (Observer observer in observersToRemove)
            {
                notifyList.Remove(observer);
            }
        }

        //----------------------------------------------------------------------------------
        // 处理输入事件
        public void TemporarilyDisableInput()
        {
            StartCoroutine(DisableInputDelayEnable(0.5f));
        }
        public bool DisableInput { get; set; }
        private IEnumerator DisableInputDelayEnable(float waitTime)
        {
            this.DisableInput = true;
            yield return new WaitForSeconds(waitTime);
            this.DisableInput = false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private bool touched = false;
        public void ResetDrag()
        {
            this.touched = false;
        }
        private Vector3 posPrevious;

        void Awake()
        {
            //设置屏幕正方向在Home键右边
            //Screen.orientation = ScreenOrientation.LandscapeRight;
            Screen.orientation = ScreenOrientation.Portrait;            
        }

        void Start()
        {
            this.touched = false;
            this.posPrevious = new Vector3();

            //设置屏幕自动旋转， 并置支持的方向
            Screen.orientation = ScreenOrientation.Portrait;
            //Screen.autorotateToLandscapeLeft = true;
            //Screen.autorotateToLandscapeRight = true;
            //Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;

            StartGPS();
        }

        void Update()
        {
            Notification noti = AsyncNotification.Instance.GetAsyncNotification();
            if (noti != null)
            {
                this.SendNotification(noti);
            }

            DealWithInput();
        } // update

        private float touchDownTime = 0;
        private int disableTouchUpCount = 0;    // 用于进入工具栏时禁止触起事件。
        public void DisableTouchUpOnce() { disableTouchUpCount++; }
        public int PeekDisableTouchUp() { return this.disableTouchUpCount > 0 ? this.disableTouchUpCount-- : 0; }
        private void DealWithInput()
        {
            if (this.DisableInput) return;            

            Notification noti = new Notification(this, Notification.NOTI_NULL);
            GameKey gk = new GameKey();
            GameKey.EKey key = gk.GetKeyUp();
            if (key != GameKey.EKey.K_NULL)
            {
                noti.Name = Notification.NOTI_KEY_UP;
                noti.Data = key;

                XxdwDebugger.Log(noti.Name + ":" + gk.GetKeyString(key));
                this.SendNotification(noti);
            }
            else if ((key = gk.GetKeyDown()) != GameKey.EKey.K_NULL )
            {
                noti.Name = Notification.NOTI_KEY_DOWN;
                noti.Data = key;
                noti.ExtraData = gk.GetModifier();

                XxdwDebugger.Log(noti.Name + ":" + gk.GetKeyString(key) + "," + gk.GetKeyString(gk.GetModifier()));
                this.SendNotification(noti);
            }

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                int touchCount = 0;
                int touch1 = 0, touch2 = 0;
                for (int i = 0; i < Input.touches.Length; i++)
                {
                    if (Input.touches[i].phase != TouchPhase.Canceled)
                    {
                        if (touchCount == 0)
                        {
                            touch1 = i;
                            touchCount++;
                        }
                        else
                        {
                            touch2 = i;
                            touchCount++;
                            break;
                        }
                    }
                }
                if (touchCount == 0) return;

                Vector3 pos = new Vector3(Input.touches[touch1].position.x, Input.touches[touch1].position.y, 0);
                if (touchCount == 2 && Input.touches[touch1].phase == TouchPhase.Moved && Input.touches[touch2].phase == TouchPhase.Moved)
                {
                    XxdwDebugger.Log("TouchPhase.SizeChanged");
                    float distNow = Vector2.Distance(Input.touches[touch1].position, Input.touches[touch2].position);
                    Vector2 pre1 = Input.touches[touch1].position - Input.touches[touch1].deltaPosition;
                    Vector2 pre2 = Input.touches[touch2].position - Input.touches[touch2].deltaPosition;
                    float distPre = Vector2.Distance(pre1, pre2);
                    if (distNow > distPre)
                    {
                        XxdwDebugger.Log("Touch Zoom in");
                        noti.Name = Notification.NOTI_ZOOM_IN;
                    }
                    else
                    {
                        XxdwDebugger.Log("Touch Zoom out");
                        noti.Name = Notification.NOTI_ZOOM_OUT;
                    }
                    noti.Data = Math.Abs(distNow - distPre);

                    if ((float)noti.Data > Parameters.MIN_SIZE_TRIGGER_ZOOM)
                    {
                        this.SendNotification(noti);
                    }
                    //XxdwDebugger.DisplayMessage("dist:" + Math.Abs(distNow - distPre));
                }
                else
                {
                    switch (Input.touches[touch1].phase)
                    {
                        case TouchPhase.Began:
                            XxdwDebugger.Log("TouchPhase.Began");
                            this.touchDownTime = Time.time;
                            this.posPrevious = pos;
                            noti.Name = Notification.NOTI_TOUCH_DOWN;
                            noti.Data = pos;

                            this.SendNotification(noti);
                            break;
                        case TouchPhase.Ended:
                            if (PeekDisableTouchUp() == 0)
                            {
                                XxdwDebugger.Log("TouchPhase.Ended.");
                                noti.Name = Notification.NOTI_TOUCH_UP;
                                noti.Data = pos;

                                this.SendNotification(noti);
                            }
                            break;
                        case TouchPhase.Moved:
                            if (Input.touches[touch1].deltaPosition.sqrMagnitude > Parameters.MIN_COORDINATE_OFFSET)
                            {
                                XxdwDebugger.Log("TouchPhase.Moved");
                                // 拖动
                                noti.Name = Notification.NOTI_DRAG;
                                noti.Data = pos;
                                noti.ExtraData = this.posPrevious;

                                this.touchDownTime = Time.time;
                                this.posPrevious = pos;

                                this.SendNotification(noti);
                            }
                            else
                            {
                                goto STATIONARY;
                            }
                            break;
                        case TouchPhase.Stationary:
                        STATIONARY:
                            if (Time.time - touchDownTime > Parameters.LONG_TOUCH_INTERVAL)
                            {
                                XxdwDebugger.Log("MouseButtonUp Long.");
                                this.touchDownTime = Time.time;

                                noti.Name = Notification.NOTI_LONG_TOUCH_UP;
                                noti.Data = Input.mousePosition;

                                this.SendNotification(noti);
                            }
                            break;
                    }
                }
            }
            else if (Application.platform <= RuntimePlatform.WindowsEditor)
            {
                // 按下鼠标
                if (Input.GetMouseButtonDown((int)Parameters.MouseTypeEnum.LEFT))
                {
                    XxdwDebugger.Log("MouseButtonDown");
                    this.touchDownTime = Time.time;
                    this.posPrevious.Set(Input.mousePosition.x, Input.mousePosition.y, 0);
                    noti.Name = Notification.NOTI_TOUCH_DOWN;
                    noti.Data = Input.mousePosition;
                    this.touched = true;

                    this.SendNotification(noti);
                }
                // 松开鼠标
                else if (Input.GetMouseButtonUp((int)Parameters.MouseTypeEnum.LEFT))
                {
                    XxdwDebugger.Log("MouseButtonUp");
                    noti.Name = Notification.NOTI_TOUCH_UP;
                    noti.Data = Input.mousePosition;

                    this.touched = false;
                    this.SendNotification(noti);
                }
                // 松开鼠标右键
                else if (Input.GetMouseButtonUp((int)Parameters.MouseTypeEnum.RIGHT))
                {
                    XxdwDebugger.Log("MouseButtonUp");
                    noti.Name = Notification.NOTI_RIGHT_CLICK;
                    noti.Data = Input.mousePosition;

                    this.SendNotification(noti);
                }
                else if (this.touched)
                {
                    Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
                    Vector2 diff = new Vector2(pos.x - this.posPrevious.x, pos.y - this.posPrevious.y);
                    if (diff.sqrMagnitude > Parameters.MIN_COORDINATE_OFFSET)
                    {
                        // 拖动
                        noti.Name = Notification.NOTI_DRAG;
                        noti.Data = Input.mousePosition;
                        noti.ExtraData = this.posPrevious;

                        this.touchDownTime = Time.time;
                        this.posPrevious = pos;

                        this.SendNotification(noti);
                    }
                    else if (Time.time - touchDownTime > Parameters.LONG_TOUCH_INTERVAL)
                    {
                        XxdwDebugger.Log("MouseButtonUp Long.");
                        noti.Name = Notification.NOTI_LONG_TOUCH_UP;
                        noti.Data = Input.mousePosition;

                        this.touched = false;
                        this.SendNotification(noti);
                    }
                }
                else
                {
                    // 鼠标滚轮拉近拉远
                    float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
                    if (Mathf.Abs(mouseScroll) > Parameters.MIN_COORDINATE_OFFSET)
                    {
                        if (mouseScroll > 0)
                        {
                            XxdwDebugger.Log("Mouse Scroll Zoom in");
                            noti.Name = Notification.NOTI_ZOOM_IN;
                            noti.Data = mouseScroll;
                        }
                        else
                        {
                            XxdwDebugger.Log("Mouse Scroll Zoom out");
                            noti.Name = Notification.NOTI_ZOOM_OUT;
                            noti.Data = -mouseScroll;
                        }
                        this.SendNotification(noti);
                    }
                }
            }
        } // DealWithInput()

        private bool runningGps = true;
        private IEnumerator DealWithGPS()
        {
            // 无法放入独立的线程中，子线程不能使用Unity的对象，只可以使用Unity的值类型变量，实际上unity没有实现多线程同步机制.
            //#if UNITY_ANDROID || UNITY_IPHONE
            while (this.runningGps)
            {
                if (!Input.location.isEnabledByUser)
                {
                    yield return new WaitForSeconds(10.0f);
                }
                else
                {
                    if (Input.location.status == LocationServiceStatus.Failed || Input.location.status == LocationServiceStatus.Stopped)
                    {
                        Notification noti = new Notification(this, Notification.NOTI_GPS_UNAVAILABLE, Input.location.status);
                        this.SendNotification(noti);

                        Input.location.Start(10.0f, 10.0f);
                        yield return new WaitForSeconds(1.0f);
                    }
                    else
                    {
                        if (Input.location.status == LocationServiceStatus.Initializing)
                        {
                            Notification noti = new Notification(this, Notification.NOTI_GPS_UNAVAILABLE, Input.location.status);
                            this.SendNotification(noti);
                        }
                        else
                        {
                            Notification noti = new Notification(this, Notification.NOTI_GPS_CHANGED, Input.location.lastData);
                            this.SendNotification(noti);
                        }
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
            yield break;
        }        

        //Thread gpsThread;
        public void StartGPS()
        {
            StartCoroutine(DealWithGPS());
        }

        public void StopGPS()
        {
            this.runningGps = false;
        }

        public bool IsGpsEnabled()
        {
            return Input.location.isEnabledByUser;
        }

        public LocationInfo GetGpsLocation()
        {
            return Input.location.lastData;
        }

        public LocationServiceStatus GetGpsStatus()
        {
            return Input.location.status;
        }
    }    
}

