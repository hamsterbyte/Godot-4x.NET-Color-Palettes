using Godot;

namespace hamsterbyte.Colors;

[Tool]
public partial class UISwatch : Control{
    [Export] private ColorRect _color;
    [Export] private ColorPickerButton _colorPickerButton;

    public Color Color{ get; private set; }

    public void SetColor(Color color){
        _color.Color = color;
        _colorPickerButton.Color = color;
        Color = color;
    }
}