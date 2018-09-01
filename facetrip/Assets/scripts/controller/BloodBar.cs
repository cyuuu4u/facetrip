using UnityEngine;
using System.Collections;
using System.Data;
using System.Collections.Generic;
using xxdwunity;
using xxdwunity.engine;
using System.IO;
using System.Threading;
using xxdwunity.comm;
using xxdwunity.vo;
using xxdwunity.util;
using xxdwunity.warehouse;
using UnityEngine.UI;

public class BloodBar : MonoBehaviour {

    // Use this for initialization
    public Image Bloodbar;
    public float Value;
    void Start () {
        Bloodbar = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        Value = Document.Instance.player.HP;
        Bloodbar.fillAmount = Value/Document.Instance.player.HP_BASE;
    }
}
