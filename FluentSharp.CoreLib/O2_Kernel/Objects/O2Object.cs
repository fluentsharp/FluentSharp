// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System.Reflection;

namespace FluentSharp.CoreLib.API
{
    public class O2Object
    {
        public O2Object(Assembly assembly, string objectType, object[] constructorArguments)
        {
            Assembly = assembly;
            Obj = PublicDI.reflection.createObject(Assembly, PublicDI.reflection.getType(Assembly, objectType),
                                             PublicDI.reflection.getRealObjects(constructorArguments));
        }

        public O2Object(Assembly assembly, object obj)
        {
            Assembly = assembly;
            Obj = obj;
        }

        public Assembly Assembly { get; set; }

        public object Obj { get; set; }

        public O2Object this[string objectToGet]
        {
            get
            {
                return null;
                //   return ctor(typeToCreate); 
            }
        }

        public object call(string methodName)
        {
            //return Reflection.invokeMethod_InstanceStaticPublicNonPublic(Obj, methodName, null);
            return call(methodName, new object[] {});
        }

        public object call(string methodName, object[] methodParameters)
        {
            methodParameters = PublicDI.reflection.getRealObjects(methodParameters);
            MethodInfo methodInfo = PublicDI.reflection.getMethod(Obj.GetType(), methodName, methodParameters);
            return PublicDI.reflection.invoke(Obj, methodInfo, methodParameters);


            //methodInfo.Invoke(Obj, methodParameters);
            //return Reflection.invokeMethod_InstanceStaticPublicNonPublic(Obj, methodName, methodParameters);
        }

        public static O2Object ctor(Assembly assembly, string typeOfObjectToCreate)
        {
            return ctor(assembly, typeOfObjectToCreate, new object[] {});
        }

        public static O2Object ctor(Assembly assembly, string typeOfObjectToCreate, object[] constructorArguments)
        {
            var o2Object = new O2Object(assembly, typeOfObjectToCreate,
                                        PublicDI.reflection.getRealObjects(constructorArguments));
            return (o2Object.Obj != null) ? o2Object : null;
        }

        public O2Object get(string propertyToGet)
        {
            object value = PublicDI.reflection.getProperty(propertyToGet, Obj);
            if (value != null)
                return new O2Object(Assembly, value);
            return null;
        }
    }
}
