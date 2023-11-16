#if TOOLS
using Godot;

namespace hamsterbyte.Colors;

[Tool]
public partial class plugin : EditorPlugin{
    private Control _dock;
    private ColorPickerButtonOverride _buttonOverride;

    public override void _EnterTree(){
        _dock = GD.Load<PackedScene>("res://addons/Color Palette/Scenes/Palettes.tscn").Instantiate<Control>();
        _dock.Name = "Palettes";
        _buttonOverride = new ColorPickerButtonOverride();
        AddControlToDock(DockSlot.RightBl, _dock);
        AddInspectorPlugin(_buttonOverride);
    }

    public override void _ExitTree(){
        RemoveControlFromDocks(_dock);
        RemoveInspectorPlugin(_buttonOverride);
        _dock.Free();
    }
}
#endif