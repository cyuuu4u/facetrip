﻿using UnityEngine;
using System.Collections;

public class ScreenControl2 : MonoBehaviour {

    public Texture replay;
    public Texture quit;
    public Texture back;

    public float m_fScreenWidth = 1366;
    public float m_fScreenHigth = 768;

    public float m_fScaleWidth;
    public float m_fScaleHeight;
    void Start()
    {
        m_fScaleWidth = (float)(Screen.width / m_fScreenWidth);
        m_fScaleHeight = (float)(Screen.height / m_fScreenHigth);
    }
    void OnGUI()
    {
        GUI.backgroundColor = Color.clear;
        if (GUI.Button(new Rect(560 * m_fScaleWidth, 450 * m_fScaleHeight, 250 * m_fScaleWidth, 60 * m_fScaleHeight), replay))
            Application.LoadLevel(1);
        if (GUI.Button(new Rect(560 * m_fScaleWidth, 540 * m_fScaleHeight, 250 * m_fScaleWidth, 60 * m_fScaleHeight), back))
            Application.LoadLevel(0);
        if (GUI.Button(new Rect(560 * m_fScaleWidth, 625 * m_fScaleHeight, 250 * m_fScaleWidth, 60 * m_fScaleHeight), quit))
            Application.Quit();
    }
}
