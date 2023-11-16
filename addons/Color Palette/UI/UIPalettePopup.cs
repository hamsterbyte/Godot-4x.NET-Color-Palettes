using Godot;

namespace hamsterbyte.Colors;

public partial class UIPalettePopup : PopupPanel{
    private void CloseIfFocusLost(){
        Hide();
    }
}