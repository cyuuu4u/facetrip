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



    public class PLAYER_Dao
    {
        private static PLAYER_Dao _dao = null;
        public static PLAYER_Dao Instance
        {
            get
            {
                if (_dao == null)
                    _dao = new PLAYER_Dao();
                return _dao;
            }
        }
        private CsvSheet cs;
    private CsvSheet cs2;
        public Player FindById(string PLAY_NUM)
        {
            this.cs = Document.Instance.GetSheet("ROLE");
        this.cs2 = Document.Instance.GetSheet("PLAYER");
            if (this.cs == null) return null;
        if (this.cs2 == null) return null;
            Player ss = new Player();
            DataRow[] drs = this.cs.Data.Select("ROLE_NUM='" + PLAY_NUM + "'");
        DataRow[] drs2 = this.cs2.Data.Select("PLAYER_NUM='" + PLAY_NUM + "'");
            if (drs.Length > 0 && drs2.Length >0)
            {
                DataRow dr = drs[0];
            DataRow dr2 = drs2[0];
                ss.PLAY_NUM = int.Parse(dr2["PLAYER_NUM"].ToString());
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
                ss.JUMP = int.Parse(dr["JUMP"].ToString());
            }
            return ss;
        }
    }

