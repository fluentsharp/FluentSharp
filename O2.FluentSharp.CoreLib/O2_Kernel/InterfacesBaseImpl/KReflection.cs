// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;



using System.Threading;

namespace FluentSharp.CoreLib.API
{
    public class KReflection //: IReflection
    {
        public bool verbose { get; set; }

        public const BindingFlags BindingFlagsAll = BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.Static;

        public const BindingFlags BindingFlagsAllDeclared = BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.Static |
                                                             BindingFlags.DeclaredOnly;

        #region get        
        
        public PropertyInfo getPropertyInfo(String sPropertyToGet, Type targetType)
        {
            return getPropertyInfo(sPropertyToGet, targetType, false);
        }

        public PropertyInfo getPropertyInfo(String sPropertyToGet, Type targetType, bool bVerbose)
        {
            try
            {
                /*var targetType = (oTargetObject is Type)
                                     ? ((Type) oTargetObject)
                                     : oTargetObject.GetType();*/
                return targetType.GetProperty(sPropertyToGet,
                                                      BindingFlags.GetProperty |
                                                      BindingFlags.NonPublic | BindingFlags.Public |
                                                      BindingFlags.Instance | BindingFlags.Static);            
            }
            catch (Exception ex)
            {
                if (bVerbose)
                    PublicDI.log.error("in reflection.getPropertyInfo: {0} ", ex.Message);
                return null;
            }
        }


        public Object getProperty(String typeName, String propertyName, object liveObject)
        {
            Object result = null;
            var type = getType(typeName);
            if (type != null)
            {
                var propertyInfo = PublicDI.reflection.getPropertyInfo(propertyName, type);
                if (propertyInfo != null)
                    result = getProperty(propertyInfo, propertyName);                    
            }
            return result;
        }

        public Object getProperty(PropertyInfo propertyToGet)
        {
            return getProperty(propertyToGet, null);
        }

        public Object getProperty(PropertyInfo propertyInfo, object liveObject)
        {
            try
            {	            
	            return propertyInfo.GetValue(liveObject,
                                                   BindingFlags.GetProperty |
                                                   BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.Static, null,
                                                   new object[] {},null);                
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in reflection.getProperty: {0} ", ex.Message);
                return null;
            }
        }    

        public Object getProperty(String sPropertyToGet, Object oTargetObject)
        {
            return getProperty(sPropertyToGet, oTargetObject, false);
        }

        public Object getProperty(String sPropertyToGet, Object oTargetObject, bool bVerbose)
        {
            try
            {
                if (oTargetObject == null)
                    return null;
                var targetType = (oTargetObject is Type)
                                     ? ((Type)oTargetObject)
                                     : oTargetObject.GetType();
                var propertyInfo = getPropertyInfo(sPropertyToGet, targetType, bVerbose);
                if (null == propertyInfo)
                {
                    if (bVerbose)
                        PublicDI.log.error(
                            "in getProperty, desired property ({0}) doesn't exist in the target object's class {1} ",
                            sPropertyToGet, oTargetObject.GetType().FullName);
                }
                else
                    return targetType.InvokeMember(sPropertyToGet,
                                                   BindingFlags.GetProperty |
                                                   BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.Static, null,
                                                   oTargetObject, new object[] { });
            }
            catch (Exception ex)
            {
                if (bVerbose)
                    PublicDI.log.error("in reflection.getInstance: {0} ", ex.Message);
            }
            return null;
        }

        public Object getInstance(Type tTypeToCreate)
        {
            try
            {
                return tTypeToCreate.InvokeMember("",
                                                  BindingFlags.Public | BindingFlags.Instance |
                                                  BindingFlags.CreateInstance, null, null, new object[] {});
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in reflection.getInstance: {0} ", ex.Message);
                return null;
            }
        }
        

        public List<Module> getModules(Assembly assembly)
        {            
            return (assembly!=null) ? new List<Module>(assembly.GetModules()) : new List<Module>();            
        }

        public List<Type> getTypes(Assembly assembly)
        {
            var types = new List<Type>();
            try
            {
                foreach (Module module in getModules(assembly))
                    types.AddRange(module.GetTypes());
                return types;
            }
            catch (ReflectionTypeLoadException ex)
            {
                PublicDI.log.error("in Reflection.getTypes for assembly {0}got error: {1}", assembly.GetName(),
                             ex.Message);
                foreach (Exception loaderExpection in ex.LoaderExceptions)
                    PublicDI.log.error("   LoaderException {0}", loaderExpection.Message);
            }
            return types;
        }

