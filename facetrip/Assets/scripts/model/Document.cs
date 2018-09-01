﻿using UnityEngine;
using System;
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

public class Document
{
    private static Document document = null;    
    public static Document Instance
    {
        get
        {
            if (document == null)
            {
                document = new Document();
            }
            return document;
        }
    }

    private Hashtable gpsMaps;
    private Hashtable csvSheets;
    private string currentMapName;
    public int MonsterHP
    {
        get;
        set;
    }
    public int MonsterATK
    {
        get;
        set;
    }
    public int MonsterDEF
    {
        get;
        set;
    }
    public int MonsterKilled
    {
        get;
        set;
    }
    public int MonsterHPB
    {
        get;
        set;
    }
    public string CurrentMapName
    {
        get { return this.currentMapName; } 
    }

    public GpsMap Map
    {
        get { return (GpsMap)this.gpsMaps[this.currentMapName]; }
    }
    public int levelnum
    {
        get;
        set;
    }
    public Document()
    {
        this.gpsMaps = new Hashtable();
        this.csvSheets = new Hashtable();
        this.currentMapName = "";
        MonsterKilled = 0;
        levelnum = 1;
    }

    public void SwitchMap(string mapName)
    {
        XxdwDebugger.Log("Document: SwitchMap() ...");

        if (!this.gpsMaps.ContainsKey(mapName))
        {
            this.currentMapName = mapName;
            this.LoadMapData(mapName);
            this.LoadCsvSheetData(mapName);
            this.LoadDbSheetData("ITEM");
            this.LoadDbSheetData("BUFF");
            this.LoadDbSheetData("TIGER_MACHINE");
            this.LoadDbSheetData("TIGER_ITEM");
            this.LoadDbSheetData("TIGER_BUFF");
            this.LoadDbSheetData("ROLE");
            this.LoadDbSheetData("PLAYER");
            this.LoadDbSheetData("MONSTER");
            this.LoadDbSheetData("BOSS");
        }
        else if (this.currentMapName != mapName)
        {
            this.currentMapName = mapName;
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_SWITCHED, this.gpsMaps[mapName]);
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_INFO_READY, GpsMap.SafeStatus.LS_INFO_DATA, mapName);
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_DATA_READY, GpsMap.SafeStatus.LS_PATH_DATA, mapName);
        }
        else
        {
            NotificationCenter.Instance.SendNotification(this, Notification.NOTI_MAP_SWITCHED, this.gpsMaps[mapName]); // 立即触发
            // 从loading初始场景中过来时，NOTI_MAP_INFO_READY已被loading场景处理，ditu场景需初始化
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_INFO_READY, GpsMap.SafeStatus.LS_INFO_DATA, mapName);
        }
    }

    public void LoadMapData(string mapName)
    {
        TextAsset ta = Resources.Load(mapName + "/" + TypeAndParameter.MAP_DATA_FILE, typeof(TextAsset)) as TextAsset;
        if (ta != null)
        {
            XxdwDebugger.Log("Document: LoadMapData() source ok -> " + mapName);
            this.gpsMaps[mapName] = new GpsMap();
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MAP_SWITCHED, this.gpsMaps[mapName]);            

            ((GpsMap)(this.gpsMaps[mapName])).LoadData(ta.text);
        }
        else
        {
            XxdwDebugger.Log("Document: LoadMapData() no source ->" + mapName);
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_SET_HINT, TypeAndParameter.STR_LOAD_MAP_RESOURCE_FAILED);
        }
    }

    public void LoadCsvSheetData(string mapName)
    {
        TextAsset ta = Resources.Load(mapName + "/" + TypeAndParameter.SCENIC_SPOT_SHEET_FILE, typeof(TextAsset)) as TextAsset;
        if (ta != null)
        {
            XxdwDebugger.Log("Document: LoadCsvSheetData() source ok -> " + mapName);
            this.csvSheets[mapName] = new CsvSheet();
            ((CsvSheet)(this.csvSheets[mapName])).LoadData(mapName, ta.text);
        }
        else
        {
            XxdwDebugger.Log("Document: LoadCsvSheetData() no source ->" + mapName);
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_SET_HINT, TypeAndParameter.STR_LOAD_SCENIC_SPOT_RESOURCE_FAILED);
        }
    }
    public void LoadDbSheetData(string filename)
    {
        TextAsset ta = Resources.Load("db/" + filename, typeof(TextAsset)) as TextAsset;
        if (ta != null)
        {
            XxdwDebugger.Log("Document: LoadDbSheetData() source ok -> " + filename);
            this.csvSheets[filename] = new CsvSheet();
            ((CsvSheet)(this.csvSheets[filename])).LoadData(filename, ta.text);
        }
        else
        {
            XxdwDebugger.Log("Document: LoadDbSheetData() no source ->" + filename);
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_SET_HINT, TypeAndParameter.STR_LOAD_SCENIC_SPOT_RESOURCE_FAILED);
        }
    }

    public CsvSheet GetSheet(string mapName)
    {
        if (!this.csvSheets.ContainsKey(mapName))return null;

        return this.csvSheets[mapName] as CsvSheet;
    }

    //////////////////////////////////////////////////////////////////////////

    public List<string> GetAllScenicSpotsKinds()
    {
        return ProtagonistDao.Instance.GetAllKinds(this.currentMapName);
    }

    public List<ScenicSpot> GetAllScenicSpots()
    {
        return ProtagonistDao.Instance.GetAll(this.currentMapName);
    }

    public List<ScenicSpot> FindAllScenicSpotsOfKind(string kind)
    {
        return ProtagonistDao.Instance.FindAllOfKind(this.currentMapName, kind);
    }

    public ScenicSpot FindScenicSpotById(string id)
    {
        return ProtagonistDao.Instance.FindById(this.currentMapName, id);
    }

    //////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////
    public Player FindPlayerById(string id)
    {
        return PLAYER_Dao.Instance.FindById(id);
    }
    public Monster FindMonsterById(string id)
    {
        return MONSTER_Dao.Instance.FindById(id);
    }
    public Boss FindBossById(string id)
    {
        return BOSS_Dao.Instance.FindById(id);
    }
    public Skill FindSkillById(string id)
    {
        return SKILL_Dao.Instance.FindById(id);
    }

    public int GetDeg(Player a, Monster b)
    {
        int Deg = a.ATK - b.DEF;
        b.HP -= Deg;
        return Deg;
    }
    public int GetDeg(Monster a, Player b)
    {
        int Deg = a.ATK - b.DEF;
        b.HP -= Deg;
        return Deg;
    }
    public int GetDeg(Player a, Boss b)
    {
        int Deg = a.ATK - b.DEF;
        b.HP -= Deg;
        return Deg;
    }
    public int GetDeg(Boss a, Player b)
    {
        int Deg = a.ATK - b.DEF;
        b.HP -= Deg;
        return Deg;
    }
    public ITEM FindItemById(string id)
    {
        return ITEMDao.Instance.FindById(id);
    }
    public BUFF FindBuffById(string id)
    {
        return BUFFDao.Instance.FindById(id);
    }
    public TIGER_MACHINE FindTigerMachineById(string id)
    {
        return TIGER_MACHINEDao.Instance.FindById(id);
    }
    public int GetItemAmount()
    {
        List<ITEM> item = ITEMDao.Instance.GetAll();
        return item.Count;
    }
    public int GetBuffAmount()
    {
        List<BUFF> buff = BUFFDao.Instance.GetAll();
        return buff.Count;
    }
    //////////////////////////////////////////////////////////////////////////
    public string Getcode(string TigerNum)
    {
        if (TigerNum == "T01")
        {
            if (this.player.HP - (int)(this.player.HP_BASE * TIGER_MACHINEDao.Instance.FindById("T01").HP) <= 0)
            {
                return "X";
            }
            else
            {
                this.player.HP = this.player.HP - (int)(this.player.HP_BASE * TIGER_MACHINEDao.Instance.FindById("T01").HP);
                System.Random ran = new System.Random();
                int IfNull = ran.Next(1, 100);
                if (IfNull < (TIGER_MACHINEDao.Instance.FindById("T01").NULL_PCT) * 100)
                {
                    return "NULL";
                }
                else
                {
                    System.Random ran2 = new System.Random();
                    double Get = ran2.Next(1, 100000);
                    List<ITEM> item = ITEMDao.Instance.GetAll();
                    List<BUFF> buff = BUFFDao.Instance.GetAll();
                    double basis = 0;
                    for (int i = 0; i < this.GetBuffAmount(); i++)
                    {
                        //string a = buff[i].NUM;
                        //string b = buff[i].NUM;
                        if (Get > basis && Get < (basis + (TIGER_BUFFDao.Instance.FindById("T01", buff[i].NUM).PCT) * 100000))
                        {
                            return buff[i].NUM;
                        }
                        basis = basis + (TIGER_BUFFDao.Instance.FindById("T01", buff[i].NUM).PCT) * 100000;
                    }
                    for (int k = 0; k < this.GetItemAmount(); k++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_ITEMDao.Instance.FindById("T01", item[k].NUM).PCT) * 100000))
                        {
                            return item[k].NUM;

                        }
                        basis = basis + (TIGER_ITEMDao.Instance.FindById("T01", buff[k].NUM).PCT) * 100000;
                    }
                }
            }
        }
        else if (TigerNum == "T02")
        {
            if (this.player.HP - (int)(this.player.HP_BASE * TIGER_MACHINEDao.Instance.FindById("T02").HP) <= 0)
            {
                return "X";
            }
            else
            {
                this.player.HP = this.player.HP - (int)(this.player.HP_BASE * TIGER_MACHINEDao.Instance.FindById("T02").HP);
                System.Random ran = new System.Random();
                int IfNull = ran.Next(1, 100);
                if (IfNull < (TIGER_MACHINEDao.Instance.FindById("T02").NULL_PCT) * 100)
                {
                    return "NULL";
                }
                else
                {
                    System.Random ran2 = new System.Random();
                    double Get = ran2.Next(1, 100000);
                    List<ITEM> item = ITEMDao.Instance.GetAll();
                    List<BUFF> buff = BUFFDao.Instance.GetAll();
                    double basis = 0;
                    for (int i = 0; i < this.GetBuffAmount(); i++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_BUFFDao.Instance.FindById("T02", buff[i].NUM).PCT) * 100000))
                        {
                            return buff[i].NUM;

                        }
                        basis = basis + (TIGER_BUFFDao.Instance.FindById("T02", buff[i].NUM).PCT) * 100000;
                    }
                    for (int k = 0; k < this.GetItemAmount(); k++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_ITEMDao.Instance.FindById("T02", item[k].NUM).PCT) * 100000))
                        {
                            return item[k].NUM;
                        }
                        basis = basis + (TIGER_ITEMDao.Instance.FindById("T02", buff[k].NUM).PCT) * 100000;
                    }
                }
            }
        }
        else if (TigerNum == "T03")
        {
            if (this.player.HP - (int)(this.player.HP_BASE * TIGER_MACHINEDao.Instance.FindById("T03").HP) <= 0)
            {
                return "X";
            }
            else
            {
                this.player.HP = this.player.HP - (int)(this.player.HP_BASE * TIGER_MACHINEDao.Instance.FindById("T03").HP);
                System.Random ran = new System.Random();
                int IfNull = ran.Next(1, 100);
                if (IfNull < (TIGER_MACHINEDao.Instance.FindById("T03").NULL_PCT) * 100)
                {
                    return "NULL";
                }
                else
                {
                    System.Random ran2 = new System.Random();
                    double Get = ran2.Next(1, 100000);
                    List<ITEM> item = ITEMDao.Instance.GetAll();
                    List<BUFF> buff = BUFFDao.Instance.GetAll();
                    double basis = 0;
                    for (int i = 0; i < this.GetBuffAmount(); i++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_BUFFDao.Instance.FindById("T03", buff[i].NUM).PCT) * 100000))
                        {
                            return buff[i].NUM;

                        }
                        basis = basis + (TIGER_BUFFDao.Instance.FindById("T03", buff[i].NUM).PCT) * 100000;
                    }
                    for (int k = 0; k < this.GetItemAmount(); k++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_ITEMDao.Instance.FindById("T03", item[k].NUM).PCT) * 100000))
                        {
                            return item[k].NUM;

                        }
                        basis = basis + (TIGER_ITEMDao.Instance.FindById("T03", buff[k].NUM).PCT) * 100000;
                    }
                }
            }
        }
        else if (TigerNum == "T04")
        {
            if (this.Gold - TIGER_MACHINEDao.Instance.FindById("T04").GOLD < 0)
            {
                return "X";
            }
            else
            {
                this.Gold = this.Gold - TIGER_MACHINEDao.Instance.FindById("T04").GOLD;
                System.Random ran = new System.Random();
                int IfNull = ran.Next(1, 100);
                if (IfNull < (TIGER_MACHINEDao.Instance.FindById("T04").NULL_PCT) * 100)
                {
                    return "NULL";
                }
                else
                {
                    System.Random ran2 = new System.Random();
                    double Get = ran2.Next(1, 100000);
                    List<ITEM> item = ITEMDao.Instance.GetAll();
                    List<BUFF> buff = BUFFDao.Instance.GetAll();
                    double basis = 0;
                    for (int i = 0; i < this.GetBuffAmount(); i++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_BUFFDao.Instance.FindById("T04", buff[i].NUM).PCT) * 100000))
                        {
                            return buff[i].NUM;

                        }
                        basis = basis + (TIGER_BUFFDao.Instance.FindById("T04", buff[i].NUM).PCT) * 100000;
                    }
                    for (int k = 0; k < this.GetItemAmount(); k++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_ITEMDao.Instance.FindById("T04", item[k].NUM).PCT) * 100000))
                        {
                            return item[k].NUM;

                        }
                        basis = basis + (TIGER_ITEMDao.Instance.FindById("T04", buff[k].NUM).PCT) * 100000;
                    }
                }
            }
        }
        else if (TigerNum == "T05")
        {
            if (this.Gold - TIGER_MACHINEDao.Instance.FindById("T05").GOLD < 0)
            {
                return "X";
            }
            else
            {
                this.Gold = this.Gold - TIGER_MACHINEDao.Instance.FindById("T05").GOLD;
                System.Random ran = new System.Random();
                int IfNull = ran.Next(1, 100);
                if (IfNull < (TIGER_MACHINEDao.Instance.FindById("T05").NULL_PCT) * 100)
                {
                    return "NULL";
                }
                else
                {
                    System.Random ran2 = new System.Random();
                    double Get = ran2.Next(1, 100000);
                    List<ITEM> item = ITEMDao.Instance.GetAll();
                    List<BUFF> buff = BUFFDao.Instance.GetAll();
                    double basis = 0;
                    for (int i = 0; i < this.GetBuffAmount(); i++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_BUFFDao.Instance.FindById("T05", buff[i].NUM).PCT) * 100000))
                        {
                            return buff[i].NUM;

                        }
                        basis = basis + (TIGER_BUFFDao.Instance.FindById("T05", buff[i].NUM).PCT) * 100000;
                    }
                    for (int k = 0; k < this.GetItemAmount(); k++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_ITEMDao.Instance.FindById("T05", item[k].NUM).PCT) * 100000))
                        {
                            return item[k].NUM;
                        }
                        basis = basis + (TIGER_ITEMDao.Instance.FindById("T05", buff[k].NUM).PCT) * 100000;
                    }
                }
            }
        }
        else
        {
            if (this.Gold - TIGER_MACHINEDao.Instance.FindById("T06").GOLD < 0)
            {
                return "X";
            }
            else
            {
                this.Gold = this.Gold - TIGER_MACHINEDao.Instance.FindById("T06").GOLD;
                System.Random ran = new System.Random();
                int IfNull = ran.Next(1, 100);
                if (IfNull < (TIGER_MACHINEDao.Instance.FindById("T06").NULL_PCT) * 100)
                {
                    return "NULL";
                }
                else
                {
                    System.Random ran2 = new System.Random();
                    double Get = ran2.Next(1, 100000);
                    List<ITEM> item = ITEMDao.Instance.GetAll();
                    List<BUFF> buff = BUFFDao.Instance.GetAll();
                    double basis = 0;
                    for (int i = 0; i < this.GetBuffAmount(); i++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_BUFFDao.Instance.FindById("T06", buff[i].NUM).PCT) * 100000))
                        {
                            return buff[i].NUM;
                        }
                        basis = basis + (TIGER_BUFFDao.Instance.FindById("T06", buff[i].NUM).PCT) * 100000;
                    }
                    for (int k = 0; k < this.GetItemAmount(); k++)
                    {
                        if (Get > basis && Get < (basis + (TIGER_ITEMDao.Instance.FindById("T06", item[k].NUM).PCT) * 100000))
                        {
                            return item[k].NUM;
                        }
                        basis = basis + (TIGER_ITEMDao.Instance.FindById("T06", buff[k].NUM).PCT) * 100000;
                    }
                }
            }
        }
        return "ERROR";
    }
    /////////////////////////////////////////////////////
    public string OpenTigerMachine(string code)
    {
        if (code.Substring(0, 1) == "N" || code.Substring(0, 1) == "E")
        {
            XxdwDebugger.Log("NULL");
            return "NULL";
        }
        else if (code.Substring(0, 1) == "I")
        {
            if (code.Substring(1, 2) == "03" || code.Substring(1, 2) == "05" || code.Substring(1, 2) == "07")
            {
                XxdwDebugger.Log("I01");
                return "I01";
            }
            else if (code.Substring(1, 2) == "04" || code.Substring(1, 2) == "06")
            {
                XxdwDebugger.Log("I02");
                return "I02";
            }
            else
            {
                XxdwDebugger.Log(code);
                return code;
            }
        }
        else if (code.Substring(0, 1) == "B")
        {
            if (code.Substring(1, 2) == "02" || code.Substring(1, 2) == "04" || code.Substring(1, 2) == "07" || code.Substring(1, 2) == "08" || code.Substring(1, 2) == "09" || code.Substring(1, 2) == "12" || code.Substring(1, 2) == "13" || code.Substring(1, 2) == "14")
            {
                XxdwDebugger.Log("B01");
                return "B01";
            }
            else if (code.Substring(1, 2) == "05" || code.Substring(1, 2) == "06" || code.Substring(1, 2) == "10" || code.Substring(1, 2) == "11" || code.Substring(1, 2) == "15" || code.Substring(1, 2) == "16" || code.Substring(1, 2) == "17")
            {
                XxdwDebugger.Log("B03");
                return "B03";
            }
            else
            {
                XxdwDebugger.Log(code);
                return code;
            }
        }
        return "NULL";
    }
    ////////////////////////////////////////////////////////
    public Player player;
    public Monster[] monster = new Monster[5];
    private int gold;
    public int Gold
    {
        set { gold = value; }
        get { return gold; }
    }

    public int getPlayerHP()
    {
        return player.HP;
    }
    public void setPlayerHP(int a)
    {
        player.HP=a;
    }

    public int getPlayerLEVEL()
    {
        return player.LEVEL;
    }
    public void setPlayerLEVEL(int a)
    {
        player.LEVEL = a;
    }
    public int getPlayerATK()
    {
        return player.ATK;
    }
    public void setPlayerATK(int a)
    {
        player.ATK = a;
    }
    public int getPlayerDEF()
    {
        return player.DEF;
    }
    public void setPlayerDEF(int a)
    {
        player.DEF = a;
    }
    public double getPlayerSPD()
    {
        return player.SPD;
    }
    public void setPlayerSPD(double a)
    {
        player.SPD = a;
    }
    public int getPlayerJUMP()
    {
        return player.JUMP;
    }
    public void setPlayerJUMP(int a)
    {
        player.JUMP = a;
    }
    public void GetBuff(string code)
    {
        BUFF getbuff = this.FindBuffById(code);
        this.player.ATK = this.player.ATK + (int)(this.player.ATK * getbuff.ATK_UP);
        if (this.player.HP + (int)(this.player.HP * getbuff.HPR_PCT) > this.player.HP_BASE)
        {
            this.player.HP = this.player.HP_BASE;
        }
        else if (this.player.HP + (int)(this.player.HP * getbuff.HPR_PCT) < this.player.HP_BASE)
        {
            this.player.HP = this.player.HP + (int)(this.player.HP * getbuff.HPR_PCT);
        }
    }
    public ITEM playeritem;
    public int checkitem()
    {
        if (this.playeritem.NUM == null)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public void GetItem(string code)
    {
        if (this.checkitem() == 1)
            this.playeritem = FindItemById(code);
        else if (this.checkitem() == 0)
            XxdwDebugger.Log("item error");
    }
    public void ThrowItem()
    {
        if (this.checkitem() == 1)
            XxdwDebugger.Log("throw error");
        else if (this.checkitem() == 0)
            this.playeritem = new ITEM();
    }
    public string GetPlayerItemName()
    {
        return playeritem.NAME;
    }
    public int GetPlayerItemFREEZE_TIME()
    {
        return playeritem.FREEZE_TIME;
    }
    public int GetPlayerItemDAMAGE_PCT()
    {
        return playeritem.DAMAGE_PCT;
    }
}
