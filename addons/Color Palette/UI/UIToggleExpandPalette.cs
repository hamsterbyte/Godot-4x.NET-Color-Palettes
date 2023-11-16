using Godot;
using System;

namespace hamsterbyte.Colors;

[Tool]
public partial class UIToggleExpandPalette : TextureButton{
    [Export] private Control _swatchContainer;

    private void ToggleExpand(bool toggled){
        _swatchContainer.Visible = toggled;
    }
}