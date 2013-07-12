// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.Reflection;

namespace FluentSharp.CoreLib.API
{
    public class O2ObjectFactory
    {
        public O2ObjectFactory(string assemblyFile)
        {
            Assembly = PublicDI.reflection.getAssembly(assemblyFile);
        }

        public O2ObjectFactory(Assembly assembly)
        {
            Assembly = assembly;
        }

        public Assembly Assembly { get; set; }


        public O2Object this[string typeToCreate]
        {
            get { return ctor(typeToCreate); }
        }

        public O2Object ctor(string typeToCreate)
        {
            return ctor(typeToCreate, new object[] {});
        }

        public O2Object ctor(string typeToCreate, object[] constructorArguments)
        {
            var o2Object = new O2Object(Assembly, typeToCreate, PublicDI.reflection.getRealObjects(constructorArguments));
            if (o2Object.Obj != null)
                return o2Object;

            return null;
        }


        public object exec(string typeToCreate, string method, object[] methodParameters)
        {
            //O2Object typeObj = O2Object.ctor(assembly, type);
            return null;
        }

        public O2Object staticTypeGetProperty(string staticTypeName, string property)
        {
            Type staticType = PublicDI.reflection.getType(Assembly, staticTypeName);
            if (staticType != null)
            {
                object obj = PublicDI.reflection.invokeMethod_Static(staticType, "get_" + property, null);
                if (obj != null)
                    return new O2Object(Assembly, obj);
            }
            return null;
        }

        public object call(string staticTypeName, string method, object[] methodParameters)
        {
            methodParameters = PublicDI.reflection.getRealObjects(methodParameters);

            Type staticType = PublicDI.reflection.getType(Assembly, staticTypeName);
            MethodInfo methodInfo = PublicDI.reflection.getMethod(staticType, method, methodParameters);
            methodInfo.Invoke(null, methodParameters);
            return null;
        }
    }
}
