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


public class CoinSum : MonoBehaviour
{
    public Text coinsum;


    // Use this for initialization
    void Start()
    {
        coinsum = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int sum;
        sum = Document.Instance.Gold;
        coinsum.text = sum.ToString();
    }
}
