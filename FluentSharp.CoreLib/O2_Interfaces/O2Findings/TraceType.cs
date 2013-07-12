// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;

namespace FluentSharp.CoreLib.Interfaces
{
    [Serializable]
    public enum TraceType
    {
        Type_0 = 0,
        Root_Call = 1,
        Source = 2,
        Known_Sink = 3,
        Type_4 = 4,
        Lost_Sink = 5,
        Type_6 = 6,
        O2JoinSink = 30,
        O2JoinSource = 31, 
        O2JoinLocation = 32,
        O2Info= 40
    }
}