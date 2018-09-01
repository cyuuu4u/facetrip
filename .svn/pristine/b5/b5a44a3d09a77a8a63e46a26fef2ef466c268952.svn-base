using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace xxdwunity.comm
{
    /*
    typedef struct {
        struct {
            GU08T reserved    :5;               // 低5位保留,应为0
            GU08T req         :1;               // 区分响应包还是请求包, 1为请求, 0为响应
            GU08T last        :1;               // 次高1位表示是否为最后一个分片
            GU08T first       :1;               // 高1位表示是否为第一个分片      
        }         flag;                         // 标志字节
        GU08T     type;                         // 包类型
    } SslAppDataHeader;

    typedef struct {
        SslAppDataHeader header;
        GU08T *fragment;                        // 数据
    } SslAppData;
    */

    public class SslAppData
    {
        public const int SSL_APP_HEADER_SIZE    = 2;
        public const int SSL_APP_CONTENT_MAX_SIZE = SslRecord.SSL_FRAGMENT_MAX_LENGTH - SSL_APP_HEADER_SIZE;
        public const byte APP_TYPE_MSG          = 0X01;         // 短消息包，不超过一个SSL Record大小
        public const byte APP_TYPE_BINARY       = 0X02;
        public const byte APP_TYPE_TEXT         = 0X03;
        public const byte APP_TYPE_JSON         = 0X04;         // 加密的
        public const byte APP_TYPE_XML          = 0X05;
        public const byte APP_TYPE_PLAIN_JSON   = 0X06;		    // 明文的
        public const byte APP_TYPE_PLAIN_XML    = 0X07;
        public const byte APP_TYPE_FILE         = 0X0A;         // 要求首个包第一行为文件名
        public const byte APP_TYPE_HEARTBEAT    = (byte)0XFF;

        public const int APP_TYPE_FILE_HEADER_SIZE		= 10;
        public const int APP_TYPE_FILE_MAX_FILENAME_LEN = 255;

        private const int MASK_FIRST            = 0X80;
        private const int MASK_LAST             = 0X40;
        private const int MASK_REQ              = 0X20;

        private byte flag;
        private byte type;
        private byte[] data;

        public SslAppData()
        {
            this.flag = 0;
            this.SetFirst(true);
            this.SetLast(true);
            this.SetReq(true);
            this.data = null;
        }

        public bool IsFirst()
        {
            return (flag & MASK_FIRST) != 0;
        }

        public void SetFirst(bool first)
        {
            if (first)
            {
                this.flag |= MASK_FIRST;
            }
            else
            {
                this.flag = (byte)(this.flag & ~MASK_FIRST);
            }

        }

        public bool IsLast()
        {
            return (flag & MASK_LAST) != 0;
        }

        public void SetLast(bool last)
        {
            if (last)
            {
                this.flag |= MASK_LAST;
            }
            else
            {
                this.flag = (byte)(this.flag & ~MASK_LAST);
            }

        }

        public bool IsReq()
        {
            return (flag & MASK_REQ) != 0;
        }

        public void SetReq(bool req)
        {
            if (req)
            {
                this.flag |= MASK_REQ;
            }
            else
            {
                this.flag = (byte)(this.flag & ~MASK_REQ);
            }

        }

        public new byte GetType()
        {
            return type;
        }

        public void SetType(byte type)
        {
            this.type = type;
        }

        public int GetDataLength()
        {
            return this.data == null ? 0 : this.data.Length;
        }

        public byte[] GetData()
        {
            return data;
        }

        public void SetData(byte[] data)
        {
            this.data = data;
        }

        public int SetData(byte[] data, int from)
        {
            return this.SetData(data, from, data.Length - from);
        }
        /**
         * 设置包中data成员
         * @param bytes
         * @param from		从哪个字节起（0始）
         * @param len		使用字节数
         * @return			使用的字节数
         */
        public int SetData(byte[] bytes, int from, int len)
        {
            if (bytes.Length - from < 0)
            {
                return 0;
            }
            else if (bytes.Length - from < len)
            {
                len = bytes.Length - from;
            }

            this.data = new byte[len];
            System.Array.Copy(bytes, from, this.data, 0, len);

            return len;
        }

        public byte[] GetBytes()
        {
            int size = SSL_APP_HEADER_SIZE + (this.data == null ? 0 : this.data.Length);
            byte[] bytes = new byte[size];

            bytes[0] = this.flag;
            bytes[1] = this.type;
            System.Array.Copy(this.data, 0, bytes, 2, this.data.Length);

            return bytes;
        }

        // 设置整个包，包括flag和type（头两个字节）
        public int SetBytes(byte[] bytes, int from)
        {
            return this.SetBytes(bytes, from, bytes.Length - from);
        }

        /**
         * 设置整个包，包括flag和type（头两个字节）
         * @param bytes
         * @param n			使用字节数
         * @param from		从哪个字节起（0始）
         * @return			使用的字节数
         */
        public int SetBytes(byte[] bytes, int from, int len)
        {
            if (bytes.Length - from < SSL_APP_HEADER_SIZE)
            {
                return 0;
            }
            else if (bytes.Length - from < len)
            {
                len = bytes.Length - from;
            }

            this.flag = bytes[0 + from];
            this.type = bytes[1 + from];

            if (len > SSL_APP_HEADER_SIZE)
            {
                this.data = new byte[len - SSL_APP_HEADER_SIZE];
                System.Array.Copy(bytes, from + SSL_APP_HEADER_SIZE, this.data, 0, len - SSL_APP_HEADER_SIZE);
            }
            else
            {
                this.data = null;
            }

            return len;
        }
    }
}
