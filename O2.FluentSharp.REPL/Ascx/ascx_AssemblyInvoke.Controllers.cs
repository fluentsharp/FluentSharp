// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using O2.DotNetWrappers.DotNet;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Windows;
using O2.Interfaces.Messages;
using O2.Kernel.CodeUtils;


namespace O2.External.SharpDevelop.Ascx
{
    public partial class ascx_AssemblyInvoke
    {
        public bool autoExecuteLastMethod { get; set; }        
        private bool executeMethodOnDoubleClick { get; set; }
        private bool onlyShowStaticMethods { get; set; }
        public bool showAssemblyExecutionPanel { get; set; } // defaulted to true
        public bool isDebuggerAvailable { get; set; }         // defaulted to true

        public MethodInfo selectedMethod { get; set; }
        public Assembly compiledAssembly { get; set; }
        public Thread threadUsedToPopulateTargetList;
        public List<Thread> currentExecutionThreads = new List<Thread>();
        public bool runOnLoad = true;

        public void onLoad()
        {
            if (false == DesignMode && runOnLoad)
            {
                btDebugMethod.Enabled = isDebuggerAvailable;
                btExecuteAssemblyUnderDebug.Enabled = isDebuggerAvailable;
                runOnLoad = false;
                showAssemblyExecutionPanel = true;
                isDebuggerAvailable = false;
            }
        }


        void ascx_AssemblyInvoke_onMessages(IO2Message o2Message)
        {
            if (InvokeRequired)
                Invoke(new EventHandler(delegate { ascx_AssemblyInvoke_onMessages(o2Message); }));
            else
            {
                if (o2Message is IM_DotNetAssemblyAvailable)
                    loadAssembly(((IM_DotNetAssemblyAvailable)o2Message).pathToAssembly);
            }
        }

        public void setControlsEnableState(bool enableState)
        {
            tvSourceCode_CompiledAssembly.Enabled = enableState;            
            dgvSourceCode_SelectedMethodParameters.Enabled = enableState;
        }

        public string SelectNodeAssembly
        {
            get { return ""; }
        }

        public string SelectNodeModule
        {
            get { return ""; }
        }

        public string SelectNodeType
        {
            get { return ""; }
        }

        public string SelectNodeMethod
        {
            get { return ""; }
        }

        public void onTreeViewAfterSelect(TreeNode selectedNode)
        {
            if (selectedNode.Tag != null)
            {
                selectedMethod = null;
                btDebugMethod.Enabled = false;
                btExecuteMethodWithoutDebugger.Enabled = false; 
                switch (selectedNode.Tag.GetType().Name)
                {
                    case "RuntimeMethodInfo":
                        //Callbacks.raiseRegistedCallbacks(onSelectedMethod, new[] {e.Node.Tag});
                        selectedMethod = (MethodInfo) selectedNode.Tag;
                        raiseO2MessageWithMethodInfo(selectedMethod);
                        lbLastMethodExecuted.Text = selectedMethod.Name;
                        DI.reflection.loadMethodInfoParametersInDataGridView(selectedMethod, dgvSourceCode_SelectedMethodParameters);
//                        btSourceCode_executeStaticMethod.Enabled =
//                            DI.reflection.doesMethodOnlyHasSupportedParameters((MethodInfo)selectedNode.Tag);
                        dgvSourceCode_SelectedMethodParameters.Enabled = true;
                        btDebugMethod.Enabled = O2Messages.isDebuggerAvailable();
                        btExecuteMethodWithoutDebugger.Enabled = true; 
                        break;
                    case "RuntimeType":
                        foreach (TreeNode childNode in selectedNode.Nodes)
                            if (childNode.Tag != null && childNode.Tag.GetType().Name == "RuntimeMethodInfo")
                                raiseO2MessageWithMethodInfo((MethodInfo)childNode.Tag);
                        //var type = (Type) e.Node.Tag;
                        //foreach (var methodInfo in DI.reflection.getMethods(type))
                        //    raiseO2MessageWithMethodInfo(methodInfo);
                        break;
                    case "Assembly":
                        var assembly = (Assembly)selectedNode.Tag;
                        foreach (var methodInfo in DI.reflection.getMethods(assembly))
                            raiseO2MessageWithMethodInfo(methodInfo);
                        break;
                }
            }
        }

        public void raiseO2MessageWithMethodInfo(MethodInfo methodInfo)
        {
            O2Messages.selectedTypeOrMethod(methodInfo);
        }

        public void executeInstanceMethod(MethodInfo methodToExecute)
        {
            DI.log.info("Executing Instance Method:{0}", methodToExecute.Name);
            var liveObject = DI.reflection.createObjectUsingDefaultConstructor(methodToExecute.DeclaringType);
            executeMethodOnSeparateThread(methodToExecute, liveObject);
        }

        public void executeStaticMethod(MethodInfo methodToExecute)
        {            
            DI.log.info("Executing Static Method:{0}", methodToExecute.Name);
            executeMethodOnSeparateThread(methodToExecute, null);
/*                raiseO2MessageWithMethodInfo(methodToExecute);
                currentExecutionThreads.Add(executeMethod(methodToExecute,
                              dgvSourceCode_SelectedMethodParameters, tbSourceCode_InvocationResult,
                              dgvSourceCode_InvocationResult, this));
                reloadExecutionThreadsList();            */
        }

