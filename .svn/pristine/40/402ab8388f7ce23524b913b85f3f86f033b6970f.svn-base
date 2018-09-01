using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace xxdwunity.util
{
    /// <summary>
    /// XxdwDebugger的Log(), LogError(), LogWarning()中记录的error和exception会显示在unity的console，同时会显示在屏幕上
    /// 也可调用DisplayMessage()将信息直接显示在屏幕上。
    /// </summary> 
    public class XxdwDebugger : MonoBehaviour
    {
        static public bool EnableLog = false;

        static List<string> mLines = new List<string>();
        static List<string> mWriteTxt = new List<string>();
        private string outpath;

        static public void Log(object message)
        {
            Log(message, null);
        }
        static public void Log(object message, Object context)
        {
            if (EnableLog)
            {
                Debug.Log(message, context);
            }
        }
        static public void LogError(object message)
        {
            LogError(message, null);
        }
        static public void LogError(object message, Object context)
        {
            if (EnableLog)
            {
                Debug.LogError(message, context);
            }
        }
        static public void LogWarning(object message)
        {
            LogWarning(message, null);
        }
        static public void LogWarning(object message, Object context)
        {
            if (EnableLog)
            {
                Debug.LogWarning(message, context);
            }
        }
        static public void DisplayMessage(string msg)
        {
            lock (mLines)
            {
                if (mLines.Count > 40)
                {
                    mLines.RemoveAt(0);
                }
                mLines.Add(msg);
            }
        }

        //===========================================================
        void Start()
        {
            //Application.persistentDataPath Unity中只有这个路径是既可以读也可以写的。
            outpath = Application.persistentDataPath + "/UnityLog.txt";
            //每次启动客户端删除之前保存的Log
            if (System.IO.File.Exists(outpath))
            {
                File.Delete(outpath);
            }
            //在这里做一个Log的监听
            //Application.RegisterLogCallback(HandleLog);
            Application.logMessageReceived += new Application.LogCallback(Application_logMessageReceived);
        }

        void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            mWriteTxt.Add(condition);
            // 错误和异常才显示于屏幕
            if (type == LogType.Error || type == LogType.Exception)
            {
                SaveLogInfo(condition);
                SaveLogInfo(stackTrace);
            }
        }

        void Update()
        {
            //因为写入文件的操作必须在主线程中完成，所以在Update中写入文件。
            if (mWriteTxt.Count > 0)
            {
                string[] temp = mWriteTxt.ToArray();
                foreach (string t in temp)
                {
                    using (StreamWriter writer = new StreamWriter(outpath, true, Encoding.UTF8))
                    {
                        writer.WriteLine(t);
                    }
                    mWriteTxt.Remove(t);
                }
            }
        }

        void OnGUI()
        {
            if (!Parameters.EngineerVersion) return;

            int fontSize = Screen.height >> 5;
            GUI.skin.label.fontSize = fontSize;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUI.skin.label.normal.textColor = Color.red;
            GUILayoutOption[] options = new GUILayoutOption[1];
            options[0] = GUILayout.Width(Screen.width - 20);

            lock (mLines)
            {
                for (int i = 0, imax = mLines.Count; i < imax; ++i)
                {
                    GUILayout.Label(mLines[i], options);
                }
            }
        }

        // 把错误的信息保存起来，用来输出在手机屏幕上
        private static void SaveLogInfo(params object[] objs)
        {
            string text = "";
            for (int i = 0; i < objs.Length; ++i)
            {
                if (i == 0)
                {
                    text += objs[i].ToString();
                }
                else
                {
                    text += ", " + objs[i].ToString();
                }
            }

            DisplayMessage(text);
        }
    }
}
