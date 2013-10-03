using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.NUnit;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib.ExtensionMethods
{    
    public class Test_Reflection : NUnitTests
    {
        [Test(Description = "Creates a new instance of an type")]
        public void ctor()
        {
            var type      = typeof(StringBuilder); 
            var typeName = "StringBuilder";
            var assembly  = "mscorlib.dll";
            var firstParam = "content";
            var secondParam = 20;

            var ctorObject1 = typeName.ctor(assembly, firstParam, secondParam);
            var ctorObject2 = type.ctor(firstParam, secondParam);
            var ctorObject3 = type.ctor<StringBuilder>(firstParam, secondParam);

            PublicDI.reflection.verbose = true;

            Assert.IsNotNull(ctorObject1);
            Assert.IsNotNull(ctorObject2);
            Assert.IsNotNull(ctorObject3);

            Assert.IsInstanceOf<StringBuilder>(ctorObject1);
            Assert.IsInstanceOf<StringBuilder>(ctorObject2);
            Assert.IsInstanceOf<StringBuilder>(ctorObject3);

            Assert.IsNotNull(typeName.ctor(assembly));
            Assert.IsNotNull(type.ctor());
            Assert.IsNotNull(type.ctor<StringBuilder>());
            

            //check exception handling            
            Assert.IsNull(type.ctor<Encoder>());
            Assert.IsNull(type.ctor<StringBuilder>(null,null));
            Assert.IsNull(typeName.ctor(""));
            Assert.IsNull(typeName.ctor(null));
            Assert.IsNull(typeName.ctor("system.dll"));
            Assert.IsNull("XmlDocument".ctor(assembly));
            Assert.IsNull((null as string).ctor(assembly));
            PublicDI.reflection.verbose = false;
        }

        [Test(Description = "Appends to provided string the Assembly version from from GetCallingAssembly, in format x.x.x.x (Major.MajorRevision.Minor.MinorRevision)")]
        public void append_Version_Calling_Assembly()
        {
            var originalString  = "aaaa".add_RandomLetters(10);
            var assembly        = Assembly.GetExecutingAssembly();
            var version         = assembly.version();
            var expected        = originalString + "1.0.0.0";
            var result          = originalString.append_Version_Calling_Assembly();

            assert_Are_Equal(result, expected);
            assert_Are_Equal(result, originalString + version);
            assert_Are_Equal((null as string).append_Version_Calling_Assembly(), version);
        }

        [Test(Description = "Appends to provided string the Assembly version from FluentSharp.CoreLib, in format x.x.x.x (Major.MajorRevision.Minor.MinorRevision)")]
        public void append_Version_FluentSharp()
        {
            var originalString  = "aaaa".add_RandomLetters(10);
            var assembly        = typeof(Files).assembly();
            var version         = assembly.version();            
            var result          = originalString.append_Version_FluentSharp();
            
            assert_Are_Equal(result, originalString + version);
            assert_Are_Equal((null as string).append_Version_FluentSharp(), version);
        }

        [Test(Description = "Appends to provided string the Assembly version from FluentSharp.CoreLib, in format x.x (Major.MajorRevision)")]
        public void append_Version_FluentSharp_Short()
        {
            var originalString  = "aaaa".add_RandomLetters(10);
            var assembly        = typeof(Files).assembly();
            var version         = assembly.version_Short();            
            var result          = originalString.append_Version_FluentSharp_Short();
            
            assert_Are_Equal(result, originalString + version);
            assert_Are_Equal((null as string).append_Version_FluentSharp_Short(), version);
        }

        [Test(Description = "returns the version number of the provided assembly in format x.x.x.x (Major.MajorRevision.Minor.MinorRevision)")]
        public void version()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();            
            assert_Are_Equal(currentAssembly   .version(), "1.0.0.0");
            assert_Are_Equal((null as Assembly).version(), "");
        }

        [Test(Description = "returns the version number of the provided assembly in format x.x.x.x (Major.MajorRevision")]
        public void version_Short()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();            
            assert_Are_Equal(currentAssembly   .version_Short(), "1.0");
            assert_Are_Equal((null as Assembly).version_Short(), "");
        }
    }

    public class Test_Reflection_Types : NUnitTests
    {
         [Test(Description="returns all base Types of a Type")]
         public void baseTypes()
         {
            var baseTypes_String        = "".type().baseTypes();            
            var baseTypes_Type          = typeof(Type).baseTypes();
            var baseTypes_Action        = typeof(Action).baseTypes();
            var baseTypes_RuntimeType   = 42.type().type().baseTypes();
            assert_Size_Is(baseTypes_String     , 1);
            assert_Size_Is(baseTypes_Type       , 2);
            assert_Size_Is(baseTypes_Action     , 3);            
            assert_Size_Is(baseTypes_RuntimeType, 4);            

            assert_Are_Equal(baseTypes_String     .first().name(), "Object");
            assert_Are_Equal(baseTypes_Type       .first().name(), "MemberInfo");
            assert_Are_Equal(baseTypes_Action     .first().name(), "MulticastDelegate");
            assert_Are_Equal(baseTypes_RuntimeType.first().name(), "TypeInfo");
        }
        [Test(Description="shows the object's type name in the info log")]
        public void infoTypeName()
        {
            var log = PublicDI.log;
            var orginalLogRedirection = log.LogRedirectionTarget;
            assert_Are_Equal    (orginalLogRedirection.type(), typeof(Logger_DiagnosticsDebug));

            var memoryLogger = log.redirectTo_Memory();             // redirect log to memory
            var logBefore    = memoryLogger.LogData.str();
            logBefore.infoTypeName();
            (null as Type).infoTypeName();
            var logAfter     = memoryLogger.LogData.str();
            log.LogRedirectionTarget = orginalLogRedirection;       // restore logger

            assert_Are_Equal    (logBefore,"");
            assert_Are_Equal    (logAfter,"INFO: String".line().add("ERROR: in infoTypeName _object was null".line()));

            
            assert_Are_Equal    (log.LogRedirectionTarget.type(), typeof(Logger_DiagnosticsDebug));

            assert_Are_Equal( logBefore.infoTypeName(), logBefore);
            assert_Are_Equal( orginalLogRedirection.infoTypeName(), orginalLogRedirection);
            assert_Is_Null  ((null as Type).infoTypeName());            
        }

        [Test(Description="returns the FullName of a Type object")]
        public void fullName()
        {
            assert_Are_Equal  ("".type()     .fullName(),"System.String");
            assert_Are_Equal  ((null as Type).fullName(), "");
            assert_Is_Not_Null((null as Type).fullName());
        }

        [Test(Description="returns the Name of a Type object")]
        public void name()
        {
            assert_Are_Equal  ("".type(     ).name(),"String");
            assert_Are_Equal  ((null as Type).name(), "");
            assert_Is_Not_Null((null as Type).name());
        }

        [Test(Description="returns a list of Names from a list of Types")]
        public void names()
        {
            var types = new List<Type> {"".type(), 42.type()};
            var names = types.names();
            assert_Are_Equal(types.size(),2);
            assert_Are_Equal(names.size(),2);

            assert_Are_Equal  (names.first(), "String");
            assert_Are_Equal  (names.second(), "Int32");
            assert_Is_Not_Null(new List<Type>().names());
            assert_Is_Empty   (new List<Type>().names());            
            assert_Is_Empty  ((null as List<Type>).names());
            
        }

        [Test(Description="returns the Type of an object")]
        public void type()
        {
            //type(this Assembly assembly, string typeName)
            var assembly = "".type().assembly();
            assert_Are_Equal(assembly.type("String"    ),typeof(string));            
            assert_Are_Equal(assembly.type("Int32"    ),typeof(int));            
            assert_Is_Null  (assembly.type("StringAAAA"));
            assert_Is_Null  (assembly.type("BBBBBBBBBB"));
            assert_Is_Null  (assembly.type(""));
            assert_Is_Null  (assembly.type(null));
            assert_Is_Null  ((null as Assembly).type(""));

            //type(this string assemblyName, string typeName)
            var assemblyName = "".type().assembly().name();
            assert_Are_Equal(assemblyName.type("String"    ),typeof(string));            
            assert_Are_Equal(assemblyName.type("Int32"    ),typeof(int));            
            assert_Is_Null  (assemblyName.type("StringAAAA"));
            assert_Is_Null  (assemblyName.type("BBBBBBBBBB"));
            assert_Is_Null  (assemblyName.type(""));
            assert_Is_Null  (assemblyName.type(null));
            assert_Is_Null  ((null as string).type(""));

            //type(this object _object)
            assert_Are_Equal("".type(     ),typeof(string));            
            assert_Is_Null  ((null as Type).type());
        }
        [Test(Description="returns the Type FullName of an object")]
        public void typeFullName()
        {
            assert_Are_Equal  (""            .typeFullName(),"System.String");
            assert_Are_Equal  ("".type()     .typeFullName(),"System.RuntimeType");
            assert_Are_Equal  ((null as Type).typeFullName(), "");
            assert_Is_Not_Null((null as Type).typeFullName());
        }

        [Test(Description="returns the Type Name of an object")]
        public void typeName()
        {
            assert_Are_Equal  (""            .typeName(),"String");
            assert_Are_Equal  ("".type()     .typeName(),"RuntimeType");
            assert_Are_Equal  ((null as Type).typeName(), "");
            assert_Is_Not_Null((null as Type).typeName());
        }
        [Test(Description="returns all Type Names in an assembly")]
        public void typesNames()
        {
            var assembly  = "".type().assembly();
            var types     = assembly.types();
            var typeNames = assembly.typesNames();
            assert_Is_Not_Empty(typeNames);
            assert_Size_Is     (typeNames,types.size());
            assert_Are_Equal   (typeNames.first() , types.first().name());
            assert_Are_Equal   (typeNames.second(), types.second().name());
            
            assert_Is_Not_Null((null as Assembly).typesNames());
        }

        [Test(Description="returns the Types in an Assembly, Type or List of of Objects")]
        public void types()
        {
             //types(this Assembly assembly)
            var assembly = "".type().assembly();        
            var types = assembly.types();       

            assert_Are_Equal  (assembly.name(),"mscorlib"); // assumes that mscorlib as 3017 types
            assert_Are_Equal  (types.size(),3017);        
            assert_Is_Empty   ((null as Assembly).types());
            assert_Is_Not_Null((null as Assembly).types());

            //types(this Type type)
            var type = typeof(TimeZoneInfo);
            assert_Are_Equal  (type.types().size(),2);   
            assert_Is_Empty   ((null as Type).types());
            assert_Is_Not_Null((null as Type).types());

            //types<T>(this List<T> list)
            var list = new List<Object> { "", 42, typeof(String),null, typeof(Int32)};
            var listTypes = list.types();
            assert_Are_Equal  (listTypes.size(),5);   
            assert_Are_Equal  (listTypes.first() .name(), "String");
            assert_Are_Equal  (listTypes.second().name(), "Int32" );
            assert_Are_Equal  (listTypes.third() .name(), "RuntimeType"); 
            assert_Are_Equal  (listTypes.fourth().name(), ""); 
            assert_Are_Equal  (listTypes.fifth() .name(), "RuntimeType"); 
            assert_Is_Empty   ((null as List<Object>).types());
            assert_Is_Not_Null((null as List<Object>).types());
        }

        [Test(Description="returns all Types with a particular BaseType")]
        public void withBaseType()
        {
            var assembly = "".type().assembly();
            var withBaseTypes = assembly.withBaseType<MulticastDelegate>();
            assert_Is_Not_Empty(withBaseTypes);
            assert_Size_Is     (withBaseTypes,71);
            assert_Are_Equal   (withBaseTypes.first().name(), "Action`1");
        }
    }
}
