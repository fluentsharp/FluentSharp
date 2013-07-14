using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_TabControl
    { 
        public static TabControl add_TabControl(this Control control)
        {
            return (TabControl) control.invokeOnThread(
                () =>
                    {
                        var tabControl = new TabControl();
                        tabControl.Dock = DockStyle.Fill;
                        control.Controls.Add(tabControl);
                        return tabControl;
                    });
        }
        public static TabPage add_Tab(this TabControl tabControl, string tabTitle)
        {
            return (TabPage) tabControl.invokeOnThread(
                () =>
                    {
                        var tabPage = new TabPage();
                        tabPage.Text = tabTitle;
                        tabControl.TabPages.Add(tabPage);
                        return tabPage;
                    });
        }
        public static TabPage onSelected(this TabPage tabPage, MethodInvoker callback)
        {
            if (callback != null)
            {
                tabPage.invokeOnThread(() =>
                    {
                        var tabControl = tabPage.parent<TabControl>();
                        tabControl.SelectedIndexChanged +=
                            (sender, e) =>
                                {
                                    if (tabControl.SelectedTab == tabPage)
                                        callback();
                                };
                        // handle the case where the tabPage is the current selected tab
                        if (tabControl.SelectedTab == tabPage)
                            callback();
                    });
            }
            return tabPage;
        }
        public static TabControl remove_Tab(this TabControl tabControl, TabPage tabPage)
        {
            return (TabControl)tabControl.invokeOnThread(
                () =>
                    {
                        tabControl.TabPages.Remove(tabPage);
                        return tabControl;
                    });
        }
        public static TabControl select_Tab(this TabControl tabControl, TabPage tabPage)
        {
            return (TabControl)tabControl.invokeOnThread(
                () =>
                    {
                        tabControl.SelectedTab = tabPage;
                        return tabControl;
                    });
        }
        public static TabControl remove_Tab(this TabControl tabControl, string text)
        {
            var tabToRemove = tabControl.tab(text);
            if (tabToRemove.notNull())
                tabControl.remove_Tab(tabToRemove);
            return tabControl;
        }		
        public static bool has_Tab(this TabControl tabControl, string text)
        {
            return tabControl.tab(text).notNull();
        }		
        public static TabPage tab(this TabControl tabControl, string text)
        {
            foreach(var tab in tabControl.tabs())
                if (tab.get_Text() == text)
                    return tab;
            return null;
        }
        public static List<TabPage> tabs(this TabControl tabControl)
        {
            return tabControl.tabPages();
        }		
        public static List<TabPage> tabPages(this TabControl tabControl)
        {
            return (List<TabPage>)tabControl.invokeOnThread(
                ()=>{
                        var tabPages = new List<TabPage>();
                        foreach(TabPage tabPage in tabControl.TabPages)
                            tabPages.Add(tabPage);
                        return tabPages;											
                });
        }
        public static TabControl selectedIndex(this TabControl tabControl, int index)
        {
            return (TabControl)tabControl.invokeOnThread(
                ()=>{
                        tabControl.SelectedIndex = index;
                        return tabControl;
                });
        }

    }
}