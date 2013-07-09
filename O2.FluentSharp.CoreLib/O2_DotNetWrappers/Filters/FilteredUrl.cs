// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;

namespace FluentSharp.CoreLib.API
{
    public class FilteredUrl
    {
        public string absolutePath;
        public string absoluteUrl;
        public bool conversionOk;
        public string errorMessage;
        public string fragement;
        public string host;
        public string originalUrl;
        public string page;
        public List<Parameter> parameters = new List<Parameter>();
        public List<string> parametersPairs;
        public string parametersRaw;
        public string path;
        public string pathAndPageAndParameters;
        public string scheme;
        public List<string> words;
        public List<string> wordsInPath;
        public List<string> wordsInPathAndPage;

        public FilteredUrl(String urlToProcess)
        {
            originalUrl = urlToProcess;
            parseUrl();
        }

        public void parseUrl()
        {
            try
            {
                var parsedUri = new Uri(originalUrl);
                host = parsedUri.Host;
                pathAndPageAndParameters = parsedUri.PathAndQuery;
                parametersRaw = parsedUri.Query.Replace("?", "");
                parametersPairs = new List<string>(parametersRaw.Split('&'));
                parametersPairs.Remove("");
                foreach (string parameter in parametersPairs)
                    parameters.Add(new Parameter(parameter));

                absoluteUrl = parsedUri.AbsoluteUri;
                fragement = parsedUri.Fragment;
                absolutePath = (fragement != "")
                                   ? parsedUri.AbsolutePath.Replace(fragement, "")
                                   : parsedUri.AbsolutePath;
                scheme = parsedUri.Scheme;

                wordsInPathAndPage = new List<string>(parsedUri.Segments);

                //wordsInPathAndPage.RemoveAll(new Predicate<string>(""));

                wordsInPath = new List<string>();
                for (int i = 0; i < wordsInPathAndPage.Count; i++)
                {
                    wordsInPathAndPage[i] = wordsInPathAndPage[i].Replace("/", "");
                    if (i < wordsInPathAndPage.Count - 1)
                        wordsInPath.Add(wordsInPathAndPage[i]);
                }
                wordsInPathAndPage.Remove("");
                wordsInPath.Remove("");
                page = (wordsInPathAndPage.Count == 0) ? "" : wordsInPathAndPage[wordsInPathAndPage.Count - 1];
                path = (page == "") ? "" : absolutePath.Replace(page, "");

                string wordsRawString = absoluteUrl;
                wordsRawString = wordsRawString.Replace('?', '/').Replace('=', '/').Replace('&', '/').Replace('#', '/');
                words = new List<string>(wordsRawString.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries));
                conversionOk = true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        #region Nested type: Parameter

        public class Parameter
        {
            public string name = "";
            public string value = "";

            public Parameter(string parameterPair)
            {
                string[] splitedParameter = parameterPair.Split('=');
                if (splitedParameter.Length == 2)
                {
                    name = splitedParameter[0];
                    value = splitedParameter[1];
                }
            }
        }

        #endregion
    }
}
