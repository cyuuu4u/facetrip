using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class ActorController : MonoBehaviour
{
    public AudioClip bgMusic;   // 背景音乐
    private Image img;
    private Rect range;         // 主角坐标范围
    private Cell cell;         // 主角所在格子
    public Rigidbody2D bullet;
    public Transform actor;
    Rigidbody2D temp_bullet;
    private bool leftorright = false;
    public bool jump = false;
    public bool falling = false;
    private Cell temp_actor;
    private Animator role;
    public bool facingLeft = true;
    private float coolTime = 10.0f;
    private float newTime = 10.0f;
    public  float leftTime;
    private AudioSource Hit;
    public AudioClip hit;
    GpsMapCell d,t,b, l, r,gnc,gpc;
    // 在此注册通知

    public static IEnumerator DelayToInvokeDo(Action action, float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);
        action();
    }
    void Awake()
    {
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_BACKGROUND_READY);
        NotificationCenter.Instance.AddObserver(this, Notification.NOTI_KEY_DOWN);
        Hit = gameObject.AddComponent<AudioSource>();
        Hit.clip = hit;
    }

    // 初始化
    void Start()
    {
        this.range = new Rect(0, 0, 0, 0);
        this.cell = new Cell(1, 1);
        this.temp_actor = new Cell(-1,-1);
        role = GameObject.Find("actor").GetComponent<Animator>();
    }

    void Update()
    {
        actor = GameObject.Find("actor").transform;
        Vector3 point = new Vector3(actor.position.x+1f, actor.position.y-0.2f, actor.position.z);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            temp_bullet = Instantiate(bullet, actor.position, actor.rotation) as Rigidbody2D;
            temp_bullet.GetComponent<bullet1>().leftorright = this.leftorright;
        }

        
        if (Input.GetKeyDown(KeyCode.C))
        {
            CoolEffect.CoolInstace.gameObject.SetActive(true);
            if (newTime>=coolTime)
            {
                Instantiate(Resources.Load("effects/wave billboard 12"), point, Quaternion.identity);
                newTime = 0.0f;
            }
        }
        newTime += Time.deltaTime;
        leftTime = coolTime - newTime;

        role.SetBool("walking", false);
        float h = Input.GetAxis("Horizontal");
        if (h > 0 && facingLeft)
            Flip();
        else if (h < 0 && !facingLeft)
            Flip();

        d = Document.Instance.Map.GetCell(temp_actor.Row, temp_actor.Col);
        t = Document.Instance.Map.GetCell(temp_actor.Row - 1, temp_actor.Col);
        b = Document.Instance.Map.GetCell(temp_actor.Row + 1, temp_actor.Col);
        l = Document.Instance.Map.GetCell(temp_actor.Row, temp_actor.Col-1);
        r = Document.Instance.Map.GetCell(temp_actor.Row, temp_actor.Col+1);
       
        if (t == null)
          temp_actor.Row += 1; 
        if (b == null)
          temp_actor.Row -= 1; 
        if (l == null)
          temp_actor.Col -= 1; 
        if (r == null)
          temp_actor.Col += 1; 

      if (this.jump)
        {
            if (!this.falling &&t!=null&& t.Type != GpsMapCell.EType.EN_BUILDING)
            {
                temp_actor.Row--;
                MoveTo(temp_actor);
                StartCoroutine(DelayToInvokeDo(() =>
                {
                    this.falling = true;
                    role.SetBool("jumping", false);
                }, 0.5f)); 
            }
                
        } 
        if (t!=null&&t.Type == GpsMapCell.EType.EN_BUILDING)
            this.falling = true;

        if (this.falling)
        {
            if (d != null && d.Type == GpsMapCell.EType.EN_SELECTION)
            {
                temp_actor.Row++;
                MoveTo(temp_actor);
            }
        }

        if (d != null && d.Type == GpsMapCell.EType.EN_DANGER)
            Application.LoadLevel(2);

    }
    private void OnNotiBackgroundReady(Notification noti)
    {
        XxdwDebugger.Log("Actor: Notification noti OnBackgroundReady");
        this.range.xMin = this.GetComponent<Renderer>().bounds.size.x / 2 + Document.Instance.Map.MapRange.xMin;
        this.range.xMax = -this.GetComponent<Renderer>().bounds.size.x / 2 + Document.Instance.Map.MapRange.xMax;
        this.range.yMin = this.GetComponent<Renderer>().bounds.size.y / 2 + Document.Instance.Map.MapRange.yMin;
        this.range.yMax = -this.GetComponent<Renderer>().bounds.size.y / 2 + Document.Instance.Map.MapRange.yMax;

        MoveTo(Document.Instance.Map.RolePosition);
    }
    public AudioClip get;
    void OnTriggerEnter2D(Collider2D otherObject)
    {
        if (otherObject.tag == "bullet2" || otherObject.tag=="monster")
        {
            Document.Instance.player.HP -= Document.Instance.MonsterATK - Document.Instance.player.DEF;
            Hit.Play();
        }
       //if (Document.Instance.player.HP <= 0)
       //     Application.LoadLevel(2);
        if (otherObject.tag == "box")
        {
            AudioSource.PlayClipAtPoint(get, actor.transform.position);//声音
            Destroy(otherObject.gameObject);
            Creatcoins();
        }
        if (otherObject.tag == "coin")
        {
            AudioSource.PlayClipAtPoint(get, coin.transform.position);//声音
            Destroy(otherObject.gameObject);
            Document.Instance.Gold++;//加金币
        }
        if (otherObject.tag == "jinkela")
        {
            Destroy(otherObject.gameObject);

        }
        if (otherObject.tag == "liquid")
        {
            Destroy(otherObject.gameObject);

        }
    }

    private void OnNotiKeyDown(Notification noti)
    {
        
        GameKey.EKey key = (GameKey.EKey)noti.Data;
        GameKey.EKey modifier = (GameKey.EKey)noti.ExtraData;
        Cell newCell = new Cell(this.cell);
        int multiple = GameKey.IsControlKey(modifier) ? 3 : 1;
        switch (key)
        {
            case GameKey.EKey.K_UpArrow:
                newCell.Row -= multiple;
                break;
            case GameKey.EKey.K_DownArrow:
                newCell.Row += multiple;
                break;
            case GameKey.EKey.K_LeftArrow:
                {
                    leftorright = false;
                    newCell.Col--;
                }
                break;

            case GameKey.EKey.K_RightArrow:
                {
                    leftorright = true;
                    newCell.Col++;
                }
                break;
            case GameKey.EKey.K_Space:
                {
                        newCell.Row--;
                }
                break;
            default:
                return;
        }

        GpsMapCell gnc = Document.Instance.Map.GetCell(this.cell.Row, this.cell.Col);
        GpsMapCell gpc = Document.Instance.Map.GetCell(newCell.Row, newCell.Col);
        if(gpc!=null)
        {
            if (gpc.Type == GpsMapCell.EType.EN_ROPE)
            {
                MoveTo(newCell);
            }

            if (key != GameKey.EKey.K_Space)
            {
                if (gpc.Type == GpsMapCell.EType.EN_ROAD || gpc.Type == GpsMapCell.EType.EN_CROSS)
                {
                    MoveTo(newCell);
                    //行走动画
                    role.SetBool("walking", true);
                    /*if (key == GameKey.EKey.K_UpArrow || key == GameKey.EKey.K_DownArrow)
                    {
                        role.SetBool("walking", false);
                        role.SetBool("climbing", true);
                    }*/
                }
                //攀爬
                if (gpc.Type == GpsMapCell.EType.EN_LADDER)
                {
                    MoveTo(newCell);
                    role.SetBool("climbing", true);
                    /*if(key == GameKey.EKey.K_Space && gpc.Type == GpsMapCell.EType.EN_SELECTION)
                    {
                        MoveTo(newCell);
                        temp_actor.Row = newCell.Row;
                        temp_actor.Col = newCell.Col;
                        this.falling = true;
                    }*/
                }

                if (gpc.Type == GpsMapCell.EType.EN_SELECTION && key != GameKey.EKey.K_UpArrow && key != GameKey.EKey.K_DownArrow)
                {
                    if (gnc.Type != GpsMapCell.EType.EN_ROPE && gnc.Type != GpsMapCell.EType.EN_LADDER)
                    {
                        MoveTo(newCell);
                        temp_actor.Row = newCell.Row;
                        temp_actor.Col = newCell.Col;
                      //  this.falling = true;
                    }
                }
            }
            // 处理跳跃

            if (gpc.Type == GpsMapCell.EType.EN_SELECTION)
            {

                if (gnc.Type == GpsMapCell.EType.EN_ROAD || gnc.Type == GpsMapCell.EType.EN_CROSS/*&& !this.falling*/)
                {
                    if (key == GameKey.EKey.K_Space )//|| key == GameKey.EKey.K_RightArrow || key == GameKey.EKey.K_LeftArrow)
                    {
                        MoveTo(newCell);
                        temp_actor.Row = newCell.Row;
                        temp_actor.Col = newCell.Col;
                        this.falling = false;
                        this.jump = true;
                        //跳跃动画判断
                        role.SetBool("jumping", true); 
                    }
                }
            }
        }
        
    }

    private void MoveTo(Cell newCell)
    {
        LimitCellRange(newCell);
        if (newCell == this.cell) return;   // 位置没变
        XxdwDebugger.Log("current cell:(" + this.cell.Row + "," + this.cell.Col + ")");
        this.cell.Row = newCell.Row;
        this.cell.Col = newCell.Col;
        Vector3 targetPosition = Document.Instance.Map.GetCellPosition(this.cell);
        this.transform.position = targetPosition;
        AsyncNotification.Instance.PostNotification(this, Notification.NOTI_MOVE_CAMERA, targetPosition);
    }

    private void LimitCellRange(Cell cell)
    {
        if (cell.Row < 0)
            cell.Row = 0;
        if (cell.Row >= Document.Instance.Map.RowNum)
            cell.Row = Document.Instance.Map.RowNum - 1;
        if (cell.Col < 0)
            cell.Col = 0;
        if (cell.Col >= Document.Instance.Map.ColNum)
            cell.Col = Document.Instance.Map.ColNum - 1;
    }

    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public GameObject coin;
    public void Creatcoins()
    {

        int x = Random.Range(this.cell.Row - 3, this.cell.Row + 3);
        int y = Random.Range(this.cell.Col - 1, this.cell.Col + 1);
        GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
        if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
        {
            Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
            GameObject go1 = (GameObject)Instantiate(coin);
            go1.transform.position = targetPosition;//当前的格子转换为世界坐标
        }

    }


    /*  
    public GameObject jinkela;
    public GameObject liquid;
    public GameObject shenbao;
    public GameObject thuner;
    public void Creatjinkela()//金克拉
    {
       // itemnum = "I01";
        int x = Random.Range(this.cell.Row - 3, this.cell.Row + 3);
        int y = Random.Range(this.cell.Col - 1, this.cell.Col + 1);
        GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
        if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
        {
            Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
            GameObject go1 = (GameObject)Instantiate(jinkela);
            go1.transform.position = targetPosition;//当前的格子转换为世界坐标
        }
    }

    public void Creatshenbao()//肾宝
    {
       // itemnum = "I02";
        int x = Random.Range(this.cell.Row - 3, this.cell.Row + 3);
        int y = Random.Range(this.cell.Col - 1, this.cell.Col + 1);
        GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
        if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
        {
            Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
            GameObject go1 = (GameObject)Instantiate(shenbao);//go[Random.Range(0,go.Length - 1)];
            go1.transform.position = targetPosition;//当前的格子转换为世界坐标
        }
    }

    public void Creatliquid()//冰冻
    {
       // itemnum = "I03";
        int x = Random.Range(this.cell.Row - 3, this.cell.Row + 3);
        int y = Random.Range(this.cell.Col - 1, this.cell.Col + 1);
        GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
        if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
        {
            Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
            GameObject go1 = (GameObject)Instantiate(liquid);
            go1.transform.position = targetPosition;//当前的格子转换为世界坐标
        }
    }

    public void Creatthuner()//雷电
    {
       // itemnum = "I04";
        int x = Random.Range(this.cell.Row - 3, this.cell.Row + 3);
        int y = Random.Range(this.cell.Col - 1, this.cell.Col + 1);
        GpsMapCell gmc = Document.Instance.Map.GetCell(x, y);//获取对应格子
        if (gmc.Type == GpsMapCell.EType.EN_CROSS || gmc.Type == GpsMapCell.EType.EN_ROAD)
        {
            Vector3 targetPosition = Document.Instance.Map.GetCellPosition(gmc);//将格子转换为世界坐标
            GameObject go1 = (GameObject)Instantiate(thuner);
            go1.transform.position = targetPosition;//当前的格子转换为世界坐标
        }
    }
    */


   
}