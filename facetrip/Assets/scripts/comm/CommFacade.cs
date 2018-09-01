using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System;
using System.Net;
using xxdwunity.engine;
using xxdwunity.comm;
using xxdwunity.util;

namespace com.hytxworld.comm
{
    public class CommFacade
	{
        public const int OP_NULL                        = 0;
        public const int OP_REGISTER				    = 1;
	    public const int OP_LOGIN				        = 2;
	    public const int OP_GEN_VERIFY_CODE		        = 3;
	    public const int OP_DOWNLOAD_SUB_LICENSE	    = 4;
	    public const int OP_AUTH_NAVIGATION		        = 5;
        public const int OP_UPLOAD_PATH_DATA            = 101;
        public const int OP_DOWNLOAD_MAP_AND_VOICE_DATA = 102;

        public string DestName { get; set; }
        public int DestPort { get; set; }

        public CommFacade()
        {
        }

        public CommFacade(string strDestName, int nDestPort)
        {
            this.DestName = strDestName;
            this.DestPort = nDestPort;
        }

        public void UploadPathData(string strPathText)
        {
            Post(OP_UPLOAD_PATH_DATA, strPathText);
        }

        public static bool WriteLocalPathData(string strPathText)
        {
            //if (Application.platform != RuntimePlatform.Android) return false;

            string path = Util.AssureGetFilePath(Util.GetStoragePath() + xxdwunity.Parameters.STR_APP_ROOT_DIRECTORY);
            if (path != "")
            {
                return Util.WriteTextFile(path, "trigger_and_path.txt", "\n\n" + strPathText);
            }
            return false;
        }

        public void DownloadMapAndVoicePackage(string strMapName)
        {
            Post(OP_DOWNLOAD_MAP_AND_VOICE_DATA, strMapName);
        }

        //////////////////////////////////////////////////////////////////////////

        private void Post(int op, object objData)
        {
            OpData od = new OpData(op, objData);
            Thread thread = new Thread(this.DataThreadFunc);
            thread.Start(od);
        }

        private void DataThreadFunc(object objData)
        {
            OpData od = objData as OpData;
            ResponseData result = new ResponseData(od.Op);

            switch (od.Op)
            {
                case OP_REGISTER:
                    break;
                case OP_LOGIN:
                    break;
                case OP_GEN_VERIFY_CODE:
                    break;
                case OP_DOWNLOAD_SUB_LICENSE:
                    break;
                case OP_AUTH_NAVIGATION:
                    break;
                case OP_UPLOAD_PATH_DATA:
                    OnUploadPathData(od.Data, result);
                    break;
                case OP_DOWNLOAD_MAP_AND_VOICE_DATA:
                    OnDownloadMapAndVoiceData(od.Data, result);
                    break;
                default:
                    return;
            }

            // 处理完毕通知主线程
            AsyncNotification.Instance.PostNotification(this, Notification.NOTI_COMM_RESULT, result);
        }

