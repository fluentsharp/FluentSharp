// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;



namespace FluentSharp.CoreLib.API
{
    public class Serialize
    {
        public static string createSerializedXmlFileFromObject(Object oObjectToProcess)
        {
            var tempFile = PublicDI.config.getTempFileInTempDirectory("xml");
            if(createSerializedXmlFileFromObject(oObjectToProcess, tempFile))
                return tempFile;
            return "";
        }

        public static bool createSerializedXmlFileFromObject(Object oObjectToProcess, String sTargetFile)
        {
            return createSerializedXmlFileFromObject(oObjectToProcess, sTargetFile, null);
        }
        
        public static bool createSerializedXmlFileFromObject(Object oObjectToProcess, String sTargetFile, Type[] extraTypes)
        {
            FileStream fileStream = null;
            try
            {
                sTargetFile.file_WaitFor_CanOpen();
                var xnsXmlSerializerNamespaces = new XmlSerializerNamespaces();
                xnsXmlSerializerNamespaces.Add("", "");
                var xsXmlSerializer = (extraTypes != null)
                                          ? new XmlSerializer(oObjectToProcess.GetType(), extraTypes)
                                          : new XmlSerializer(oObjectToProcess.GetType());
                fileStream = new FileStream(sTargetFile, FileMode.Create);                
                xsXmlSerializer.Serialize(fileStream, oObjectToProcess, xnsXmlSerializerNamespaces);
                        
                 return true;
            }
            catch (Exception ex)
            {
                PublicDI.log.ex(ex, "In createSerializedXmlStringFromObject");
                return false;
            }
            finally 
            {
                if (fileStream != null)
                {
                    fileStream.Close();                    
                }
            }

        }

        public static string createSerializedXmlStringFromObject(Object oObjectToProcess)
        {
            return createSerializedXmlStringFromObject(oObjectToProcess, null);
        }
                
        public static String createSerializedXmlStringFromObject(Object oObjectToProcess , Type[] extraTypes)
        {
            if (oObjectToProcess == null)
                PublicDI.log.error("in createSerializedXmlStringFromObject: oObjectToProcess == null");
            else
            {
                try
                {
                    /*// handle cases file names are bigger than 220
                    if (sTargetFile.Length > 220)
                    {
                        sTargetFile = sTargetFile.Substring(0, 200) + ".xml";
                        PublicDI.log.error("sTargetFile.Length was > 200, so renamig it to: {0}", sTargetFile);
                    }*/
                    var xnsXmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xnsXmlSerializerNamespaces.Add("", "");
                    var xsXmlSerializer = (extraTypes != null)
                                              ? new XmlSerializer(oObjectToProcess.GetType(), extraTypes)
                                              : new XmlSerializer(oObjectToProcess.GetType());
                    var memoryStream = new MemoryStream();
                    xsXmlSerializer.Serialize(memoryStream, oObjectToProcess, xnsXmlSerializerNamespaces);

                    return Encoding.ASCII.GetString(memoryStream.ToArray());
                }
                catch (Exception ex)
                {
                    PublicDI.log.ex(ex,"In createSerializedXmlStringFromObject");
                }
            }
            return "";
        }

        public static Object getDeSerializedObjectFromString(String stringWithSerializedContent, Type tTypeToProcess)
        {
            return getDeSerializedObjectFromString(stringWithSerializedContent, tTypeToProcess, true);
        }

        public static Object getDeSerializedObjectFromString(String stringWithSerializedContent, Type tTypeToProcess, bool showError)
        {
            Object deserializedObject = null;
            try
            {                               
                var stringReader = new StringReader(stringWithSerializedContent);
                var xsXmlSerializer = new XmlSerializer(tTypeToProcess);
                //var stringStream = new StringStream(stringWithSerializedContent);                
                deserializedObject = xsXmlSerializer.Deserialize(stringReader);
            }
            catch (Exception eException)
            {
                if (showError)
                {
                    PublicDI.log.error("ERROR: {0} ", eException.Message);
                    if (eException.InnerException != null)
                        PublicDI.log.error("   ERROR (InnerException): {0} ", eException.InnerException.Message);
                }
            }           
            return deserializedObject;
        }

