using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxdwunity.vo
{
    public class Role
    {
        public string ROLE_NUM;
        public string NAME;
        public int LEVEL;
        public int HP;
        public int HP_BASE;
        public double HP_ADD;
        public int ATK;
        public int ATK_BASE;
        public double ATK_ADD;
        public int DEF;
        public int DEF_BASE;
        public double DEF_ADD;
        public double SPD;
        public int ATK_JULI;
        public int JUMP;
        public int getHP()
        {
            return HP;
        }//返回角色当前生命值
        public int getATK()
        {
            return ATK;
        }//返回角色攻击力
        public int getDEF()
        {
            return DEF;
        }//返回角色防御力
        public int getLEVEL()
        {
            return LEVEL;
        }//返回角色等级
    }
}
