using System;
using FluentSharp.CoreLib.API;
using System.IO.Pipes;
using System.IO;

namespace FluentSharp.CoreLib
{
    public static class NamedPipes_ExtensionMethods
    {
        public static NamedPipeServerStream namePipeServer(this string pipeName, Action<string> dataReceived)
        {
            return NamePipes.createNamePipeServer(pipeName, dataReceived);
        }

        public static NamedPipeClientStream namePipeClient(this string pipeName)
        {
            return NamePipes.createNamePipeClient(pipeName);
        }

        public static StreamWriter writer(this NamedPipeClientStream namedPipeClientStream)
        {
            var streamWriter = new StreamWriter(namedPipeClientStream);
            streamWriter.AutoFlush = true;
            return streamWriter;
        }

        public static NamedPipeClientStream send(this NamedPipeClientStream namedPipeClientStream, string textToWrite)
        {
            namedPipeClientStream.writer().send(textToWrite);
            return namedPipeClientStream;
        }

        public static StreamWriter send(this StreamWriter streamWriter, string textToWrite)
        {
            "sending text:{0}".format(textToWrite).debug();
            streamWriter.WriteLine(textToWrite);
            return streamWriter;
        }
    }
}

