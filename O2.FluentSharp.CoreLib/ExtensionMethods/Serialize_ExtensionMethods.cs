using System;
using FluentSharp.CoreLib.API;



namespace FluentSharp.CoreLib
{
    public static class Serialize_ExtensionMethods
    {
        public static string    serialize(this object _object)
        {
            var tempFile = PublicDI.config.getTempFileInTempDirectory(".xml");
            if (_object.serialize(tempFile))
                return tempFile;
            return "";
        }
        public static bool      serialize(this Object _object, string file)
        {
            return Serialize.createSerializedXmlFileFromObject(_object, file);
        }
        public static string    serialize(this object _object, bool serializeToFile)
        {
            if (serializeToFile)
                return _object.serialize();
            return Serialize.createSerializedXmlStringFromObject(_object);
        }
        public static string    save(this object _object)
        {
            return _object.serialize();
        }
        public static bool      saveAs(this object _object, string pathToSave)
        {
            return _object.serialize(pathToSave);
        }
        public static string    toXml(this object _object)
        {
            return _object.serialize(false);
        }
    }

    public static class DeSerialize_ExtensionMethods
    { 
        public static T deserialize<T>(this string file)
        {
            return (T)Serialize.getDeSerializedObjectFromXmlFile(file, typeof(T));
        }
        public static T deserialize<T>(this string _string, bool fromDisk)
        {
            if (fromDisk && _string.fileExists())
                return _string.deserialize<T>();
            
            return (T)Serialize.getDeSerializedObjectFromString(_string, typeof(T));  
        }
        public static T load<T>(this string pathToSerializedObject)
        {
            return pathToSerializedObject.deserialize<T>();
        }
    }
}
