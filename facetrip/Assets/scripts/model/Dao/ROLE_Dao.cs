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


    public class ROLE_Dao
    {
    private static ROLE_Dao _dao = null;
    public static ROLE_Dao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new ROLE_Dao();
            return _dao;
        }
    }
    private CsvSheet cs;
    public Role FindroleById(string ROLE_NUM)
        {
            this.cs = Document.Instance.GetSheet("ROLE");
            if (this.cs == null) return null;
            Role ss = new Role();
            DataRow[] drs = this.cs.Data.Select("ROLE_NUM='" + ROLE_NUM + "'");
            if (drs.Length > 0)
            {
                DataRow dr = drs[0];
                ss.ROLE_NUM = dr["ROLE_NUM"].ToString();
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


