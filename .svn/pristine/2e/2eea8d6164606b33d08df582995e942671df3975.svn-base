using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;
using com.hytxworld.comm;


   public class BOSS_Dao
    {
        private static BOSS_Dao _dao = null;
        public static BOSS_Dao Instance
        {
            get
            {
                if (_dao == null)
                    _dao = new BOSS_Dao();
                return _dao;
            }
        }
        private CsvSheet cs;
        public Boss FindById(string BOSS_NUM)
        {
            this.cs = Document.Instance.GetSheet("BOSS");
            if (this.cs == null) return null;
            Boss ss = new Boss();
            DataRow[] drs = this.cs.Data.Select("BOSS_NUM='" + BOSS_NUM + "'");
            if (drs.Length > 0)
            {
                DataRow dr = drs[0];
                ss.BOSS_NUM = int.Parse(dr["BOSS_NUM"].ToString());
                ss.NAME = dr["NAME"].ToString();
                ss.LEVEL = int.Parse(dr["LEVEL"].ToString());
                ss.HP = int.Parse(dr["HP"].ToString());
                ss.HP_BASE = int.Parse(dr["HP_BASE"].ToString());
                ss.HP_ADD = double.Parse(dr["HP_ADD"].ToString());
                ss.ATK = int.Parse(dr["ATK"].ToString());
                ss.ATK_BASE = int.Parse(dr["ATK_BASE"].ToString());
                ss.ATK_ADD = double.Parse(dr["ATK_ADD"].ToString());
                ss.SPD = int.Parse(dr["SPD"].ToString());
                ss.ATK_JULI = int.Parse(dr["ATK_JULI"].ToString());
                ss.JUMP = int.Parse(dr["JUMO"].ToString());
                ss.GOLD = int.Parse(dr["GOLD"].ToString());
                ss.MOVE_RULE = int.Parse(dr["MOVE_RULE"].ToString());
                ss.ITEM_NUM = int.Parse(dr["ITEM_NUM"].ToString());
            }
            return ss;
        }
    }

