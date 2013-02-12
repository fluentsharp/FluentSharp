using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using O2.Kernel;

using System.Reflection;
using System.Threading;
using O2.DotNetWrappers.DotNet;

namespace O2.DotNetWrappers.ExtensionMethods
{

    public static class Reflection_ExtensionMethods_Assemby
    { 
        public static Assembly              assembly                     (this string assemblyName)
        {
            return PublicDI.reflection.getAssembly(assemblyName);
        }
        public static Assembly              assembly                     (this byte[] bytes)
        {
            try
            {
                return Assembly.Load(bytes);
            }
            catch (Exception ex)
            {
                ex.log("Failed to load assembly from provided bytes");
                return null;
            }            
        }
        public static string                assemblyLocation             (this Type type)
        {
            if (type.isNull())
                return null;
            return type.Assembly.Location;
        }
        public static string                append_CurrentAssemblyVersion(this string aString)
        {
            return Assembly.GetCallingAssembly().version();            
        }
        public static string                append_O2Version             (this string aString)
        {
            return Assembly.GetExecutingAssembly().version();
        }
        
        public static string                version                      (this Assembly assembly)
        {
            if (assembly.notNull())
                return assembly.GetName().Version.ToString();
            return "";
        }
        public static List<AssemblyName>    referencedAssemblies         (this Assembly assembly)
        {
            return assembly.GetReferencedAssemblies().toList();
        }		
        public static bool                  isAssemblyName               (this string _string)
        {
            return _string.contains("PublicKeyToken") &&   // ensure these exists since .assemblyName() will work for simple filenames
                   _string.assemblyName().notNull();
        }		
        public static AssemblyName          assemblyName                 (this string _string)
        {
            try
            {
                return new AssemblyName(_string);
            }
            catch//(Exception ex)
            {
                //"[assemblyName] {0}".error(ex.Message);
                return null;
            }
        }
        public static bool                  isDynamic                    (this Assembly assembly)
        {
            return assembly.prop<bool>("IsDynamic");
        }
        public static List<Assembly>        notDynamic                   (this List<Assembly> assemblies)
        {
            return (from assembly in assemblies                    
                    where assembly.isDynamic().isFalse()
                    select assembly).toList();
        }
        public static List<Assembly>        with_Valid_Location          (this List<Assembly> assemblies)
        {
            return assemblies.notDynamic().where((assembly) => assembly.Location.valid()).toList();
        }
        public static List<string>          names                        (this List<AssemblyName> assemblyNames)
        {
            return (from assemblyName in assemblyNames
                    select assemblyName.name()).toList();
        }
        public static string                name                         (this Assembly assembly)
        {
            if (assembly.notNull())
                return assembly.GetName().Name;
            return null;
        }
        public static string                name                         (this AssemblyName assemblyName)
        {
            if(assemblyName.notNull())
                return assemblyName.Name;
            return null;
        }					
        public static Assembly              assembly                     (this AssemblyName assemblyName)
        {
            return assemblyName.str().assembly();
        }		
        public static string                assembly_Location            (this string assemblyName)
        {
            return assemblyName.assembly().location();
        }		
        public static string                location                     (this Assembly assembly)
        {
            if (assembly.notNull())
            {
                if (assembly.Location.notValid())                
                    return AssemblyResolver.saveEmbeddedAssemblyToDisk(assembly.GetName());
                return assembly.Location;
            }
            return null;
        }
        public static List<AssemblyName>    referencedAssemblies         (this AssemblyName assemblyName)
        {
            return assemblyName.assembly().referencedAssemblies();
        }
        public static List<AssemblyName>    referencedAssemblies         (this Assembly assembly, bool recursiveSearch)
        {
            return assembly.referencedAssemblies(recursiveSearch, true);
        }
        public static List<AssemblyName>    referencedAssemblies         (this Assembly assembly, bool recursiveSearch, bool removeGacEntries)
        {
            var mappedReferences = new List<string>();
            var resolvedAssemblies = new List<AssemblyName>();

            Action<List<AssemblyName>> resolve = null;

            resolve = (assemblyNames) =>
            {
                if (removeGacEntries)
                    assemblyNames = assemblyNames.removeGacAssemblies();
                if (assemblyNames.isNull())
                    return;
                foreach (var assemblyName in assemblyNames)
                {
                    if (mappedReferences.contains(assemblyName.str()).isFalse())
                    {
                        mappedReferences.add(assemblyName.str());
                        resolvedAssemblies.add(assemblyName);
                        resolve(assemblyName.referencedAssemblies());
                    }
                }
            };

            resolve(assembly.referencedAssemblies());

            "there where {0} NonGac  assemblies resolved for {1}".debug(resolvedAssemblies.size(), assembly.Location);
            return resolvedAssemblies;
        }
        public static List<AssemblyName>    removeGacAssemblies          (this List<AssemblyName> assemblyNames)
        {
            var systemRoot = Environment.GetEnvironmentVariable("SystemRoot");
            return (from assemblyName in assemblyNames
                    let assembly = assemblyName.assembly()
                    where assembly.notNull() && assembly.Location.starts(systemRoot).isFalse()
                    select assemblyName).toList();
        }        
        public static List<string>			locations                    (this List<AssemblyName> assemblyNames)
        {

            var locations = new List<string>();
            try
            {
                foreach (var assemblyName in assemblyNames)
                {
                    var location = assemblyName.assembly().Location;
                    locations.add(location);
                }
            }
            catch (Exception ex)
            {
                "[Reflection] locations, could not resolve {0}".error(ex.Message);
            }
            return locations;
        }
        public static string				imageRuntimeVersion          (this Assembly assembly)
        {
            if (assembly.notNull())
                return assembly.ImageRuntimeVersion;
            return null;
        }
        