        public List<Type> getTypes(Module module)
        {
            return new List<Type>(module.GetTypes());
        }

        public List<Type> getTypes(Type type)
        {
            return new List<Type>(type.GetNestedTypes());
        }

        public Type getType(string assemblyName, string typeToFind)
        {
            Assembly assembly = getAssembly(assemblyName);
            if (assembly != null)
                return getType(assembly, typeToFind);
            return null;
        }

        public Type getType(Assembly assembly, string typeToFind)
        {
            if (assembly != null && false == string.IsNullOrEmpty(typeToFind))
            {
                if (assembly.GetType(typeToFind) != null)
                    return assembly.GetType(typeToFind);
                foreach (Type type in getTypes(assembly))
                    if (type.FullName == typeToFind || type.Name == typeToFind)
                        return type;
            }
            return null;
        }

        public Type getType(Type type, string typeToFind)
        {
            if (type != null && false == string.IsNullOrEmpty(typeToFind))            
                foreach (Type nestedType in type.GetNestedTypes())
                    if (nestedType.FullName == typeToFind || nestedType.Name == typeToFind)
                        return type;            
            return null;
        }

        public Type getType(string typeToFind)
        {
            foreach (var assembly in getAssembliesInCurrentAppDomain())
            {
                var type = getType(assembly, typeToFind);
                if (type != null)
                    return type;
            }
            if (PublicDI.log != null)
                PublicDI.log.info("In KReflection.getType, could not find the requested type in all appDomain assemblies: {0}", typeToFind);
            return null;
        }

        // this is an example of using dynamic reflection so that we don't have to add reference to O2_Kernel.dll
        //tip (to document): the code below can also be represented like this
        //var typeName = "Microsoft.VisualBasic".assembly().type("Information").invoke("TypeName");
        // or
        //var typeName = "Microsoft.VisualBasic".type("Information").invoke("TypeName");
        public string getComObjectTypeName(object _object)
        {                        
            var assembly = PublicDI.reflection.getAssembly("Microsoft.VisualBasic");
            if (assembly != null)
            {
                var type = PublicDI.reflection.getType(assembly, "Information");
                if (type != null)
                {
                    var method = PublicDI.reflection.getMethod(type, "TypeName");
                    if (method != null)
                    {
                        var typeName = PublicDI.reflection.invoke(method, new [] {_object});
                        if (typeName is string)
                            return typeName.ToString();
                    }
                }
            }
            return null;
        }  
        public List<MethodInfo> getMethods(string pathToassemblyToProcess)
        {
            return getMethods(getAssembly(pathToassemblyToProcess));
        }

        public List<MethodInfo> getMethods(Assembly assembly)
        {
            var methods = new List<MethodInfo>();
            foreach (Type type in getTypes(assembly))
                methods.AddRange(type.GetMethods(BindingFlagsAllDeclared));
            return methods;
        }

        public List<MethodInfo> getMethods(Type type, Attribute attribute)
        {
            var methods = getMethods(type);
            var results = new List<MethodInfo>();
            try
            {
                foreach (var method in methods)
                {
                    var attributes = getAttributes(method);
                    if (attributes.Contains(attribute))
                        results.Add(method);
                }
            }
            catch (Exception ex)
            {
                ex.log("in KReflection.getMethods(Type type, Attribute attribute)");
            }
            return results;
        }

        public Attribute getAttribute(MethodInfo method, Type type)
        {
            var attributes = getAttributes(method);
            foreach (var attribute in attributes)
                if (attribute.GetType() == type)
                    return attribute;
            return null;
        }

        public List<Attribute> getAttributes(MethodInfo methodInfo)
        {
            var attributes = new List<Attribute>();
            foreach(var attribute in methodInfo.GetCustomAttributes(true))
                if (attribute is Attribute)
                    attributes.Add((Attribute)attribute);
            return attributes;
        }
        
        public List<Attribute> getAttributes(Type targetType)
        {
            var attributes = new List<Attribute>();
            foreach(var attribute in targetType.GetCustomAttributes(true))
                if (attribute is Attribute)
                    attributes.Add((Attribute)attribute);
            return attributes;
        }
        
