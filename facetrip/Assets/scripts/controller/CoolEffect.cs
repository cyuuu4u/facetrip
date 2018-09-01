using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolEffect : MonoBehaviour {
    public static CoolEffect CoolInstace;
    private ActorController Lefttime;
    private Image FillImage;
	// Use this for initialization
	void Start () {
        FillImage =GameObject.FindGameObjectWithTag("effectC").GetComponent<Image>();
        Lefttime = GameObject.FindGameObjectWithTag("Player").GetComponent<ActorController>();
        CoolInstace = this;
        this.gameObject.SetActive(false);
        }
	
	// Update is called once per frame
	void Update () {
        FillImage.fillAmount =Lefttime.leftTime/10.0f;
	}
}
