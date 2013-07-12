
using System.Reflection.Emit;

namespace FluentSharp.CoreLib.API
{
    public class TypeConfusionBuilder
    {
        public static string create_DLL_TO_castViaTypeConfusion()
        {
            var assemblyName = "TypeConfusion";
            var dllName = assemblyName + ".dll";            
            var targetDir = "".tempDir(false);
            var targetFile = targetDir.pathCombine(dllName);
            if (targetFile.fileExists())
                return targetFile;

            var assemblyBuilder = assemblyName.assemblyBuilder_forSave(targetDir);  // use this if wanting to Save the assembly created
            var moduleBuilder = assemblyBuilder.dynamicModule(dllName);
            var typeBuilder = moduleBuilder.dynamicType(assemblyName);

            var methodBuilder = typeBuilder.dynamicMethod("castObjectIntoType", null, typeof(object));

            var genericParameters = methodBuilder.DefineGenericParameters("T");
            var returnType = genericParameters[0];
            methodBuilder.SetReturnType(returnType);

            var ilGenerator = methodBuilder.il();
            ilGenerator.DeclareLocal(typeof(object));
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.ret();            

            assemblyBuilder.Save(dllName);
            return targetFile;
        }
    }


    public static class TypeConfusion_ExtensionMethods
    {

        /*
		//this one doesn't work directly (but was the bases for the create_DLL_TO_castViaTypeConfusion method
        public static T castViaTypeConfusion<T>(this object _objectToCast)
        {
            var assemblyName = "TypeConfusion";
            var dllName = assemblyName + ".dll";
            var appDomain = AppDomain.CurrentDomain;
            var assemblyBuilder = assemblyName.assemblyBuilder();
            //var assemblyBuilder = assemblyName.assemblyBuilder_forSave(targetDir);  // use this if wanting to Save the assembly created
            var moduleBuilder = assemblyBuilder.dynamicModule(dllName);
            var typeBuilder = moduleBuilder.dynamicType(assemblyName);

            var methodBuilder = typeBuilder.dynamicMethod("castObjectIntoType", null, typeof(object));

            var genericParameters = methodBuilder.DefineGenericParameters("T");
            var returnType = genericParameters[0];
            methodBuilder.SetReturnType(returnType);

            var ilGenerator = methodBuilder.il();
            ilGenerator.DeclareLocal(typeof(object));
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stloc_0);
            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.ret();

            var type = typeBuilder.create();

            //assemblyBuilder.Save(dllName);

            var liveObject = type.ctor();
            var method = type.method("castObjectIntoType");
            var generic = method.MakeGenericMethod(typeof(T));

            return (T)generic.Invoke(liveObject, new object[] { _objectToCast });
        }*/
    }
}
