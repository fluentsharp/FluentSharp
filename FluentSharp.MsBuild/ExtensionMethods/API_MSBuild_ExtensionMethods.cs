using System;
using System.Collections.Generic;
using System.Text;
using FluentSharp.CoreLib;

namespace FluentSharp.MsBuild
{
    public static class API_MSBuild_ExtensionMethods
    {
        public static API_MSBuild build(this string compilationTarget)
        {
            return new API_MSBuild().build(compilationTarget);
        }
		
        public static API_MSBuild start(this API_MSBuild msBuild)
        {
            return msBuild.build();
        }
		
        public static API_MSBuild build(this API_MSBuild msBuild)
        {
            return msBuild.build(msBuild.CompilationTarget);
        }
		
        public static API_MSBuild build(this API_MSBuild msBuild, string compilationTarget)
        {			
            msBuild.CompilationTarget = compilationTarget;
            if (compilationTarget.fileExists())
            {
                var arguments = "\"{0}\" {1}".format(msBuild.CompilationTarget, msBuild.ExtraBuildArguments);
                if (msBuild.PlatformTarget.valid())
                    arguments+= " /p:PlatformTarget=\"{0}\"".format(msBuild.PlatformTarget);
                msBuild.ConsoleOut 				= new StringBuilder();					
                msBuild.BuildStartTime 			= DateTime.Now;								
                msBuild.MSBuild_Process 		= msBuild.MSBuild_Exe.startProcess(arguments, msBuild.OnConsoleOut);
            }
            else
                "[API_MSBuild] in build, provided compilationTarget file doesn't exists: {0}".error(compilationTarget);
            return msBuild;
        }
		
        public static API_MSBuild waitForBuildCompletion(this API_MSBuild msBuild)
        {
            if (msBuild.MSBuild_Process.notNull())
            {
                msBuild.MSBuild_Process.WaitForExit();
                msBuild.BuildDuration = msBuild.BuildStartTime - DateTime.Now;
            }
            return msBuild;
        }
		
        public static bool build_Ok(this API_MSBuild msBuild, string compilationTarget)
        {			
            msBuild.build(compilationTarget)
                .waitForBuildCompletion();
			
            var buildResult = msBuild.ConsoleOut.str().contains("Build succeeded.");
            if (buildResult)
                "[API_MSBuild] Project build OK: {0}".debug(compilationTarget);
            else				
                "[API_MSBuild] Project Failed to build: {0}".error(compilationTarget);
            return buildResult;
        }

        //Misc
        public static List<string> csproj_Files(this string path)
        {
            return path.files("*.csproj",true);
        }
    }
}