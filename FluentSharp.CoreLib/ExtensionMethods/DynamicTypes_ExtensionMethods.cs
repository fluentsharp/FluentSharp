using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;

namespace FluentSharp.CoreLib
{
    public static class DynamicTypes_ExtensionMethods
    {
        //AssemblyBuilder
        public static AssemblyBuilder assemblyBuilder(this string assemblyName)
        {
            return assemblyName.assemblyBuilder(AppDomain.CurrentDomain);
        }

        public static AssemblyBuilder assemblyBuilder(this string assemblyName, AppDomain appDomain)
        {
            return appDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
        }

        public static AssemblyBuilder assemblyBuilder_forSave(this string assemblyName, string targetDir)
        {
            return assemblyName.assemblyBuilder_forSave(AppDomain.CurrentDomain, targetDir);
        }

        public static AssemblyBuilder assemblyBuilder_forSave(this string assemblyName, AppDomain appDomain, string targetDir)
        {
            return appDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndSave, targetDir);
        }

        //ModuleBuilder
        public static ModuleBuilder dynamicModule(this string assemblyName)
        {
            var moduleName = "DynamicModule_{0}".format(assemblyName);
            return assemblyName.assemblyBuilder()
                               .dynamicModule(moduleName);
        }

        public static ModuleBuilder dynamicModule(this AssemblyBuilder assemblyBuilder)
        {
            return assemblyBuilder.dynamicModule("DynamicModule");
        }

        public static ModuleBuilder dynamicModule(this AssemblyBuilder assemblyBuilder, string moduleName)
        {
            return assemblyBuilder.DefineDynamicModule(moduleName);
        }

        //TypeBuilder
        public static TypeBuilder dynamicType(this string typeName)
        {
            var assemblyName = "DynamicAssembly_{0}".format(typeName);
            var moduleName = "DynamicModule_{0}".format(typeName);

            return assemblyName.assemblyBuilder()
                               .dynamicModule(moduleName)
                               .dynamicType(typeName);
        }

        public static TypeBuilder dynamicType(this ModuleBuilder moduleBuilder, string typeName)
        {
            return moduleBuilder.DefineType(typeName, TypeAttributes.Public);
        }

        //PropertyBuilder
        public static PropertyBuilder dynamicProperty<T>(this TypeBuilder typeBuilder, string propertyName)
        {
            return typeBuilder.dynamicProperty(propertyName, typeof(T));
        }

