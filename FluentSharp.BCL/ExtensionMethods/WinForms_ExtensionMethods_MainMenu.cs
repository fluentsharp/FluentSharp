using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;
using FluentSharp.CoreLib.API;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_MainMenu
    {
        public static MainMenu          mainMenu(this Control control)
        {
            if (control.notNull())
            {
                var parentForm = control.parentForm();
                if (parentForm.notNull())
                {
                    return (MainMenu)control.invokeOnThread(
                        ()=>{						
                                if (parentForm.Menu.notNull())
                                    return parentForm.Menu;
                                var mainMenu =  new MainMenu();
                                parentForm.Menu = mainMenu;
                                return mainMenu;
                        });					
                }
            }
            "provided control is null or is not hosted by a Form".error();
            return null;
        }
        public static MainMenu          add_MainMenu(this Control control)
        {
            return control.mainMenu().clear();
        }		
        public static Form              parentForm(this MenuItem menuItem)
        {
            if (menuItem.isNull())
                return null;
            return menuItem.Parent.GetMainMenu().GetForm();
        }						
        public static MainMenu          mainMenu(this MenuItem menuItem)
        {
            return menuItem.Parent.GetMainMenu();
        }
        public static MenuItem          add(this MenuItem menuItem, string name, Action onSelect)
        {
            return menuItem.add_MenuItem(name, onSelect);
        }
        public static MenuItem          add_Menu(this MainMenu mainMenu , string name)
        {
            return (MenuItem)mainMenu.GetForm()
                                     .invokeOnThread(
                                         ()=>{
                                                 var newMenuItem = new MenuItem();
                                                 newMenuItem.Text = name;
                                                 mainMenu.MenuItems.Add(newMenuItem);
                                                 return newMenuItem; 
                                         });					
        }		
        public static MenuItem          add_Menu(this MenuItem menuItem, string name)
        {
            return (MenuItem)menuItem.parentForm()
                                     .invokeOnThread(
                                         ()=>{
                                                 return menuItem.mainMenu().add_Menu(name);
                                         });
        }	
        public static MenuItem          add_MenuItem(this MenuItem menuItem, string name)
        {
            return menuItem.add_MenuItem(name,false,null);
        }		
        public static MenuItem          add_MenuItem(this MenuItem menuItem , string name, Action onClick)
        {
            return menuItem.add_MenuItem(name, false, onClick);
        }		
        public static MenuItem          add_MenuItem(this MenuItem menuItem , string name, bool returnNewMenuItem, Action onClick)
        {
            return (MenuItem)menuItem.parentForm()
                                     .invokeOnThread(
                                         ()=>{
                                                 var newMenuItem = new MenuItem();
                                                 newMenuItem.Text = name;
                                                 if(onClick.notNull())
                                                     newMenuItem.Click+= (sender,e)=>{
                                                                                         O2Thread.mtaThread(()=> onClick());
                                                     };
                                                 menuItem.MenuItems.Add(newMenuItem);							 
                                                 if (returnNewMenuItem)
                                                     return newMenuItem;
                                                 return menuItem; 
                                         });					
        }        
        public static MenuItem          add_Separator(this MenuItem menuItem)
        {
            return menuItem.add_MenuItem("-", () => { });
        }
        public static List<MenuItem>    items(this MainMenu mainMenu)
        {
            return (List<MenuItem>)mainMenu.GetForm()
                                           .invokeOnThread(
                                               () =>
                                                   {
                                                       var menuItems = new List<MenuItem>();
                                                       foreach (MenuItem menuItem in mainMenu.MenuItems)  // Linq query doesn't work
                                                           menuItems.Add(menuItem);
                                                       return menuItems;
                                                   });
        }
        public static MenuItem          menu(this MainMenu mainMenu, string name)
        {
            return (MenuItem)mainMenu.GetForm()
                                     .invokeOnThread(
                                         () =>
                                             {
                                                 var topMenu = mainMenu.items().where((item) => item.Text == name).first();
                                                 if (topMenu.notNull())
                                                     return topMenu;
                                                 return null;
                                             });
        }
        public static MainMenu          clear(this MainMenu mainMenu)
        {
            return (MainMenu)mainMenu.GetForm()
                                     .invokeOnThread(
                                         () =>
                                             {
                                                 mainMenu.MenuItems.Clear();
                                                 return mainMenu;
                                             });

        }
        public static MenuItem          item(this MenuItem menuItem, string name)
        {
            return menuItem.menuItem(name);
        }
        public static MenuItem          menuItem(this MenuItem menuItem, string name)
        {
            return menuItem.menuItems().where((childItem) => childItem.Text == name).first();
        }
        public static List<MenuItem>    items(this MenuItem menuItem)
        {
            return menuItem.menuItems();
        }
        public static List<MenuItem>    menuItems(this MenuItem menuItem)
        {
            var items = new List<MenuItem>();
            if (menuItem.notNull())
                foreach (MenuItem childMenu in menuItem.MenuItems)
                    items.add(childMenu);
            return items;
        }
        public static string            get_Text(this MenuItem menuItem)
        {
            return menuItem.parentForm()
                           .invokeOnThread(() => menuItem.Text);
        }
        public static MenuItem          set_Text(this MenuItem menuItem, string value)
        {
            return menuItem.parentForm()
                           .invokeOnThread(() => { menuItem.Text = value; return menuItem; });
        }
        
    }
}