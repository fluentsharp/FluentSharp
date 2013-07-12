using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentSharp.CoreLib.API;

namespace FluentSharp.CoreLib
{
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
}