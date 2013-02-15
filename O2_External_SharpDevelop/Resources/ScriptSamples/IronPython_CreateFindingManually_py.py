print "**************************************************************"
print "This is an Iron Phyton O2 Script"
print "that will manually create a finding (with a trace) and saved it"
print "**************************************************************"

import clr
import sys

sys.path.append(r"E:\O2\_Bin_(O2_Binaries)")
clr.AddReference("O2_Kernel")
clr.AddReference("O2_DotNetWrappers")
clr.AddReference("O2_ImportExport_OunceLabs")

from O2.Kernel import *
from O2.DotNetWrappers.O2Findings import *
from O2.ImportExport.OunceLabs.Ozasmt_OunceV6 import *


o2a = O2Assessment()
finding = O2Finding()
finding.vulnName = 'hello world'
finding.vulnType = 'really bad'

root_trace = O2Trace('root')
source = O2Trace('source')
source.traceType = Interfaces.Ozasmt.TraceType.Source
root_trace.childTraces.Add(source)
sink_trace = O2Trace('sink')
sink_trace.traceType = Interfaces.Ozasmt.TraceType.Known_Sink
root_trace.childTraces.Add(sink_trace)

finding.o2Traces.Add(root_trace)

print finding

o2a.o2Findings.Add(finding)
tempfile = o2a.save(O2AssessmentSave_OunceV6())
print "saved assessment was saved to " + tempfile

