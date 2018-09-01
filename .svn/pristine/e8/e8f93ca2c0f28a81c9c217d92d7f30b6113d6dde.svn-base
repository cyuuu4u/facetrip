using UnityEngine;
using System.Collections;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;
using com.hytxworld.comm;

public class PanelMediator : MonoBehaviour 
{
    protected GameObject view = null;
    protected bool notiMe = false;

    // 在此注册通知
    void Awake()
    {
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_OPEN_WINDOW);
    }

    protected void FocusInPanel()
    {
        XxdwDebugger.Log("EnterPanel");
        NotificationCenter.Instance.DisableTouchUpOnce();
    }

    protected virtual void OnNotiOpenWindow(Notification noti)
    {
        GameObject view = (GameObject)noti.Data;
        this.notiMe = (view.GetComponent<PanelMediator>() == this);
        if (this.notiMe)
        {
            this.view = view;
            NotificationCenter.Instance.EnqueueInput(this);
        }       
    }

    public virtual void OnCloseWindow()
    {
        NotificationCenter.Instance.ResetDrag();
        NotificationCenter.Instance.DequeueInput();
        this.view.SetActive(false);
        NotificationCenter.Instance.TemporarilyDisableInput();
    }
}
