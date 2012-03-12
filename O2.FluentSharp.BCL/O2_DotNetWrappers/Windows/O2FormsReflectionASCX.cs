// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using O2.DotNetWrappers.ExtensionMethods;
using O2.DotNetWrappers.Filters;
using O2.Interfaces.O2Core;
using O2.Kernel.InterfacesBaseImpl;

namespace O2.DotNetWrappers.Windows
{
    public class O2FormsReflectionASCX : KReflection, IReflectionASCX
    {
        // todo: duplicate defintions (they are also at O2.External.O2Mono.ViewHelpers.CecilASCX)
        private const int iconIndex_method = 4;
        private const int iconIndex_module = 1;
        private const int iconIndex_namespace = 2;
        private const int iconIndex_type = 3;

        public List<String> lsSupportParameterTypes =
            new List<string>(new[] {"System.Boolean", "System.String", "System.Int32", "System.UInt32"});

        #region IReflectionASCX Members

        public List<TreeNode> populateTreeNodeWithObjectChilds(TreeNode node, object tag, bool populateFirstChildNodes)
        {
            var newNodes = new List<TreeNode>();
            try
            {
                // DI.log.debug("populating :{0} - {1}", node, tag);                
                if (node != null && tag != null)
                    switch (tag.GetType().Name)
                    {
                            // Reflection objects
                        case "Assembly":
                            foreach (Module module in DI.reflection.getModules((Assembly) tag))
                                newNodes.Add(O2Forms.newTreeNode(node, module.Name, iconIndex_module, module));
                            break;
                        case "Module":
                            // when showing module's contents (i.e. Types) we need to break them by namespace
                            Dictionary<string, List<Type>> reflectionTypesMappedToNamespaces =
                                DI.reflection.getDictionaryWithTypesMappedToNamespaces((Module) tag);
                            foreach (string typeNamespace in reflectionTypesMappedToNamespaces.Keys)
                            {
                                TreeNode namespaceNode = O2Forms.newTreeNode(node, typeNamespace, iconIndex_namespace,
                                                                             reflectionTypesMappedToNamespaces[
                                                                                 typeNamespace]);
                                foreach (Type type in reflectionTypesMappedToNamespaces[typeNamespace])
                                    O2Forms.newTreeNode(namespaceNode, type.Name, iconIndex_type, type);
                                newNodes.Add(namespaceNode);
                            }
                            break;
                        case "RuntimeType":
                            // add nested types in Type
                            foreach (Type type in DI.reflection.getTypes((Type) tag))
                                newNodes.Add(O2Forms.newTreeNode(node, type.Name, iconIndex_type, type));
                            // add methods in Type
                            foreach (MethodInfo method in DI.reflection.getMethods((Type) tag))
                                newNodes.Add(O2Forms.newTreeNode(node, new FilteredSignature(method).getReflectorView(),
                                                                 iconIndex_method, method));
                            break;
                        case "RuntimeMethodInfo":
                            //     newNodes.Add(O2Forms.newTreeNode(node, node.Text, iconIndex_method, tag));
                            break;
                        case "List`1":
                            foreach (object item in (IEnumerable) tag)
                            {
                                switch (item.GetType().Name)
                                {
                                    case "RuntimeType":
                                        newNodes.Add(O2Forms.newTreeNode(node, ((Type) item).Name, iconIndex_type, item));
                                        break;
                                }
                            }
                            break;
                    }
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "in populateTreeNodeWithObjectChilds");
            }
            return newNodes;
        }

        public void loadObjectDataIntoTreeNodesCollection(Object oLiveObjectToLoad,
                                                          TreeNodeCollection tnbTargetTreeNodesCollection)
        {
            Type tTypeOfLiveObject = oLiveObjectToLoad.GetType();
            foreach (
                PropertyInfo piProperty in
                    tTypeOfLiveObject.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static |
                                                    BindingFlags.DeclaredOnly))
            {
                if (piProperty.GetType().IsArray)
                {
                }
                try
                {
                    Object oPropertyObject = piProperty.GetValue(oLiveObjectToLoad,
                                                                 BindingFlags.GetProperty | BindingFlags.NonPublic |
                                                                 BindingFlags.Instance | BindingFlags.Static |
                                                                 BindingFlags.DeclaredOnly, null, new Object[] {}, null);
                    TreeNode tnProperty = O2Forms.newTreeNode(piProperty.Name, "", 0, oPropertyObject);
                    tnbTargetTreeNodesCollection.Add(tnProperty);
                }
                catch (Exception ex)
                {
                    DI.log.error("Reflecting property {0}", ex.Message);
                }
            }

