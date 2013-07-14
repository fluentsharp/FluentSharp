// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Drawing;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtUtils
    {
        public static Color KnownSinkColor = Color.Red;
        public static Color LostSinkColor = Color.DarkOrange;
        public static Color SourceColor = Color.DarkRed;
        public static Color NotMappedColor = Color.Black;

        public static IO2Trace getSink(List<IO2Trace> o2Traces)
        {
            var sinkTrace = getKnownSink(o2Traces);
            return (sinkTrace) ?? getLostSink(o2Traces);
        }
        public static IO2Trace getKnownSink(IO2Trace o2Trace)
        {
            return getTraceType(new List<IO2Trace>().add(o2Trace), TraceType.Known_Sink);
        }

        public static IO2Trace getLostSink(IO2Trace o2Trace)
        {
            return getTraceType(new List<IO2Trace>().add(o2Trace), TraceType.Lost_Sink);
        }

        public static IO2Trace getSource(IO2Trace o2Trace)
        {
            return getTraceType(new List<IO2Trace>().add(o2Trace), TraceType.Source);
        }

        public static IO2Trace getKnownSink(List<IO2Trace> o2Traces)
        {
            return getTraceType(o2Traces, TraceType.Known_Sink);
        }

        public static IO2Trace getLostSink(List<IO2Trace> o2Traces)
        {
            return getTraceType(o2Traces, TraceType.Lost_Sink);
        }

        public static IO2Trace getSource(List<IO2Trace> o2Traces)
        {
            return getTraceType(o2Traces, TraceType.Source);
        }

        public static IO2Trace getTraceType(List<IO2Trace> o2Traces, TraceType traceType)
        {
            foreach (IO2Trace o2Trace in o2Traces)
            {
                if (o2Trace.traceType == traceType)
                    return o2Trace;
                if (o2Trace.childTraces != null)
                    //foreach (O2Trace childO2Trace in o2Trace.childTraces)
                {
                    IO2Trace result = getTraceType(o2Trace.childTraces, traceType);
                    if (null != result)
                        return result;
                }
            }
            return null;
        }   
     
        /// <summary>
        /// (recursive call) returns a direct path between the found traceType and the first o2trace provided in o2Traces
        /// </summary>
        /// <param name="o2Traces"></param>
        /// <param name="traceType"></param>
        /// <param name="pathToTraceType"></param>
        /// <returns></returns>
        public static bool getPathToTraceType(List<IO2Trace> o2Traces, TraceType traceType, List<IO2Trace> pathToTraceType)
        {
            foreach (IO2Trace o2Trace in o2Traces)
            {
                if (o2Trace.traceType == traceType)
                {
                    pathToTraceType.Add(o2Trace);
                    return true;
                }
                if (o2Trace.childTraces != null)                
                {
                    if (getPathToTraceType(o2Trace.childTraces, traceType, pathToTraceType))
                    {
                        pathToTraceType.Add(o2Trace);
                        return true;
                    }                    
                }
            }
            return false;

        }

        public static List<IO2Trace> getPathToSource(List<IO2Trace> o2Traces)
        {
            var pathToTraceType = new List<IO2Trace>();
            getPathToTraceType(o2Traces, TraceType.Source, pathToTraceType);
            return pathToTraceType;
        }

        public static List<string> getUniqueListOfSignatures(IEnumerable<IO2Finding> o2Findings)
        {
            var uniqueVulnNames = new List<string>();// (from IO2Finding o2Finding in o2Findings select o2Finding.vulnName).ToList();

            foreach (var ofFinding in o2Findings)
            {
                var signaturesInTraces =  getStringListWithAllUniqueSignatures(ofFinding.o2Traces);
                foreach(var signatureInTrace in signaturesInTraces)
                    if(false == uniqueVulnNames.Contains(signatureInTrace))
                        uniqueVulnNames.Add(signatureInTrace);
            }
            return uniqueVulnNames;
        }

        public static Dictionary<String, List<IO2Trace>> getDictionaryWithO2AllSubTraces(O2Assessment o2Assessment)
        {
            return getDictionaryWithO2AllSubTraces(o2Assessment, true);
        }

        public static Dictionary<String, List<IO2Trace>> getDictionaryWithO2AllSubTraces(O2Assessment o2Assessment, bool uniqueList)
        {
            var allTracesInAssessmment = new Dictionary<String, List<IO2Trace>>();
            foreach (IO2Finding o2Finding in o2Assessment.o2Findings)
            {
                getAllTraces(o2Finding.o2Traces, allTracesInAssessmment, uniqueList);
            }
            return allTracesInAssessmment;
        }

        public static IEnumerable<string> getStringListWithAllUniqueSignatures(List<IO2Trace> o2Traces)
        {
            var allTraces = getAllTraces(o2Traces, true /*uniqueList*/);
            return allTraces.Keys;
            //return new List<string> (allTraces.Keys);
        }

        public static void calculateUniqueListOfO2Traces(IO2Finding o2Finding, List<IO2Trace> uniqueO2Traces)
        {
            calculateUniqueListOfO2Traces(o2Finding.o2Traces, uniqueO2Traces);
        }

        public static void calculateUniqueListOfO2Traces(List<IO2Trace> o2TracesToProcess, List<IO2Trace> uniqueO2Traces)
        {
            if (o2TracesToProcess!= null)
                foreach (var o2Trace in o2TracesToProcess)
                {
                    if (false == uniqueO2Traces.Contains(o2Trace))
                        uniqueO2Traces.Add(o2Trace);
                    calculateUniqueListOfO2Traces(o2Trace.childTraces, uniqueO2Traces);
                }
        }

        public static Dictionary<String, List<IO2Trace>> getAllTraces(List<IO2Trace> o2Traces)
        {
            return getAllTraces(o2Traces,false /*uniqueList*/);
        }

        public static Dictionary<String, List<IO2Trace>> getAllTraces(List<IO2Trace> o2Traces, bool uniqueList)
        {
            var allTraces = new Dictionary<String, List<IO2Trace>>();
            getAllTraces(o2Traces, allTraces, uniqueList);
            return allTraces;
        }

        public static void getAllTraces(List<IO2Trace> o2Traces, Dictionary<String, List<IO2Trace>> allTraces, bool uniqueList)
        {
            foreach (IO2Trace o2Trace in o2Traces)
            {
                //if (o2Trace.childTraces.Count > 0)                
                    if (o2Trace.signature != "")
                    {
                        if (uniqueList == false || false == allTraces.ContainsKey(o2Trace.signature)) // if uniqueList==true only add one trace per signature
                        {
                            if (false == allTraces.ContainsKey(o2Trace.signature))
                                allTraces.Add(o2Trace.signature, new List<IO2Trace>());
                            allTraces[o2Trace.signature].Add(o2Trace);
                        }
                        getAllTraces(o2Trace.childTraces, allTraces, uniqueList);                        
                    }
                
                /*if (o2Trace.childTraces.Count > 0)
                    foreach (O2Trace childTrace in o2Trace.childTraces)
                        getAllTraces(childTrace, allTraces);*/
            }
        }

        public static List<String> getListWithWordsFromSignature(string signature)
        {
            var filteredSignature = new FilteredSignature(signature);
            List<string> wordsFromSignature = filteredSignature.lsFunctionClass_Parsed; // words in namespace & class
            wordsFromSignature.Add(filteredSignature.sFunctionName); // also add the method name
            return wordsFromSignature;
        }

        public static Color getTraceColorBasedOnRuleType(IO2Rule o2Rule)
        {
            switch (o2Rule.RuleType)
            {
                case O2RuleType.Source:
                    return SourceColor;
                case O2RuleType.Sink:
                    return KnownSinkColor;
                case O2RuleType.LostSink:
                    return LostSinkColor;
                case O2RuleType.Callback:
                    return Color.Salmon;
                case O2RuleType.PropageTaint:
                    return Color.DarkGreen;
                case O2RuleType.DontPropagateTaint:
                    return Color.Purple;
                case O2RuleType.NotMapped:
                    return Color.DarkGray;
                default:
                    return NotMappedColor;
            }
        }
        
        public static Color getTraceColorBasedOnTraceType(IO2Trace o2Trace)
        {
            switch (o2Trace.traceType)
            {
                case TraceType.Type_0:
                    return Color.DarkBlue;
                case TraceType.Known_Sink:
                    return KnownSinkColor;
                case TraceType.Lost_Sink:
                    return LostSinkColor;
                case TraceType.Root_Call:
                case TraceType.Type_6:
                    return Color.DarkBlue;
                case TraceType.Source:
                    return SourceColor;                    
                case TraceType.Type_4:
                    return Color.Green;
                case TraceType.O2JoinSink:
                    return Color.DeepSkyBlue;
                case TraceType.O2JoinSource:
                    return Color.LimeGreen;
                case TraceType.O2JoinLocation:
                    return Color.Purple;
                case TraceType.O2Info:
                    return Color.Black;

                default:
                    return Color.Gray;
            }
        }

        /*public static Dictionary<String, List<O2Finding>> getDictionaryWithO2TracesPerMethod(O2Assessment o2Assessment, TraceType traceType)
        {
            var stringIndexes = new Dictionary<String, List<O2Finding>>();
            foreach (var o2Finding in o2Assessment.o2Findings)
            {
                var index = "";
                switch (traceType)
                {
                    case TraceType.Known_Sink:
                        index = OzasmtUtils.getKnownSink(o2Finding.o2Trace).signature;
                        break;
                    default:
                        //case TraceType.Source
                        index = OzasmtUtils.getSource(o2Finding.o2Trace).signature;
                        break;
                }
                if (false == stringIndexes.ContainsKey(index))
                    stringIndexes.Add(index, new List<O2Finding>());

                stringIndexes[index].Add(o2Finding);
            }
            return stringIndexes;
        }*/
  
        public static byte getSeverityFromString(string severity)
        {
            switch (severity)
            {
                case "High":
                    return 0;
                case "Medium":
                    return 1;
                case "Low":
                    return 2;
                case "Info":
                    return 3;
                default:
                    return 0xFF; // "(unrecognized severity value)";
            }
        }

        public static string getSeverityFromId(byte severityId)
        {
            switch (severityId)
            {
                case 0:
                    return "High";                    
                case 1:
                    return  "Medium";                    
                case 2:
                    return "Low";                    
                case 3:
                    return "Info";                    
                default:
                    return "(unrecognized severity value)";                    
            }
        }

        public static string getConfidenceFromId(byte confidenceID)
        {
            switch (confidenceID)
            {
                case 1:
                    return "Vulnerability";                    
                case 2:
                    return "Type I";                    
                case 3:
                    return "Type II";                    
                default:
                    return "(unrecognized confidence value)";                    
            }
        }

        // not working at the moment since the .Net XsmlSerializer doesn't support Interfaces (prob we will need to use the WCF serializer)
        /*
        public static string createSerializedXmlStringFromO2Finding(IO2Finding o2FindingToSerialize)
        {
            return Serialize.createSerializedXmlStringFromObject(o2FindingToSerialize, new [] { typeof(O2Trace), typeof(List<O2Trace>)} );
        }*/

        public static Dictionary<String, List<IO2Trace>> getDictionaryWithO2AllSubTraces(IO2AssessmentLoad o2AssessmentLoad, String assessmentFile)
        {
            return getDictionaryWithO2AllSubTraces(new O2Assessment(o2AssessmentLoad, assessmentFile), false /*uniqueList*/);
        }

        public static IO2AssessmentLoad getO2AssessmentLoadEngine(string pathToFileToLoad, List<IO2AssessmentLoad> o2AssessmentLoadEngines)
        {
            foreach (IO2AssessmentLoad o2AssessmentLoad in o2AssessmentLoadEngines)
                if (o2AssessmentLoad.canLoadFile(pathToFileToLoad))
                    return o2AssessmentLoad;
            PublicDI.log.error("in ozasmtUtils.getO2AssessmentLoadEngine, could not find a load engine for the file :{0}", pathToFileToLoad);
            return null;
        }

        public static void fixExternalSourceSourceMappingProblem(O2Finding o2Finding)
        {
            try
            {
                // fix the external_source callback generated finding problem since the source should be the callback back methods and not the <external_source>(...) rule
                if (o2Finding.Source.IndexOf("<external_source>") > -1)
                {
                    o2Finding.getSource().traceType = TraceType.Root_Call;
                    o2Finding.o2Traces[0].childTraces[1].traceType = TraceType.Source;
                }

            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in getO2Finding, while trying to fix the external_source callback generated finding problem");
            }
        }

        public static List<IO2Trace> getListWithAllTraces(O2Finding o2Finding)
        {
            var allTraces = new List<IO2Trace>();
            try
            {
                getListWithAllTraces(o2Finding.o2Traces, allTraces);
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in getListWithAllTraces: {0}", ex.Message);
            }            
            return allTraces;
        }

        private static void getListWithAllTraces(IEnumerable<IO2Trace> o2TracesToMap, ICollection<IO2Trace> listWithAllTraces)
        {
            foreach(var o2Trace in o2TracesToMap)
            {
                listWithAllTraces.Add(o2Trace);
                getListWithAllTraces(o2Trace.childTraces, listWithAllTraces);
            }
        }
    }
}
