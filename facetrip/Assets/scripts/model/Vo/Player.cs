using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxdwunity.vo
{
    public class Player : Role
    {
        public int PLAY_NUM;
        public Player()
        {
            LEVEL = 1;
            HP = HP_BASE = 150;
            HP_ADD = 1.3;
            ATK = ATK_BASE = 20;
            ATK_ADD = 1.45;
            DEF = DEF_BASE = 8;
            DEF_ADD = 1.32;
        }
    }
}
