using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class TIGER_MACHINEDao
{
    private static TIGER_MACHINEDao _dao = null;
    public static TIGER_MACHINEDao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new TIGER_MACHINEDao();
            return _dao;
        }
    }

    private CsvSheet cs;
    private TIGER_MACHINEDao()
    {

    }

    public List<TIGER_MACHINE> GetAll()
    {
        List<TIGER_MACHINE> list = new List<TIGER_MACHINE>();
        this.cs = Document.Instance.GetSheet("TIGER_MACHINE");
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            TIGER_MACHINE ss = new TIGER_MACHINE();

            ss.NUM = dr["NUM"].ToString();
            ss.GOLD = int.Parse(dr["GOLD"].ToString());
            ss.HP = double.Parse(dr["HP"].ToString());
            ss.NULL_PCT = double.Parse(dr["NULL_PCT"].ToString());
            list.Add(ss);
        }

        return list;
    }

    public TIGER_MACHINE FindById(string id)
    {
        this.cs = Document.Instance.GetSheet("TIGER_MACHINE");
        if (this.cs == null) return null;

        TIGER_MACHINE ss = new TIGER_MACHINE();
        DataRow[] drs = this.cs.Data.Select("NUM='" + id + "'");
        if (drs.Length > 0)
        {
            DataRow dr = drs[0];
            ss.NUM = dr["NUM"].ToString();
            ss.GOLD = int.Parse(dr["GOLD"].ToString());
            ss.HP = double.Parse(dr["HP"].ToString());
            ss.NULL_PCT = double.Parse(dr["NULL_PCT"].ToString());
        }

        return ss;
    }
}