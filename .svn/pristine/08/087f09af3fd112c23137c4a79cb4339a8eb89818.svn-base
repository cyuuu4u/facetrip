using UnityEngine;
using System.Collections;

public class bullet1 : MonoBehaviour {
	public bool leftorright=false ; 
	public int speed=3;
	private Transform monster;
	public Vector3 moveDirection;
    // Use this for initialization
    public GameObject prefab;

    void Start () {
		Destroy (gameObject, 2f);
    }
	
	// Update is called once per frame
	void Update () {
		if (leftorright == true) {
			transform .Translate (Vector3.up* Time.deltaTime *4);
			transform .Translate (Vector3.right* Time.deltaTime * speed);

        }
		if (leftorright == false) {
			transform .Translate (Vector3.up* Time.deltaTime *4);
			transform .Translate (Vector3.left* Time.deltaTime * speed);
		}

    }

	void OnTriggerEnter2D(Collider2D otherObject)
	{
		if (otherObject.tag == "Monster" || otherObject.tag == "bullet2")
        {
            Vector3 hitpoint = otherObject.transform.position;
            Instantiate(Resources.Load("effects/Explosion 16"), hitpoint, Quaternion.identity);
            Destroy(gameObject);
        }
    }
  
}
