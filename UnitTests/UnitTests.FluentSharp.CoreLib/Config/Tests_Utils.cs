using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentSharp.CoreLib;
using NUnit.Framework;

namespace UnitTests.FluentSharp_CoreLib
{
    public class Tests_Utils
    {
        public static Type Bad_Type_Object()
        {
            var type = new object().type();                     // get some object's type
            var corruptedType = (Type)new TypeDelegator(type);  // create an instance of TypeDelegator

            Assert.IsNotNull(corruptedType.Assembly);           // check that before assembly was ok

            corruptedType.obj().field("typeImpl",null);         // set the typeImpl value to null 
            Assert.Throws<NullReferenceException>(()=> corruptedType.Assembly.str());              // check that after it fails
            return corruptedType;
        }
    }
}
