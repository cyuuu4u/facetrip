using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class ITEMDao
{
    private static ITEMDao _dao = null;
    public static ITEMDao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new ITEMDao();
            return _dao;
        }
    }

    private CsvSheet cs;
    private ITEMDao()
    {

    }

    public List<ITEM> GetAll()
    {
        List<ITEM> list = new List<ITEM>();
        this.cs = Document.Instance.GetSheet("ITEM");
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            ITEM ss = new ITEM();

            ss.NUM = dr["NUM"].ToString();
            ss.NAME = dr["NAME"].ToString();
            ss.DAZE_TIME = int.Parse(dr["DAZE_TIME"].ToString());
            ss.POISON_TIME = int.Parse(dr["POISON_TIME"].ToString());
            ss.POISON_DAM = double.Parse(dr["POISON_DAM"].ToString());
            ss.FREEZE_TIME = int.Parse(dr["FREEZE_TIME"].ToString());
            ss.DAMAGE_PCT = int.Parse(dr["DAMAGE_PCT"].ToString());
            ss.TELEPORT = int.Parse(dr["TELEPORT"].ToString());
            ss.TARGET = int.Parse(dr["TARGET"].ToString());
            ss.USE_REP = bool.Parse(dr["USE_REP"].ToString());
            ss.CD = int.Parse(dr["CD"].ToString());
            ss.LV_UP = int.Parse(dr["LV_UP"].ToString());
            ss.LV_DOWN = int.Parse(dr["LV_DOWN"].ToString());
            list.Add(ss);
        }

        return list;
    }

    public ITEM FindById(string id)
    {
        XxdwDebugger.Log("hhhhhhhhhh");
        this.cs = Document.Instance.GetSheet("ITEM");
        if (this.cs == null) return null;

        ITEM ss = new ITEM();
        DataRow[] drs = this.cs.Data.Select("NUM='" + id + "'");
        if (drs.Length > 0)
        {
            XxdwDebugger.Log("hhhhhhhhhh");
            DataRow dr = drs[0];
            ss.NUM = dr["NUM"].ToString();
            ss.NAME = dr["NAME"].ToString();
            ss.DAZE_TIME = int.Parse(dr["DAZE_TIME"].ToString());
            ss.POISON_TIME = int.Parse(dr["POISON_TIME"].ToString());
            ss.POISON_DAM = int.Parse(dr["POISON_DAM"].ToString());
            ss.FREEZE_TIME = int.Parse(dr["FREEZE_TIME"].ToString());
            ss.DAMAGE_PCT = int.Parse(dr["DAMAGE_PCT"].ToString());
            ss.TELEPORT = int.Parse(dr["TELEPORT"].ToString());
            ss.TARGET = int.Parse(dr["TARGET"].ToString());
            ss.USE_REP = bool.Parse(dr["USE_REP"].ToString());
            ss.CD = int.Parse(dr["CD"].ToString());
            ss.LV_UP = int.Parse(dr["LV_UP"].ToString());
            ss.LV_DOWN = int.Parse(dr["LV_DOWN"].ToString());
        }

        return ss;
    }
}