        public List<Attribute> getAttributes(Assembly assembly)
        {
            var attributes = new List<Attribute>();
            foreach(var attribute in assembly.GetCustomAttributes(true))
                if (attribute is Attribute)
                    attributes.Add((Attribute)attribute);
            return attributes;
        }


        public List<MethodInfo> getMethods(Type type, BindingFlags bindingFlags)
        {
            return  (type== null)?  new List<MethodInfo>() : new List<MethodInfo>(type.GetMethods(bindingFlags));
        }

        public List<MethodInfo> getMethods(Type type)
        {
            return (type == null) ? new List<MethodInfo>() : new List<MethodInfo>(type.GetMethods(BindingFlagsAllDeclared));
        }

        public MethodInfo getMethod(string pathToAssembly, string methodName)
        {
            return getMethod(pathToAssembly, methodName, new object[0]);
        }

        public MethodInfo getMethod(string pathToAssembly, string methodName, object[] methodParameters)
        {
            try
            {
                foreach (Type type in getTypes(getAssembly(pathToAssembly)))
                {
                    MethodInfo methodInfo = getMethod(type, methodName, (methodParameters ?? new object[0]));
                    if (methodInfo != null)
                        return methodInfo;
                }
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "imn getMethod");
            }
            
            return null;
        }        
        /// <summary>
        /// This will return the first method in control Type that matches the methodName
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public MethodInfo getMethod(Type controlType, string methodName)
        {            
            if (controlType !=null)
            {
                foreach (var method in PublicDI.reflection.getMethods(controlType))
                    if (method.Name == methodName)
                        return method;
            }
            return null;
        }

        public MethodInfo getMethod(Type type, string methodName, object[] methodParameters)
        {
            return type.GetMethod(methodName,
                                  BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                  BindingFlags.Static, null, GetObjectsTypes(methodParameters).ToArray(), null);
        }

        public List<string> getParametersName(MethodInfo method)
        {
            var parametersType = new List<string>();
            if (method!= null)
                foreach(var parameter in method.GetParameters())
                    parametersType.Add(parameter.Name);
            return parametersType;
        }

        public List<Type> getParametersType(MethodInfo method)
        {
            var parametersType = new List<Type>();
            if (method != null)
                foreach (var parameter in method.GetParameters())
                    parametersType.Add(parameter.ParameterType);
            return parametersType;
        }

        public List<ParameterInfo> getParameters(MethodInfo method)
        {
            var parametersType = new List<ParameterInfo>();
            if (method != null)                
                parametersType.AddRange(method.GetParameters());
            return parametersType;
        }

        public List<Type> GetObjectsTypes(object[] objectsToGetType)
        {
            var types = new List<Type>();
            if (objectsToGetType != null)
                foreach (object objectToGetType in objectsToGetType)
                    if (objectToGetType!=null)
                        types.Add(objectToGetType.GetType());
            return types;
        }

        public List<MemberInfo> getMembers(Assembly assembly)
        {
            var members = new List<MemberInfo>();
            foreach (Type type in getTypes(assembly))
                members.AddRange(type.GetMembers());
            return members;
        }

        public List<MemberInfo> getMembers(Type type)
        {
            return new List<MemberInfo>(type.GetMembers());
        }

        public List<FieldInfo> getFields(Type type)
        {
            try
            {
                return new List<FieldInfo>(type.GetFields(BindingFlagsAllDeclared));
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.getFields");
                return new List<FieldInfo>();
            }
            
        }

        public FieldInfo getField(Type type, string fieldName)
    	{
            var fields = getFields(type);
            foreach (var field in fields)
                if (field.Name.Contains(fieldName))
                    return field;
            return null;
    	}

        public Object getFieldValue(FieldInfo fieldToGet, Object oTargetObject)
        {
            return fieldToGet.GetValue(oTargetObject);
        }

        public Object getFieldValue(String sFieldToGet, Object oTargetObject)
        {
            if (oTargetObject != null)
            {
                FieldInfo fiFieldInfo = oTargetObject.GetType().GetField(sFieldToGet,
                                                                         BindingFlags.GetField |
                                                                         BindingFlags.NonPublic |
                                                                         BindingFlags.Public | BindingFlags.Instance |
                                                                         BindingFlags.Static);
                if (fiFieldInfo != null)
                    return fiFieldInfo.GetValue(oTargetObject);
            }

            return null;
        }

