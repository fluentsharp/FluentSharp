# FluentSharp

FluentSharp are a number of Open Source .Net APIs that dramatically simplifies the use of .NET Framework APIs. 

They makes extensive use of .NET ExtensionMethods and reduces the amount of code required (while making the original code more readable).

By now, a large part of the .NET Framework is covered by FluentSharp ExtensionMethods (String, Xml, IO, WinForms, 
Reflection, etc..). 

As an example, the FluentSharp .NET Reflection ExtensionMethods are very powerful, since they provide 
(via user-friendly methods) full access to .NET classes, methods, properties, fields and enums 
(regardless of their public/private status).


## What is the OWASP O2 Platform

The FluentSharp APIs are at the core of the [OWASP O2 Platform](https://www.owasp.org/index.php/OWASP_O2_Platform) which is usually described like this:
'The O2 platform represents a new paradigm for how to perform, document and distribute Web Application security reviews. 
O2 is designed to Automate Security Consultants Knowledge and Workflows and to Allow non-security experts to access and 
consume Security Knowledge'

### FluentSharp in Action: Example 1

For example if you want to list all files from a particular directory you can just execute:

```csharp 
@"C:\O2".files();
```

...if you wanted all files in all directories (i.e. recursive search), you would use:

```csharp
@"C:\O2".files(true);
```

... all *.cs files:

```csharp
@"C:\O2".files(true,"*.cs");
```

... all *.cs and *.h2 files:

```csharp
@"C:\O2".files(true,"*.cs" ,"*.h2");
```

... show files in treeView:

```csharp
new TreeView().add_Nodes(@"C:\O2".files(true,"*.cs","*.h2"));
```

 ...show results in a popupWindow (i.e.  in Windows Form) with treeView showing files:

```csharp
"New popup Window with TreeView".popupWindow().add_TreeView().add_Nodes(@"C:\O2".files(true,"*.cs","*.h2"));
```

... swow in popupWindow with the TreeVIew with files and a source code viewer that shows the selected file:

```csharp
var targetFolder = @"C:\O2";
var sourceCodeViewer ="View C# and H2 files in folder".popupWindow()
						      .add_SourceCodeViewer();
sourceCodeViewer.insert_Left(200)
		.add_TreeView()
		.add_Nodes(targetFolder.files(true,"*.cs","*.h2"), (file)=>file.fileName())
		.after_Selected<string>((file)=>sourceCodeViewer.open(file));
```

The key objective of this style of programming is to make it really easy read and understand what is going on.

The way these extension methods are written is to focus on the Intent of the action, 
and then hide all technology behind the scenes.

For example in the last example, this code 

```csharp
.add_TreeView().add_Nodes(targetFolder ,"*.cs","*.h2"), (file)=>file.fileName());
```

can be further refactored to:

```csharp
.add_TreeView().add_Files(targetFolder ,"*.cs","*.h2");
```


### FluentSharp in Action: Example 2

Here is a more complete example that shows these FluentSharp APIs in action (inside VisualStudio):

```csharp
using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using FluentSharp.O2.Views.ASCX.classes.MainGUI;
using FluentSharp.O2.Kernel.ExtensionMethods;
using FluentSharp.O2.DotNetWrappers.ExtensionMethods;


namespace FluentSharp_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var topPanel = O2Gui.open<Panel>("FluentSharp - Simple Test", 1000, 300);            
            
            //TextBox that will hold the data
            var textBox = topPanel.add_GroupBox("TextBox with selected data").add_TextArea();

            //Left-Hand-side TreeView with filenames (when click shows content on TextBox)
            var folder = @"C:\O2\Demos\FluentSharp";
            textBox.parent()
                   .insert_Left<Panel>(300)
                   .add_GroupBox("Files in Folder")
                   .add_TreeView()
                   .backColor(Color.Azure)
                   .add_Nodes(folder.files("*.cs", true))
                   .afterSelect<string>((file) => textBox.set_Text(file.fileContents()))
                   .selectFirst();

            //Right-Hand-side TreeView with types from an Assembly (when click shows list of methods on TextBox)
            var assemblyToShow = "FluentSharp_O2.dll";
            textBox.parent()
                   .insert_Right<Panel>(300)
                   .add_GroupBox("Types in Assembly")
                   .add_TreeView()
                   .backColor(Color.LightGreen)
                   .add_Nodes(assemblyToShow.assembly().types())
                   .afterSelect<Type>((type) =>
                  	 {
                       		textBox.set_Text("Methods in Type: {0}".line().format(type.fullName()));
	                        foreach (var method in type.methods())
                           		textBox.append_Line("  -  {0}".format(method.Name));                 	 
			 });		
        }
    }
}
```
The code above when executed will look like this:

![](/Docs/Images/FluentSharp_ScriptExample_1.jpeg)

Here is an example of how to use the FluentSharp REPL (also part of the O2 Development Environment) to develop and quickly execute the script above:



![](/Docs/Images/FluentSharp_ScriptExample_2.jpeg)


# GitHub Repositories

There a number of GitHub repositories used to host the multiple parts of the FluentSharp and O2 Platform components:

* https://github.com/o2platform/FluentSharp - main FluentSharp projects
* https://github.com/o2platform/FluentSharp_Fork.WatiN - Watin ExtensionMethods
* https://github.com/o2platform/FluentSharp_Fork.CassiniDev - Fork of Cassini with some bug fixes and lots of ExtensionMethods
* https://github.com/o2platform/FluentSharp_Fork.SharpDevelopEditor - Fork of SharpDevelop Editor (which is used by the FluentSharp REPL)
* https://github.com/o2platform/O2.Platform.Scripts - hundreds of Scripts exposed, consumed and developed via the O2 Platform UI
* https://github.com/o2platform/FluentSharp.VisualStudio - VisualStudio Addin
* https://github.com/o2platform/O2.Platform.Projects - a bit legacy (needs clean up)
* https://github.com/o2platform/Web_CSharp_REPL - code for web repl that can be see at http://csharp-repl.apphb.com/1

There is also the https://github.com/o2platform/Book_WebAutomation which contains the first pass at creating an book about the O2 Platform and WatiN APIs

# Being involved and Fixing Issues

If you want to help (or learn C#) please take a look at the issues currently opened and see if you can provide a fix for them:

* [FluentSharp issues](https://github.com/o2platform/FluentSharp_Fork.WatiN/issues) - Issues for the main FluentSharp Projects: CoreLib, REPL,NUnit, etc...
* [FluentSharp_Fork.WatiN](https://github.com/o2platform/FluentSharp_Fork.WatiN/issues) - Issues for the WatiN Extension Methods (Browser/IE automation)
* [O2.Platform.Scripts](https://github.com/o2platform/O2.Platform.Scripts/issues) - Issues for the O2 Platform Scripts



# Why Fluent?

Rational and References for Fluent Interfaces, Fluent Programming

The O2 Platform scripting environment makes extensive use of C# 3.0 features like Linq, Lambda and ExtensionMethods. 
Here is a great article that explains how these technologies work and provide C# code samples of how to use them: 
http://msdn.microsoft.com/en-us/library/bb308959.aspx . 
Some of these already have their representation in O2, for exampe the 'Where' function has an equivalent lowercase 
'where' ExtensionMethod , with the O2 version handling the checking if the original data is null, 
and returning a List<T> (instead of an Enumerable<T>) 

I found the 'Fluent' concept when I was looking at JQuery and noticed that its API (Philosophy) looked a lot like O2's API.

JQuery calls what it does 'Fluent API' and I had a look at what it means and found what I'm doing with O2's Scripting 
is a lot like it. Here are some references for past 'Fluent' threads:

* http://www.martinfowler.com/bliki/FluentInterface.html
* http://blog.troyd.net/PermaLink,guid,5cdd4862-857a-488d-a577-c6d21b548f19.aspx
* http://stackoverflow.com/questions/224730/tips-for-writing-fluent-interfaces-in-c-3 
* http://en.wikipedia.org/wiki/Fluent_interface
* http://codebetter.com/kylebaley/2009/07/27/fluent-sharp-nhibernate-persistence-configuration-or-how-to-build-credibility-through-oss-name-dropping/
* http://blog.raffaeu.com/archive/2010/06/26/how-to-write-fluent-interface-with-c-and-lambda.aspx
* http://devhawk.net/2007/07/12/thoughts-on-c-fluent-interfaces/

What is interesting  about the links above is that:

a)  they are a bit old (circa 2008) and
b) they don't mention .Net's extension methods (which for me are the KEY reason why I went the 'fluent way' in O2's API).
The other thing that I do very differently in O2, is that I don't create special 'Fluent Interface' classes, I just extend the ones that already exist.
