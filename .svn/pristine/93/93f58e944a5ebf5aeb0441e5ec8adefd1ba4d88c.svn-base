using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace xxdwunity.util
{
    public class Util
    {
        public delegate void DlgtSetWWWSource<T>(T t);
        public static IEnumerator WWWLoadTexture2D(string url, DlgtSetWWWSource<Texture2D> setter)
        {
            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                setter(www.texture);
                www.Dispose();
            }
            else
            {
                XxdwDebugger.Log(www.error);
            }
        }

        public static IEnumerator WWWLoadAudioClip(string url, DlgtSetWWWSource<AudioClip> setter)
        {
            WWW www = new WWW(url);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                setter(www.audioClip);
                www.Dispose();
            }
            else
            {
                XxdwDebugger.Log(www.error);
            }
        }

        public static string WWWStreamingAssetsPath()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath + "/";
            }
            return "file://" + Application.streamingAssetsPath + "/";
        }

        public static string AssureGetFilePath(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (!path.EndsWith("/"))
                {
                    path += "/";
                }

                return path;
            }
            catch (System.Exception)
            {
                return "";
            }
        }

        public static bool WriteTextFile(string path, string filename, string text)
        {
            bool rs = false;
            StreamWriter sw = null;
            try
            {
                if (!path.EndsWith("/")) path += "/";

                FileInfo t = new FileInfo(path + filename);
                if (!t.Exists)
                {
                    sw = t.CreateText();
                }
                else
                {
                    sw = t.AppendText();
                }
                
                sw.WriteLine(text);

                rs = true;
            }
            catch (System.Exception)
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                    sw.Dispose();
                }
            }

            return rs;
        }

        //////////////////////////////////////////////////////////////////////////
        // Android Method
#if UNITY_ANDROID
        //获取当前App的Activity  
        public static AndroidJavaObject AndroidMainActivity()
        {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        }

        //获取指定包名的Activity  
        public static AndroidJavaObject AndroidGetActivity(string package_name, string activity_name)
        {
            return new AndroidJavaClass(package_name).GetStatic<AndroidJavaObject>(activity_name);
        }

        // UI线程中运行  
        public static void AndroidRunOnUIThread(AndroidJavaRunnable r)
        {
            AndroidMainActivity().Call("runOnUiThread", r);
        }

        //获取包名  
        public static string AndroidGetPackageName()
        {
            // call<返回值类型>("方法名");  
            return AndroidMainActivity().Call<string>("getPackageName");
        }
        
        //设置不自动锁屏  
        public static void AndroidDisableScreenLock()
        {
            // call("方法名",参数1);  
            AndroidMainActivity().Call<AndroidJavaObject>("getWindow").Call("addFlags", 128);
        }
#endif        
        //////////////////////////////////////////////////////////////////////////
        // 分平台的通用方法　

        // 获取内置SD卡路径  
        public static string GetStoragePath()
        {
            string path = "";

            if (Application.platform == RuntimePlatform.Android)
            {
                //new AndroidJavaClass("全类名")  ---new一个Android原生类  
                //CallStatic<返回类型>("方法名")  ---静态方法获取一个Android原生类型
#if UNITY_ANDROID
                path = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory").Call<string>("getPath");
#endif
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.WindowsWebPlayer
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                path = "d:/temp/";
            }

            if (!path.EndsWith("/"))
            {
                path += "/";
            }
            return path;
        }        
    }
}
