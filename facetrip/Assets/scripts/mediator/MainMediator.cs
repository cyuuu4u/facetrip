using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;

public class MainMediator : MonoBehaviour {

    public Material material;
	public GameObject PlayerControl;//获取人物对象
    public Transform actor;
    private Transform monster;
	public GameObject  Monster;
	GameObject temp_Monster;
	public GameObject []Monsters=new GameObject[3]; 
    Thread dataThread; // 声明线程
    bool running;
    // 在此注册通知
    void Awake()
    {
        XxdwDebugger.EnableLog = true;      // 决定是否记录调试信息日志
        Parameters.EngineerVersion = false;  // 决定有无工程版的功能（含在屏幕显示调试信息功能）
        XxdwDebugger.Log("MainMediator -> Awake()");
        
        Application.runInBackground = true;
        AsyncNotification.Instance.PostNotification(this, Notification.NOTI_APP_LOADING);

        // 注册通知消息
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_SWITCH_SCENIC_RESORT);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_MAP_DATA_READY);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_KEY_DOWN);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_TO_OPEN_WINDOW);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_DIALOG_QUIT);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_SET_HINT);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_COMM_RESULT);
    }

    void Start()
    {
		//Monstercopy ();
        XxdwDebugger.Log("MainMediator -> Start()");
        //XxdwDebugger.DisplayMessage("StoragePath: " + Util.GetStoragePath());
		//InvokeRepeating ("bulletcopy", 2, 2);
        this.running = true;
        //this.dataThread = new Thread(new ThreadStart(DataThreadFunc));
        //this.dataThread.Start();

        Document.Instance.SwitchMap(TypeAndParameter.instance.DEFAULT_MAP);
        Document.Instance.player = Document.Instance.FindPlayerById("100");
        Document.Instance.MonsterHP = Document.Instance.FindMonsterById("200").HP;
        Document.Instance.MonsterATK = Document.Instance.FindMonsterById("200").ATK;
        Document.Instance.MonsterDEF = Document.Instance.FindMonsterById("200").DEF;
        Document.Instance.MonsterHPB = Document.Instance.FindMonsterById("200").HP_BASE;
    }
	void Update()
	{
		Monsters = GameObject .FindGameObjectsWithTag ("Monster");
		if(Monsters.Length <3)
		{
			Vector3 v = PlayerControl.transform.position; 
			float x = Random.Range (v.x - 4, v.x + 4);
			float y = Random.Range (v.y - 3, v.y + 3);
			Vector3 vN = new Vector3 (x, y, v.z);
			temp_Monster = (GameObject)GameObject.Instantiate (Monster, vN, Monster .gameObject.transform.rotation);
		}

        //调用生成宝箱
       if(Document.Instance.MonsterKilled == 1)//如果杀敌数量等于1，生成宝箱
        {
            CreatBox();
        }
        
	}
		private void DataThreadFunc()
    {
        int i = 0;
        while (this.running && i++ < 30)
        {
            XxdwDebugger.DisplayMessage("thread msg");
            XxdwDebugger.DisplayMessage(Util.GetStoragePath());
            //CommFacade.WriteLocalPathData("just a test\n");
            Thread.Sleep(1000);
        }
    }

    private void OnNotiSwitchScenicResort(Notification noti)
    {
        NotificationCenter.Instance.DisableInput = true;
        Document.Instance.SwitchMap((string)noti.Data);
    }

    private void OnNotiMapDataReady(Notification noti)
    {
        if (Document.Instance.Map.IsInfoDataReady())
        {
            NotificationCenter.Instance.DisableInput = false;
        }
        CreatTIGER_MACHINE1();//地图载入时生成老虎机
        CreatTIGER_MACHINE2();
    }

    private void OnNotiCommResult(Notification noti)
    {
        
    }

    string TigerNum;
    private Vector3 playerPosition;//人物位置坐标
    GpsMapCell tiger1;
    GpsMapCell tiger2;
    string item;
    public AudioClip get;
    private void OnNotiKeyDown(Notification noti)
    {
        GameKey.EKey key = (GameKey.EKey)noti.Data;
       // GameKey.EKey modifier = (GameKey.EKey)noti.ExtraData;
        if (key == GameKey.EKey.K_A)//在老虎机的周围，按键A，生成道具（没有扣除金币数）,没有调用接口
        {

            actor = GameObject.Find("actor").transform;
            //playerPosition = PlayerControl.transform.position; //人物Position
            playerPosition = actor.position;
            GpsMapCell player = Document.Instance.Map.GetCell(playerPosition);
            int x1 = player.Row - tiger1.Row;//行
            int y1 = player.Col - tiger1.Col;//列
            int x2 = player.Row - tiger2.Row;
            int y2 = player.Col - tiger2.Col;
            if (y1 < 3 && y1 > -3 && x1 < 1 && x1 > -1)
            {
                AudioSource.PlayClipAtPoint(get, TIGER_MACHINE1.transform.position);//声音
                item=Document.Instance.OpenTigerMachine(Document.Instance.Getcode("T01"));//T01老虎机编号，调用接口
            }

            if (y2 < 3 && y2 > -3 && x2 < 1 && x2 > -1)
            {
                AudioSource.PlayClipAtPoint(get, TIGER_MACHINE2.transform.position);//声音
                Document.Instance.OpenTigerMachine(Document.Instance.Getcode("T04"));//T01老虎机编号，调用接口
            }

        }

    }

    //创建宝箱
    //public ActorController actor;//静态获取
 

    public GameObject box;
    GameObject go = null;
    public void CreatBox()
    {
        actor = GameObject.Find("actor").transform;
        //playerPosition = PlayerControl.transform.position; //人物Position
        playerPosition = actor.position;
        GpsMapCell player = Document.Instance.Map.GetCell(playerPosition);

        go = null;

        for (int i = 0; i < 50; i++) 
        {
            if (player.Row > 3 && player.Col > 6)
            {

                int x = Random.Range(player.Row - 1, player.Row + 1);//行
              // int y = Random.Range(player.Col- 5, player.Col - 1) | Random.Range(player.Col + 1, player.Col + 5); ;//列
                 int y = Random.Range(player.Col - 5, player.Col + 5); ;//列
                GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
                if ( x != player.Row && y != player.Col)
                {
                    if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
                  {
                       Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
                       go = (GameObject)Instantiate(box);
                       go.transform.position = targetPosition;//当前的格子转换为世界坐标             
                       XxdwDebugger.Log("生成宝箱");
                       break;
                     }
                }
            
            }
            if (go != null)
                break;
        }
    }

    public GameObject TIGER_MACHINE1;//老虎机T01
    public GameObject TIGER_MACHINE2;//老虎机T02
    GameObject TIGERMACHINE1 = null;

    public void CreatTIGER_MACHINE1()
    {
        TigerNum = "T01";
        while (TIGERMACHINE1 == null)
        {
            int x = Random.Range(0, Document.Instance.Map.RowNum - 1);
            int y = Random.Range(0, Document.Instance.Map.ColNum - 1);
            GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
            if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
            {
                Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
                TIGERMACHINE1 = (GameObject)Instantiate(TIGER_MACHINE1);
                TIGERMACHINE1.transform.position = targetPosition;//当前的格子转换为世界坐标
                tiger1 = gmc;
                break;
            }

        }
    }

    GameObject TIGERMACHINE2 = null;
    public void CreatTIGER_MACHINE2()
    {
        TigerNum = "T02";
        while (TIGERMACHINE2 == null)
        {
            int x = Random.Range(0, Document.Instance.Map.RowNum - 1);
            int y = Random.Range(0, Document.Instance.Map.ColNum - 1);
            GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
            if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
            {
                Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
                TIGERMACHINE2 = (GameObject)Instantiate(TIGER_MACHINE2);
                TIGERMACHINE2.transform.position = targetPosition;//当前的格子转换为世界坐标             
                tiger2 = gmc;
                break;
            }

        }
    }
    public GameObject jinkela;
    public GameObject liquid;
    public GameObject shenbao;
    public GameObject thuner;
    //测试，调用道具类里的编号
    public void Creatitem()
    {
        actor = GameObject.Find("actor").transform;
        //playerPosition = PlayerControl.transform.position; //人物Position
        playerPosition = actor.position;
        GpsMapCell player = Document.Instance.Map.GetCell(playerPosition);
        GameObject go1;
        int x = Random.Range(player.Row - 3, player.Row + 3);
        int y = Random.Range(player.Col - 1, player.Col + 1);
        GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
        if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
        {
            Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标


            if (item == "I01")//雷电
            {
                go1 = (GameObject)Instantiate(thuner);
                go1.transform.position = targetPosition;
            }
            else if (item == "I02")//冰冻
            {
                go1 = (GameObject)Instantiate(liquid);
                go1.transform.position = targetPosition;
            }
            else if (item == "B01")//肾宝
            {
                go1 = (GameObject)Instantiate(shenbao);
                go1.transform.position = targetPosition;
            }
            else if (item == "B03")//金克拉
            {
                go1 = (GameObject)Instantiate(jinkela);
                go1.transform.position = targetPosition;
            }

        }
    }
}
