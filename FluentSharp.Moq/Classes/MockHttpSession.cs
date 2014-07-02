using System.Collections.Generic;
using System.Web;
using FluentSharp.CoreLib;

namespace FluentSharp.Moq
{
    public class MockHttpSession : HttpSessionStateBase
    {
        Dictionary<string,object> sessionData = new Dictionary<string,object> ();
        string                    sessionId   = "".add_RandomLetters(15);

        public override string SessionID
        {
            get { return sessionId; }
        }
        public override object this[string key]
        {
            get
            {                
                return (sessionData.hasKey(key)) ? sessionData[key] :  null;
            }
            set { sessionData[key] = value; }
        }
        public override int Count
        {
            get { return sessionData.Count; }
        }
    }
}