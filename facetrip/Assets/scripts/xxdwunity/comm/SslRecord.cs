using UnityEngine;
using System.Collections.Generic;
using System.Text;

namespace xxdwunity.comm
{
    /************************************************************************
    /* 错误信息以5字节发送，这5个字节与SSL3.0的记录头格式一致               *
    /* 首字节为错误码,其后4字节固定为: 0X03,0X00,0X00,0X00                  *
    /************************************************************************/
    //#define ERR_PACKAGE_SIZE            5
    //#define ERR_PACKAGE_SUFFIX          (IS_BIG_ENDIAN ? 0X03000000 : 0X00000003)
    //#define ERR_NET_UNKNOWN             128
    //#define ERR_NET_FORMAT              129
    //#define ERR_NET_SERVER_BUSY         130
    //#define ERR_NET_TOO_FAST            131
    /**
     * contentType: CT_CHANGE_CIPHER_SPEC=20, CT_ALERT=21, CT_HANDSHAKE=22, CT_APPLICATION_DATA=23  // 大等于128的值为错误码
     * 
     */
	public class SslRecord
	{
        public const int SSL_RECORD_HEADER_SIZE		= 5;
	    // SSL记录字节数最大为 2^14 + 5, length字段表示的fragment的字节数最大为2^14
	    public const int SSL_RECORD_MAX_LENGTH		= 0X4005;
	    public const int SSL_FRAGMENT_MAX_LENGTH	= 0X4000;
	
	    public const short CT_CHANGE_CIPHER_SPEC	= 20;	// 0X14
	    public const short CT_ALERT					= 21;	// 0X15
	    public const short CT_HANDSHAKE				= 22;	// 0X16
	    public const short CT_APPLICATION_DATA		= 23;	// 0X17
	    public const short CT_ERR_NET_UNKNOWN		= 128;	// 0X80
	    public const short CT_ERR_NET_FORMAT		= 129;	// 0X81
	    public const short CT_ERR_NET_SERVER_BUSY	= 130;	// 0X82
	    public const short CT_ERR_NET_TOO_FAST		= 131;	// 0X83
	    public const short VER_SSL_30				= 0X0300;
	    public const short VER_SSL_31				= 0X0301;
	
	
	    private short		contentType;		// ubyte
	    private short		version;			// ushort
	    private int			length;				// ushort
	    private byte[]		fragment;
	
	    public SslRecord() 
        {
		    this.contentType	= CT_APPLICATION_DATA;
		    this.version		= VER_SSL_30;
		    this.length			= 0;
		    this.fragment		= null;
	    }
	
	    public SslRecord(short contentType) 
        {
		    this.contentType	= contentType;
		    this.version		= VER_SSL_30;
		    this.length			= 0;
		    this.fragment		= null;
	    }
	
	    public SslRecord(short contentType, byte[] fragment) 
        {
		    this.contentType	= contentType;
		    this.version		= VER_SSL_30;
		    this.length			= fragment.Length;
		    this.fragment		= fragment;
	    }

	    public short GetContentType() 
        {
		    return contentType;
	    }

	    public void SetContentType(short contentType) 
        {
		    this.contentType = contentType;
	    }

	    public int GetVersion() 
        {
		    return version;
	    }

	    public void SetVersion(short version) 
        {
		    this.version = version;
	    }

	    public int GetLength() 
        {
		    return length;
	    }

	    public byte[] GetFragment() 
        {
		    return fragment;
	    }

	    public void SetFragment(byte[] fragment) 
        {
		    this.length			= fragment.Length;
		    this.fragment		= fragment;
	    }
	
	    /**
	     * 取整个Record序列化数组
	     * @return	整个Record序列化数组，可作为socket的数据发送
	     */
	    public byte[] GetBytes() 
        {
		    int size = SSL_RECORD_HEADER_SIZE + (this.fragment == null ? 0 : this.fragment.Length); 
		    byte[] bytes = new byte[size];
		
		    bytes[0] = (byte)this.contentType;
		    bytes[1] = (byte)(this.version >> 8);
		    bytes[2] = (byte)(this.version & 0X00FF);
		    bytes[3] = (byte)(this.length >> 8);
		    bytes[4] = (byte)(this.length & 0X00FF);
		    System.Array.Copy(this.fragment, 0, bytes, 5, this.fragment.Length);
		
		    return bytes;
	    }
	
	    public int SetBytes(byte[] bytes) 
        {
		    return this.SetBytes(bytes, bytes.Length);
	    }
	
	    /**
	     * 将bytes数组解释为一条SslRecord记录
	     * @param bytes	SSL记录数据缓冲
	     * @param n		记录数据缓冲中的有效长度
	     * @return		绝对值为实际使用的字节数，非正数时表示不能成功解析到一个SslRecord记录
	     */
	    public int SetBytes(byte[] bytes, int n) 
        {
		    int iSizeUsed = 0; 
	        int iSizeNeeded = SSL_RECORD_HEADER_SIZE;
	    
		    if (bytes.Length < n || n < iSizeNeeded) {
			    return 0;
		    }
		
		    this.contentType 	= bytes[0];
		    this.version		= (short) ((((short)bytes[1] & 0X00FF) << 8) | ((short)bytes[2] & 0X00FF));
		    this.length			= (int) ((((short)bytes[3] & 0X00FF) << 8) | ((short)bytes[4] & 0X00FF));
		
		    iSizeUsed = SSL_RECORD_HEADER_SIZE;
		    iSizeNeeded = this.length;
		    if (this.version != VER_SSL_30 || n - iSizeUsed < iSizeNeeded) {
			    return -iSizeUsed;
		    }
		
		    if (iSizeNeeded == 0) { // 只有包头
			    return iSizeUsed;
		    }
		
		    this.fragment = new byte[iSizeNeeded];
            System.Array.Copy(bytes, iSizeUsed, this.fragment, 0, iSizeNeeded);
		    iSizeUsed += iSizeNeeded;
		
		    return iSizeUsed;
	    }
	}
}
