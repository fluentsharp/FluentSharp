using System;
using NGit;

namespace FluentSharp.ExtensionMethods
{
    public static class API_NGit_Objects
    {

        public static ObjectLoader  open_Object(this API_NGit nGit, string sha1)    
        {
            return nGit.repository().open(sha1);
        }
        public static ObjectLoader  open(this Repository repository, string sha1)   
        {
            try
            {
                if (repository.notNull())
                {
                    var objectId = sha1.objectId();
                    if (objectId.valid())
                        return repository.Open(objectId);
                }                
            }
            catch (Exception ex)
            {
                ex.log("[API_NGit][open]");
            }
            return null;
        }        
        public static ObjectStream  stream(this ObjectLoader objectLoader)          
        {
            if (objectLoader.notNull())
                return objectLoader.OpenStream();
            return null;
        }
        public static byte[]        bytes(this ObjectStream objectStream)           
        {
            if (objectStream.notNull())
            {
                try
                {
                    var bytes = new byte[objectStream.Available()];
                    objectStream.Read(bytes);
                    return bytes;
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][ObjectStream][bytes]");
                }
            }
            return new byte[] {};
        }
        public static byte[]        bytes(this ObjectLoader objectLoader)
        {
            if (objectLoader.notNull())
                try
                {
                    return objectLoader.GetBytes();
                }
                catch (Exception ex)
                {
                    ex.log("[API_NGit][ObjectStream][bytes]");
                }
            return new byte[] {};
        }
    }
}
