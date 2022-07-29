using Godot;
using System;
// using PropertyDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;
// using PropArr = Godot.Collections.Array<Godot.Collections.Dictionary>;

class CustomResource: Resource {    
    protected PropertyList _propertyList = new PropertyList();

    public override Godot.Collections.Array _GetPropertyList() {
        return _propertyList.GetList();
    }

    public override object _Get(string property) {
        return _propertyList.Get(property);
    }

    public override bool _Set(string property, object value) {
        return _propertyList.Set(property, value);
    }
}

class PropertyList {
    public Godot.Collections.Array _propertyList;

    public PropertyList() {
        _propertyList = new Godot.Collections.Array();
    }

    public PropertyList Add(PropertyHint hint, string hintString, string name, Variant.Type type, UsageFlags usageFlags, object value) {
        _propertyList.Add( new Godot.Collections.Dictionary() {
            {"class_name", ""}, 
            {"hint", hint},
            {"hint_string", hintString},
            {"name", name},
            {"type", type},
            {"usage", (int) usageFlags},
            {"value", value}
        });
        return this;
    }

    public PropertyList AddSeparator(string name) {
        return Add(PropertyHint.None, "", name, Variant.Type.Nil, UsageFlags.Category, 0);
    }
    public PropertyList AddGroup(string name) {
        return Add(PropertyHint.None, "", name, Variant.Type.Nil, UsageFlags.Group, 0);
    }

    public bool SetProperty(string property, string key, object value) {
        int idx = FindProperty(property);
        if (idx != -1) {
            ((Godot.Collections.Dictionary)_propertyList[idx])[key] = value;
            return true;
        }
        return false;
    }

    public bool SetUsage(string property, UsageFlags usageFlags) {
        return SetProperty(property, "usage", usageFlags);
    }
    
    public bool Set(string property, object value) {
        return SetProperty(property, "value", value);
    }

    public object Get(string property) {
        int idx = FindProperty(property);
        if (idx != -1) {
            return ((Godot.Collections.Dictionary)_propertyList[idx])["value"];
        }
        return null;
    }

    private int FindProperty(string property) {
        int i = 0;
        foreach (Godot.Collections.Dictionary dict in _propertyList) {
            if ((string) dict["name"] == property) {
                return i;
            }
            ++i;
        }
        return -1;
    }
    
    public Godot.Collections.Array GetList() {
        return _propertyList;
    }

    public enum UsageFlags {
        Default = PropertyUsageFlags.Default | PropertyUsageFlags.ScriptVariable,
        Group = PropertyUsageFlags.Group,
        Category = PropertyUsageFlags.Category,
        Hidden = PropertyUsageFlags.Noeditor | PropertyUsageFlags.ScriptVariable,
        HiddenGroup = PropertyUsageFlags.Noeditor | PropertyUsageFlags.Group,
        HiddenCategory = PropertyUsageFlags.Noeditor | PropertyUsageFlags.Category
    }
}

/*
[
    {class_name:, hint:0, hint_string:, name:Node, type:0, usage:256}, 
    {class_name:, hint:18, hint_string:, name:editor_description, type:4, usage:1048578}, 
    {class_name:, hint:0, hint_string:, name:_import_path, type:15, usage:1048581}, 
    {class_name:, hint:3, hint_string:Inherit,Stop,Process, name:pause_mode, type:2, usage:7}, 
    {class_name:, hint:0, hint_string:, name:name, type:4, usage:0}, 
    {class_name:, hint:0, hint_string:, name:filename, type:4, usage:0}, 
    {class_name:Node, hint:17, hint_string:Node, name:owner, type:17, usage:0}, 
    {class_name:MultiplayerAPI, hint:17, hint_string:MultiplayerAPI, name:multiplayer, type:17, usage:0}, 
    {class_name:MultiplayerAPI, hint:17, hint_string:MultiplayerAPI, name:custom_multiplayer, type:17, usage:0}, 
    {class_name:, hint:0, hint_string:, name:process_priority, type:2, usage:7}, 

    {class_name:, hint:0, hint_string:, name:AudioStreamPlayer, type:0, usage:256}, 

    {class_name:AudioStream, hint:17, hint_string:AudioStream, name:stream, type:17, usage:7}, 
    {class_name:, hint:1, hint_string:-80,24, name:volume_db, type:3, usage:7}, 
    {class_name:, hint:1, hint_string:0.01,4,0.01,or_greater, name:pitch_scale, type:3, usage:7}, 
    {class_name:, hint:0, hint_string:, name:playing, type:1, usage:2}, 
    {class_name:, hint:0, hint_string:, name:autoplay, type:1, usage:7}, 
    {class_name:, hint:0, hint_string:, name:stream_paused, type:1, usage:7}, 
    {class_name:, hint:3, hint_string:Stereo,Surround,Center, name:mix_target, type:2, usage:7}, 
    {class_name:, hint:3, hint_string:Master,UnderWater,Reverb, name:bus, type:4, usage:7}, 
    {class_name:Script, hint:17, hint_string:Script, name:script, type:17, usage:7}, 

    {class_name:, hint:0, hint_string:, name:Script Variables, type:0, usage:256}, 
    {class_name:, hint:24, hint_string:17/17:AudioStream, name:samples, type:19, usage:8199}, 
    {class_name:, hint:0, hint_string:, name:randomSound, type:1, usage:8199}, 
    {class_name[...]
]
*/

// public static class DictExtension {
//     public static bool HasKey(this PropertyDict dict, string key) {
//         return ((GDArr) dict.Keys).Contains(key);
//         return dict.HasKey(key);
//     }
// }