            foreach (
                FieldInfo fiFieldInfo in
                    tTypeOfLiveObject.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static |
                                                BindingFlags.DeclaredOnly))
            {
                if (fiFieldInfo.GetType().IsArray)
                {
                }
                Object oFieldObject = fiFieldInfo.GetValue(oLiveObjectToLoad);
                TreeNode tnField = O2Forms.newTreeNode(fiFieldInfo.Name, "", 1, oFieldObject);
                tnbTargetTreeNodesCollection.Add(tnField);
            }
        }

        public void loadMethodInfoParametersInDataGridView(MethodInfo mMethod, DataGridView dgvTargetDataGridView)
        {
            dgvTargetDataGridView.Columns.Clear();
            O2Forms.newDataGridViewTextBoxColumn(dgvTargetDataGridView, "Name", "Name", true, null);
            O2Forms.newDataGridViewTextBoxColumn(dgvTargetDataGridView, "Type", "Type", true, null);
            dgvTargetDataGridView.Columns.Add("Value", "Value");
            // ReSharper disable PossibleNullReferenceException
            dgvTargetDataGridView.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // ReSharper restore PossibleNullReferenceException
            ParameterInfo[] apiParameters = mMethod.GetParameters();
            foreach (ParameterInfo piParameter in apiParameters)
            {
                Int32 iNewRowId = dgvTargetDataGridView.Rows.Add(dgvTargetDataGridView.RowTemplate.Clone());
                dgvTargetDataGridView.Rows[iNewRowId].Cells["Name"].Value = piParameter.Name;
                dgvTargetDataGridView.Rows[iNewRowId].Cells["Type"].Value = piParameter.ParameterType.FullName;
                if (false == lsSupportParameterTypes.Contains(piParameter.ParameterType.FullName))
                {
                    DI.log.error("The parameter type {1} is not supported",
                                 mMethod.ReflectedType + "." + mMethod.Name, piParameter.ParameterType.FullName);
                    dgvTargetDataGridView.Rows[iNewRowId].Cells["Name"].Style.ForeColor = Color.Red;
                    dgvTargetDataGridView.Rows[iNewRowId].Cells["Type"].Style.ForeColor = Color.Red;
                }
                switch (piParameter.ParameterType.ToString())
                {
                    case "System.Boolean":
                        dgvTargetDataGridView.Rows[iNewRowId].Cells["Value"] =
                            O2Forms.newDataGridViewComboBoxCell_forBooleanValues(true, false);
                        break;
                    case "System.Int32":
                    case "System.UInt32":
                        dgvTargetDataGridView.Rows[iNewRowId].Cells["Value"].Value = 0;
                        dgvTargetDataGridView.Rows[iNewRowId].Cells["Value"].Tag = piParameter.ParameterType;
                        break;
                }
            }

            if (dgvTargetDataGridView.SelectedRows.Count == 1)
                dgvTargetDataGridView.SelectedRows[0].Cells["Value"].Selected = true;

            //   DI.log.debug(mMethod.Name);            
        }

        public void loadTypeDataIntoTreeView(Type tTypeToLoad, TreeView tvTargetTreeView,
                                             bool bViewAllMethods_IncludingOnesWithNotSupportedParams,
                                             bool bShowArguments, bool bShowReturnParameter, string sFilter)
        {
            loadTypeDataIntoTreeView(tTypeToLoad, tvTargetTreeView, bViewAllMethods_IncludingOnesWithNotSupportedParams,
                                     bShowArguments, bShowReturnParameter, sFilter, true /*bClearTreeView*/);
        }

        public void loadTypeDataIntoTreeView(Type tTypeToLoad, TreeView tvTargetTreeView,
                                             bool bViewAllMethods_IncludingOnesWithNotSupportedParams,
                                             bool bShowArguments, bool bShowReturnParameter, string sFilter,
                                             bool bClearTreeView)
        {
            if (sFilter == "")
                DI.log.debug("Loading '{0}' object data into TreeView", tTypeToLoad.Name);
            else
            {
                DI.log.debug("Loading '{0}' object data into TreeView using filter {1}", tTypeToLoad.Name, sFilter);
                sFilter = sFilter.ToUpper();
            }
            if (bClearTreeView)
                tvTargetTreeView.Nodes.Clear();
            Object[] oCustomAttributes = tTypeToLoad.GetCustomAttributes(false);
            foreach (Object oCustomAttribute in oCustomAttributes)
            {
                //      if (oCustomAttribute.GetType() == typeof (ExposedAndInvokable))
                {
                    TreeNode tnType = O2Forms.newTreeNode(tTypeToLoad.Name, oCustomAttribute.GetType().Name, 1,
                                                          oCustomAttribute);
                    foreach (
                        MethodInfo mMethod in
                            tTypeToLoad.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                                   BindingFlags.DeclaredOnly))
                    {
                        if (sFilter == "" || (sFilter != "" && mMethod.Name.ToUpper().IndexOf(sFilter) > -1))
                            if (bViewAllMethods_IncludingOnesWithNotSupportedParams)
                            {
                                int iNewNode = addMethodInfoToTreeNode(tnType, mMethod, bShowArguments,
                                                                       bShowReturnParameter);
                                //TreeNode tnMethod = O2Forms.newTreeNode(mMethod.Name, "", 2, mMethod);
                                if (false == doesMethodOnlyHasSupportedParameters(mMethod))
                                    tnType.Nodes[iNewNode].ForeColor = Color.DarkRed;
                                //tnMethod.ForeColor = Color.Red;
                                //tnType.Nodes.Add(tnMethod);
                            }
                            else if (doesMethodOnlyHasSupportedParameters(mMethod))
                            {
                                addMethodInfoToTreeNode(tnType, mMethod, bShowArguments, bShowReturnParameter);
                            }
                    }
                    if (tnType.Nodes.Count > 0)
                        tvTargetTreeView.Nodes.Add(tnType);
                    //tTypeInfo.Name + "    -    " + oCustomAttribute.GetType().Name);
                }
            }
            if (tvTargetTreeView.Nodes.Count > 0)
                tvTargetTreeView.SelectedNode = tvTargetTreeView.Nodes[0];
        }

        public int addMethodInfoToTreeNode(TreeNode tnTargetTreeNode, MethodInfo mMethod, bool bShowArguments,
                                           bool bShowReturnParameter)
        {
            String sNodeText = mMethod.Name;
            if (bShowArguments)
            {
                String sShowArguments = "";
                foreach (ParameterInfo pParameter in mMethod.GetParameters())
                    sShowArguments += pParameter.ParameterType.Name + " ; ";
                if (sShowArguments != "")
                    sShowArguments = sShowArguments.Substring(0, sShowArguments.Length - 3);
                sNodeText += "(" + sShowArguments + ")";
            }
            if (bShowReturnParameter)
                sNodeText += " : " + mMethod.ReturnParameter.ParameterType.Name;
            return tnTargetTreeNode.Nodes.Add(O2Forms.newTreeNode(sNodeText, "", 2, mMethod));
        }

        public Object[] getParameterObjectsFromDataGridColumn(DataGridView dgvDataGridViewWithData,
                                                              String sColumnWithParameters)
        {
            return (object[])dgvDataGridViewWithData.invokeOnThread(
                () =>
                    {
                        var aoParams = new Object[dgvDataGridViewWithData.Rows.Count];
                        try
                        {
                            for (int i = 0; i < dgvDataGridViewWithData.Rows.Count; i++)
                            {
                                Object oParameterTag = dgvDataGridViewWithData.Rows[i].Cells[sColumnWithParameters].Tag;
                                if (null != oParameterTag && ("System.RuntimeType" == oParameterTag.GetType().FullName))
                                {
                                    switch (((Type) oParameterTag).FullName)
                                    {
                                        case "System.Boolean":

                                            aoParams[i] = "true" ==
                                                          (String) dgvDataGridViewWithData.Rows[i].Cells["Value"].Value;
                                            break;
                                        case "System.Int32":
                                            aoParams[i] =
                                                Int32.Parse(
                                                    dgvDataGridViewWithData.Rows[i].Cells["Value"].Value.ToString());
                                            break;
                                        case "System.UInt32":
                                            aoParams[i] =
                                                UInt32.Parse(
                                                    dgvDataGridViewWithData.Rows[i].Cells["Value"].Value.ToString());
                                            break;
                                    }
                                }
                                else
                                    aoParams[i] = dgvDataGridViewWithData.Rows[i].Cells["Value"].Value;
                            }
                        }
                        catch (Exception ex)
                        {
                            DI.log.ex(ex, "in getParameterObjectsFromDataGridColumn: {0}");
                        }
                        return aoParams;
                    });
        }

        public void executeMethodAndOutputResultInTextBoxOrDataGridView(MethodInfo mMethod, Object[] aoParams,
                                                                        Object oLiveInstanceOfObject,
                                                                        TextBox tbTextBox_Results,
                                                                        DataGridView dgvDataGridView_Results)
        {
            try
            {
            
            object oInvocationResult = mMethod.Invoke(oLiveInstanceOfObject, aoParams);
            dgvDataGridView_Results.invokeOnThread(
                () =>
                    {
                        try
                        {
                            //Type tMethodTye = mMethod.ReflectedType;

                            //Reflection.invokeMethod_InstanceStaticPublicNonPublic();
                            
                            tbTextBox_Results.Visible = false;
                            if (dgvDataGridView_Results != null)
                                dgvDataGridView_Results.Visible = false;
                            if (null == oInvocationResult)
                            {
                                tbTextBox_Results.Text = String.Format("Info: Method {0} executed sucessfull",
                                                                       mMethod.Name);
                                tbTextBox_Results.Visible = true;
                            }
                            else
                            {
                                switch (oInvocationResult.GetType().FullName)
                                {
                                    case "System.Data.DataTable":
                                        if (dgvDataGridView_Results != null)
                                        {
                                            dgvDataGridView_Results.DataSource = oInvocationResult;
                                            dgvDataGridView_Results.Visible = true;
                                        }

                                        break;
                                    case
                                        "System.Collections.Generic.List`1[[System.String, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]"
                                        :
                                        var lResults = (List<String>) oInvocationResult;
                                        if (lResults.Count == 0)
                                            String.Format("Info: the List<String> returned had no items");
                                        else
                                        {
                                            tbTextBox_Results.Text = "";
                                            foreach (String sLine in lResults)
                                                tbTextBox_Results.Text += sLine;
                                        }
                                        tbTextBox_Results.Visible = true;

                                        break;
                                    default:
                                        tbTextBox_Results.Text = oInvocationResult.ToString();
                                        tbTextBox_Results.Visible = true;
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DI.log.ex(ex,
                                      "Invocaton Error in (GUI thread)executeMethodAndOutputResultInTextBoxOrDataGridView for method: " +
                                      mMethod.Name);
                            tbTextBox_Results.Text = ex.Message;
                            if (ex.InnerException != null)
                            {
                                DI.log.ex(ex, "   InnerException + " + ex.InnerException.Message);
                                tbTextBox_Results.Text += Environment.NewLine + "    InnerException:" +
                                                          ex.InnerException.Message;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                DI.log.ex(ex, "Invocaton Error in (MTA thread)executeMethodAndOutputResultInTextBoxOrDataGridView for method: " +
                                                      mMethod.Name);
            }
        }

        public bool doesMethodOnlyHasSupportedParameters(MethodInfo mMethod)
        {
            ParameterInfo[] apiParameters = mMethod.GetParameters();
            foreach (ParameterInfo piParameter in apiParameters)
            {
                if (false == lsSupportParameterTypes.Contains(piParameter.ParameterType.FullName))
                    return false;
            }
            return true;
        }

        #endregion
    }
}