        public static Object getDeSerializedObjectFromXmlFile(String sFileToProcess, Type tTypeToProcess)
        {
            return getDeSerializedObjectFromXmlFile(sFileToProcess, tTypeToProcess, false /*copyBeforeDeserialize*/);
        }

        public static Object getDeSerializedObjectFromXmlFile(String sFileToProcess, Type tTypeToProcess, bool copyBeforeDeserialize)
                  
        {
            FileStream fFileStream = null;            
            Object deserializedObject = null;      
            try
            {                                
                if (copyBeforeDeserialize)
                {
                    String sTempFile = PublicDI.config.TempFileNameInTempDirectory;
                    File.Copy(sFileToProcess, sTempFile);
                    sFileToProcess = sTempFile;
                }
                
                var xsXmlSerializer = new XmlSerializer(tTypeToProcess);
                fFileStream = new FileStream(sFileToProcess, FileMode.Open, FileAccess.Read);
                deserializedObject = xsXmlSerializer.Deserialize(fFileStream);
            }
            catch (Exception eException)
            {
                PublicDI.log.error("ERROR: {0} ", eException.Message);
                if (eException.InnerException != null)
                    PublicDI.log.error("   ERROR (InnerException): {0} ", eException.InnerException.Message);
            }
            finally
            {
                if (fFileStream != null)
                    fFileStream.Close();
                if (copyBeforeDeserialize)
                    File.Delete(sFileToProcess);
            }
            return deserializedObject;
        }

        public static bool createSerializedBinaryFileFromObject(Object oObjectToProcess, String sTargetFile)
        {            
            if (oObjectToProcess != null)
            {
                FileStream fsFileStream = null;
                if (sTargetFile == "")
                    sTargetFile = PublicDI.config.TempFileNameInTempDirectory + ".binary";

                try
                {
                    var bfBinaryFormatter = new BinaryFormatter();

                    fsFileStream = new FileStream(sTargetFile, FileMode.Create);
                    bfBinaryFormatter.Serialize(fsFileStream, oObjectToProcess);
                    //fsFileStream.Close();
                    PublicDI.log.debug("Serialized object saved to: {0}", sTargetFile);
                    return true;
                }
                catch (Exception ex)
                {
                    PublicDI.log.error("In createSerializedBinaryFileFromObject: {0}", ex.Message);
                }
                finally
                {
                    if (fsFileStream != null)
                    {
                        fsFileStream.Close();
                        //fsFileStream = null;
                    }
                }
            }
            return false;
        }

        public static Object getDeSerializedObjectFromBinaryFile(String fileToProcess, Type tTypeToProcess)
        {
            FileStream fsFileStream = null;
            O2Timer tO2Timer = new O2Timer("Loaded DeSerialized object from " + Path.GetFileName(fileToProcess)).start();
            try
            {
                
                var bfBinaryFormatter = new BinaryFormatter();
                fsFileStream = new FileStream(fileToProcess, FileMode.Open);

                Object deserializedObject = bfBinaryFormatter.Deserialize(fsFileStream);

                if (deserializedObject.GetType().FullName == tTypeToProcess.FullName)
                {
                    PublicDI.log.info("sucessfully deserialized file {0} into type {1}", fileToProcess, tTypeToProcess.FullName);
                    return deserializedObject;
                }
                PublicDI.log.error("Could not deserialize file {0} into type {1}", fileToProcess, tTypeToProcess.FullName);
                return null;
            }
            catch (Exception ex)
            {
                PublicDI.log.error("In loadSerializedO2CirDataObject: {0}", ex.Message);
                return null;
            }
            finally
            {
                if (fsFileStream != null)
                    fsFileStream.Close();
                tO2Timer.stop();
            }            
        }


        
    }
}
