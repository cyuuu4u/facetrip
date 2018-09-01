using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using xxdwunity.util;

namespace xxdwunity.comm
{
    public class ClientSocket
    {
        private Socket s = null;
	
	    public ClientSocket(String dstName, int dstPort)
        {
		    try   
            {
                this.s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPAddress[] ia = Dns.GetHostAddresses(dstName);                
                //IPAddress ip = IPAddress.Parse(dstName);
                //IPEndPoint ipe = new IPEndPoint(ia[0], dstPort);
                this.s.Connect(ia, dstPort); 
            } 
            catch (Exception e)   
            {
                this.s = null;
                XxdwDebugger.Log(e.ToString());
                throw e;
            }   
	    }
	
	    public void Close() 
        {
            if (this.s != null)
            {
                this.s.Shutdown(SocketShutdown.Both);
                this.s.Close();
            }
	    }
	
	    public void Send(SslAppData sad)
        {
		    SslRecord sr = new SslRecord();
		
		    sr.SetContentType(SslRecord.CT_APPLICATION_DATA);
		    sr.SetFragment(sad.GetBytes());
		
		    this.Send(sr);
	    }
	
	    public void Send(SslRecord sr) 
        {
            this.s.Send(sr.GetBytes());
	    }
	
	    public int Recv(SslAppData sad) 
        {
		    SslRecord sr = new SslRecord();		
		    int n = this.Recv(sr);
            sad.SetBytes(sr.GetBytes(), SslRecord.SSL_RECORD_HEADER_SIZE, n - SslRecord.SSL_RECORD_HEADER_SIZE);		
		    return n;
	    }
	
	    public int Recv(SslRecord sr)
        {
		    int n = 0;
            byte[] b = new byte[SslRecord.SSL_RECORD_MAX_LENGTH];

            n = this.s.Receive(b);
            sr.SetBytes(b, n);
				
		    return n;
	    }
    } 
}
