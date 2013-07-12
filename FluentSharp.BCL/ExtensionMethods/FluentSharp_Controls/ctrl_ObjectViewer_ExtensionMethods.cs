using System;
using System.Collections.Generic;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;
using FluentSharp.BCL.Controls;

//ctrl_ObjectViewer_ExtensionMethods
namespace FluentSharp.BCL
{
    /*public class test_ctrl_ObjectViewer
    {
        public static void launchGui()
        {
            //var _object = new List<string>().add("aaa").add("bbb").add("ccc");
            var _object = new List<object>().add(PublicDI.config)
                                            .add("aaa")
                                            .add(AppDomain.CurrentDomain);
            _object.Add("789".wrapOnList().add("456").add("123"));

            _object.showObject();

        }
    }*/

    public static class ctrl_ObjectViewer_ExtensionMethods
    {

        public static T showDetails<T>(this T _object) where T : class
        {
            return _object.showObject();
        }

        public static T objectDetails<T>(this T _object) where T : class
        {
            return _object.showObject();
        }

        public static T viewObject<T>(this T _object) where T : class
        {
            return _object.showObject();
        }

        public static T showObject<T>(this T _object) where T : class
        {
            if (_object.isNull())
                "in showObject object provided was null".error();
            else
            {
                var formTitle = "Object Viewer = {0}".format(_object.type());
                var objectViewer = O2Gui.open<ctrl_ObjectViewer>(formTitle, 800, 400);
                objectViewer.show(_object);
            }
            return _object;
        }

        public static T details<T>(this T _object) where T : class
        {
            O2Thread.mtaThread(() => _object.showObject());
            return _object;
        }
    }
}
