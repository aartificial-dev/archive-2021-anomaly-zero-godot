using Godot;
using System;
// using PropertyDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, object>>;
// using PropArr = Godot.Collections.Array<Godot.Collections.Dictionary>;

[Tool]
class ItemResource: Resource {
    [Export]
    public int _c_Category;
    [Export]
    public bool PrintStuff;
    [Export]
    public bool aaa = false;
    [Export]
    public int _c_StuffThere;

}
