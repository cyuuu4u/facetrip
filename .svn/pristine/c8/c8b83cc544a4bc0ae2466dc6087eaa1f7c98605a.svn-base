using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class ProtagonistDao
{
    private static ProtagonistDao _dao = null;
    public static ProtagonistDao Instance
    {
        get
        {
            if (_dao == null)
                _dao = new ProtagonistDao();
            return _dao;
        }
    }

    private CsvSheet cs;
    private ProtagonistDao()
    {
    }
    
    public List<ScenicSpot> GetAll(string mapName)
    {
        List<ScenicSpot> list = new List<ScenicSpot>();
        this.cs = Document.Instance.GetSheet(mapName);
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            ScenicSpot ss = new ScenicSpot();

            ss.Id = dr["Id"].ToString();
            ss.Name = dr["Name"].ToString();
            ss.Summary = dr["Summary"].ToString();
            ss.AudioUrl = dr["AudioUrl"].ToString();
            ss.AudioSegments = dr["AudioSegments"].ToString();
            ss.PictureUrl = dr["PictureUrl"].ToString();
            ss.StarLevel = dr["StarLevel"].ToString();
            ss.Kind = dr["Kind"].ToString();

            list.Add(ss);
        }

        return list;
    }

    public ScenicSpot FindById(string mapName, string id)
    {
        this.cs = Document.Instance.GetSheet(mapName);
        if (this.cs == null) return null;

        ScenicSpot ss = new ScenicSpot();
        DataRow[] drs = this.cs.Data.Select("Id='" + id + "'");
        if (drs.Length > 0)
        {
            DataRow dr = drs[0];
            ss.Id = dr["Id"].ToString();
            ss.Name = dr["Name"].ToString();
            ss.Summary = dr["Summary"].ToString();
            ss.AudioUrl = dr["AudioUrl"].ToString();
            ss.AudioSegments = dr["AudioSegments"].ToString();
            ss.PictureUrl = dr["PictureUrl"].ToString();
            ss.StarLevel = dr["StarLevel"].ToString();
            ss.Kind = dr["Kind"].ToString();
        }

        return ss;
    }

    public List<string> GetAllKinds(string mapName)
    {
        List<string> list = new List<string>();
        this.cs = Document.Instance.GetSheet(mapName);
        if (this.cs == null) return list;

        string others = "";
        DataRow[] drs = this.cs.Data.Select();
        foreach (DataRow dr in drs)
        {
            string tmp = dr["Kind"].ToString();
            int index = list.BinarySearch(tmp);
            if (index < 0)
            {
                if (tmp == "QT" || tmp == "Others")
                {
                    others = tmp;
                }
                else
                {
                    list.Insert(~index, tmp);
                }
            }            
        }
        if (others != "")
        {
            list.Add(others);
        }

        return list;
    }

    public List<ScenicSpot> FindAllOfKind(string mapName, string kind)
    {
        List<ScenicSpot> list = new List<ScenicSpot>();
        this.cs = Document.Instance.GetSheet(mapName);
        if (this.cs == null) return list;

        DataRow[] drs = this.cs.Data.Select("Kind='" + kind + "'");
        foreach (DataRow dr in drs)
        {
            ScenicSpot ss = new ScenicSpot();

            ss.Id = dr["Id"].ToString();
            ss.Name = dr["Name"].ToString();
            ss.Summary = dr["Summary"].ToString();
            ss.AudioUrl = dr["AudioUrl"].ToString();
            ss.AudioSegments = dr["AudioSegments"].ToString();
            ss.PictureUrl = dr["PictureUrl"].ToString();
            ss.StarLevel = dr["StarLevel"].ToString();
            ss.Kind = dr["Kind"].ToString();

            list.Add(ss);
        }

        return list;
    }
}
