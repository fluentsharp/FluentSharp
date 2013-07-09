using System.Collections.Generic;
using System.Xml.Serialization;

namespace FluentSharp.CoreLib.API
{        
    public class KeyValueStrings
    {
        public List<KeyValueString> Items { get; set; }

        public KeyValueStrings()
        {
            Items = new List<KeyValueString>();
        }

        public KeyValueStrings add(string key, string value)
        {
            this[key] = value;                        
            return this;
        }

        public int size
        {
            get { return Items.Count; }
        }

        public bool hasKey(string key)
        {
            return this[key] != null;
        }

        public string this[string key]
        {
            get
            {
                foreach(var item in Items)
                    if(item.Key == key)
                        return item.Value;
                return null;
            }
            set
            { 
                if (value != null)
                    foreach (var item in Items)
                        if (item.Key == key)
                        {
                         item.Value = value;
                             return;
                        }
                Items.Add(new KeyValueString(key, value));               
            }
        }                
    }



    public class KeyValueString
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public string Value { get; set; }

        public KeyValueString()
        {
            Key = "";
            Value = "";
        }

        public KeyValueString(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
