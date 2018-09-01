using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class BUFFDao
{
    private static BUFFDao _dao = null;
    public static BUFFDao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new BUFFDao();
            return _dao;
        }
    }

    private CsvSheet cs;
    private BUFFDao()
    {

    }

    public List<BUFF> GetAll()
    {
        List<BUFF> list = new List<BUFF>();
        this.cs = Document.Instance.GetSheet("BUFF");
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            BUFF ss = new BUFF();

            ss.NUM = dr["NUM"].ToString();
            ss.NAME = dr["NAME"].ToString();
            ss.LEVEL = int.Parse(dr["LEVEL"].ToString());
            ss.ATK_UP = double.Parse(dr["ATK_UP"].ToString());
            ss.SPD_UP = double.Parse(dr["SPD_UP"].ToString());
            ss.DEF_UP = double.Parse(dr["DEF_UP"].ToString());
            ss.HPR_PCT = double.Parse(dr["HRP_PCT"].ToString());
            ss.CS_PCT = double.Parse(dr["CS_PCT"].ToString());
            ss.QG = bool.Parse(dr["QG"].ToString());
            ss.TIME = int.Parse(dr["TIME"].ToString());
            ss.BBACK_DIST = int.Parse(dr["BBACK_DIST"].ToString());
            ss.TRIGGER_TIME = int.Parse(dr["TRIGGER_TIME"].ToString());
            ss.TRIGGER_PCT = double.Parse(dr["TRIGGER_PCT"].ToString());
            ss.FREEZE_TIME = int.Parse(dr["FREEZE_TIME"].ToString());
            ss.POISON_TIME = int.Parse(dr["POISON_TIME"].ToString());
            ss.POISON_DAM = double.Parse(dr["POISON_DAM"].ToString());
            ss.DAZE_TIME = int.Parse(dr["DAZE_TIME"].ToString());
            ss.CD_DOWN = double.Parse(dr["CD_DOWN"].ToString());
            ss.DEAD = bool.Parse(dr["DEAD"].ToString());
            ss.CHUNGE = bool.Parse(dr["CHUNGE"].ToString());
            ss.ARMOR = double.Parse(dr["ARMOR"].ToString());
            ss.JUMP_UP = int.Parse(dr["JUMP_UP"].ToString());
            ss.LV = dr["LV"].ToString();
            list.Add(ss);
        }

        return list;
    }

    public BUFF FindById(string id)
    {
        this.cs = Document.Instance.GetSheet("BUFF");
        if (this.cs == null) return null;

        BUFF ss = new BUFF();
        DataRow[] drs = this.cs.Data.Select("NUM='" + id + "'");
        if (drs.Length > 0)
        {
            DataRow dr = drs[0];
            ss.NUM = dr["NUM"].ToString();
            ss.NAME = dr["NAME"].ToString();
            ss.LEVEL = int.Parse(dr["LEVEL"].ToString());
            ss.ATK_UP = double.Parse(dr["ATK_UP"].ToString());
            ss.SPD_UP = double.Parse(dr["SPD_UP"].ToString());
            ss.DEF_UP = double.Parse(dr["DEF_UP"].ToString());
            ss.HPR_PCT = double.Parse(dr["HRP_PCT"].ToString());
            ss.CS_PCT = double.Parse(dr["CS_PCT"].ToString());
            ss.QG = bool.Parse(dr["QG"].ToString());
            ss.TIME = int.Parse(dr["TIME"].ToString());
            ss.BBACK_DIST = int.Parse(dr["BBACK_DIST"].ToString());
            ss.TRIGGER_TIME = int.Parse(dr["TRIGGER_TIME"].ToString());
            ss.TRIGGER_PCT = double.Parse(dr["TRIGGER_PCT"].ToString());
            ss.FREEZE_TIME = int.Parse(dr["FREEZE_TIME"].ToString());
            ss.POISON_TIME = int.Parse(dr["POISON_TIME"].ToString());
            ss.POISON_DAM = int.Parse(dr["POISON_DAM"].ToString());
            ss.DAZE_TIME = int.Parse(dr["DAZE_TIME"].ToString());
            ss.CD_DOWN = double.Parse(dr["CD_DOWN"].ToString());
            ss.DEAD = bool.Parse(dr["DEAD"].ToString());
            ss.CHUNGE = bool.Parse(dr["CHUNGE"].ToString());
            ss.ARMOR = int.Parse(dr["ARMOR"].ToString());
            ss.JUMP_UP = int.Parse(dr["JUMP_UP"].ToString());
            ss.LV = dr["LV"].ToString();
        }

        return ss;
    }
}


