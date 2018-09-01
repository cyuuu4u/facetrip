using UnityEngine;
using System.Collections;
using System.IO;

public class TypeAndParameter
{
    public const string VERSION = "慧游天下1.0校园版\nwww.hytxworld.com";
    public const int AVAILABLE_MAP_NUM = 2;
    public const string MAP_DATA_FILE = "mapdata";
    public const string SCENIC_SPOT_SHEET_FILE = "scenic_spot";
    //public const string DEFAULT_MAP = "level1";
    public string DEFAULT_MAP
    {
        get;
        set;
    }

    public const string UI_PANEL_MESSAGE_DIALOG = "uiPanelMessageDialog";
    public const string UI_PANEL_VIEW = "uiPanelView";
    public const string UI_PANEL_OUTLINE = "uiPanelOutline";
    public const string UI_PANEL_TOOLBAR = "uiPanelToolbar";
    public const string UI_PANEL_NAVIGATE_DIALOG = "uiPanelNavigateDialog";
    public const string UI_PANEL_SCENIC_SPOT_DIALOG = "uiPanelScenicSpotDialog";
    public const string UI_PANEL_RECOMMEND_DIALOG = "uiPanelRecommendDialog";
    public const string UI_PANEL_SETTING_DIALOG = "uiPanelSettingDialog";
    public const string UI_PANEL_RECORDER_DIALOG = "uiPanelRecorderDialog";

    public const string UI_TEXT_SHORT_HINT = "uiTextShortHint";

    public const string STR_GPS_DISABLED = "请打开手机的GPS定位功能!";
    public const string STR_GPS_UNAVAILABLE = "GPS定位失败，请稍后再试.";
    public const string STR_START_NAVIGATING = "开始导航...";
    public const string STR_GPS_BEYOND_MAP = "GPS位置不在地图范围内。";
    public const string STR_ARE_YOU_SURE_QUIT_NAVIGATING = "您确定要退出导航吗？";
    public const string STR_ARE_YOU_SURE_QUIT_APP = "您确定要退出吗？";
    public const string STR_NO_PATH_TO_BUILDING = "建筑无可达路径";
    public const string STR_CURRENT_POSITION = "--当前位置--";
    public const string STR_NOT_SELECTED = "--未选择--";

    public const string STR_LOAD_MAP_RESOURCE_FAILED = "加载地图资源文件失败";
    public const string STR_LOAD_SCENIC_SPOT_RESOURCE_FAILED = "加载景点资源文件失败";
    public const string STR_FIND_PATH_FAILED = "搜索导航路径失败";

    public const string STR_CONNECT_WITH_SERVER_FAILED = "无法连接到服务器!";
    public const string STR_UPLOAD_PATH_DATA_FINISHED = "上传路线信息完成!";
    public const string STR_WRITE_PATH_DATA_FAILED = "写入本地路线信息失败!";
    public const string STR_WRITE_PATH_DATA_FINISHED = "写入本地路线信息完成!";

    private static TypeAndParameter tap = null;
    public static TypeAndParameter instance
    {
        get
        {
            if (tap == null)
            {
                tap = new TypeAndParameter();
            }
            return tap;
        }
    }

    public TypeAndParameter() 
    {
        DEFAULT_MAP = "level1";
    }

    public static string GetDataPath()
    {
        string url =
#if UNITY_ANDROID       //安卓
        "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE      //iPhone
        Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR      //windows平台和web平台  
        "file://" + Application.dataPath + "/StreamingAssets/";  
#else  
        string.Empty;
#endif  
        return url;
    }

    public static string GetPersistentDataPath()
    {
        return Application.persistentDataPath + "/";
    }

    public static string GetSdcardDataPath()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        string path = "/sdcard/huiyoutianxia";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path + "/";
#else
        return GetPersistentDataPath();
#endif
    }
}

