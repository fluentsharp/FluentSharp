using System;
using System.Net;
using System.Net.Sockets;

namespace FluentSharp.CoreLib
{
    public static class NetWork_ExtensionMethods
    {
        //TcpClient Client
        public static TcpClient     tcpClient(this int localPort)
        {
            try
            {
                var  tcpClient = new TcpClient();
                tcpClient.Connect(IPAddress.Loopback, localPort);
                return tcpClient;    
            }
            catch (Exception ex)
            {
                ex.log("TCPClient");
                return null;
            }
        }
        
        public static NetworkStream stream  (this TcpClient tcpClient)
        {
            if (tcpClient.notNull())
                try
                {
                    return tcpClient.GetStream();
                }
                catch (Exception ex)
                {
                    ex.log("[tcpClient][stream]");

                }
            return null;
        }
        public static bool          canRead (this TcpClient tcpClient)
        {
            var stream = tcpClient.stream();
            return stream.notNull() && stream.CanRead;
        }
        public static TcpClient     close   (this TcpClient tcpClient) 
        {
            if (tcpClient.notNull())
                try
                {
                    tcpClient.Close();
                }
                catch (Exception ex)
                {
                    ex.log("[tcpClient][close]");

                }
            return tcpClient;
        }
        public static bool          canWrite(this TcpClient tcpClient)
        {
            var stream = tcpClient.stream();
            return stream.notNull() && stream.CanWrite;
        }
        public static bool          connected(this TcpClient tcpClient) 
        {
            if (tcpClient.notNull())
                try
                {
                    return tcpClient.Connected;
                }
                catch (Exception ex)
                {
                    ex.log("[tcpClient][connected]");

                }
            return false;
        }
        public static TcpClient     write(this TcpClient tcpClient, string value)
        {
            return tcpClient.write(value.bytes_Ascii());
        }
        public static TcpClient     write(this TcpClient tcpClient, byte[] bytes)
        {
            var stream = tcpClient.stream();
            if (stream.notNull())
                try
                {
                    stream.Write(bytes, 0, bytes.size());
                    stream.Flush();
                    return tcpClient;
                }
                catch (Exception ex)
                {
                    ex.log("[tcpClient][connected]");

                }
            return null;
        }

        //TcpListener
        public static byte[]        getAvailableBytes(this Socket socket)
        {            
            if (socket.notNull())
            {
                var available = socket.Available;                
                if (available > 0)
                {

                    var bytes = new byte[available];
                    socket.Receive(bytes);
                    return bytes;
                }
            }
            return new byte[0];
        }

        public static TcpListener   tcpListener(this int localPort)
        {
            try
            {
                var tcpListener = new TcpListener(IPAddress.Loopback, localPort).start();                
                return tcpListener;
            }
            catch (Exception ex)
            {
                ex.log("[TcpListener]");
                return null;
            }
        }
        
        public static TcpListener   start             (this TcpListener tcpListener)
        {
            if (tcpListener.notNull())
                try
                {
                    tcpListener.Start();
                    return tcpListener;
                }
                catch (Exception ex)
                {
                    ex.log("[TcpListener][stop]");                    
                }
            return null;
        }
        public static TcpListener   stop              (this TcpListener tcpListener)
        {
            if (tcpListener.notNull())
                try
                {
                    tcpListener.Stop();
                    return tcpListener;
                }
                catch (Exception ex)
                {
                    ex.log("[TcpListener][stop]");

                }
            return null;
        }
        public static bool          active            (this TcpListener tcpListener)
        {
            if (tcpListener.notNull())
                try
                {
                    return tcpListener.prop<bool>("Active");
                }
                catch (Exception ex)
                {
                    ex.log("[TcpListener][active]");

                }
            return false;
        }
        public static Socket        server_socket     (this TcpListener tcpListener)
        {
            if (tcpListener.notNull())
                try
                {
                    return tcpListener.Server;
                }
                catch (Exception ex)
                {
                    ex.log("[TcpListener][socket]");

                }
            return null;
        }
        public static Socket        waitForConnection (this TcpListener tcpListener)
        {
            if (tcpListener.notNull())
                return tcpListener.AcceptSocket();
            return null;
        }

    }
}
