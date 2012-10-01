using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extensibility;
using EnvDTE;

namespace O2.FluentSharp.VisualStudio
{
    public class VsConnect_AddIn_Test : IDTExtensibility2, IDTCommandTarget
    {

        public void OnAddInsUpdate(ref Array custom)
        {
            throw new NotImplementedException();
        }

        public void OnBeginShutdown(ref Array custom)
        {
            throw new NotImplementedException();
        }

        public void OnConnection(object Application, ext_ConnectMode ConnectMode, object AddInInst, ref Array custom)
        {
            throw new NotImplementedException();
        }

        public void OnDisconnection(ext_DisconnectMode RemoveMode, ref Array custom)
        {
            throw new NotImplementedException();
        }

        public void OnStartupComplete(ref Array custom)
        {
            throw new NotImplementedException();
        }

        public void Exec(string CmdName, vsCommandExecOption ExecuteOption, ref object VariantIn, ref object VariantOut, ref bool Handled)
        {
            throw new NotImplementedException();
        }

        public void QueryStatus(string CmdName, vsCommandStatusTextWanted NeededText, ref vsCommandStatus StatusOption, ref object CommandText)
        {
            throw new NotImplementedException();
        }
    }
}
