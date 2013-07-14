// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using System.IO;
using System.Drawing;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms.Controls
{
    public partial class O2ObjectModel
    {
        public List<Assembly> assembliesLoaded = new List<Assembly>();
        public List<MethodInfo> methodsLoaded = new List<MethodInfo>();
        public Dictionary<string, MethodInfo> methodsLoaded_bySignature = new Dictionary<string, MethodInfo>();
        public List<FilteredSignature> filteredSignatures = new List<FilteredSignature>();

        public bool runOnLoad = true;

        public void onLoad()
        {
            if (DesignMode == false && runOnLoad)
            {                
                filteredFunctionsViewer.setNamespaceDepth(0);                
                runOnLoad = false;
                refreshViews();                
            }
        }        
        private static List<Assembly> getDefaultLoadedAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().toList()
                                        .with_Valid_Location()
                                        .where((assembly) => assembly.name().contains("FluentSharp"));
            foreach(var assembly in assemblies)
                "Loading data for assembly: {0}".info(assembly.Location);
            return assemblies;
            /*
            var assembliesPaths = CompileEngine.getListOfO2AssembliesInExecutionDir();                       
            var assemblies = new List<Assembly>();
            foreach (var assemblyPath in assembliesPaths)
            {
             //   PublicDI.log.error("Loading file: {0}", assemblyPath);
                var assembly = PublicDI.reflection.getAssembly(assemblyPath);
                //PublicDI.reflection.getAssembliesInCurrentAppDomain()
                if (assembly != null)
                    assemblies.Add(assembly);
                else
                {
                    PublicDI.log.debug("could not load assembly: {0}", assemblyPath);
                }

            }           
            return assemblies;*/
        }

        public void refreshViews()
        {
            PublicDI.log.info("Refreshing Views and remaping assemblies");
            assembliesLoaded = getAssembliesToLoad(tvExtraAssembliesToLoad);
            O2Forms.loadTreeViewWith_AssembliesInCurrentAppDomain(tvExtraAssembliesToLoad, assembliesLoaded, true);
            refreshLoadedAssembliesView();            
            refreshO2ObjectModelView(cbHideCSharpGeneratedMethods.Checked);
            showFilteredMethods(null);
        }

        private static List<Assembly> getAssembliesToLoad(TreeView tvTreeView)
        {
            return (List<Assembly>)tvTreeView.invokeOnThread(
                () =>
                {
                    if (tvTreeView.Nodes.Count ==0)
                        return getDefaultLoadedAssemblies();
                    
                    var assembliesToLoad = new List<Assembly>();
                    foreach(TreeNode treeNode in tvTreeView.Nodes)
                        if (treeNode.Checked && treeNode.Tag !=null && treeNode.Tag is Assembly)
                            assembliesToLoad.Add((Assembly)treeNode.Tag);
                    return assembliesToLoad;                
                });
        }

        public void refreshLoadedAssembliesView()
        {
            tvAssembliesLoaded.invokeOnThread(
                () =>
                {                    
                    foreach (TreeNode treeNode in tvExtraAssembliesToLoad.Nodes)
                        if (treeNode.Checked && treeNode.Tag is Assembly)
                        {
                            var assembly = (Assembly)treeNode.Tag;
                            if (false == assembliesLoaded.Contains(assembly))
                                assembliesLoaded.Add(assembly);
                            else
                            {
                            }
                        }

                        //        O2Forms.newTreeNode(tvAssembliesLoaded.Nodes, treeNode.Text, 0, treeNode.Tag);

                    tvAssembliesLoaded.Nodes.Clear();
                    // add from assembliesLoaded list
                    foreach (var assembly in assembliesLoaded)
                        O2Forms.newTreeNode(tvAssembliesLoaded.Nodes, assembly.GetName().Name, 0, assembly);

                    // add the ones checked in the tvAssembliesLoaded
                    //foreach (TreeNode treeNode in tvExtraAssembliesToLoad.Nodes)
                    //    if (treeNode.Checked)
                    //        O2Forms.newTreeNode(tvAssembliesLoaded.Nodes, treeNode.Text, 0, treeNode.Tag);

                    // colorcode the ones with pdb symbols
                    foreach (TreeNode treeNode in tvAssembliesLoaded.Nodes)
                    {
                        var loadedAssembly = (Assembly)treeNode.Tag;
                        if (loadedAssembly != null && loadedAssembly.Location != "")
                        {
                            var pdbFile = loadedAssembly.Location.Replace(Path.GetExtension(loadedAssembly.Location), ".pdb");
                            if (File.Exists(pdbFile))
                                treeNode.ForeColor = Color.DarkGreen;
                        }
                    }

                    tvAssembliesLoaded.Sort();
                    tvExtraAssembliesToLoad.Sort();
                });
        }        

        public void refreshO2ObjectModelView(bool hideCSharpGeneratedMethods)
        {
            try
            {
                PublicDI.log.debug("from loaded assemblies, calulating list of (reflection) method information");
                methodsLoaded = new List<MethodInfo>();
                foreach (var assembly in assembliesLoaded)
                    methodsLoaded.add(PublicDI.reflection.getMethods(assembly));
                PublicDI.log.debug("Convering method information into filtered signatures objects");
                mapMethodsToFilteredSignatures(methodsLoaded, ref filteredSignatures, ref methodsLoaded_bySignature, hideCSharpGeneratedMethods);
                PublicDI.log.info("there are {0} O2 assemblies", assembliesLoaded.Count);
                PublicDI.log.info("there are {0} methods", methodsLoaded.Count);
                var methodsSignature = getMethodSignatures(filteredSignatures, hideCSharpGeneratedMethods);

                PublicDI.log.info("there are {0} methods sigantures", methodsSignature.Count);

                functionsViewer.showSignatures(methodsSignature);
            }
            catch (Exception ex)
            {
                "[O2 Object Model] refreshO2ObjectModelView: {0}".error(ex.Message);
            }

            //var functionsViewer = (ascx_FunctionsViewer)O2AscxGUI.openAscx(typeof(ascx_FunctionsViewer), O2DockState.Float, "O2 Object Model");
            //functionsViewer.showSignatures(methodsSignature);
            //return true;
        }


        public static void mapMethodsToFilteredSignatures(List<MethodInfo> methodsToMap, ref List<FilteredSignature> filteredSignatures, ref Dictionary<string, MethodInfo> methods_bySignature, bool hideCSharpGeneratedMethods)
        {
            filteredSignatures = new List<FilteredSignature>();
            methods_bySignature = new Dictionary<string, MethodInfo>();
            foreach (var method in methodsToMap)
            {
                try
                {
                    var filteredSignature = new FilteredSignature(method);
                    if (hideCSharpGeneratedMethods == false || (filteredSignature.sSignature.IndexOf("<>") == -1 &&
                                                                 false == filteredSignature.sFunctionName.StartsWith("b__")))
                    {
                        // create methodsLoaded_bySignature                       
                        if (methods_bySignature.ContainsKey(filteredSignature.sSignature))
                        {
                            if (method.type().Assembly.GetName().Name.contains("FluentSharp"))
                                PublicDI.log.error("in mapMethodsToFilteredSignatures, repeated signature: {0}", filteredSignature.sSignature);
                        }
                        else
                        {
                            filteredSignatures.Add(filteredSignature);
                            methods_bySignature.Add(filteredSignature.sSignature, method);
                        }
                    }
                    else
                    {
                        //PublicDI.log.info("Skipping: {0}", method.Name);
                    }
                }
                catch (Exception ex)
                {
                    "[O2 Object Model] mapMethodsToFilteredSignatures: {0}".error(ex.Message);
                }
            }
        }

        public static List<String> getMethodSignatures(List<FilteredSignature> filteredSignatures, bool hideCSharpGeneratedMethods)
        {
            var methodsSignature = new List<String>();
            foreach (var filteredSignature in filteredSignatures)
                    methodsSignature.Add(filteredSignature.sSignature);                
            return methodsSignature;
        }

        public void showFilteredMethods(KeyEventArgs e)
        {
            if (e == null || e.KeyData == Keys.Enter)
                O2Thread.mtaThread(
                    () =>
                    showFilteredMethods(cbPerformRegExSearch.Checked, tbFilterBy_MethodType.Text, tbFilterBy_MethodName.Text,
                                        tbFilterBy_ParameterType.Text,
                                        tbFilterBy_ReturnType.Text, filteredSignatures, filteredFunctionsViewer)
                    );
        }

        // DC: need to find a more optimized way to do this (this is 4am code :)  )
        public static void showFilteredMethods(bool useRegExSearch,string methodType, string methodName, string parameterType, string returnType, List<FilteredSignature> filteredSignatures, ascx_FunctionsViewer functionsViewer)
        {
            var typesFilter = new List<FilteredSignature>();
            var methodsFilter = new List<FilteredSignature>();
            var parametersFilter = new List<FilteredSignature>();
            var returnTypeFilter = new List<FilteredSignature>();

            // methodType
            if (methodType == "")
                typesFilter = filteredSignatures;
            else
                foreach (var filteredSignature in filteredSignatures)
                {
                    if (useRegExSearch)
                    {
                        if (RegEx.findStringInString(filteredSignature.sFunctionClass, methodType))
                            typesFilter.Add(filteredSignature);
                    }
                    else
                        if (filteredSignature.sFunctionClass.Contains(methodType))
                            typesFilter.Add(filteredSignature);
                }

            // methodName
            if (methodName == "")
                methodsFilter = typesFilter;
            else
                foreach (var filteredSignature in typesFilter)
                {
                    if (useRegExSearch)
                    {
                        if (RegEx.findStringInString(filteredSignature.sFunctionName, methodName))
                            methodsFilter.Add(filteredSignature);
                    }
                    else
                        if (filteredSignature.sFunctionName.Contains(methodName))
                            methodsFilter.Add(filteredSignature);
                }

            // parameterType
            if (parameterType == "")
                parametersFilter = methodsFilter;
            else
                foreach (var filteredSignature in methodsFilter)
                {
                    if (useRegExSearch)
                    {
                        if (RegEx.findStringInString(filteredSignature.sParameters, parameterType))
                            parametersFilter.Add(filteredSignature);
                    }
                    else
                        if (filteredSignature.sParameters.Contains(parameterType))
                            parametersFilter.Add(filteredSignature);
                }
            // returnType
            if (returnType == "")
                returnTypeFilter = parametersFilter;
            else
                foreach (var filteredSignature in parametersFilter)
                {
                    if (useRegExSearch)
                    {
                        if (RegEx.findStringInString(filteredSignature.sReturnClass, returnType))
                            returnTypeFilter.Add(filteredSignature);
                    }
                    else
                        if (filteredSignature.sReturnClass.Contains(returnType))
                            returnTypeFilter.Add(filteredSignature);
                }
            // get list of signatures to show using the last filter result (returnTypeFilter)
            var signaturesToShow = new List<string>();
            foreach (var filteredSignature in returnTypeFilter)
                signaturesToShow.Add(filteredSignature.sSignature);
            functionsViewer.showSignatures(signaturesToShow);
        }

        public void showSelectedMethodDetails(FilteredSignature filteredSignature)
        {
            if (filteredSignature != null)
            {
                tbMethodDetails_Name.invokeOnThread(
                    () =>
                    {
                        tbMethodDetails_Name.Text = filteredSignature.sFunctionName;
                        tbMethodDetails_OriginalSignature.Text = filteredSignature.sOriginalSignature;
                        tbMethodDetails_Parameters.Text = filteredSignature.sParameters;
                        tbMethodDetails_ReturnType.Text = filteredSignature.sReturnClass;
                        tbMethodDetails_Signature.Text = filteredSignature.sSignature;
                        tbMethodDetails_Type.Text = filteredSignature.sFunctionClass;
                    });
            }
        }

        public void handleDrop(DragEventArgs e)
        {
            var fileOrFolder = Dnd.tryToGetFileOrDirectoryFromDroppedObject(e);
            if (false == string.IsNullOrEmpty(fileOrFolder))
            {
                if (File.Exists(fileOrFolder))
                {
                    addAssemblyFile(fileOrFolder,true);
                }
                if (Directory.Exists(fileOrFolder))
                {
                    var assembliesToAdd =Files.getFilesFromDir_returnFullPath(fileOrFolder, new List<string>().add("*.dll").add("*.exe"), true);
                    if (assembliesToAdd.Count > 0)
                    {
                        foreach (var file in assembliesToAdd)
                            addAssemblyFile(file, false);
                        refreshViews();
                    }
                }
            }
        }

        public void addAssemblyFile(string file, bool refreshGUI)
        {
            var assembly = PublicDI.reflection.getAssembly(file);
            if (assembly != null)
            {
                assembliesLoaded.Add(assembly);
                if (refreshGUI)
                    refreshViews();
            }
        }

        private void handleOnItemDrag(object oObject)
        {
            List<FilteredSignature> filteredSignatures = new List<FilteredSignature>();
            if (oObject is List<FilteredSignature>)
                filteredSignatures = (List<FilteredSignature>)oObject;
            else if (oObject is FilteredSignature)
                filteredSignatures.Add((FilteredSignature)oObject);
            if (filteredSignatures.Count == 0)
                PublicDI.log.error("in handleOnItemDrag there were no FilteredSignature to process");
            else
            {
                var methodsToDrag = new List<MethodInfo>();
                foreach (var filteredSignature in filteredSignatures)
                    if (methodsLoaded_bySignature.ContainsKey(filteredSignature.sSignature))
                        methodsToDrag.Add(methodsLoaded_bySignature[filteredSignature.sSignature]);
                    else
                        PublicDI.log.error("in handleOnItemDrag could not find loaded MethodInfo for :{0}", filteredSignature);
                
                // invoke the DragAndDrop on the functionsViewer thread
                functionsViewer.invokeOnThread(()=> DoDragDrop(methodsToDrag, DragDropEffects.Copy));
            }
            
        }        
    }
}