        public static PortableExecutableKinds	portableExecutableKind              (this Assembly assembly)
        {
            PortableExecutableKinds peKind;
            ImageFileMachine imageFileMachine;
            assembly.ManifestModule.GetPEKind(out peKind, out imageFileMachine);
            return peKind;
        }
        public static string					value                               (this PortableExecutableKinds peKind)
        {
            switch (peKind)
            {
                case PortableExecutableKinds.ILOnly:
                    return "AnyCPU";
                case PortableExecutableKinds.Required32Bit:
                    return "x86";
                case PortableExecutableKinds.PE32Plus:
                    return "x64";
                case PortableExecutableKinds.Unmanaged32Bit:
                    return "Unmanaged32Bit";
                case PortableExecutableKinds.NotAPortableExecutableImage:
                    return "NotAPortableExecutableImage";
                default:
                    return peKind.str();
                //throw new ArgumentOutOfRangeException();
            }
        }
        public static List<Assembly>            removeAssembliesSignedByMicrosoft   (this IEnumerable<Assembly> assemblies)
        {
            return (from assembly in assemblies
                    where assembly.isDynamic().isFalse()
                    where assembly.FullName.contains("PublicKeyToken=b03f5f7f11d50a3a",         //need to identify other public keys used by Microsoft
                                                     "PublicKeyToken=b77a5c561934e089", 
                                                     "PublicKeyToken=31bf3856ad364e35").isFalse()
                    select assembly).toList();
        }        
        public static PortableExecutableKinds	assembly_PortableExecutableKind     (this string assemblyLocation)
        {
            return Assembly.ReflectionOnlyLoadFrom(assemblyLocation).portableExecutableKind();
        }        
    }

    public static class Reflection_ExtensionMethods_Ctor
    { 
        public static object                ctor(this string className, string assembly, params object[] parameters)
        {
            var obj = PublicDI.reflection.createObject(assembly, className, parameters);
            if (PublicDI.reflection.verbose)
                if (obj == null)
                    PublicDI.log.error("in ctor, could not created object: {0}!{1}", assembly, className);
                else
                    PublicDI.log.debug("in ctor, created object of type: {0}", obj.GetType());
            return obj;
        }
        public static Object                ctor(this Type type, params object[] constructorParams)
        {
            return PublicDI.reflection.createObject(type, constructorParams);
        }
        public static List<ConstructorInfo> ctors(this Type type)
        {
            return type.GetConstructors(BindingFlags.NonPublic | 
                                        BindingFlags.Public | 
                                        BindingFlags.Instance).toList();
        }		    
        public static Array                 createArray<T>(this Type arrayType,  params T[] values)			
        {
            try
            {
                if (values.isNull())
                    return  Array.CreateInstance (arrayType,0);	
                    
                var array =  Array.CreateInstance (arrayType,values.size());	
                
                if (values.notNull())
                    for(int i=0 ; i < values.size() ; i ++)
                        array.SetValue(values[i],i);
                return array;				 				
            }
            catch(Exception ex)
            {
                ex.log("in Array.createArray");
            }
            return null;
        }	
    }

