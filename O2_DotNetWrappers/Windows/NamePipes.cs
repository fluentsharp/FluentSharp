using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2.Kernel.ExtensionMethods;
using O2.DotNetWrappers.ExtensionMethods;
using System.IO.Pipes;
using O2.DotNetWrappers.DotNet;
using System.IO;

namespace O2.DotNetWrappers.Windows
{
    public class NamePipes
    {
        public static NamedPipeServerStream createNamePipeServer(string pipeName, Action<string> dataReceived)
        {
            return createNamePipeServer(pipeName, dataReceived, null);
        }

        public static NamedPipeServerStream createNamePipeServer(string pipeName, Action<string> dataReceived, Action onPipeTerminated)
        {
            var namedPipeServerStream = new NamedPipeServerStream(pipeName);            
            "[NamePipeServer][{0}] Pipe created {1}".format(pipeName, namedPipeServerStream.GetHashCode()).info();
            O2Thread.mtaThread(
                () =>
                {                    
                    namedPipeServerStream.WaitForConnection();
                    "[NamePipeServer][{0}] Pipe connection established".format(pipeName).info();
                    using (StreamReader streamReader = new StreamReader(namedPipeServerStream))
                    {                                                                        
                        string line;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            //"received text:{0}".format(line).debug();
                            dataReceived(line);
                        }
                        "[NamePipeServer][{0}] Connection lost or pipe terminated".format(pipeName).info();
                        if (onPipeTerminated != null)
                            onPipeTerminated();
                    }
                });
            return namedPipeServerStream;
        }

        public static NamedPipeClientStream createNamePipeClient(string pipeName)
        {
            return createNamePipeClient(pipeName, 1000);
        }

        public static NamedPipeClientStream createNamePipeClient(string pipeName, int maxConnectTime)
        {
            NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(pipeName);                        
            "[NamePipeClient][{0}] Trying to connected to pipe".format(pipeName).info();
            namedPipeClientStream.Connect(maxConnectTime);
            "[NamePipeClient][{0}] Pipe connection established".format(pipeName).info();                
            return namedPipeClientStream;                
        }    
    }
}

