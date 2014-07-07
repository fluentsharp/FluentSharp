using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.WinForms.Utils;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_Icon
    {        
        public static Icon      set_As_Default_Form_Icon(this Icon icon)
        {
            typeof(Form).fieldValue("defaultIcon", icon);
            return icon;
        }
        public static Icon      file_Icon(this string filePath)
        {
            if(filePath.fileExists())
            {
                try
                {
                    return Icon.ExtractAssociatedIcon(filePath);
                }
                catch(Exception ex)
                {
                    ex.log("[filePath].file_Icon");
                }
            }
            return null;
        }
		
        public static Image     toImage(this Icon icon)
        {
            if (icon.notNull())
                return icon.ToBitmap();
            return null;
			
        }
        public static Icon      asIcon(this Image image)
        {
            return image.asBitmap().asIcon();
        }
        public static Icon      asIcon(this Bitmap bitmap)
        {
            try
            {
                return Icon.FromHandle(bitmap.GetHicon());
            }
            catch (Exception ex)
            {
                ex.log();
                return null;
            }
            
        }
        public static Icon      icon(this string iconFile)
        {
            try
            {
                if(iconFile.valid())
                    return new Icon(iconFile);
            }
            catch(Exception ex)
            {
                "[icon] {0}".error(ex.Message);                
            }
            return null;
        }
        public static string    saveAs_Icon(this Bitmap bitmap)
        {
            return bitmap.saveAs_Icon(".ico".tempFile());
        }
        public static string    saveAs_Icon(this Bitmap bitmap, string targetFile)
        {
            return bitmap.asIcon().saveAs(targetFile);
        }
        public static string    save(this Icon icon)
        {            
            return icon.saveAs(".ico".tempFile());
        }
        public static string    saveAs(this Icon icon, string targetFile)
        {
            using (var fileStream = new FileStream(targetFile, FileMode.Create))
                icon.Save(fileStream);
            return targetFile;
        }
        public static T         set_Form_Icon<T>(this T control, string iconFile)			where T : Control
        {
            return control.set_Form_Icon(iconFile.icon());
        }		
        public static T         set_Form_Icon<T>(this T control, Icon icon)			where T : Control
        {
            control.parentForm().set_Icon(icon);
            return control;
        }
        public static T         set_Icon<T>(this T control, string iconName) where T : Control
        {
            var file = iconName.local_Or_Resource();
            var parentForm = control.parentForm();
            if (file.fileExists() && parentForm.notNull())
            {
                parentForm.set_Icon(file.icon());
                return control;
            }
            "Error setting parent Form's Icon to: {0}".error(iconName);
            return control;
        }
        public static Form      set_Icon(this Form form, Icon icon)
        {
            form.invokeOnThread(()=> form.Icon = icon);
            return form;
        }								
        public static T          add_H2Icon<T>(this T control)			where T : Control
        {
            //return control.set_Form_Icon("H2Logo.ico".local());
            return control.set_Form_Icon(FormImages.H2Logo);
        }
        public static Form      set_H2Icon(this Form form)	
        {
            //return form.set_Icon("H2Logo.ico".local().icon());
            return form.set_Form_Icon(FormImages.H2Logo);			
        }
        public static Form      set_O2Icon(this Form form)	
        {   
            return form.set_Form_Icon(FormImages.O2Logo);			
            //return form.set_Icon("O2Logo.ico".local().icon());
        }
        public static Form      set_DefaultIcon(this Form form)
        {
            try
            {
                var entryAssembly = Assembly.GetEntryAssembly();

                var icon = (entryAssembly.notNull())
                    ? Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location)
                    : FormImages.H2Logo;
                form.set_Icon(icon);
            }
            catch (Exception ex)
            {
                ex.log("in set_DefaultIcon");
            }
            return form;        
        }
        public static Form      clientSize(this Form form, int width, int height)
        {
            return form.invokeOnThread(() =>
            {
                form.ClientSize = new Size(width, height);
                return form;
            });
        }
        public static Image     formImage(this string imageKey)
        {
            return (Image)typeof(FormImages).prop(imageKey);
        }
    }
}