        public List<PropertyInfo> getProperties(Object targetObject)
        {
            return (targetObject != null) ? getProperties(targetObject.GetType()) : new List<PropertyInfo>();
        }

        public List<PropertyInfo> getProperties(Type type)
        {
            var properties = new List<PropertyInfo>();
            if (type!=null)
                foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    //if (property.IsSpecialName == false)
                        properties.Add(property);
                }
            return properties;
            //return (type!=null) ? new List<PropertyInfo>(
            //    ) :  new List<PropertyInfo>();
        }

        public Dictionary<string, List<Type>> getDictionaryWithTypesMappedToNamespaces(Module module)
        {
            var typesMappedToNamespaces = new Dictionary<string, List<Type>>();
            foreach (Type type in getTypes(module))
            {
                string typeNamespace = type.Namespace;
                if (typeNamespace == null)
                {
                    if (!typesMappedToNamespaces.ContainsKey(""))
                        typesMappedToNamespaces.Add("", new List<Type>());
                    typesMappedToNamespaces[""].Add(type);
                }
                else
                {
                    if (!typesMappedToNamespaces.ContainsKey(typeNamespace))
                        typesMappedToNamespaces.Add(typeNamespace, new List<Type>());
                    typesMappedToNamespaces[typeNamespace].Add(type);
                }
            }
            return typesMappedToNamespaces;
        }

        public Assembly getAssembly(string pathToAssemblyToLoad)
        {
            return loadAssembly(pathToAssemblyToLoad);
        }

        public Assembly getCurrentAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        public List<Assembly> getAssembliesInCurrentAppDomain()
        {
            return new List<Assembly>(AppDomain.CurrentDomain.GetAssemblies());
        }

        public object[] getRealObjects(object[] objectsToConvert)
        {
            var realObjects = new List<object>();
            if (objectsToConvert!=null)
                foreach (object objectToConvert in objectsToConvert)
                    if (objectToConvert is O2Object)
                        realObjects.Add(((O2Object) objectToConvert).Obj);
                    else
                        realObjects.Add(objectToConvert);
            return realObjects.ToArray();
        }

        public List<MethodInfo> getTypesWithAttribute(Assembly assembly, Type attributeType)
        {
            var methods = new List<MethodInfo>();
            if (assembly != null)
            { }
            return methods;
        }

        public List<MethodInfo> getMethodsWithAttribute(Assembly assembly, Type attributeType)
        {
            var methods = new List<MethodInfo>();
            
            return methods;
        }

        public List<MethodInfo> getMethodsWithAttribute(Type targetType, Type attributeType)
        {
            var methods = new List<MethodInfo>();

            return methods;
        }

      /*  public Object getLiveObject(object liveObject, string typeToFind)
        {
            return null;
        }*/


        #endregion

        #region set        

        public bool setField(FieldInfo fieldToSet, object fieldValue)
        {
            return setField(fieldToSet, null, fieldValue);
        }

        public bool setField(FieldInfo fieldToSet, object liveObject, object fieldValue)
        {            
            try
            {                
                fieldToSet.SetValue(liveObject, fieldValue,
                                            BindingFlags.SetField | BindingFlags.Public |
                                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, 
                                            // ReSharper disable AssignNullToNotNullAttribute
                                            default(CultureInfo));
                                            // ReSharper restore AssignNullToNotNullAttribute
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.setField");
            }
            return false;
        }

        public bool setField(String fieldToSet, Type targetType, object fieldValue)
        {
            try
            {
                if (targetType != null)
                    targetType.InvokeMember(fieldToSet,
                                            BindingFlags.SetField | BindingFlags.Public |
                                            BindingFlags.NonPublic | BindingFlags.Static, null,
                                            targetType, new[] { fieldValue });
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.setField");
            }
            return false;
        }

        public bool setField(String fieldToSet, object targetObject, object fieldValue)
        {
            try
            {
                if (targetObject != null)
                    targetObject.GetType().InvokeMember(fieldToSet,
                                                         BindingFlags.SetField | BindingFlags.Public |
                                                         BindingFlags.NonPublic | BindingFlags.Instance, null,
                                                         targetObject, new[] { fieldValue });
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.setProperty");
            }
            return false;
        }

        public bool setProperty(PropertyInfo propertyToSet, object propertyValue)
        {
            return setProperty(propertyToSet, null, propertyValue);
        }

        public bool setProperty(PropertyInfo propertyToSet, object liveObject, object propertyValue)
        {
            try
            {
                propertyToSet.SetValue(liveObject,propertyValue,
                                            BindingFlags.SetProperty | BindingFlags.Public |
                                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,null,
                                            // ReSharper disable AssignNullToNotNullAttribute
                                            null );
                                            // ReSharper restore AssignNullToNotNullAttribute
                                            
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.setProperty");
            }
            return false;
        }
       

        public bool setProperty(String propertyToSet, Type targetType, object propertyValue)
        {
            try
            {
                var propertyInfo = getPropertyInfo(propertyToSet, targetType);
                if (propertyInfo == null)
                    PublicDI.log.error("in setProperty, could not find property {0} in type {1}", propertyToSet, targetType.FullName);
                else
                    return setProperty(propertyInfo, propertyValue);
                
                /*return false;
                if (targetType != null)
                    targetType.InvokeMember(propertyToSet,
                                            BindingFlags.SetProperty | BindingFlags.Public |
                                            BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null,
                                            targetType, new[] {propertyValue});
                return true;*/
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.setProperty");
            }
            return false;
        }

        public bool setProperty(String sPropertyToSet, object oTargetObject, object propertyValue)
        {
            try
            {
                if (oTargetObject != null)
                    oTargetObject.GetType().InvokeMember(sPropertyToSet,
                                                         BindingFlags.SetProperty | BindingFlags.Public |
                                                         BindingFlags.NonPublic | BindingFlags.Instance, null,
                                                         oTargetObject, new[] {propertyValue});
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in Reflection.setProperty");
            }
            return false;
        }

        #endregion

        #region load

        public Assembly loadAssembly(string assemblyToLoad)
        {
			try
			{
                if (AssemblyResolver.CachedMappedAssemblies.hasKey(assemblyToLoad))
                { 
					var cachedAssembly = AssemblyResolver.CachedMappedAssemblies[assemblyToLoad];
                    return cachedAssembly;
                }
                Assembly assembly = AssemblyResolver.loadFromDiskOrResource(assemblyToLoad);
				if (assembly != null)
				{
					AssemblyResolver.CachedMappedAssemblies.add(assemblyToLoad, assembly);
					AssemblyResolver.CachedMappedAssemblies.add(assembly.FullName, assembly);         // this has side effects on dynamic code scanning
					AssemblyResolver.CachedMappedAssemblies.add(assembly.GetName().Name, assembly);
					return assembly;
				}

				// try with load method #1                
                #pragma warning disable 618
                try
                {
                    assembly = Assembly.LoadWithPartialName(assemblyToLoad);

                    if (assembly.isNull() && assemblyToLoad.lower().ends(".dll") || assemblyToLoad.lower().ends(".exe"))
                    {
                        assembly = Assembly.LoadWithPartialName(assemblyToLoad.fileName_WithoutExtension());
                    }
                }               
                catch { }                
                #pragma warning restore 618
				if (assembly.isNull())
				{
					// try with load method #2
					try
					{
						//if (System.IO.File.Exists(assemblyToLoad) == false &&
						//   System.IO.File.Exists(System.IO.Path.Combine( PublicDI.config.CurrentExecutableDirectory,assemblyToLoad)))         // if this assembly is not on the current executable folder                                 
						//   new O2GitHub().tryToFetchAssemblyFromO2GitHub(assemblyToLoad);		
						//assembly = Assembly.Load(assemblyToLoad);		
						assembly = Assembly.LoadFrom(assemblyToLoad);
					}
					catch //(Exception ex1)
					{
						// try with load method #3
						try
						{
							assembly = Assembly.Load(AssemblyName.GetAssemblyName(assemblyToLoad).FullName);
						}
						catch// (Exception ex2)
						{
							// try with load method #4
							try
							{
								assembly = Assembly.Load(assemblyToLoad);
							}                            
							catch //(Exception ex3)                            
							{
								//            		    PublicDI.log.error("in loadAssembly (Assembly.LoadFrom) :{0}", ex1.Message);
								//            		    PublicDI.log.error("in loadAssembly (Assembly.Load) :{0}", ex2.Message);
								//                        PublicDI.log.error("in loadAssembly (Assembly.LoadWithPartialName) :{0}", ex3.Message);
							}
						}
					}
				}
				if (assembly != null)
				{
					AssemblyResolver.CachedMappedAssemblies.add(assemblyToLoad, assembly);
					AssemblyResolver.CachedMappedAssemblies.add(assembly.FullName, assembly);
					AssemblyResolver.CachedMappedAssemblies.add(assembly.GetName().Name, assembly);
					return assembly;
				}
			}
			catch (Exception ex)
			{
				"[loadAssembly] {0}".error(ex.Message);
			}
			PublicDI.log.info("could not load/find assembly ('{0}')",assemblyToLoad); 
			//PublicDI.log.error("load using partial name ('{0}') returned null",assemblyToLoad);					            		    
            return null;
        }

        public bool loadAssemblyAndCheckIfAllTypesCanBeLoaded(string assemblyFile)
        {
            try
            {
                Assembly reflectionAssembly = getAssembly(assemblyFile);
                List<Type> reflectionTypes = getTypes(reflectionAssembly);
                if (reflectionTypes != null)
                    return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("in loadAssemblyAndCheckIfAllTypesCanBeLoaded:{0}", ex.Message);
            }
            return false;
        }

        #endregion

        #region invoke


        public Thread invokeASync(MethodInfo methodInfo, Action<object> onMethodExecutionCompletion)
        {
            return O2Thread.mtaThread(
                    () =>
                        {
                            var result = invokeMethod_Static(methodInfo);
                            onMethodExecutionCompletion(result);
                        });            
        }

        public Thread invokeASync(object oLiveObject, MethodInfo methodInfo, Action<object> onMethodExecutionCompletion)
        {
            return invokeASync(oLiveObject, methodInfo, null, onMethodExecutionCompletion);
        }

        public Thread invokeASync(object oLiveObject, MethodInfo methodInfo, object[] methodParameters, Action<object> onMethodExecutionCompletion)
        {
            return O2Thread.mtaThread(
                () =>
                    {
                        try
                        {
                            //var result = invoke(oLiveObject, methodInfo, methodParameters);
                            var result = methodInfo.Invoke(oLiveObject, methodParameters);
                            onMethodExecutionCompletion(result);
                        }
                        catch (Exception ex)
                        {
                            //PublicDI.log.ex(ex, "in reflection.invokeASync", true);
                            PublicDI.log.error("in reflection.invokeASync: {0}", ex.Message);

                            var exceptionMessage = "Exception occured during invocation of method: " + methodInfo.Name;
                            exceptionMessage += "      Exception.Message: " + ex.Message;
                            if (ex.InnerException != null)
                                exceptionMessage += "     InnerException.Message: " + ex.InnerException.Message;

                            onMethodExecutionCompletion(exceptionMessage);
                        }
                    });
        }

        public bool invokeMethod_returnSucess(Object oLiveObject, String sMethodToInvoke, Object[] oParams)
        {
            try
            {
                oLiveObject.GetType().InvokeMember(sMethodToInvoke,
                                                   BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                                                   BindingFlags.Instance | BindingFlags.InvokeMethod, null, oLiveObject,
                                                   (oParams ?? new object[0]));
                return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in reflection..invokeMethod", true);
                //log.error("in reflection.invokeMethod: {0} ", ex.Message);
                return false;
            }
        }

        public object invoke(MethodInfo methodInfo)
        {
            if (methodInfo.IsStatic)
                return invokeMethod_Static(methodInfo);
            
            return invokeMethod_Instance(methodInfo);
        }        

        /// <summary>
        /// invokes static or instance method
        /// (for instance methods, the default constructor will be used to create the method's class)
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="methodParameters"></param>
        /// <returns></returns>
        public object invoke(MethodInfo methodInfo, object[] methodParameters)
        {
            try
            {
                if (methodInfo.IsStatic)
                    return invoke(null, methodInfo, methodParameters);

                object oLiveObject = createObjectUsingDefaultConstructor(methodInfo.ReflectedType);
                return invoke(oLiveObject, methodInfo, methodParameters);

            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in reflection.invokeMethod_InstanceStaticPublicNonPublic", true);
            }
            return null;
        }

        public object invoke(object oLiveObject, MethodInfo methodInfo)
        {
            return invoke(oLiveObject, methodInfo, new object[] {});
        }

        public object invoke(object oLiveObject, MethodInfo methodInfo, object[] methodParameters)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return methodInfo.Invoke(oLiveObject, BindingFlags.OptionalParamBinding , null, methodParameters, null);
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch (Exception ex)
            {
                if (ex is TargetParameterCountException || ex is MissingMethodException || ex is ArgumentException)
                {
                    //Hack: to deal with weird reflection behaviour that happens when the parameter is an array
                    methodParameters = new object[] { methodParameters };
                    try
                    {
                        // ReSharper disable AssignNullToNotNullAttribute
                        return methodInfo.Invoke(oLiveObject, BindingFlags.OptionalParamBinding, null, methodParameters, null);
                        // ReSharper restore AssignNullToNotNullAttribute
                    }
                    catch (Exception ex2)
                    {
                        PublicDI.log.ex(ex2, "in reflection.invoke", true);
                        return null;
                    }
                }
                PublicDI.log.ex(ex, "in reflection.invoke", true);
                return null;
            } 
            
        }

        public object invoke(object oLiveObject, string sMethodToInvoke, object[] methodParameters)
        {
            return invokeMethod_InstanceStaticPublicNonPublic(oLiveObject, sMethodToInvoke, methodParameters);
        }

        public object invokeMethod_InstanceStaticPublicNonPublic(object oLiveObject, string sMethodToInvoke,
                                                                 object[] methodParameters)
        {
            try
            {
                return oLiveObject.GetType().InvokeMember(sMethodToInvoke,
                                                          BindingFlags.Public | BindingFlags.NonPublic |
                                                          BindingFlags.Static | BindingFlags.Instance |
                                                          BindingFlags.InvokeMethod, null, oLiveObject, (methodParameters ?? new object[0]));
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in reflection.invokeMethod_InstanceStaticPublicNonPublic", true);
                return null;
            }
        }


        public object invokeMethod_Instance(MethodInfo methodInfo)
        {
            try
            {
                PublicDI.log.info("Executing Instance Method:{0}", methodInfo.Name);
                var liveObject = PublicDI.reflection.createObjectUsingDefaultConstructor(methodInfo.DeclaringType);
                if (liveObject != null)
                    return invoke(liveObject, methodInfo);
            }
            catch (Exception ex)
            {                
                PublicDI.log.ex(ex, "in invokeMethod_Instance: ");
            }
            return null;
        }

        public Object invokeMethod_Static(MethodInfo methodToExecute)
        {
            return invokeMethod_Static(methodToExecute, null);
        }

        public Object invokeMethod_Static(MethodInfo methodToExecute ,object[] oParams)
        {
            try
            {
                return methodToExecute.Invoke(null,
                                              BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static | BindingFlags.InvokeMethod, null,                                              
                                              (oParams ?? new object[0]), 
                                              // ReSharper disable AssignNullToNotNullAttribute
                                              null);
                                              // ReSharper restore AssignNullToNotNullAttribute
            }
            catch (Exception ex)
            {
                if (ex is TargetParameterCountException || ex is MissingMethodException || ex is ArgumentException)
                {
                    try
                    {
                        //HACK: deal with the problem of invoking into a method with an array as a parameter
                        oParams = new object[] { oParams };
                        return methodToExecute.Invoke(null,
                                                  BindingFlags.Public | BindingFlags.NonPublic |
                                                  BindingFlags.Static | BindingFlags.InvokeMethod, null,
                                                  (oParams), 
                                                  // ReSharper disable AssignNullToNotNullAttribute
                                                  null);
                                                  // ReSharper restore AssignNullToNotNullAttribute
                    }
                    catch (Exception ex2)
                    {
                        PublicDI.log.ex(ex2, "in reflection..invokeMethod_Static", true);
                        return null;
                    }
                }
                PublicDI.log.ex(ex, "in reflection..invokeMethod_Static", true);
                return null;
            }
        }

        public Object invokeMethod_Static(Type tMethodToInvokeType, string sMethodToInvokeName, object[] oParams)
        {
            try
            {
                return tMethodToInvokeType.InvokeMember(sMethodToInvokeName,
                                                        BindingFlags.Public | BindingFlags.NonPublic |
                                                        BindingFlags.Static | BindingFlags.InvokeMethod, null, null,
                                                        (oParams ?? new object[0]));
            }
            catch (Exception ex)
            {
                if (ex is TargetParameterCountException || ex is MissingMethodException || ex is ArgumentException)
                {
                    try
                    {
                        //HACK: deal with the problem of invoking into a method with an array as a parameter
                        oParams = new object[] { oParams };
                        return tMethodToInvokeType.InvokeMember(sMethodToInvokeName,
                                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                                BindingFlags.Static | BindingFlags.InvokeMethod, null, null,
                                                                (oParams));
                    }
                    catch (Exception ex2)
                    {
                        PublicDI.log.ex(ex2, "in reflection.invokeMethod_Static", true);
                        return null;
                    }
                }
                PublicDI.log.ex(ex, "in reflection..invokeMethod_Static", true);
                return null;
            }
        }

        public Object invokeMethod_Static(string sMethodToInvokeType, string sMethodToInvokeName,
                                          object[] oParams)
        {
            try
            {
                // first get the type
                Type tMethodToInvokeType = PublicDI.reflection.getType(sMethodToInvokeType);
                // then invoke the method
                return tMethodToInvokeType.InvokeMember(sMethodToInvokeName,
                                                        BindingFlags.Public | BindingFlags.NonPublic |
                                                        BindingFlags.Static | BindingFlags.InvokeMethod, null, null,
                                                        (oParams ?? new object[0]));
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "in reflection.invokeMethod_Static", true);
                //log.ex(ex);
                return null;
            }
        }

        #endregion

        #region create

        public object createObject(String assemblyToLoad, String typeToCreateObject,
                                   params object[] constructorArguments)
        {
            try
            {
                Assembly assembly = getAssembly(assemblyToLoad);
                return createObject(assembly, typeToCreateObject, constructorArguments);
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, " in createObject(String assemblyToLoad,");
            }
            return null;
        }

        public object createObject(Assembly assembly, Type typeToCreateObject,
                                   params object[] constructorArguments)
        {
            return (typeToCreateObject != null)
                       ? createObject(assembly, typeToCreateObject.FullName, constructorArguments)
                       : null;
        }

     
        public object createObject(Assembly assembly, String typeToCreateObject,
                                   params object[] constructorArguments)
        {                            
                Type type = getType(assembly, typeToCreateObject);
                return createObject(type,constructorArguments);
        }

        public object createObject(Type type, params object[] constructorArguments)
        {
            try 
            {
            	if (type == null)
            	{
            		"in createObject, type provided was null".error();
            		return null;
            	}
                var constructorArgumentTypes = new List<Type>();
                if (constructorArguments != null)
                    foreach (object argument in constructorArguments)
                        constructorArgumentTypes.Add(argument.GetType());
                ConstructorInfo constructor = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, constructorArgumentTypes.ToArray(), null);
                if (constructor == null)
                {                	
                	"In createObject, could not find constructor for type: {0}"
                		.format(type.Name).error();
                	return null;
                }
                return constructor.Invoke(constructorArguments ?? new object[0]);
            /*try
            {
                var constructorArgumentTypes = new List<Type>();
                if (constructorArguments != null)
                    foreach (object argument in constructorArguments)
                        constructorArgumentTypes.Add(argument.GetType());
                ConstructorInfo constructor = type.GetConstructor(constructorArgumentTypes.ToArray());
                return constructor.Invoke(constructorArguments ?? new object[0]);
            */
                // this only really works for types derived from marshal by object
                //var wrappedObject = Activator.CreateInstanceFrom(assemblyToLoad, typeToCreateObject, arguments); 
                //return (wrappedObject != null) ? wrappedObject.Unwrap() : null;            
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, " createObject for type: " + type.Name,true);
            }
            return null;
        }

        public Object createObjectUsingDefaultConstructor(Type tTypeToCreateObject)
        {
            try
            {                
                return Activator.CreateInstance(tTypeToCreateObject);
            }
            catch (Exception ex)
            {
                PublicDI.log.error("..In createObjectUsingDefaultConstructor: {0}", ex.Message);
                if (ex.InnerException != null)
                PublicDI.log.error("    InnerException : {0}", ex.InnerException.Message);	
                return null;
            }
        }
        
        #endregion
    }
}
