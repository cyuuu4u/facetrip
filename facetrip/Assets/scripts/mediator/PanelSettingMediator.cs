using UnityEngine;
using System.Collections;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class PanelSettingMediator : PanelMediator
{
    //public UnityEngine.UI.Text uiContent;
    private object observer;
    public UnityEngine.UI.Text uiVersion;

    // 在此注册通知
    void Awake()
    {
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_OPEN_WINDOW);
    }

    protected override void OnNotiOpenWindow(Notification noti)
    {
        base.OnNotiOpenWindow(noti);
        this.uiVersion.text = TypeAndParameter.VERSION;

        Notification originNoti = noti.ExtraData as Notification;
        if (this.notiMe && originNoti != null)
        {
            //this.uiContent.text = originNoti.Data.ToString();
            this.observer = originNoti.Sender;
        }
    }

    public void OnOkButtonClicked()
    {
        AsyncNotification.Instance.PostNotification(this, Notification.NOTI_DIALOG_QUIT,
            this.observer, Parameters.EDialogReturnValue.DRV_OK);

        base.OnCloseWindow();
    }

    public void OnCancelButtonClicked()
    {
        OnCloseWindow();
    }

    public override void OnCloseWindow()
    {
        AsyncNotification.Instance.PostNotification(this, Notification.NOTI_DIALOG_QUIT,
            this.observer, Parameters.EDialogReturnValue.DRV_CANCEL);

        base.OnCloseWindow();
    }
}
