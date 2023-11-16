using Godot;
using hamsterbyte.Colors;

public partial class Test : Control{
    [Export] private ColorRect _colorRect;

    public override void _EnterTree(){
        _colorRect.Color = Palettes.Get("antiquity16")[7];
    }
}