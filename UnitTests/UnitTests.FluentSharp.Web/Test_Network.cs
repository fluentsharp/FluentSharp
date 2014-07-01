using System;
using System.Net.Sockets;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{
    [TestFixture]
    public class Test_Network_TcpClient
    {
        public int TestPort { get; set; }

        public Test_Network_TcpClient()
        {
            TestPort = 30000 + 20000.random();
        }

        //TcpClient Client
        [Test(Description = "Returns true if the TcpClient is connected")]
        public void connected()
        {
            //tcpClient();

            //Test Exception handling
            var tcpClient = new TcpClient();
            tcpClient.field("m_ClientSocket", null);
            KO2Log.Last_Exception = null;            
            Assert.IsNull(tcpClient.field("m_ClientSocket"));
            Assert.IsFalse(tcpClient.connected());
            Assert.IsInstanceOf<NullReferenceException>(KO2Log.Last_Exception);            
        }

        [Test(Description = "Gets a TcpClient NetworkStream object")]
        public void stream()
        {
            //tcpClient();

            //Test Exception handling
            var tcpClient = new TcpClient();
            KO2Log.Last_Exception = null;
            Assert.IsNotNull(tcpClient.field("m_ClientSocket"));
            Assert.IsNull   (tcpClient.stream());
            Assert.IsInstanceOf<InvalidOperationException>(KO2Log.Last_Exception);            
        }

        [Test(Description = "Gets a TcpClient object for a give port")]
        public void tcpClient()
        {
            //try when port is not open
            var testPort = 30000 + 20000.random();
            var tcpClient1 = testPort.tcpClient();            
            Assert.Greater(UInt16.MaxValue, testPort);
            Assert.IsNull(tcpClient1);            
            //not finished

            //open port and try to connect 
            var tcpListener2 = testPort.tcpListener();
            var tcpClient2 = testPort.tcpClient();     
       
            Assert.IsNotNull(tcpListener2);            
            Assert.IsTrue   (tcpListener2.active());            
            Assert.IsNotNull(tcpClient2);
            Assert.IsNotNull(tcpClient2.stream());
            Assert.IsTrue   (tcpClient2.connected());
            Assert.IsTrue   (tcpClient2.canRead());
            Assert.IsTrue   (tcpClient2.canWrite());
            //stop Listener
            tcpListener2.stop();
            tcpClient2.close();

            Assert.IsFalse   (tcpListener2.active());                
            Assert.IsFalse   (tcpClient2.canRead());
            Assert.IsFalse   (tcpClient2.canWrite());
            Assert.IsFalse   (tcpClient2.connected());  
        }

        [Test(Description = "Writes bytes or an string to an open TcpClient")]
        public void write()
        {            
            var bytes = 200.randomBytes();            

            var tcpListener = TestPort.tcpListener();
            var tcpClient = TestPort.tcpClient();
            tcpClient.write(bytes);
            
            //not finished
            tcpListener.stop();
            tcpClient.close();

        }

    }

    [TestFixture]
    public class Test_Network_TcpListener
    {
        public int TestPort { get; set; }

        public Test_Network_TcpListener()
        {
            TestPort = 30000 + 20000.random();
        }
        
        [Test(Description = "Gets bytes current available in the tcpListener")]
        public void getAvailableBytes()
        {
            var tcpListener = TestPort.tcpListener();                        
            var tcpClient   = TestPort.tcpClient();
            var socket      = tcpListener.waitForConnection();                                         
            
            var message1   = "This is a longer ping";
            var message2   = "Followed by this one";
            var size1      = message1.size();
            var size2      = message2.size();

            //send message1
            tcpClient.write(message1);
            
            var availableBytes1 = socket.getAvailableBytes();
            Assert.AreEqual(availableBytes1.size(), size1);
            Assert.AreEqual(availableBytes1.ascii(), message1);

            tcpClient.write(message2);
            //send message2
            var availableBytes2 = socket.getAvailableBytes();
            Assert.AreEqual(availableBytes2.size(), size2);
            Assert.AreEqual(availableBytes2.ascii(), message2);

            socket.Close();
            tcpListener.stop();            
            tcpClient.close(); 
            Assert.IsFalse(socket.Connected);
            Assert.IsFalse(tcpClient.connected());
            Assert.IsFalse(tcpListener.active());
        }

        [Test(Description = "Gets a Socket object from an TcpListener")]
        public void socket()
        {
            var tcpListener = TestPort.tcpListener();                        
            var tcpClient = TestPort.tcpClient();

            Assert.IsNotNull(tcpListener.server_socket());
            Assert.IsFalse  (tcpListener.server_socket().Connected);
            var socket = tcpListener.AcceptSocket();
            
            Assert.IsNotNull(socket);
            Assert.IsTrue(socket.Connected);
            
            var message1 = "Ping";
            var size     = message1.size();
            tcpClient.write(message1);
            Assert.AreEqual(size, socket.Available);

            var received_Bytes = new byte[size];
            socket.Receive(received_Bytes);
            var received_String = received_Bytes.ascii();
            Assert.AreEqual(0, socket.Available);
            Assert.AreEqual(message1, received_String);            

            socket.Close();
            tcpListener.stop();            
            tcpClient.close(); 
            Assert.IsFalse(socket.Connected);
            Assert.IsFalse(tcpClient.connected());
            Assert.IsFalse(tcpListener.active());
        }
        [Test(Description = "Starts TcpListener (called by the tcpListener method)")]
        public void start()
        {
            //tcpListener()  
        }

        [Test(Description = "Stops TcpListener")]
        public void stop()
        {   
            var testPort = 30000 + 20000.random();
            var tcpListener1 = testPort.tcpListener();
            Assert.NotNull  (tcpListener1);
            Assert.IsNotNull(tcpListener1.field("m_ServerSocketEP"));
            //Test Exception handling                     
            tcpListener1.field("m_ServerSocketEP", null);
            KO2Log.Last_Exception = null;
            Assert.IsNull(tcpListener1.field("m_ServerSocketEP"));            
            Assert.IsNull(tcpListener1.stop());
            
            Assert.IsInstanceOf<NullReferenceException>(KO2Log.Last_Exception);            
        }

        [Test(Description = "Gets a TcpListener object for a give port")]
        public void tcpListener()
        {
            //open a tcpListener
            var testPort = 30000 + 20000.random();
            var tcpListener1 = testPort.tcpListener();
            Assert.Greater  (UInt16.MaxValue, testPort);
            Assert.IsNotNull(tcpListener1);           
            Assert.IsTrue  (tcpListener1.active());

            //open a tcpListener while there is one open
            var tcpListener2 = testPort.tcpListener();            
            Assert.IsNull  (tcpListener2);
            Assert.IsFalse  (tcpListener2.active());
            Assert.IsInstanceOf<SocketException>(KO2Log.Last_Exception, "");

           //close first one
            tcpListener1.stop();
            Assert.IsFalse  (tcpListener2.active());            
             
            //open another one on the same port
            var tcpListener3 = testPort.tcpListener();            
            Assert.IsNotNull (tcpListener3);
            Assert.IsTrue   (tcpListener3.active());            

            //close and check that all are not active
            tcpListener3.stop();
            Assert.IsFalse  (tcpListener1.active());            
            Assert.IsFalse  (tcpListener2.active());
            Assert.IsFalse  (tcpListener3.active());


            //Test Exception handling
            Assert.IsNull((-1).tcpListener());
        }


    }
}
