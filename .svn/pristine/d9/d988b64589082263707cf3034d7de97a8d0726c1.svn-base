using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class TIGER_ITEMDao
{
    private static TIGER_ITEMDao _dao = null;
    public static TIGER_ITEMDao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new TIGER_ITEMDao();
            return _dao;
        }
    }

    private CsvSheet cs;
    private TIGER_ITEMDao()
    {

    }

    public List<TIGER_ITEM> GetAll()
    {
        List<TIGER_ITEM> list = new List<TIGER_ITEM>();
        this.cs = Document.Instance.GetSheet("TIGER_ITEM");
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            TIGER_ITEM ss = new TIGER_ITEM();

            ss.TIGER_NUM = dr["TIGER_NUM"].ToString();
            ss.ITEM_NUM = dr["ITEM_NUM"].ToString();
            ss.PCT = double.Parse(dr["PCT"].ToString());
            list.Add(ss);
        }

        return list;
    }

    public TIGER_ITEM FindById(string id1,string id2)
    {
        this.cs = Document.Instance.GetSheet("TIGER_ITEM");
        if (this.cs == null) return null;

        TIGER_ITEM ss = new TIGER_ITEM();
        DataRow[] drs = this.cs.Data.Select("TIGER_NUM='" + id1 + "'and ITEM_NUM='" + id2 + "'");
        if (drs.Length > 0)
        {
            DataRow dr = drs[0];
            ss.TIGER_NUM = dr["TIGER_NUM"].ToString();
            ss.ITEM_NUM = dr["ITEM_NUM"].ToString();
            ss.PCT = double.Parse(dr["PCT"].ToString());
        }

        return ss;
    }
}