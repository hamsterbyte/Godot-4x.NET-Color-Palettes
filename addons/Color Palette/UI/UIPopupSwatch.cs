using Godot;
using System;

namespace hamsterbyte.Colors;

[Tool]
public partial class UIPopupSwatch : Control{
    [Export] private ColorRect _color;
    [Export] private Button _button;
    public Button Button => _button;

    public Color Color{ get; private set; }

    public void SetColor(Color color){
        _color.Color = color;
        Color = color;
    }
}