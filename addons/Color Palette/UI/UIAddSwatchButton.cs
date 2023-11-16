using Godot;

namespace hamsterbyte.Colors;

[Tool]
public partial class UIAddSwatchButton : TextureButton{
    public void AddNewSwatch(){
        UIPalette palette = (UIPalette)GetParent().GetParent().GetParent();
        palette.AddSwatch(Godot.Colors.White);
        palette.SavePalette();
    }
}