        private void OnUploadPathData(object objData, ResponseData result)
        {
            ClientSocket cs = null;
            SslAppData sad = new SslAppData();

            try
            {

                SimpleJson.JsonObject jsonPathData = new SimpleJson.JsonObject();
                jsonPathData.Add("op", "UPLOAD_PATH_DATA");
                jsonPathData.Add("data", (string)objData);
                //jsonPathData.Add("data", "no actual data.");
                string strJsonText = jsonPathData.ToString();

                cs = Connect();
                if (cs == null)
                {
                    result.Code = ResponseData.RESP_SERVER_UNREACHABLE;
                    result.Msg = TypeAndParameter.STR_CONNECT_WITH_SERVER_FAILED;
                    return;
                }

                // 发送数据
                sad.SetType(SslAppData.APP_TYPE_PLAIN_JSON);
                byte[] allBytes = System.Text.Encoding.Default.GetBytes(strJsonText);
                if (allBytes.Length > SslAppData.SSL_APP_CONTENT_MAX_SIZE)
                {
                    sad.SetFirst(true);
                    sad.SetLast(false);
                    sad.SetData(allBytes, 0, SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                    cs.Send(sad);

                    sad.SetFirst(false);
                    int i = SslAppData.SSL_APP_CONTENT_MAX_SIZE;
                    while(i < allBytes.Length)
                    {
                        if (allBytes.Length - i <= SslAppData.SSL_APP_CONTENT_MAX_SIZE)
                        {
                            sad.SetLast(true);
                            sad.SetData(allBytes, i);
                            i += allBytes.Length - i;
                        }
                        else
                        {
                            sad.SetData(allBytes, i, SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                            i += SslAppData.SSL_APP_CONTENT_MAX_SIZE;
                        }                        
                        cs.Send(sad);
                    }
                }
                else
                {
                    sad.SetData(allBytes);
                    //sad.SetData(System.Text.Encoding.UTF8.GetBytes(strJsonText));
                    cs.Send(sad);
                }

                // 接收数据
                cs.Recv(sad);
                byte[] jsonData = sad.GetData();
                //strJsonText = System.Text.Encoding.Default.GetString(jsonData);
                strJsonText = System.Text.Encoding.UTF8.GetString(jsonData);

                var rsp = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(strJsonText);
                if ("AE_SUCCESS" == (rsp["err"] as string))
                {
                    result.Code = ResponseData.RESP_SUCCESS;
                    result.Msg = TypeAndParameter.STR_UPLOAD_PATH_DATA_FINISHED;
                }
                else
                {
                    result.Msg = rsp["msg"] as string;
                }
            }
            catch (Exception e)
            {
                XxdwDebugger.LogError(e.StackTrace);
            }
            finally
            {
                if (cs != null) 
                    cs.Close();
            }
        }

        private void OnUploadPathData2(object objData, ResponseData result)
        {
            ClientSocket cs = null;
            SslAppData sad = new SslAppData();

            try
            {
                string str = (string)objData;
                cs = Connect();
                if (cs == null)
                {
                    result.Code = ResponseData.RESP_SERVER_UNREACHABLE;
                    result.Msg = TypeAndParameter.STR_CONNECT_WITH_SERVER_FAILED;
                    return;
                }

                // 发送数据
                sad.SetType(SslAppData.APP_TYPE_FILE);
                byte[] allBytes = System.Text.Encoding.Default.GetBytes(str);
                byte[] filenameBytes = System.Text.Encoding.Default.GetBytes("test.txt\0");
                byte[] d = new byte[SslAppData.APP_TYPE_FILE_HEADER_SIZE + filenameBytes.Length + SslAppData.SSL_APP_CONTENT_MAX_SIZE];
                int s = IPAddress.HostToNetworkOrder(allBytes.Length);  // 文件大小
                System.Array.Copy(BitConverter.GetBytes(s), 0, d, 0, 4);
                //System.Array.Copy(BitConverter.GetBytes(91), 0, d, 4, 4); // 偏移，为调试
                short t = IPAddress.HostToNetworkOrder((short)filenameBytes.Length);
                System.Array.Copy(BitConverter.GetBytes(t), 0, d, 8, 2);
                System.Array.Copy(filenameBytes, 0, d, SslAppData.APP_TYPE_FILE_HEADER_SIZE, filenameBytes.Length);
                
                if (allBytes.Length > SslAppData.SSL_APP_CONTENT_MAX_SIZE)
                {
                    sad.SetFirst(true);
                    sad.SetLast(false);

                    System.Array.Copy(allBytes, 0, d, SslAppData.APP_TYPE_FILE_HEADER_SIZE + filenameBytes.Length, SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                    sad.SetData(d, 0, SslAppData.APP_TYPE_FILE_HEADER_SIZE + filenameBytes.Length + SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                    cs.Send(sad);

                    sad.SetFirst(false);
                    int i = SslAppData.SSL_APP_CONTENT_MAX_SIZE;
                    while (i < allBytes.Length)
                    {
                        if (allBytes.Length - i <= SslAppData.SSL_APP_CONTENT_MAX_SIZE)
                        {
                            sad.SetLast(true);
                            sad.SetData(allBytes, i);
                            i += allBytes.Length - i;
                        }
                        else
                        {
                            sad.SetData(allBytes, i, SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                            i += SslAppData.SSL_APP_CONTENT_MAX_SIZE;
                        }
                        cs.Send(sad);
                    }
                }
                else
                {
                    System.Array.Copy(allBytes, 0, d, SslAppData.APP_TYPE_FILE_HEADER_SIZE + filenameBytes.Length, allBytes.Length);
                    sad.SetData(d, 0, SslAppData.APP_TYPE_FILE_HEADER_SIZE + filenameBytes.Length + allBytes.Length);
                    cs.Send(sad);
                }

                // 接收数据
                cs.Recv(sad);
                byte[] jsonData = sad.GetData();
                //string strJsonText = System.Text.Encoding.Default.GetString(jsonData);
                string strJsonText = System.Text.Encoding.UTF8.GetString(jsonData);

                var rsp = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(strJsonText);
                if ("AE_SUCCESS" == (rsp["err"] as string))
                {
                    result.Code = ResponseData.RESP_SUCCESS;
                    result.Msg = TypeAndParameter.STR_UPLOAD_PATH_DATA_FINISHED;
                }
                else
                {
                    // utf-8格式的消息转unicode
                    //byte[] buffer= Encoding.UTF8.GetBytes(rsp["msg"] as string); 
                    //result.Msg = Encoding.Unicode.GetString(buffer);
                    result.Msg = rsp["msg"] as string;
                }
            }
            catch (Exception e)
            {
                XxdwDebugger.LogError(e.StackTrace);
            }
            finally
            {
                if (cs != null)
                    cs.Close();
            }
        }

        private void OnDownloadMapAndVoiceData(object objData, ResponseData result)
        {
            ClientSocket cs = null;
            SslAppData sad = new SslAppData();

            try
            {

                SimpleJson.JsonObject jsonPathData = new SimpleJson.JsonObject();
                jsonPathData.Add("op", "UPLOAD_PATH_DATA");
                jsonPathData.Add("data", (string)objData);
                //jsonPathData.Add("data", "no actual data.");
                string strJsonText = jsonPathData.ToString();

                cs = Connect();
                if (cs == null)
                {
                    result.Code = ResponseData.RESP_SERVER_UNREACHABLE;
                    result.Msg = TypeAndParameter.STR_CONNECT_WITH_SERVER_FAILED;
                    return;
                }

                // 发送数据
                sad.SetType(SslAppData.APP_TYPE_PLAIN_JSON);
                byte[] allBytes = System.Text.Encoding.Default.GetBytes(strJsonText);
                if (allBytes.Length > SslAppData.SSL_APP_CONTENT_MAX_SIZE)
                {
                    sad.SetFirst(true);
                    sad.SetLast(false);
                    sad.SetData(allBytes, 0, SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                    cs.Send(sad);

                    sad.SetFirst(false);
                    int i = SslAppData.SSL_APP_CONTENT_MAX_SIZE;
                    while (i < allBytes.Length)
                    {
                        if (allBytes.Length - i <= SslAppData.SSL_APP_CONTENT_MAX_SIZE)
                        {
                            sad.SetLast(true);
                            sad.SetData(allBytes, i);
                            i += allBytes.Length - i;
                        }
                        else
                        {
                            sad.SetData(allBytes, i, SslAppData.SSL_APP_CONTENT_MAX_SIZE);
                            i += SslAppData.SSL_APP_CONTENT_MAX_SIZE;
                        }
                        cs.Send(sad);
                    }
                }
                else
                {
                    sad.SetData(allBytes);
                    //sad.SetData(System.Text.Encoding.UTF8.GetBytes(strJsonText));
                    cs.Send(sad);
                }

                // 接收数据
                cs.Recv(sad);
                byte[] jsonData = sad.GetData();
                //strJsonText = System.Text.Encoding.Default.GetString(jsonData);
                strJsonText = System.Text.Encoding.UTF8.GetString(jsonData);

                var rsp = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(strJsonText);
                if ("AE_SUCCESS" == (rsp["err"] as string))
                {
                    result.Code = ResponseData.RESP_SUCCESS;
                    result.Msg = TypeAndParameter.STR_UPLOAD_PATH_DATA_FINISHED;
                }
                else
                {
                    result.Msg = rsp["msg"] as string;
                }
            }
            catch (Exception e)
            {
                XxdwDebugger.LogError(e.StackTrace);
            }
            finally
            {
                if (cs != null)
                    cs.Close();
            }
        }

        private ClientSocket Connect()
        {
            ClientSocket cs = null;
            try
            {
                cs = new ClientSocket(this.DestName, this.DestPort);
                return cs;
            }
            catch (Exception e)
            {
                XxdwDebugger.LogError(e.Message);
            }

            return null;
        }
	}

    class OpData
    {
        public int Op { get; set; }
        public object Data { get; set; }
        public OpData(int op, object data)
        {
            this.Op = op;
            this.Data = data;
        }
    }
}
