// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.Interfaces
{
    public interface IXRulesDatabase
    {
        void installXRulesDatabase(string pathTo_XRulesDatabase_FromO2, string pathTo_XRulesTemplates);
        bool loadArtifact(string fileToLoad, Dictionary<Type, object> loadedArtifaces, bool loadFileAsObject);
    }
}