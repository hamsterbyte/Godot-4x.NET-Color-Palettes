#if TOOLS
using Godot;

namespace hamsterbyte.Colors;

public partial class ColorPickerButtonOverride : EditorInspectorPlugin{
    public override bool _CanHandle(GodotObject @object){
        return true;
    }

    public override bool _ParseProperty(GodotObject @object, Variant.Type type,
        string name, PropertyHint hintType, string hintString,
        PropertyUsageFlags usageFlags, bool wide){
        if (type != Variant.Type.Color) return false;
        AddPropertyEditor(name, new ColorPickerPropertyOverride());
        return true;
    }
}
#endif