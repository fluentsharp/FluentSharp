// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Collections.Generic;
using FluentSharp.CoreLib.Interfaces;

namespace FluentSharp.CoreLib.API
{
    public class KScanTarget : IScanTarget
    {
        private string _target;
        private string _workDirectory;

        public KScanTarget()
        {
            missingDependencies = new List<string>();
            useFileNameOnWorkDirecory = true;
            WorkDirectory = PublicDI.config.O2TempDir;
        }

        #region IScanTarget Members

        public virtual string Target
        {
            get { return _target; }
            set
            {
                _target = value;
                if (useFileNameOnWorkDirecory)
                    WorkDirectory = WorkDirectory.pathCombine(Target.fileName_WithoutExtension());
            }
        }

        public string WorkDirectory
        {
            get { return _workDirectory; }
            set
            {
                _workDirectory = value;
                O2Kernel_Files.checkIfDirectoryExistsAndCreateIfNot(WorkDirectory);
            }
        }

        public string ApplicationFile { get; set; }

        public bool useFileNameOnWorkDirecory { get; set; }

        public List<string> missingDependencies { get; set; }

        public override string ToString()
        {
            return Target  ?? "";  // to deal with '...Attempted to read or write protected memory..' issue ;
        }

        #endregion
    }
}
