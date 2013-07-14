// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtGlue
    {
        /*public static List<O2Finding> glueSinksToSources(O2Assessment o2AssessmentWithSinks, O2Assessment o2AssessmentWithSources)
        {
            var webLayerSources = getO2StringIndexes(webLayer, TraceType.Source);

            foreach (var clickButtonFinding in clickButton.o2Findings)
            {
                results.Add(clickButtonFinding);
                var sink = OzasmtUtils.getKnownSink(clickButtonFinding.o2Trace);
                PublicDI.log.debug(sink.signature);
                if (webLayerSources.ContainsKey(sink.signature))
                    foreach (var webLayerSource in webLayerSources[sink.signature])
                    {
                        results.Add(OzasmtGlue.createCopyAndGlueTraceAndSink(clickButtonFinding,
                                                                             OzasmtUtils.getSource(
                                                                                 webLayerSource.o2Trace)));
                        /*var o2NewFinding = OzasmtCopy.createCopy(clickButtonFinding);
                        var newFindingSink = OzasmtUtils.getKnownSink(o2NewFinding.o2Trace);
                        newFindingSink.traceType = TraceType.Type_4;
                        var sourceToGlue = OzasmtCopy.createCopy(OzasmtUtils.getSource(webLayerSource.o2Trace));
                        sourceToGlue.traceType = TraceType.Type_0;
                        newFindingSink.childTraces.Add(sourceToGlue);
                        results.Add(o2NewFinding);*/
        /*         }
        }*/

        public static IO2Finding createCopyAndGlueTraceSinkWithSource(IO2Finding o2TemplateFinding,
                                                                      IO2Trace o2TraceWithSource)
        {
            IO2Trace sourceToGlue = OzasmtCopy.createCopy(OzasmtUtils.getSource(o2TraceWithSource));
            return createCopyAndGlueTraceSinkWithTrace(o2TemplateFinding, sourceToGlue);
        }

        public static IO2Finding createCopyAndGlueTraceSinkWithTrace(IO2Finding o2TemplateFinding, IO2Trace o2TracesToGlue)
        {
            return createCopyAndGlueTraceSinkWithTrace(o2TemplateFinding, new List<IO2Trace>().add(o2TracesToGlue));
        }

        public static IO2Finding createCopyAndGlueTraceSinkWithTrace(IO2Finding o2TemplateFinding,
                                                                     List<IO2Trace> o2TracesToGlue)
        {
            IO2Finding o2NewFinding = OzasmtCopy.createCopy(o2TemplateFinding);
            //IO2Trace newFindingSink = OzasmtUtils.getKnownSink(o2NewFinding.o2Traces);
            IO2Trace newFindingSink = OzasmtUtils.getSink(o2NewFinding.o2Traces);
            newFindingSink.traceType = TraceType.Root_Call;
            foreach (O2Trace o2TraceToGlue in o2TracesToGlue)
            {
                o2TraceToGlue.traceType = TraceType.Root_Call;
                newFindingSink.childTraces.Add(o2TraceToGlue);
            }
            return o2NewFinding;
        }

     
        public static List<IO2Finding> glueOnTraceNames(List<IO2Finding> o2FindingsWithSinks,
                                                        List<IO2Finding> o2FindingsWithSources, string gluedFindingVulnType)
        {
            var results = new List<IO2Finding>();
            //            foreach(var o2FindingInSinks in o2AssessmentOfOzasmtWithSinks.o2Findings)
            //                foreach(var o2FindinInSources in o2AssessmentOfOzasmtWithSources.o2Findings)
            foreach (IO2Finding o2FindingInSinks in o2FindingsWithSinks)
                foreach (IO2Finding o2FindinInSources in o2FindingsWithSources)
                    if (o2FindingInSinks.vulnName == o2FindinInSources.vulnName)
                    {
                        IO2Finding o2gluedFinding = createCopyAndGlueTraceSinkWithTrace(o2FindingInSinks,
                                                                                        o2FindinInSources.o2Traces);
                        o2gluedFinding.vulnType = gluedFindingVulnType;
                        results.Add(o2gluedFinding);
                    }
            return results;
        }


        public static List<IO2Finding> glueOnSinkSourceNameMatch(List<IO2Finding> o2FindingsWithSinks,
                                                        List<IO2Finding> o2FindingsWithSources, string gluedFindingVulnType)
        {
            var results = new List<IO2Finding>();
            //            foreach(var o2FindingInSinks in o2AssessmentOfOzasmtWithSinks.o2Findings)
            //                foreach(var o2FindinInSources in o2AssessmentOfOzasmtWithSources.o2Findings)
            foreach (O2Finding o2FindingInSinks in o2FindingsWithSinks)
                foreach (O2Finding o2FindinInSources in o2FindingsWithSources)
                    if (o2FindingInSinks.Sink == o2FindinInSources.Source)
                    {
                        IO2Finding o2gluedFinding = createCopyAndGlueTraceSinkWithTrace(o2FindingInSinks,
                                                                                        o2FindinInSources.o2Traces);
                        o2gluedFinding.vulnType = gluedFindingVulnType;
                        results.Add(o2gluedFinding);
                    }
            return results;
        }

        public static List<IO2Finding> glueOnSinkToAproximateSourceNameMatch(List<IO2Finding> o2FindingsWithSinks,
                                                        List<IO2Finding> o2FindingsWithSources, string gluedFindingVulnType)
        {
            var results = new List<IO2Finding>();
            //            foreach(var o2FindingInSinks in o2AssessmentOfOzasmtWithSinks.o2Findings)
            //                foreach(var o2FindinInSources in o2AssessmentOfOzasmtWithSources.o2Findings)
            foreach (O2Finding o2FindingInSinks in o2FindingsWithSinks)
            {
                if (o2FindingInSinks.Sink.IndexOf("purchaseForm") > -1)
                {
                }
                foreach (O2Finding o2FindinInSources in o2FindingsWithSources)
                {

                    if (o2FindingInSinks.Sink == o2FindinInSources.Source ||
                        o2FindinInSources.Source.IndexOf(o2FindingInSinks.Sink + ".") > -1)
                    {
                        IO2Finding o2gluedFinding = createCopyAndGlueTraceSinkWithTrace(o2FindingInSinks,
                                                                                        o2FindinInSources.o2Traces);
                        o2gluedFinding.vulnType = gluedFindingVulnType;
                        results.Add(o2gluedFinding);
                    }
                }

                    
            }
            return results;
        }

        public static void deleteO2Trace(List<IO2Trace> o2Traces, IO2Trace o2TraceToDelete)
        {
            foreach (O2Trace o2Trace in o2Traces)
            {
                if (o2Trace == o2TraceToDelete)
                {
                    o2Traces.Remove(o2TraceToDelete);
                    return;
                }
                if (deleteO2Trace(o2Trace, o2TraceToDelete))
                    return;
            }
            /*if (o2Finding.o2Traces == o2TraceToDelete)
                o2Finding.o2Trace = null;
            else*/
        }

        public static bool deleteO2Trace(IO2Trace o2RootTrace, IO2Trace o2TraceToDelete)
        {
            foreach (O2Trace o2ChildTrace in o2RootTrace.childTraces)
                if (o2ChildTrace == o2TraceToDelete)
                {
                    o2RootTrace.childTraces.Remove(o2ChildTrace);
                    return true;
                }
                else
                {
                    bool foundItemToDelete = deleteO2Trace(o2ChildTrace, o2TraceToDelete);
                    if (foundItemToDelete)
                        return true;
                }
            return false;
        }

        public static List<IO2Finding> glueTraceSinkWithSources(IO2AssessmentLoad o2AssessmentLoad,String ozasmtWithSinks, String ozasmtWithSoures)
        {
            var results = new List<IO2Finding>();
            Dictionary<string, List<IO2Trace>> o2TracesWithSources = OzasmtUtils.getDictionaryWithO2AllSubTraces(o2AssessmentLoad,ozasmtWithSoures);
            foreach (IO2Finding o2FindingWithSink in new O2Assessment(o2AssessmentLoad, ozasmtWithSinks).o2Findings)
            {
                string sinkToFind = OzasmtUtils.getKnownSink(o2FindingWithSink.o2Traces).signature;
                if (o2TracesWithSources.ContainsKey(sinkToFind))
                {
                    foreach (IO2Trace o2TraceWithSourcre in o2TracesWithSources[sinkToFind])
                        results.Add(createCopyAndGlueTraceSinkWithSource(o2FindingWithSink, o2TraceWithSourcre));
                }
            }
            return results;
        }
         //"Glue WebInspect -> Ounce Finding (Sql Injection)";
        public static List<IO2Finding> glueOnTraceNames(IO2AssessmentLoad o2AssessmentLoad, String ozasmtWithSinks, String ozasmtWithSoures, string gluedFindingVulnType)
        {
            var o2AssessmentOfOzasmtWithSinks = new O2Assessment(o2AssessmentLoad,ozasmtWithSinks);
            var o2AssessmentOfOzasmtWithSources = new O2Assessment (o2AssessmentLoad,ozasmtWithSoures);
            return glueOnTraceNames(o2AssessmentOfOzasmtWithSinks.o2Findings, o2AssessmentOfOzasmtWithSources.o2Findings, gluedFindingVulnType);
        }
    }
}
