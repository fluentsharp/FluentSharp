using System;
using System.Collections.Generic;
using System.Text;
using FluentSharp.CoreLib;
using FluentSharp.Web;
using NUnit.Framework;

namespace UnitTests.FluentSharp.Web_4_5
{
    [TestFixture]
    public class Test_Json_ExtensionMethods
    {           
        public int          Obj_Int      { get; set; }
        public string       Obj_String   { get; set; }
        public List<string> Obj_List     { get; set; }
        public object       Obj_Null     { get; set; }
       
        public string       Json_Int   { get; set; }
        public string       Json_String   { get; set; }
        public string       Json_List     { get; set; }

        public Test_Json_ExtensionMethods()
        {
            Obj_Int     = 10000.randomNumber();
            Obj_String  = 10   .randomLetters();
            Obj_List    = new List<string> {10.randomLetters(), 10.randomLetters()};
            Obj_Null    = null;
 
            Json_Int    = Obj_Int.str();
            Json_String = "\"{0}\"".format(Obj_String);
            Json_List   = "[\"{0}\",\"{1}\"]".format(Obj_List[0], Obj_List[1]);
        }
        [Test] public void json()
        {
            Assert.AreEqual(Obj_Int   .json() , Json_Int);
            Assert.AreEqual(Obj_String.json() , Json_String);
            Assert.AreEqual(Obj_List  .json() , Json_List);
            Assert.IsNull  (Obj_Null  .json() , Json_List);
        }                        
        [Test] public void json_Deserialize()
        {
            Assert.AreEqual(Obj_Int   .json_Serialize().json_Deserialize() , Obj_Int);
            Assert.AreEqual(Obj_String.json_Serialize().json_Deserialize() , Obj_String);
            Assert.AreEqual(Obj_List  .json_Serialize().json_Deserialize() , Obj_List);
            Assert.AreEqual(Obj_Null  .json_Serialize().json_Deserialize() , Obj_Null);

            Assert.AreEqual(Json_Int   .json_Deserialize<int>          () , Obj_Int);
            Assert.AreEqual(Json_Int   .json_Deserialize<Int32>        () , Obj_Int);
            Assert.AreEqual(Json_String.json_Deserialize<string>       () , Obj_String);
            Assert.AreEqual(Json_String.json_Deserialize<String>       () , Obj_String);
            Assert.AreEqual(Json_List  .json_Deserialize<List<String>> () , Obj_List);

            Assert.AreEqual(Json_Int   .json_Deserialize<StringBuilder>() , null);            
            Assert.AreEqual(Json_String.json_Deserialize<StringBuilder>() , null);
            Assert.AreEqual(Json_List  .json_Deserialize<StringBuilder>() , null);            
        }
        [Test] public void json_Serialize()
        {
            Assert.AreEqual((null as string)       .json_Serialize(), null);
            Assert.AreEqual((null as List<string>) .json_Serialize(), null);             
            Assert.AreEqual(AppDomain.CurrentDomain.json_Serialize(), null );

            //the other cases are tested by json_Deserialize
        }
        [Test] public void javascript_Deserialize()
        {
            Assert.AreEqual(Obj_Int   .javascript_Serialize().javascript_Deserialize() , Obj_Int);
            Assert.AreEqual(Obj_String.javascript_Serialize().javascript_Deserialize() , Obj_String);
            Assert.AreEqual(Obj_List  .javascript_Serialize().javascript_Deserialize() , Obj_List);
            Assert.AreEqual(Obj_Null  .javascript_Serialize().javascript_Deserialize() , Obj_Null);

            Assert.AreEqual(Json_Int   .javascript_Deserialize<int>          () , Obj_Int);
            Assert.AreEqual(Json_Int   .javascript_Deserialize<Int32>        () , Obj_Int);
            Assert.AreEqual(Json_String.javascript_Deserialize<string>       () , Obj_String);
            Assert.AreEqual(Json_String.javascript_Deserialize<String>       () , Obj_String);
            Assert.AreEqual(Json_List  .javascript_Deserialize<List<String>> () , Obj_List);

            Assert.AreEqual(Json_Int   .javascript_Deserialize<StringBuilder>() , null);            
            Assert.AreEqual(Json_String.javascript_Deserialize<StringBuilder>() , null);
            Assert.AreEqual(Json_List  .javascript_Deserialize<StringBuilder>() , null);            
        }

        [Test] public void javascript_Serialize()
        {            
            Assert.AreEqual((null as string)       .javascript_Serialize() , "null");
            Assert.AreEqual((null as List<string>) .javascript_Serialize() , "null");             
            Assert.AreEqual(AppDomain.CurrentDomain.javascript_Serialize() ,  null );
            //the other cases are tested by javascript_Deserialize
        }
        
    }
}
