using UnityEngine;
using System.Collections;
public class bullet2 : MonoBehaviour {
	private Transform actor;
	public int speed=5;
	private Vector3 moveDirection;
    private bool bulletmove=false; 
	// Use this for initialization
	void Start () {
		actor = GameObject.FindGameObjectWithTag("Player").transform;
		this.moveDirection = actor.position - transform .position;
		if (this.moveDirection .x < 0) 
			bulletmove = true;
		if (this.moveDirection .x >= 0) 
			bulletmove = false;
		Destroy (gameObject, 1f);
	}
	// Update is called once per frame
	void Update () {
		if(bulletmove ==false)
			transform .Translate(Vector3 .right *Time .deltaTime * speed  );
		if(bulletmove ==true)
			transform .Translate(Vector3 .left *Time .deltaTime * speed  );
	}
	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.tag == "Player" || otherObject.tag == "bullet1")
			Destroy (gameObject);
	}
}
