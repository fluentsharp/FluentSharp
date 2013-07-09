using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.Git.APIs;
using NGit;
using NGit.Revwalk;

namespace FluentSharp.Git
{
    public static class RevWalk_ExtensionMethods
    {
        public static RevWalk revWalk(this API_NGit nGit)
        {
            if (nGit.notNull() && nGit.Repository.notNull())
                return new RevWalk(nGit.Repository);
            return null;
        }

        public static List<RevObject> objects(this RevWalk revWalk)
        {
            var revObjects = (ObjectIdOwnerMap<RevObject>) revWalk.field("objects");            
            return revObjects.toList();            

        }

        public static RevObject object_Get(this RevWalk revWalk, string objectId)
        {
            return revWalk.object_Get(objectId.objectId());
        }

        public static RevObject object_Get(this RevWalk revWalk, AnyObjectId objectId)
        {
            if (revWalk.notNull() && objectId.notNull())
                return revWalk.LookupOrNull(objectId);
            return null;
        }
    }
}
