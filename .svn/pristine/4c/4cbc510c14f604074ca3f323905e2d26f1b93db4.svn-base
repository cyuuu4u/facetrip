using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;

namespace xxdwunity.engine
{
    public class AsyncNotification
    {
        private static AsyncNotification _instance = null;
        // 非线程安全，应该保证首先在主线程调用。
        public static AsyncNotification Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AsyncNotification();
                }
                return _instance;
            }
        }

        // The asynchronize messages sending between different threads.
        Queue<Notification> messages = new Queue<Notification>();
        public void PostNotification(object aSender, string aName)
        {
            PostNotification(new Notification(aSender, aName));
        }
        public void PostNotification(object aSender, string aName, object aData)
        {
            PostNotification(new Notification(aSender, aName, aData));
        }
        public void PostNotification(object aSender, string aName, object aData, object aExtraData)
        {
            PostNotification(new Notification(aSender, aName, aData, aExtraData));
        }
        public void PostNotification(Notification noti)
        {
            Monitor.Enter(messages);
            if (messages.Count > 0 && messages.Peek().Sender == noti.Sender && messages.Peek().Name == noti.Name)
            {
                // 删除前一条同样的消息
                messages.Dequeue();
            }
            messages.Enqueue(noti);
            Monitor.Exit(messages);
        }
        public Notification GetAsyncNotification()
        {
            Notification noti = null;
            Monitor.Enter(messages);
            if (messages.Count > 0)
            {
                noti = this.messages.Dequeue();
            }
            Monitor.Exit(messages);

            return noti;
        }
    }

    // The Notification class is the object that is sent to receiving objects of a notification type.
    // This class contains the sending GameObject, the name of the notification, and optionally a hashtable containing data.
    public class Notification
    {
        public const string NOTI_NULL = "NOTI_NULL";

        public const string NOTI_APP_LOADING = "OnNotiAppLoading";
        public const string NOTI_APP_LOADED = "OnNotiAppLoaded";
        public const string NOTI_MAP_INFO_READY = "OnNotiMapInfoReady";
        public const string NOTI_MAP_DATA_READY = "OnNotiMapDataReady";
        public const string NOTI_MAP_SWITCHED = "OnNotiMapSwitched";
        public const string NOTI_MODEL_DATA_READY = "OnNotiModelDataReady";
        public const string NOTI_BACKGROUND_READY = "OnNotiBackgroundReady";
        public const string NOTI_VIEW_PORT_CHANGED = "OnNotiViewPortChanged";

        public const string NOTI_TOUCH_DOWN = "OnNotiTouchDown";
        public const string NOTI_TOUCH_UP = "OnNotiTouchUp";
        public const string NOTI_DRAG = "OnNotiDrag";
        public const string NOTI_LONG_TOUCH_UP = "OnNotiLongTouchUp";
        public const string NOTI_ZOOM_IN = "OnNotiZoomIn";
        public const string NOTI_ZOOM_OUT = "OnNotiZoomOut";
        public const string NOTI_RIGHT_CLICK = "OnNotiRightClick";

        public const string NOTI_KEY_DOWN = "OnNotiKeyDown";
        public const string NOTI_KEY_UP = "OnNotiKeyUp";
        public const string NOTI_KEY = "OnNotiKey";

        public const string NOTI_SWITCH_SCENIC_RESORT = "OnNotiSwitchScenicResort";
        public const string NOTI_GPS_UNAVAILABLE = "OnNotiGpsUnavailable";
        public const string NOTI_GPS_CHANGED = "OnNotiGpsChanged";
        public const string NOTI_PLAY_COMMENTARY = "OnNotiPlayCommentary";

        public const string NOTI_TO_OPEN_WINDOW = "OnNotiToOpenWindow";
        public const string NOTI_OPEN_WINDOW = "OnNotiOpenWindow";
        public const string NOTI_SET_HINT = "OnNotiSetHint";

        public const string NOTI_DIALOG_QUIT = "OnNotiDialogQuit";

        public const string NOTI_START_NAVIGATING = "OnNotiStartNavigating";
        public const string NOTI_STOP_NAVIGATING = "OnNotiStopNavigating";
        public const string NOTI_MOVE_CAMERA = "OnNotiMoveCamera";
        public const string NOTI_CHANGE_MAP_MODE = "OnNotiChangeMapMode";

        public const string NOTI_COMM_RESULT = "OnNotiCommResult";

        public const string NOTI_PLAY_BACKGROUND_MUSIC = "OnNotiPlayBackgroundMusic";

        public object Sender { get; set; }
        public string Name { get; set; }
        public object Data { get; set; }
        public object ExtraData { get; set; }
        // 接收方的返回值，为true时终止冒泡, 仅SendNotification有用。
        // 当用SendNotification (Notification aNotification)方法时，消息接收方也可通过data和extraData返回值给发送方。
        public bool Ret { get; set; }

        private static string[] INPUT_NOTI_NAMES = new string[] {
            NOTI_TOUCH_DOWN,
            NOTI_TOUCH_UP,
            NOTI_DRAG,
            NOTI_LONG_TOUCH_UP,
            NOTI_ZOOM_IN,
            NOTI_ZOOM_OUT,
            NOTI_RIGHT_CLICK,
            NOTI_KEY_DOWN,
            NOTI_KEY_UP,
            NOTI_KEY
        };
        public bool IsInput
        {
            get
            {
                foreach (string input in INPUT_NOTI_NAMES)
                {
                    if (this.Name == input) return true;
                }
                return false;
            }
        }
        public Notification(object aSender, string aName) { Sender = aSender; Name = aName; Data = null; ExtraData = null; Ret = false; }
        public Notification(object aSender, string aName, object aData) { Sender = aSender; Name = aName; Data = aData; ExtraData = null; Ret = false; }
        public Notification(object aSender, string aName, object aData, object aExtraData) { Sender = aSender; Name = aName; Data = aData; ExtraData = aExtraData; Ret = false; }
    }
} 
