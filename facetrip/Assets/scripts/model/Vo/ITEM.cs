using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
    public class ITEM
    {
        public string NUM;//道具编号
        public string NAME;//道具名
        public int DAZE_TIME;//眩晕时间
        public int POISON_TIME;//中毒时间
        public double POISON_DAM;//每秒中毒伤害（使用者攻击百分比）
        public int FREEZE_TIME;//冰冻时间
        public int DAMAGE_PCT;//伤害倍率
        public int TELEPORT;//闪现距离
        public int TARGET;//作用目标
        public bool USE_REP;//是否可重复使用
        public int CD;//冷却时间
        public int LV_UP;//等级+1
        public int LV_DOWN;//等级-1
    }