using Godot;

namespace hamsterbyte.Colors;

[Tool]
public partial class UISwatchButton : ColorPickerButton{
    [Export] private ColorRect _color;
    [Export] private UISwatch _uiSwatch;

    private void UpdatePickerColor(InputEvent e){
        if (e is not InputEventMouseButton m) return;
        if (!m.IsPressed()) return;
        switch (m.ButtonMask){
            case MouseButtonMask.Left:
                GetPicker().Color = _color.Color;
                GetPicker().PresetsVisible = false;
                Color = _color.Color;
                break;
            case MouseButtonMask.Right:
                UIPalette palette = (UIPalette)GetParent().GetParent().GetParent().GetParent();
                palette.Swatches.Remove((UISwatch)GetParent());
                palette.SavePalette();
                GetParent().QueueFree();
                break;
        }
    }

    private void ChangeColor(Color color){
        _uiSwatch.SetColor(color);
    }

    private void SavePalette(){
        UIPalette palette = (UIPalette)GetParent().GetParent().GetParent().GetParent();
        palette.SavePalette();
    }
}