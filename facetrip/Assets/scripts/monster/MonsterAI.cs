using UnityEngine;
using System.Collections;
public class MonsterAI : MonoBehaviour {
	private Vector3 moveDirection;
	public GameObject  Monster;
    //private Animator animator;//获取Animator组件
    //private CharacterController control;
	private Transform actor;
    private int speed;
    private float Speed;
	public Rigidbody2D bullet;
	private Rigidbody2D  temp_bullet;
    public AudioClip hit;
    private AudioSource Hit;
	void Awake()
	{
		    
		//animator = GetComponent<Animator> ();
		InvokeRepeating ("bulletcopy",3, 2);
        Hit = gameObject.AddComponent<AudioSource>();
        Hit.clip = hit;
        actor = GameObject.Find("actor").transform;
	}
	// Use this for initialization
	void Start () {
        speed =Random.Range(2, 20);
        Speed = (float)speed / 10;
}
	void Update () {
		//人物Position
		//actor  = GameObject.Find("actor").transform;
		//float dis = Vector3.Distance(transform.position,actor.position);
		this.moveDirection = actor.position - transform.position;
		this.moveDirection.z = 0;
		this.moveDirection.Normalize();
		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		Vector3 to = moveDirection *Speed * Time.deltaTime + transform.position;
		transform.position = to;
		if (this.moveDirection .x < 0)	transform.rotation = Quaternion.Euler (0, 0, targetAngle - 180);
		if (this.moveDirection .x >= 0) transform.rotation = Quaternion.Euler (0, 0, targetAngle);

	}
	void bulletcopy()
	{
	    temp_bullet = Instantiate (bullet, this.gameObject.transform.position, bullet .transform.rotation)as Rigidbody2D ;
	}
	
	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.tag == "bullet1") {
            Hit.Play();
            Document.Instance.MonsterHP -= (Document.Instance.player.ATK - Document.Instance.MonsterDEF);
            float rate = (float)(Document.Instance.MonsterHP / (double)Document.Instance.MonsterHPB);
            if (Document.Instance.MonsterHP > 0)
                transform.FindChild("red").localScale = new Vector3(rate, 1f, 1f);
            if (Document.Instance.MonsterHP <= 0)
			{
				Destroy (gameObject);
				Document.Instance.MonsterKilled++;
                if (Document.Instance.MonsterKilled == 2)
                {
                    Application.LoadLevel(3);
                    Document.Instance.MonsterKilled = 0;
                }
                Document.Instance.MonsterHP = Document.Instance.MonsterHPB;
			}
		}
        if (otherObject.tag == "lighting")
        {
            Document.Instance.MonsterKilled++;
            Destroy(gameObject, 1f);
            Speed = 0.3f;
        }
	}


}

