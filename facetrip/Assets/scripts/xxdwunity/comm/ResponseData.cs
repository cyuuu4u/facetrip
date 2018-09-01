using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xxdwunity.comm
{
    public class ResponseData
    {
        public const int RESP_SUCCESS			= 0;
	    public const int RESP_FAIL				= -1;
	
	    public const int RESP_SERVER_UNREACHABLE = 100;
	    public const int RESP_NO_LICENSE 		= 101;
	    public const int RESP_WRONG_PASSWORD		= 102;
	    public const int RESP_INVALID_LICENSE	= 103;
	    public const int RESP_DATE_EXPIRED		= 104;

        public int RequestId { get; set; }          // 标识请求ID
        public int Code { get; set; }               // 出错码
        public string Msg { get; set; }
        public object Data { get; set; }
	
	    public ResponseData() {
            this.RequestId  = 0;
		    this.Code 	    = RESP_FAIL;
		    this.Msg 	    = "";
		    this.Data	    = null;
	    }

        public ResponseData(int requestId)
        {
            this.RequestId = requestId;
            this.Code = RESP_FAIL;
            this.Msg = "";
            this.Data = null;
        }

	    public ResponseData(int requestId, int code, string msg) {
            this.RequestId  = requestId;
            this.Code 	    = code;
		    this.Msg	    = msg;
		    this.Data	    = null;
	    }
    }
}
