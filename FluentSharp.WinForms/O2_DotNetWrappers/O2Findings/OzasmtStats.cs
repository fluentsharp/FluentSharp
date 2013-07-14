// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FluentSharp.CoreLib.API;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.O2Findings
{
    public class OzasmtStats
    {
        public static void populateTreeNodeWithAssessmentStats(TreeNode treeNode, O2Assessment o2Assessment,
                                                               int imageIndex)
        {
            O2Forms.newTreeNode(treeNode, "# Findings:" + getNumberOf_Findings(o2Assessment), imageIndex, null);
            O2Forms.newTreeNode(treeNode, "# Smart Traces:" + getNumberOf_Findings_WithSmartTrace(o2Assessment),
                                imageIndex, null);

            /*   O2Forms.newTreeNode(treeNode, "File:" + Path.GetFileName(o2Assessment.lastOzasmtImportFile), imageIndex,
                                   null);
               O2Forms.newTreeNode(treeNode, "Size:" + o2Assessment.lastOzasmtImportFileSize, imageIndex, null);
               string importTimeString =
                   ((o2Assessment.lastOzasmtImportTimeSpan.Hours > 0)
                        ? o2Assessment.lastOzasmtImportTimeSpan.Hours + "h : "
                        : "") +
                   ((o2Assessment.lastOzasmtImportTimeSpan.Minutes > 0)
                        ? o2Assessment.lastOzasmtImportTimeSpan.Minutes + "m : "
                        : "") +
                   ((o2Assessment.lastOzasmtImportTimeSpan.Seconds > 0)
                        ? o2Assessment.lastOzasmtImportTimeSpan.Seconds + "s : "
                        : "") +
                   ((o2Assessment.lastOzasmtImportTimeSpan.Milliseconds > 0)
                        ? o2Assessment.lastOzasmtImportTimeSpan.Milliseconds + "ms"
                        : "");
               O2Forms.newTreeNode(treeNode, "Imported in:   " + importTimeString, imageIndex, null);*/
        }

        public static int getNumberOf_Findings(O2Assessment o2Assessment)
        {
            return o2Assessment.o2Findings.Count;
        }

        public static int getNumberOf_Findings_WithSmartTrace(O2Assessment o2Assessment)
        {
            /*
             * query = new NLinqQuery(
    @"  from m in methods
          where !m.IsStatic
          orderby m.Name
          group m by m.Name into g
          select new { MethodName = g.Key, Overloads = g.Count() }");
*/

            try
            {
                //var O2Timer = new O2Timer("Calculating Findings with Native Linq").start();
                return
                    (from O2Finding finding in o2Assessment.o2Findings where finding.o2Traces.Count > 0 select finding).
                        Count();
                //O2Timer.stop();
                //if (findingsCountNative != null)// && findingsCountNlinqQuery is List<object>)
                //{
                //PublicDI.log.debug("{0} == {1}", findingsCountNative, findingsCountNlinqQuery.Count);
                //  return (findingsCountNlinqQuery).Count;

                /*var timer2 = new O2Timer("Calculating Findings with NLinqQuery").start();
                var query =
                    new NLinqQuery(
                        //"from O2Finding finding in o2Findings where finding.o2Trace != null select finding).Count()");
                        "from O2Finding finding in o2Findings select finding");
                var linq = new LinqToMemory(query);
                linq.AddSource("o2Findings", o2Assessment.o2Findings);
                var findingsCountNlinqQuery = (List<object>)linq.Evaluate();
                timer2.stop();
                if (findingsCountNlinqQuery != null)// && findingsCountNlinqQuery is List<object>)
                {
                    PublicDI.log.debug("{0} == {1}", findingsCountNative, findingsCountNlinqQuery.Count);
                    return (findingsCountNlinqQuery).Count;
                }
                */
                //foreach (object o in linq.Enumerate())
                //{
                //    PublicDI.log.info(o.ToString());
                //}
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "error execution NLinqQuery");
            }


            /*        var O2Timer = new O2Timer("Calculating Findings with Smart Trace").start();
            var findingsCount =  (from O2Finding finding in o2Assessment.o2Findings where finding.o2Trace != null select finding).Count();
            O2Timer.stop();*/
            //return findingsCount;
            return 0;
        }
    }
}
