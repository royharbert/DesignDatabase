using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//namespace DesignDB_Library.Operations
//{
//    public static class VPNCheck
//    {
//        static void GetHostName(IAsyncResult result)
//        {
//            var host = "";
//            Boolean isWANConnected = false;
//            String ConnectMessage;
//            AsyncCallback callBack = new AsyncCallback(GetHostName);
//            Dns.BeginGetHostEntry(host, callBack, host);

//                    string hostname = (string)result.AsyncState;
//                    try
//                    {
//                        IPHostEntry host = Dns.EndGetHostEntry(result)
//                       ConnectMessage = host as String;
//                    }
//                    catch (SocketException e)
//                    {
//                        isWANConnected = false;
//                        ConnectMessage = e.Message;
//                    }
//                }
//    }
//}
