using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxdwunity
{
    public class Parameters
    {
        // 方向枚举，用于指定格子的哪个相邻格
        public enum EDirection
        {
            DIR_R, DIR_D, DIR_L, DIR_U, DIR_RD, DIR_LD, DIR_LU, DIR_RU, DIR_NUM
        };

        // 鼠标枚举
        public enum MouseTypeEnum
        {
            LEFT = 0, RIGHT = 1, MIDDLE = 2
        }

        // 对话框返回值
        public enum EDialogReturnValue
        {
            DRV_YES, DRV_NO, DRV_OK, DRV_CANCEL, DRV_OPEN, DRV_SAVE, DRV_CLOSE
        }

        public static bool EngineerVersion              = true;
        public const string LIB_VERSION                 = "1.0";
        public const double SQRT_TWO                    = 1.4142135623731;
        public const float POSITIVE_INFINITE            = 9999.0f;
        public const float NEGATIVE_INFINITE            = -9999.0f;
        public const float GPS_CHNAGED_THRESHOLD        = 1.0E-05f;
        public const float LONG_TOUCH_INTERVAL          = 1.0f;         // 触发长按的秒数
        public const float MAX_EDGE_WEIGHT              = 9999.0f;
        public const float MIN_EDGE_WEIGHT              = 1.0f;
        public const float MIN_COORDINATE               = -9999.0f;
        public const float MAX_COORDINATE               = 9999.0f;
        public const float MIN_COORDINATE_OFFSET        = 0.01f;
        public const float DOT_RADIUS                   = 0.08f;
        public const int MAX_SEARCH_CELL_RADIUS         = 15;           // 搜索邻近格子的最大半径(格子数)
        public const int GUIDE_VOICE_TRIGGER_DISTANCE   = 10;         // 播放导游语音的距离权重阈值(格子数)
        public const int SLICE_WIDTH                    = 192;
        public const int SLICE_HEIGHT                   = 192;

        public const float MIN_SIZE_TRIGGER_ZOOM        = 2.2f;
        public const float ACTOR_MOVE_SPEED             = 3.0f;
        public const float SHORT_HINT_DELAY_SECONDS     = 5.0f;
        public const float MOVE_CAMERA_DELAY_SECONDS    = 3.0f;

        //----------------------------------------------------------------------
        public const string STR_APP_ROOT_DIRECTORY      = "hytx";
        public const string STR_PREFIX_SCENIC_SPOT      = "JD";
        public const string STR_PREFIX_EXPLAIN_SPOT     = "EX";
        public const string STR_PREFIX_NAVIGATE_SPOT    = "DH";
        public const string STR_PREFIX_MONITOR_POINT    = "MonitorPoint-";
        public const string STR_REACH_MAX_SIZE          = "已是最大地图";
        public const string STR_REACH_MIN_SIZE          = "已是最小地图";
        public const string STR_CSV_DUPLICATE_TITLE     = "CSV文件列名重复";
    }
}
