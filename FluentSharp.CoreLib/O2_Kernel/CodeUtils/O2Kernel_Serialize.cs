// This file is part of the OWASP O2 Platform (http://www.owasp.org/index.php/OWASP_O2_Platform) and is released under the Apache 2.0 License (http://www.apache.org/licenses/LICENSE-2.0)
using System;
using System.IO;
using System.Xml.Serialization;

namespace FluentSharp.CoreLib.API
{
    public class O2Kernel_Serialize
    {
        // Similar code exist in O2.DotNetWrappers.DotNet.Serialize
        public static bool createSerializedXmlFileFromObject(Object oObjectToProcess, String sTargetFile, Type[] extraTypes)
        {
            FileStream fileStream = null;
            try
            {
                var xnsXmlSerializerNamespaces = new XmlSerializerNamespaces();
                xnsXmlSerializerNamespaces.Add("", "");
                var xsXmlSerializer = (extraTypes != null)
                                          ? new XmlSerializer(oObjectToProcess.GetType(), extraTypes)
                                          : new XmlSerializer(oObjectToProcess.GetType());

                fileStream = new FileStream(sTargetFile, FileMode.Create);

                xsXmlSerializer.Serialize(fileStream, oObjectToProcess, xnsXmlSerializerNamespaces);
                //fileStream.Close();

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

        // Similar code exist in O2.DotNetWrappers.DotNet.Serialize
        public static Object getDeSerializedObjectFromXmlFile(String sFileToProcess, Type tTypeToProcess)
        {
            FileStream fFileStream = null;
            Object deserializedObject = null;
            try
            {
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
            }
            return deserializedObject;
        }


    }
}