    public static class Reflection_ExtensionMethods_Types
    { 
        public static Type          type(this Assembly assembly, string typeName)
        {
            return PublicDI.reflection.getType(assembly, typeName);
        }
        public static Type          type(this string assemblyName, string typeName)
        {
            return PublicDI.reflection.getType(assemblyName, typeName);
        }
        public static Type          type(this object _object)
        {
            if (_object.isNull())
                return null;
            return _object.GetType();
        }
        public static List<Type>    types(this Assembly assembly)
        {
            return PublicDI.reflection.getTypes(assembly);
        }
        public static List<Type>    types(this Type type)
        {
            return PublicDI.reflection.getTypes(type);
        }

        public static List<Type>	types<T>(this List<T> list)
        {
            return list.Select((item) => item.type()).toList();
        }
        public static object		infoTypeName(this object _object)
        {
            if (_object.notNull())
                _object.typeName().info();
            else
                "in infoTypeName _object was null".error();
            return _object;
        }
        public static string        typeName(this object _object)
        {
            if (_object != null)
                return _object.type().Name;
            return "";
        }
        public static string        typeFullName(this object _object)
        {
            return _object.type().FullName;
        }
        public static string        name(this Type type)
        {
            return type.Name;
        }
        public static string        fullName(this Type type)
        {
            return type.FullName;
        }

        public static List<string> names(this List<Type> types)
        {
            return types.Select((type) => type.Name).toList();
        }

        public static string        comTypeName(this object _object)
        {
            return PublicDI.reflection.getComObjectTypeName(_object);
        }
        public static List<Type>    baseTypes(this Type type)
        {
            var baseType = new List<Type>();
            if (type.BaseType.notNull())
            {
                baseType.Add(type.BaseType);
                baseType.AddRange(baseTypes(type.BaseType));                
            }
            return baseType;
        }
        public static List<Type>    withBaseType<T>(this Assembly assembly)
        {
            return assembly .types()
                            .Where(type => type.baseTypes()
                                               .Contains(typeof (T)))
                            .ToList();
        }

        public static List<string>  typesNames(this Assembly assembly)
        {
            return (from type in assembly.types()
                    select type.Name).ToList();
        }    
    }