         public void executeMethodOnSeparateThread(MethodInfo methodToExecute, object liveInstanceOfObject)
         {
             raiseO2MessageWithMethodInfo(methodToExecute);
             currentExecutionThreads.Add(executeMethod(methodToExecute,
                           dgvSourceCode_SelectedMethodParameters, tbSourceCode_InvocationResult,
                           dgvSourceCode_InvocationResult, liveInstanceOfObject));
             executionThreads_Refresh();            
         }



        public void loadAssembly(object assemblyToLoad)
        {
            if (assemblyToLoad is Assembly)
                loadAssembly((Assembly)assemblyToLoad, false);
            else
                loadAssembly(DI.reflection.getAssembly(assemblyToLoad.ToString()), false);
        }

        public void loadAssembly(string assemblyToLoad)
        {
            loadAssembly(DI.reflection.getAssembly(assemblyToLoad), false);
        }

        public void loadAssembly(Assembly aCompiledAssembly)
        {
            loadAssembly(aCompiledAssembly, false);
        }

        public void loadAssembly(Assembly _compiledAssembly, bool _autoExecuteLastMethod)
        {
            compiledAssembly = _compiledAssembly;
            showDetailsOfLoadedAssembly(_autoExecuteLastMethod);
        }

        public void showDetailsOfLoadedAssembly(bool _autoExecuteLastMethod)
        {
            if (compiledAssembly != null)
                this.invokeOnThread(
                    () =>
                    //if (ExtensionMethods.okThread((Control) this, delegate { showDetailsOfLoadedAssembly(_autoExecuteLastMethod); }))
                        {
                            killThreadUsedToPopulateTargetList();
                            DI.log.info("Loading assembly:{0}", compiledAssembly.Location);
                            autoExecuteLastMethod = _autoExecuteLastMethod;
                            threadUsedToPopulateTargetList =
                                CompileEngine_WinForms.loadAssesmblyDataIntoTreeView(compiledAssembly,
                                                                                  tvSourceCode_CompiledAssembly,
                                                                                  lbLastMethodExecuted,
                                                                                  onlyShowStaticMethods);
                            checkAutoExecutionOfLastInvokedMethod();
                            tvSourceCode_CompiledAssembly.Enabled = true;
                            btExecuteAssemblyUnderDebug.Enabled = compiledAssembly.EntryPoint != null;
                            btExecuteMethodWithoutDebugger.Enabled = false;
                            btDebugMethod.Enabled = false;
                        });
        }

        public void killThreadUsedToPopulateTargetList()
        {
            if (threadUsedToPopulateTargetList != null && threadUsedToPopulateTargetList.IsAlive)
            {
                threadUsedToPopulateTargetList.Abort();
                //Processes.Sleep(500);
            }

        }

        public List<String> getTypes()
        {
            var loadedClasses = new List<String>();
            if (compiledAssembly != null)
            {
                var types = DI.reflection.getTypes(compiledAssembly );
                foreach(var type in types)
                    loadedClasses.Add(type.FullName);
            }            
            return loadedClasses;
        }

        public List<String> getMethods()
        {
            var loadedMethods = new List<String>();
            if (compiledAssembly != null)
            {
                var methods = DI.reflection.getMethods(compiledAssembly);
                foreach (var method in methods)
                    loadedMethods.Add(method.ToString());
            }
            return loadedMethods;
        }

        public string getAssemblyLocation()
        {
            return  (compiledAssembly != null) ? compiledAssembly.Location : "";
        }

        private void executeMethod()
        {
            if (tvSourceCode_CompiledAssembly.SelectedNode != null &&
                tvSourceCode_CompiledAssembly.SelectedNode.Tag != null &&
                tvSourceCode_CompiledAssembly.SelectedNode.Tag.GetType().Name == "RuntimeMethodInfo")
            {
                var methodToExecute = (MethodInfo) tvSourceCode_CompiledAssembly.SelectedNode.Tag;
                if (methodToExecute.IsStatic)
                    executeStaticMethod(methodToExecute);
                else
                    executeInstanceMethod(methodToExecute);
            }
        }

        private void executeAndDebugAssembly()
        {

            O2Messages.raiseO2MDbgDebugProcessRequest(compiledAssembly.Location);

            //Object[] aoMethodParameters =
            //    DI.reflection.getParameterObjectsFromDataGridColumn(
            //        dgvSourceCode_SelectedMethodParameters, "Value");
            //executeStaticMethod();

        }

