using System;
using System.Collections.Generic;
using System.Linq;
using FluentSharp.CoreLib;

namespace FluentSharp.Web35
{
    public static class Web_ExtensionMethods_QueryString
    { 
        public static List<string>              queryParameters_Names(this List<Uri> uris)
        {			
            return (from uri in uris
                from name in uri.queryParameters_Indexed_ByName().Keys					
                select name).Distinct().toList();
        }		
        public static List<string>              queryParameters_Values(this List<Uri> uris, string parameterName)
        {
            return (from uri in uris
                let parameters = uri.queryParameters_Indexed_ByName()										
                where parameters.hasKey(parameterName)					
                select parameters[parameterName]).toList();					
        }		
        public static List<string>              queryParameters_Values(this List<Uri> uris)
        {
            var values = new List<string>();
            foreach(var uri in uris)
                values.AddRange(uri.queryParameters_Indexed_ByName().Values);								
            return values;				
        }		
        public static Dictionary<string,string> queryParameters_Indexed_ByName(this Uri uri)
        {		
            var queryParameters = new Dictionary<string,string>();
            if (uri.notNull())
            {
                var query = uri.Query;
                if (query.starts("?"))
                    query = query.removeFirstChar();
                foreach(var parameter in query.split("&"))				
                    if (parameter.valid())
                    {
                        var splitParameter = parameter.split("=");
                        if (splitParameter.size()==2)
                            if (queryParameters.hasKey(splitParameter[0]))
                            {	
                                "duplicate parameter key in property '{0}', adding extra parameter in a new line".info(splitParameter[0]);
                                queryParameters.add(splitParameter[0], queryParameters[splitParameter[0]].line() + splitParameter[1]);
                            }
                            else
                                queryParameters.add(splitParameter[0], splitParameter[1]);
                        else						
                            "Something's wrong with the parameter value, there should only be one = in there: '{0}' ".info(parameter);
                    }					
            } 
            return queryParameters;
        }
    }
}