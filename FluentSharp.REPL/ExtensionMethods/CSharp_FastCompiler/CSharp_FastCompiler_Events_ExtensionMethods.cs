using System;
using FluentSharp.AST;
using FluentSharp.CoreLib;
using FluentSharp.REPL.Utils;

namespace FluentSharp.REPL
{
    public static class CSharp_FastCompiler_Events_ExtensionMethods
    {
        public static CSharp_FastCompiler_Events events(this CSharp_FastCompiler csharpCompiler)
        {
            return csharpCompiler.notNull() ? csharpCompiler.Events : null;
        }

        public static CSharp_FastCompiler        set_BeforeSnippetAst   (this CSharp_FastCompiler csharpCompiler, Action action)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().BeforeSnippetAst = action;
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        set_OnAstFail          (this CSharp_FastCompiler csharpCompiler, Action action)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnAstFail = action;
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        set_OnAstOK            (this CSharp_FastCompiler csharpCompiler, Action action)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnAstOK = action;
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        set_OnCompileFail      (this CSharp_FastCompiler csharpCompiler, Action action)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnCompileFail = action;
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        set_OnCompileOK        (this CSharp_FastCompiler csharpCompiler, Action action)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnCompileOK = action;
            return csharpCompiler;
        }

        public static CSharp_FastCompiler        raise_BeforeCompile    (this CSharp_FastCompiler csharpCompiler)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().BeforeCompile.invoke();
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        raise_BeforeSnippetAst (this CSharp_FastCompiler csharpCompiler)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().BeforeSnippetAst.invoke();
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        raise_OnAstFail        (this CSharp_FastCompiler csharpCompiler)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnAstFail.invoke();
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        raise_OnAstOK          (this CSharp_FastCompiler csharpCompiler)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnAstOK.invoke();
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        raise_OnCompileOK      (this CSharp_FastCompiler csharpCompiler)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnCompileOK.invoke();
            return csharpCompiler;
        }
        public static CSharp_FastCompiler        raise_OnCompileFail    (this CSharp_FastCompiler csharpCompiler)
        {
            if(csharpCompiler.events().notNull())
                csharpCompiler.events().OnCompileFail.invoke();
            return csharpCompiler;
        }
    }
}