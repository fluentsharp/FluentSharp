using System;
using System.IO.Compression;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace FluentSharp.MsBuild
{
    /// <summary>
    /// Class used by Package_Scripts_ExtensionMethods.package_Script to create the standalone exe
    /// </summary>
    public class Program_FluentSharp_Generated
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        public static bool saveEmbededLibrariesToDisk = false;
        public static string saveEmbededLibrariesToFolder = "O2.Temp";
        
        static void Main(string[] args)
        {        	
            setAssemblyResolver();                        
            //loadDependencies();
            new Program_FluentSharp_Generated().invokeMain(args);
            
        }

        public void invokeMain(string[] args)
        {            
        	//var assembly = getAssemblyfromResources("GraphSharp", true);
        	try
        	{
            	new DynamicType().dynamicMethod();
            }
            catch(Exception ex)
            {
            	MessageBox.Show(ex.Message, "Execution Error");
            }
        }

        public static void setAssemblyResolver()
        {
        	var startAssemblyName = Assembly.GetEntryAssembly().GetName().Name;
        	saveEmbededLibrariesToFolder = Path.Combine(saveEmbededLibrariesToFolder, "_Files For " + startAssemblyName);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);			
        }        

        public static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var nameToFind = args.Name;
        	if (nameToFind.IndexOf(",") > -1)
                nameToFind = nameToFind.Substring(0,nameToFind.IndexOf(","));
            return getAssemblyfromResources(nameToFind, saveEmbededLibrariesToDisk);
     	}
     	
     	public static Assembly getAssemblyfromResources(string nameToFind, bool saveToDisk)
     	{     		
     		try
     		{
	     		var nameToFind_Lower = nameToFind.ToLower();
	            var targetAssembly = Assembly.GetExecutingAssembly();            
	            foreach (var resourceName in targetAssembly.GetManifestResourceNames())
	                if (resourceName.ToLower().Contains(nameToFind_Lower) && 
	                	(resourceName.ToLower().Contains(nameToFind_Lower + ".dll") || resourceName.ToLower().Contains(nameToFind_Lower+".exe")) )
	                {
	                    var assemblyStream = targetAssembly.GetManifestResourceStream(resourceName);
	                    byte[] compressedData = new BinaryReader(assemblyStream).ReadBytes((int)assemblyStream.Length);
	                    var data = gzip_Decompress(compressedData);
	                    
	                    if (saveToDisk == false)
	                    {
	                    	try
	                    	{
	                    		return Assembly.Load(data);
	                    	}
	                    	catch(Exception ex)
	                    	{	
	                    		MessageBox.Show(ex.Message, "Load Assembly from bytes failed");	
	                    		saveToDisk = true;  // if this fail, try to load it form disk
	                    	}
	                    }
	                    
	                    if (saveToDisk)
	                    {
	                    	var targetDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
	                    	targetDir = Path.Combine(targetDir,saveEmbededLibrariesToFolder);
	                    	if (Directory.Exists(targetDir) == false)
	                    		Directory.CreateDirectory(targetDir);
	                    	var targetFile = Path.Combine(targetDir, nameToFind + (resourceName.Contains(".dll") ? ".dll" : ".exe"));                    	
	                    	if (File.Exists(targetFile) == false)
	                    	{
		                    	using (FileStream fs = File.Create(targetFile))
		                		{
		                    		fs.Write(data, 0, data.Length);
		                    		fs.Close();                    
		                		}
		                	}
	                		return Assembly.LoadFrom(targetFile);
	                    }                    
	                }
	        }
	        catch(Exception ex)
	        {
	        	MessageBox.Show(ex.Message, "getAssemblyfromResources failed");	
	        }
            return null;
     	}
     	
     	public static byte[] gzip_Decompress(byte[] bytes)
		{
			var inputStream = new MemoryStream();
			inputStream.Write(bytes, 0, bytes.Length);
			inputStream.Position = 0;
			var outputStream = new MemoryStream();
			using (var gzipStream= new GZipStream(inputStream,CompressionMode.Decompress))
			{
			    byte[] buffer = new byte[4096];
			    int numRead;
			    while ((numRead = gzipStream.Read(buffer, 0, buffer.Length)) != 0)			    
			        outputStream.Write(buffer, 0, numRead);			    
			}	  
			outputStream.Position=0;
			return outputStream.GetBuffer();
		}                
    }
}
