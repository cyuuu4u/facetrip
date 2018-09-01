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

    public class MONSTER_Dao
    {
        private static MONSTER_Dao _dao = null;
        public static MONSTER_Dao Instance
        {
            get
            {
                if (_dao == null)
                    _dao = new MONSTER_Dao();
                return _dao;
            }
        }
        private CsvSheet cs;
    private CsvSheet cs2;
    public Monster FindById(string MONSTER_NUM)
        {
            this.cs = Document.Instance.GetSheet("MONSTER");
        this.cs2 = Document.Instance.GetSheet("ROLE");
        if (this.cs == null) return null;
        if (this.cs2 == null) return null;
        Monster ss = new Monster();
            DataRow[] drs = this.cs.Data.Select("MONSTER_NUM='" + MONSTER_NUM + "'");
        DataRow[] drs2 = this.cs2.Data.Select("ROLE_NUM='" + MONSTER_NUM + "'");
        if (drs.Length > 0 && drs2.Length > 0)
            {
                DataRow dr = drs[0];
            DataRow dr2 = drs2[0];
            ss.MONSTER_NUM = int.Parse(dr["MONSTER_NUM"].ToString());
                ss.NAME = dr2["NAME"].ToString();
                ss.LEVEL = int.Parse(dr2["LEVEL"].ToString());
                ss.HP = int.Parse(dr2["HP"].ToString());
                ss.HP_BASE = int.Parse(dr2["HP_BASE"].ToString());
                ss.HP_ADD = double.Parse(dr2["HP_ADD"].ToString());
                ss.ATK = int.Parse(dr2["ATK"].ToString());
                ss.ATK_BASE = int.Parse(dr2["ATK_BASE"].ToString());
                ss.ATK_ADD = double.Parse(dr2["ATK_ADD"].ToString());
                ss.SPD = double.Parse(dr2["SPD"].ToString());
                ss.ATK_JULI = int.Parse(dr2["ATK_JULI"].ToString());
                ss.JUMP = int.Parse(dr2["JUMP"].ToString());
                ss.GOLD = int.Parse(dr["GOLD"].ToString());
                ss.MOVE_RULE = int.Parse(dr["MOVE_RULE"].ToString());
            }
            return ss;
        }
    }


