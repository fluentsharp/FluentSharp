using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentSharp.CoreLib;

namespace FluentSharp.WinForms
{
    public static class WinForms_ExtensionMethods_TrackBar
    {
        public static TrackBar  insert_Above_Slider(this Control control)
        {					
            return control.insert_Above(20).add_TrackBar();
        }		
        public static TrackBar  add_Slider(this Control control)
        {
            return control.add_TrackBar();
        }
        public static TrackBar  add_TrackBar(this Control control)
        {
            return control.add_Control<TrackBar>();  
        }		
        public static TrackBar  maximum(this TrackBar trackBar, int value)
        {
            return (TrackBar)trackBar.invokeOnThread(
                ()=>{
                        trackBar.Maximum = value;
                        return trackBar;
                });
        }		
        public static TrackBar  set_Data<T>(this TrackBar trackBar, List<T> data)
        {
            trackBar.Tag = data;  
            trackBar.maximum(data.size());
            return trackBar;
        }
        public static TrackBar  onSlide<T>(this TrackBar trackBar, Action<T> onSlide)
        {
            return trackBar.onSlide((index)=>
                {
                    var tag = trackBar.Tag;
                    if (tag is List<T>)
                    {
                        var items = (List<T>)tag;
                        if (index > items.size()-1)
                            "[TrackBar][onSlide] provided index is bigger that items list".error();
                        else
                            onSlide(items[index]);
                    }					
                });				
        }		
        public static TrackBar  onSlide(this TrackBar trackBar, Action<int> onSlideCallback)
        {
            return (TrackBar)trackBar.invokeOnThread(
                ()=>{
                        trackBar.Scroll+= (sender,e) => onSlideCallback(trackBar.Value);
                        return trackBar;
                });
        }
        public static int       value(this TrackBar trackBar)
        {
            return trackBar.invokeOnThread(() => trackBar.Value);
        }
        public static TrackBar  value(this TrackBar trackBar, int value)
        {
            return trackBar.invokeOnThread(() => { trackBar.Value = value; return trackBar; });
        }
        public static TrackBar  onValueChanged(this TrackBar trackBar, Action<int> onSlideCallback)
        {
            return (TrackBar)trackBar.invokeOnThread(
                () =>
                    {
                        trackBar.ValueChanged += (sender, e) => onSlideCallback(trackBar.Value);
                        return trackBar;
                    });
        }
    }
}