        public static PropertyBuilder dynamicProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            return typeBuilder.dynamicProperty(propertyName, propertyType, PropertyAttributes.None);
        }

        public static PropertyBuilder dynamicProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType, PropertyAttributes propertyAttributes)
        {
            var property = typeBuilder.DefineProperty(propertyName,
                                                       propertyAttributes,
                                                       propertyType,
                                                       new [] { propertyType });

            var field = typeBuilder.dynamicField_Private("_" + propertyName, propertyType);

            var currGetPropMthdBldr = typeBuilder.dynamicMethod("get_value", propertyType);

            currGetPropMthdBldr.il_get_field(field)
                               .il_ret();

            var currSetPropMthdBldr = typeBuilder.dynamicMethod("set_value", null, propertyType);
            currSetPropMthdBldr.il_set_field(field)
                               .il_ret();

            property.SetGetMethod(currGetPropMthdBldr);
            property.SetSetMethod(currSetPropMthdBldr);

            return property;
        }

        //FieldBuilder
        public static FieldBuilder dynamicField<T>(this TypeBuilder typeBuilder, string fieldName)
        {
            return typeBuilder.dynamicField(fieldName, typeof(T));
        }

        public static FieldBuilder dynamicField(this TypeBuilder typeBuilder, string fieldName, Type fieldType)
        {
            return typeBuilder.dynamicField(fieldName, fieldType, FieldAttributes.Public);
        }

        public static FieldBuilder dynamicField_Private(this TypeBuilder typeBuilder, string fieldName, Type fieldType)
        {
            return typeBuilder.dynamicField(fieldName, fieldType, FieldAttributes.Private);
        }

        public static FieldBuilder dynamicField(this TypeBuilder typeBuilder, string fieldName, Type fieldType, FieldAttributes fieldAttributes)
        {
            return typeBuilder.DefineField(fieldName, fieldType, fieldAttributes);
        }

        //MethodBuilder
        public static MethodBuilder dynamicMethod<T>(this TypeBuilder typeBuilder, string methodName)
        {
            return typeBuilder.dynamicMethod(methodName, typeof(T));
        }

        /*public static MethodBuilder dynamicMethod(this TypeBuilder typeBuilder, string methodName, Type returnType)
        {						
            return typeBuilder.dynamicMethod(methodName, returnType,  Type.EmptyTypes);
        }*/
        public static MethodBuilder dynamicMethod(this TypeBuilder typeBuilder, string methodName)
        {
            return typeBuilder.dynamicMethod(methodName, null);
        }

        public static MethodBuilder dynamicMethod(this TypeBuilder typeBuilder, string methodName, Type returnType, params Type[] parameterTypes)
        {
            var methodAttributes = MethodAttributes.Public | MethodAttributes.HideBySig;
            return typeBuilder.dynamicMethod(methodName, methodAttributes, returnType, parameterTypes ?? Type.EmptyTypes);
        }

        /*public static MethodBuilder dynamicMethod(this TypeBuilder typeBuilder, string methodName, MethodAttributes methodAttributes, Type returnType)
        {
            return typeBuilder.dynamicMethod(methodName, methodAttributes, returnType, Type.EmptyTypes);
        }*/

        public static MethodBuilder dynamicMethod(this TypeBuilder typeBuilder, string methodName, MethodAttributes methodAttributes, Type returnType, params Type[] parameterTypes)
        {
            return typeBuilder.DefineMethod(methodName, methodAttributes, returnType, parameterTypes);
        }

        // Reflection Type
        public static Type create(this TypeBuilder typeBuilder)
        {
            try
            {
                return typeBuilder.CreateType();
            }
            catch (Exception ex)
            {
                "Error in Type create(this TypeBuilder typeBuilder): {0}".error(ex.Message);
                return null;
            }
        }

        public static object create_Object(this TypeBuilder typeBuilder)
        {
            return typeBuilder.create().ctor();
        }

        public static List<object> create_List(this TypeBuilder typeBuilder)
        {
            var typeObject = typeBuilder.create_Object();
            var list = typeObject.wrapOnList();
            list.Clear();
            return list;
        }
        //pageData_Type.ctor().wrapOnList().Clear(); 

        //IL Generation Helpers
        public static MethodBuilder il_get_field(this MethodBuilder methodBuilder, FieldBuilder fieldBuilder)
        {
            methodBuilder.il()
                         .ldarg0()
                         .ldfld(fieldBuilder);
            return methodBuilder;
        }

        public static MethodBuilder il_set_field(this MethodBuilder methodBuilder, FieldBuilder fieldBuilder)
        {
            methodBuilder.il()
                         .ldarg0()
                         .ldarg1()
                         .stfld(fieldBuilder);
            return methodBuilder;
        }

        public static MethodBuilder il_ret(this MethodBuilder methodBuilder)
        {
            methodBuilder.il().ret();
            return methodBuilder;
        }

    }

    public static class DynamicMethods_Opcodes_ExtensionMethods
    {
        public static ILGenerator il(this MethodBuilder methodBuilder)
        {
            return methodBuilder.ilGenerator();
        }

        public static ILGenerator ilEmit(this MethodBuilder methodBuilder)
        {
            return methodBuilder.ilGenerator();
        }

        public static ILGenerator ilGenerator(this MethodBuilder methodBuilder)
        {
            return methodBuilder.GetILGenerator();
        }

        public static ILGenerator ldarg0(this ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ldarg_0);
            return ilGenerator;
        }

        public static ILGenerator ldarg1(this ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ldarg_1);
            return ilGenerator;
        }

        public static ILGenerator ldfld(this ILGenerator ilGenerator, FieldBuilder fieldBuilder)
        {
            ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            return ilGenerator;
        }

        public static ILGenerator stfld(this ILGenerator ilGenerator, FieldBuilder fieldBuilder)
        {
            ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
            return ilGenerator;
        }


        public static ILGenerator ret(this ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ret);
            return ilGenerator;
        }
    }

    public static class add_Helpers_ExtensionMethods
    {
        public static List<PropertyBuilder> add_Properties(this TypeBuilder typeBuilder, params string[] propertyNames)
        {
            return typeBuilder.add_Properties<string>(propertyNames);
        }

        public static List<PropertyBuilder> add_Properties<T>(this TypeBuilder typeBuilder, params string[] propertyNames)
        {
            var properties = new List<PropertyBuilder>();
            foreach (var propertyName in propertyNames)
                properties.Add(typeBuilder.add_Property(propertyName, typeof(T)));
            return properties;
        }
        public static PropertyBuilder add_Property<T>(this TypeBuilder typeBuilder, string propertyName)
        {
            return typeBuilder.add_Property(propertyName, typeof(T));
        }

        public static PropertyBuilder add_Property(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            return typeBuilder.dynamicProperty(propertyName, propertyType);
        }

        public static object create_LiveObject_From_MethodInfo_Parameters(this MethodInfo methodInfo)
        {
            return methodInfo.create_LiveObject_From_MethodInfo_Parameters("Dynamic_Type_From_Parameters");
        }

        public static object create_LiveObject_From_MethodInfo_Parameters(this MethodInfo methodInfo, string dynamicTypeName)
        {
            var dynamicType = dynamicTypeName.dynamicType();
            foreach (var parameter in methodInfo.parameters())
                dynamicType.add_Property(parameter.Name, parameter.ParameterType);
            var liveObject = dynamicType.create().ctor();
            return liveObject;
        }

        public static object[] getProperties_AsArray(this object _object)
        {
            var properties = new List<object>();
            foreach (var property in _object.type().properties())
                properties.add(_object.property(property.Name));
            return properties.ToArray();
        }

        public static List<string> getProperties_AsStringList(this object _object)
        {
            return (from item in _object.getProperties_AsArray()
                    select item.str()).toList();
        }

    }
}
