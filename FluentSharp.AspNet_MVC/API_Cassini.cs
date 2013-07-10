using CassiniDev;
using FluentSharp.CoreLib;

namespace FluentSharp.AspNet_MVC
{
    public class API_Cassini
    {
        public Server CassiniServer     { get; set; }
        public string PhysicalPath      { get; set; }        
        
        public API_Cassini() : this("CassiniSite".tempDir(false))
        {            
        }
        public API_Cassini(string physicalPath)
        {
            PhysicalPath = physicalPath;
            CassiniServer = new Server(physicalPath);            
        }
        public API_Cassini(string physicalPath, string virtualPath, int port)
        {
            PhysicalPath = physicalPath;
            CassiniServer = new Server(port, virtualPath, physicalPath);
        }        
    }

    public static class API_Cassini_ExtensionMethods
    {
        public static Server server(this API_Cassini apiCassini)
        {
            return (apiCassini.notNull())
                       ? apiCassini.CassiniServer
                       : null;
        }

        public static int    port(this API_Cassini apiCassini)
        {
            return apiCassini.notNull() 
                        ? apiCassini.CassiniServer.Port 
                        : -1;
        }

        public static string url(this API_Cassini apiCassini)
        {
            if (apiCassini.notNull())
            {
                var server = apiCassini.server();
                return "http://{0}:{1}/".format(server.IPAddress.str(), server.Port);
            }
            return null;
        }

        public static API_Cassini start(this API_Cassini apiCassini)
        {
            if (apiCassini.notNull())
                apiCassini.CassiniServer.Start();
            return apiCassini;
        }
        public static API_Cassini stop(this API_Cassini apiCassini)
        {
            if (apiCassini.notNull())
                apiCassini.CassiniServer.ShutDown();
            return apiCassini;
        }
    }
}