    public static class Reflection_ExtensionMethods_Properties
    { 
        public static List<PropertyInfo>    properties(this Type type)
        {
            return PublicDI.reflection.getProperties(type);
        }
        public static List<PropertyInfo>	properties<T>(this Type type)
        {
            return type.properties().where((prop)=>prop.PropertyType == typeof(T));
        }        
        public static List<PropertyInfo>    properties_public_declared(this Type type, BindingFlags bindingFlags)
        { 
            return type.properties(BindingFlags.Public | BindingFlags.DeclaredOnly);
        }
        public static List<PropertyInfo>    properties(this Type type, BindingFlags bindingFlags)
        {            
            return new List<PropertyInfo>(type.GetProperties(bindingFlags));
        }        
        public static object                property(this Type type, string propertyName)
        {
            return type.prop(propertyName);
        }
        public static T                     property<T>(this object _object, string propertyName)
        {						
            if (_object.notNull())
            {
                var result = _object.property(propertyName);
                if (result is T)
                    return (T)result;
            }
            return default(T);
        }		
        public static object                property(this object _object, string propertyName, object value)
        {
            var propertyInfo = PublicDI.reflection.getPropertyInfo(propertyName, _object.type());  
            if (propertyInfo.isNull())
                "Could not find property {0} in type {1}".error(propertyName, _object.type());  
            else
            {
                PublicDI.reflection.setProperty(propertyInfo, _object,value);    
        //		"set {0}.{1} = {2}".info(_object.type(),propertyName, value);
            }
            return _object;

        }		
        public static object                property(this object liveObject, string propertyName)
        {
            return liveObject.prop(propertyName);
        }        
        public static Type                  propertyType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType;
        }
        public static List<object>          propertyValues(this object _object)
        {
            /*var propertyValues = new List<object>();
            var names = _object.type().properties().names();
            foreach(var name in names)
                propertyValues.Add(_object.prop(name));
            return propertyValues;*/
            return _object.type()
                          .properties()
                          .names()
                          .select(name => _object.prop(name));            
        }
        public static List<T>               propertyValues<T>(this object _object)
        {            
            return _object.type         ()
                          .properties<T>()
                          .select       (property => (T) _object.prop(property.Name));                          
        }
        public static Dictionary<string,T> propertyValues_MappedBy_Name<T>(this object _object)
        {
            var propertyValues = new Dictionary<string,T>();
            var properties = _object.type().properties<T>();
            foreach (var property in properties)
                propertyValues.add(property.Name, (T)_object.prop(property.Name));
            return propertyValues;
        }
        public static object                prop(this Type type, string propertyName)
        {
            return PublicDI.reflection.getProperty(propertyName, type);
        }    
        public static T                     prop<T>(this object liveObject, string propertyName)
        {
            if (liveObject.notNull())
            {
                var value = liveObject.prop(propertyName);
                if (value is T)
                    return (T)value;
            }
            return default(T);
        }
        public static object                prop(this object liveObject, string propertyName)
        {            
            return PublicDI.reflection.getProperty(propertyName, liveObject);
        }
        public static object				prop(this object liveObject, PropertyInfo propertyInfo, object value)
        {
            PublicDI.reflection.setProperty(propertyInfo, liveObject, value);
            return liveObject;
        }        
        public static object                prop(this object liveObject, string propertyName, object value)
        {
            PublicDI.reflection.setProperty(propertyName, liveObject, value);
            return liveObject;
        }
        public static List<string>          names(this List<PropertyInfo> properties)
        {
            return (from property in properties
                    select property.Name).toList();
        }		
        
    }

    public static class Reflection_ExtensionMethods_Fields
    { 
        public static List<FieldInfo>   fields(this Type type)
        {
            return PublicDI.reflection.getFields(type);
        }
        public static List<FieldInfo>	fields<T>(this Type type)
        {
            return type.fields().where((field) => field.FieldType == typeof(T));
        }
        public static object            field(this object liveObject, string fieldName)
        {
            return PublicDI.reflection.getFieldValue(fieldName, liveObject);
        }
        public static object            field(this Type type, string fieldName)
        {
            return PublicDI.reflection.getField(type, fieldName);
        }
        public static object            field(this object liveObject, string fieldName, object value)
        {
            PublicDI.reflection.setField(fieldName, liveObject, value);
            return liveObject;
        }        
        public static T                 field<T>(this object _object, string fieldName)
        {
            var value = _object.field(fieldName);
            if (value is T)
                return (T) value;
            return default(T);
        }		
        public static object            field(this object _object, Type type, string fieldName)
        {						
            var fieldInfo =  (FieldInfo)type.field(fieldName);
            return PublicDI.reflection.getFieldValue(fieldInfo, type);
        }		
        public static object            fieldValue(this Type type, string fieldName)
        {
            var fieldInfo = (FieldInfo)type.field(fieldName);
            return PublicDI.reflection.getFieldValue(fieldInfo, null);
        }
        public static Type              fieldValue(this Type type, string fieldName, object value)
        {
            var fieldInfo = (FieldInfo)type.field(fieldName);			
            PublicDI.reflection.setField(fieldInfo, fieldInfo.ReflectedType, value);
            return type;
        }		
        
    }

    public static class Reflection_ExtensionMethods_Enums
    { 
        public static object    enumValue(this Type enumType, string value)
        {
            return enumType.enumValue<object>(value);
        }
        public static T         enumValue<T>(this Type enumType, string value)
        {
            var fieldInfo = (FieldInfo) enumType.field(value);
            if (fieldInfo.notNull())
                return (T)fieldInfo.GetValue(enumType);
            return default(T);
        }		
    }

    public static class Reflection_ExtensionMethods_Parameters
    { 
        public static List<ParameterInfo> parameters(this MethodInfo methodInfo)
        {
            return PublicDI.reflection.getParameters(methodInfo);
        }
    }

    public static class Reflection_ExtensionMethods_Methods
    {
        public static List<MethodInfo>  methods(this Type type)
        {
            return PublicDI.reflection.getMethods(type);
        }
        public static List<MethodInfo>  methods(this Assembly assembly)
        {
            return PublicDI.reflection.getMethods(assembly);
        }
        public static List<MethodInfo> methods(this Type type, string methodName)
        {
            return (from method in type.methods()
                    where method.Name == methodName
                    select method).toList();
        }
        public static MethodInfo method_bySignature(this Type type, string methodSignature)
        {
            return (from method in type.methods()
                    where method.str() == methodSignature
                    select method).first();
        }
        public static List<MethodInfo>  methods_public(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance);
        }
        public static List<MethodInfo>  methods_private(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Instance);
        }
        public static List<MethodInfo>  methods_declared(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }
        public static List<MethodInfo>  methods_static(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);
        }
        public static List<MethodInfo>  methods_instance(this Type type)
        {
            return PublicDI.reflection.getMethods(type, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);
        }
        public static MethodInfo        firstMethod(this Assembly assembly)
        {
            // to skip the methods created by properties
            return assembly.methods().FirstOrDefault(method => false == method.IsSpecialName);
        }

        public static List<String>      names(this List<MethodInfo> methods)
        {
            var names = from method in methods select method.Name;
            return names.ToList();
        }
        public static MethodInfo        method(this Type type, string name)
        {
            return type.methods().first(method => method.Name == name);
        }
        public static string            signature(this MethodInfo methodInfo)
        {
            if (Environment.Version.Major == 4)
            {                
                var method = methodInfo.type().method("ConstructName");
                var result = method.Invoke(methodInfo, new object[] { });
                return result.str();        
            }
            return "mscorlib".assembly().type("RuntimeMethodInfo")
                                            .method("ConstructName")
                                            .invokeStatic(methodInfo)
                                            .str();                
            
        }		    
        public static List<MethodInfo>  methods(this List<Type> types)
        {
            return (from type in types
                    from method in type.methods()
                    select method).toList();					
        }				
    }

    public static class Reflection_ExtensionMethods_Invoke
    {
        public static List<Action>          invoke(this List<Action> actions)
        {
            if (actions.notNull())
                foreach (var action in actions)
                    action.invoke();
            return actions;
        }
        public static List<Action<T>>       invoke<T>(this List<Action<T>> actions, T param1)
        {
            if (actions.notNull())
                foreach (var action in actions)
                    action.invoke(param1);
            return actions;
        }
        public static List<Action<T1,T2>>   invoke<T1, T2>(this List<Action<T1, T2>> actions, T1 param1, T2 param2)
        {
            if (actions.notNull())
                foreach (var action in actions)
                    action.invoke(param1, param2);
            return actions;
        }
        public static Action                invoke(this Action action)
        {
            if (action.notNull())
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    ex.log("[in Action.invoke]");
                }
            }
            return action;
        }
        public static Action<T>             invoke<T>(this Action<T> action, T param)
        {
            if (action.notNull())
                try
                {
                    action(param);
                }
                catch (Exception ex)
                {
                    ex.log("[in Action.invoke<T>]");
                }
            return action;
        }
        public static Action<T1,T2>         invoke<T1,T2>(this Action<T1,T2> action, T1 param1, T2 param2)
        {
            if (action.notNull())
                try
                {
                    action(param1, param2);
                }
                catch (Exception ex)
                {
                    ex.log("[in Action.invoke<T>]");
                }
            return action;
        }
        public static T                     invoke<T>(this Func<T> func)
        {
            if (func.notNull())            
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    ex.log("[in Func.invoke<T>]");
                }
            
            return default(T);
        }
        public static T2                    invoke<T1,T2>(this Func<T1,T2> func, T1 param)
        {
            if (func.notNull())
                try
                {
                    return func(param);
                }
                catch (Exception ex)
                {
                    ex.log("[in Func.invoke<T1,T2>]");
                }
            return default(T2);
        }
        public static object                invoke(this object liveObject, MethodInfo methodInfo, params object[] parameters)
        {
            return methodInfo.invoke_on_LiveObject(liveObject, parameters);
        }
        public static object                invoke(this object liveObject, string methodName, params object[] parameters)
        {
            return PublicDI.reflection.invoke(liveObject, methodName, parameters);
        }
        public static object                invokeStatic(this Type type, string methodName, params object[] parameters)
        {
            return PublicDI.reflection.invokeMethod_Static(type, methodName, parameters);
        }  
        public static object                invoke(this MethodInfo methodInfo, params object[] parameters)
        {
            return PublicDI.reflection.invoke(methodInfo, parameters);
        }        
        public static object                invoke_on_LiveObject(this MethodInfo methodInfo, object liveObject, object[] parameters)
        {         
            return PublicDI.reflection.invoke(liveObject, methodInfo, parameters);            
        }    
        public static Thread				invokeStatic_StaThread(this MethodInfo methodInfo, params object[] invocationParameters)
        {
            return O2Thread.staThread(()=>methodInfo.invokeStatic(invocationParameters));
        }		        
        public static object				invokeStatic(this MethodInfo methodInfo, params object[] invocationParameters)
        {            
            if (invocationParameters.notNull())
                return PublicDI.reflection.invokeMethod_Static(methodInfo, invocationParameters);
                //return PublicDI.reflection.invokeMethod_Static(methodInfo, new object[] { invocationParameters});			
            return PublicDI.reflection.invokeMethod_Static(methodInfo);
        }
        public static object				invokeStatic(this Assembly assembly, string type, string method, params object[] parameters)
        {
            return assembly.type(type).invokeStatic(method, parameters);
        }
    }

    public static class Reflection_ExtensionMethods_Interfaces
    { 
        public static List<Type>    interfaces(this Type type)
        {
            return new List<Type>( type.GetInterfaces() );
        }
    }

    public static class Reflection_ExtensionMethods_Attributes
    { 
        public static List<Attribute>           attributes      (this Assembly assembly)
        {
            return PublicDI.reflection.getAttributes(assembly);
       
        }		

        public static List<T>                   attributes<T>   (this Assembly assembly)			where T : Attribute
        {
            return assembly.attributes().attributes<T>();
        }	
        public static T                         attribute<T>    (this Assembly assembly)			where T : Attribute
        {
            return assembly.attributes<T>().first();
        }	        	
        public static bool                      hasAttribute<T>(this Assembly assembly)			    where T : Attribute
        {
            return assembly.attribute<T>().notNull();
        }
        public static List<Attribute>    attributes(this List<MethodInfo> methods)
        {
            return (from method in methods 
                    from attribute in method.attributes()
                    select attribute).toList();
        }
        public static List<Attribute>           attributes(this MethodInfo method)
        {
            return PublicDI.reflection.getAttributes(method);
        }		
        public static List<T>                   attributes<T>(this MethodInfo method)			        where T : Attribute
        {
            return method.attributes().attributes<T>();
        }		
        public static List<T>                   attributes<T>(this List<MethodInfo> methods)			where T : Attribute
        {
            var attributes = new List<T>();
            foreach(var method in  methods)
                foreach (var attribute in method.attributes<T>())
                    attributes.add(attribute);
            return attributes;
        }

        public static List<T>                   attributes<T>(this List<Attribute> attributes)			where T : Attribute
        {
            return (from attribute in attributes
                    where attribute is T
                    select (T)attribute).toList();
        }						
        public static List<Attribute>           attributes(this List<MethodInfo> methods, string name)
        {
            return methods.attributes().withName(name);
        }		
        public static Attribute                 attribute(this MethodInfo methodInfo, string name)
        {
            return methodInfo.attributes().FirstOrDefault(attribute => attribute.name() == name);
        }

        public static T                         attribute<T>(this MethodInfo methodInfo)			where T : Attribute
        {
            return methodInfo.attributes<T>().first();			
        }		
        public static List<MethodInfo>          methodsWithAttribute<T>(this Assembly assembly)			where T : Attribute
        {
            return assembly.methods().withAttribute<T>();
        }		
        public static List<MethodInfo>          withAttribute(this Assembly assembly, string attributeName)
        {
            return assembly.methods().withAttribute(attributeName);
        }		
        public static List<MethodInfo>          withAttribute(this List<MethodInfo> methods, string attributeName)
        { 
            return (from method in methods 
                    from attribute in method.attributes()
                    let type = attribute.TypeId as Type
                    where type != null && attributeName == type.Name.remove("Attribute")
                    select method).toList();						
        }				
        public static List<MethodInfo>          withAttribute<T>(this List<MethodInfo> methods)			where T : Attribute
        {
            return (from method in methods 
                    from attribute in method.attributes()		  
                    where attribute is T
                    select method).toList();						
        }		        
        public static List<Attribute>           withName(this List<Attribute> attributes, string name)
        {
            return (from attribute in attributes
                    where attribute.name() == name
                    select attribute).toList();
        }
        public static string                    name(this Attribute attribute)
        {
            return attribute.typeName().remove("Attribute");
        }		
        public static List<string>              names(this List<Attribute> attributes)
        {
            return (from attribute in attributes
                    select attribute.name()).toList();
        }		
        
    }
    
    public static class Reflection_ExtensionMethods_WebServices_SOAP
    {
        public static List<MethodInfo> webService_SoapMethods(this Assembly assembly)
        {
            var soapMethods = new List<MethodInfo >(); 
            foreach(var type in assembly.types())
                soapMethods.AddRange(type.webService_SoapMethods());
            return soapMethods;
                    
        }
        public static List<MethodInfo> webService_SoapMethods(this object _object)
        {
            Type type = (_object is Type) 	
                            ? (Type)_object
                            : _object.type();
            return (from   method    in type.methods()
                    from   attribute in method.attributes()
                    where  attribute.typeFullName() == "System.Web.Services.Protocols.SoapDocumentMethodAttribute" || 
                           attribute.typeFullName() == "System.Web.Services.Protocols.SoapRpcMethodAttribute"
                    select method).ToList();
        }
        
        public static Items property_Values_AsStrings(this object _object)
        {		
            var propertyValues_AsStrings = new Items();
            foreach(var property in _object.type().properties())				
                propertyValues_AsStrings.add(property.Name.str(), _object.property(property.Name).str());
            return propertyValues_AsStrings;
        }				
    }		

    public static class Reflection_ExtensionMethods_DynamicallyLoadingAssemblies
    {
        public static Assembly resolveAssembly(this string nameOrPath)
        {
            return nameOrPath.resolveAssembly((nameToResolve)=> nameToResolve);
        }		
        public static Assembly resolveAssembly(this string name, string resolvedPath)
        {
            return name.resolveAssembly((nameToResolve)=> resolvedPath);
        }		
        public static Assembly resolveAssembly(this string nameOrPath, Func<string,string> resolveName)
        {
            ResolveEventHandler assemblyResolve =  
                (sender, args)=>{						
                                    var name = args.prop("Name").str();
                                    //"[AssemblyResolve] for name: {0}".debug(name);
                                    var location = resolveName(name);						
                                    if (location.valid())
                                    { 								
                                        //"[AssemblyResolve] found location: {0}".info(location);
                                        var assembly = Assembly.Load(location.fileContents_AsByteArray());
                                        if (assembly.notNull())
                                        {										
                                            //"[AssemblyResolve] loaded Assembly: {0}".info(assembly.FullName);
                                            return assembly;
                                        }                                        
                                        "[AssemblyResolve] failed to load Assembly from location: {0}".error(location);
                                    }
                                    else
                                        "[AssemblyResolve] could not find a location for assembly with name: {0}".error(name);
                                    return null;
                                };
             
            Func<string,Assembly> loadAssembly = 
                (assemblyToLoad)=>{
                                        AppDomain.CurrentDomain.AssemblyResolve += assemblyResolve;
                                        var assembly = assemblyToLoad.assembly();
                                        AppDomain.CurrentDomain.AssemblyResolve -= assemblyResolve;
                                        return assembly;
                                  };
            return loadAssembly(nameOrPath);
        }				
        public static Assembly loadAssemblyAndAllItsDependencies(this string pathToAssemblyToLoad) 	
        {
            var referencesFolder = pathToAssemblyToLoad.directoryName();
            var referencesFiles = referencesFolder.files(true,"*.dll", "*.exe");
            
            Func<string,string> resolveAssemblyName = 
                (name)=>{   			
                            if (name.starts("System"))
                                return name; 
                            if(name.isAssemblyName())		 		
                                name = name.assemblyName().name();  										
                            
                            var resolvedPath = referencesFiles.find_File_in_List(name, name+ ".dll", name+ ".exe");					
                            
                            if(resolvedPath.fileExists())
                            {
                                //"**** Found match:{0}".info(resolvedPath);	 
                                return resolvedPath;
                            }				 
                            
                            //"**** Couldn't match:{0}".error(resolvedPath);	
                            return null;  
                        };
            
            
            var loadedAssemblies = new Dictionary<string, Assembly>();
            // ReSharper disable ImplicitlyCapturedClosure
            // ReSharper disable AccessToModifiedClosure
            Action<Assembly> loadReferencedAssemblies = (assembly) => { };
            Func<string, Assembly> loadAssembly = null;
            loadAssembly = 
                (assemblyPathOrName) => {
                                            if (loadedAssemblies.hasKey(assemblyPathOrName))
                                                return loadedAssemblies[assemblyPathOrName];
                                            var assembly = assemblyPathOrName.resolveAssembly(resolveAssemblyName);											
                                            if(assembly.notNull())
                                            {
                                                loadedAssemblies.add(assemblyPathOrName, assembly);                                                
                                                loadReferencedAssemblies(assembly); 												                                                
                                                if (assembly.Location.valid().isFalse())
                                                {
                                                    loadAssembly(assembly.FullName.assemblyName().name());
                                                    loadAssembly(assembly.ManifestModule.Name != "<Unknown>"
                                                                     ? assembly.ManifestModule.Name
                                                                     : assembly.ManifestModule.ScopeName);
                                                }
                                                //loadAssembly(assembly.ManifestModule.Name);
                                                 
                                            } 
                                            return assembly;
                                        };
                                        
            loadReferencedAssemblies = 
                (assembly)=>{
                                var referencedAssemblies =  assembly.referencedAssemblies();								
                                foreach(var referencedAssembly in referencedAssemblies)
                                {									
                                    var assemblyName = referencedAssembly.str();																																				
                                        if (loadAssembly(assemblyName).isNull())
                                            "COULD NOT LOAD Referenced Assembly: {0}".error(assemblyName);
                                }
                            };										
            // ReSharper restore ImplicitlyCapturedClosure
            // ReSharper restore AccessToModifiedClosure                                                
            var mainAssembly = loadAssembly(pathToAssemblyToLoad);
        
            "[loadAssemblyAndAllItsDependencies] there were {0} references loaded/mapped from '{1}'".info(loadedAssemblies.size(), pathToAssemblyToLoad);
            //show.info(loadedAssemblies);			
            
            return mainAssembly;
        }		
    }

    public static class Reflection_ExtensionMethods_Resources
    {
        public static List<String>  embeddedResourceNames(this Assembly assembly)
        {
            return assembly.GetManifestResourceNames().toList();
        }
        public static List<String>  embeddedAssembliesNames(this Assembly assembly)
        {
            return assembly.GetManifestResourceNames().toList()
                                                      .where((name)=>name.ends(new [] {".dll",".exe",".dll.gz",".exe.gz"}));
        }
        public static byte[]        embeddedResource(this Assembly assembly, string name)
        {            
            if (assembly.isNull())
                return null;
            var assemblyStream = assembly.GetManifestResourceStream(name);
            var bytes = assemblyStream.isNull() 
                            ? null 
                            : new BinaryReader(assemblyStream).ReadBytes((int) assemblyStream.Length);
            return name.contains(".gz") 
                            ? bytes.gzip_Decompress() 
                            : bytes;
        }
    }

}
