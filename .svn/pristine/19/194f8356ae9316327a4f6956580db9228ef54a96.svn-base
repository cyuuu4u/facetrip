using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class TIGER_BUFFDao
{
    private static TIGER_BUFFDao _dao = null;
    public static TIGER_BUFFDao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new TIGER_BUFFDao();
            return _dao;
        }
    }

    private CsvSheet cs;
    private TIGER_BUFFDao()
    {

    }

    public List<TIGER_BUFF> GetAll()
    {
        List<TIGER_BUFF> list = new List<TIGER_BUFF>();
        this.cs = Document.Instance.GetSheet("TIGER_BUFF");
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            TIGER_BUFF ss = new TIGER_BUFF();

            ss.TIGER_NUM = dr["TIGER_NUM"].ToString();
            ss.BUFF_NUM = dr["BUFF_NUM"].ToString();
            ss.PCT = double.Parse(dr["PCT"].ToString());
            list.Add(ss);
        }

        return list;
    }

    public TIGER_BUFF FindById(string id1,string id2)
    {
        this.cs = Document.Instance.GetSheet("TIGER_BUFF");
        if (this.cs == null) return null;

        TIGER_BUFF ss = new TIGER_BUFF();
        DataRow[] drs = this.cs.Data.Select("TIGER_NUM = '" + id1 + "' and BUFF_NUM = '" + id2 + "'"); 
        if (drs.Length > 0)
        {
            DataRow dr = drs[0];
            ss.TIGER_NUM = dr["TIGER_NUM"].ToString();
            ss.BUFF_NUM = dr["BUFF_NUM"].ToString();
            ss.PCT = double.Parse(dr["PCT"].ToString());
        }

        return ss;
    }
}