        /// <summary>
        /// Executes method
        /// </summary>
        /// <param name="mMethodToExecute"></param>
        /// <param name="dgvSourceCode_SelectedMethodParameters"></param>
        /// <param name="tbSourceCode_InvocationResult"></param>
        /// <param name="dgvSourceCode_InvocationResult"></param>
        /// <param name="oLiveInstanceOfObject"></param>
        public static Thread executeMethod(MethodInfo mMethodToExecute,
                                         DataGridView dgvSourceCode_SelectedMethodParameters,
                                         TextBox tbSourceCode_InvocationResult,
                                         DataGridView dgvSourceCode_InvocationResult, Object oLiveInstanceOfObject)
        {
            return O2Thread.mtaThread(mMethodToExecute.Name, () =>
                                   {
                                       Object[] aoMethodParameters =
                                           DI.reflection.getParameterObjectsFromDataGridColumn(
                                               dgvSourceCode_SelectedMethodParameters, "Value");
                                       DI.reflection.executeMethodAndOutputResultInTextBoxOrDataGridView(
                                           mMethodToExecute, aoMethodParameters,
                                           oLiveInstanceOfObject,
                                           tbSourceCode_InvocationResult,
                                           dgvSourceCode_InvocationResult);
                                   });
        }
        private void createStandAloneExeAndDebugMethod(string loadDllsFrom)
        {
            O2Thread.mtaThread(() =>
                                   {
                                       if (selectedMethod != null)
                                           O2Messages.raiseO2MDbgDebugMethodInfoRequest(selectedMethod, loadDllsFrom);
                                   });
        }

        /*private string createStandAloneExe(MethodInfo methodToExecute)
        {
            var standAloneExe = StandAloneExe.createMainPointingToMethodInfo(methodToExecute);

            return standAloneExe;
        }*/

        public void executionThreads_Refresh()
        {
            this.invokeOnThread(
                () =>
            {
                tvExecutionThreads.Nodes.Clear();
                foreach(var executionThread in currentExecutionThreads)                
                    if (executionThread.IsAlive)
                        addThreadToTreeView(tvExecutionThreads, executionThread);                        

                currentExecutionThreads.Clear();
                foreach (TreeNode executionThread in tvExecutionThreads.Nodes)
                    currentExecutionThreads.Add((Thread)executionThread.Tag);
                DI.log.info("There are currently {0} active threads", currentExecutionThreads.Count);
            });
        }

        public static void addThreadToTreeView(TreeView treeView, ProcessThread thread)
        {
            var nodeText = thread.Id.str();
            O2Forms.newTreeNode(treeView.Nodes, nodeText, 0, thread, true /*addDummyNode */);                      
        }

        public static void addThreadToTreeView(TreeView treeView, Thread thread)
        {
            var nodeText = (String.IsNullOrEmpty(thread.Name)) ? "[no name]" : thread.Name;
            O2Forms.newTreeNode(treeView.Nodes, nodeText, 0, thread, true /*addDummyNode */);                      
        }

        public void stopExecutionThread(Thread executionThread)
        {
            if (executionThread.IsAlive)
            {
                executionThread.Abort();
                executionThreads_Refresh();
                return;
            }
            currentExecutionThreads.Remove(executionThread);
            executionThreads_Refresh();            
        }

        public void setShowAssemblyExecutionPanel(bool value)
        {
            showAssemblyExecutionPanel = value;
            scTopLevel.Panel2Collapsed = value;
        }

        public void setIsDebuggerAvailable(bool value)
        {
            isDebuggerAvailable = value;
        }

        public void showThreadDetailsOnTreeNode(TreeNode treeNode, ProcessThread processThread)
        {
            treeNode.Nodes.Clear();
            foreach(var property in DI.reflection.getProperties(processThread))
            {
                var propertyValue = DI.reflection.getProperty(property,processThread);
                var nodeText = string.Format("{0} = {1}", property.Name, propertyValue ?? "[null]");
                O2Forms.newTreeNode(treeNode.Nodes, nodeText);
            }
        }

        public void showThreadDetailsOnTreeNode(TreeNode treeNode, Thread thread)
        {
            treeNode.Nodes.Clear();

            foreach (var property in DI.reflection.getProperties(thread))
            {
                var propertyValue = DI.reflection.getProperty(property, thread);
                var nodeText = string.Format("{0} = {1}", property.Name, propertyValue ?? "[null]");
                O2Forms.newTreeNode(treeNode.Nodes, nodeText);
            }

            /*O2Forms.newTreeNode(treeNode.Nodes, thread.Name);
            O2Forms.newTreeNode(treeNode.Nodes, "Is Alive:" + thread.IsAlive);
            O2Forms.newTreeNode(treeNode.Nodes, "Is IsBackground:" + thread.IsBackground);
            O2Forms.newTreeNode(treeNode.Nodes, "Is IsThreadPoolThread:" + thread.IsThreadPoolThread);
            O2Forms.newTreeNode(treeNode.Nodes, "ApartmentState:" + thread.GetApartmentState());
            O2Forms.newTreeNode(treeNode.Nodes, "Priority:" + thread.Priority);
            O2Forms.newTreeNode(treeNode.Nodes, "ThreadState:" + thread.ThreadState);           */

        }

        public void showO2Threads()
        {
            //this.invokeOnThread(
           //     () =>
           //         {
            tvExecutionThreads.Nodes.Clear();
            foreach(var processThread in Processes.getCurrentProcessThreads())
                addThreadToTreeView(tvExecutionThreads, processThread);
           //         });
        }
    }
}
