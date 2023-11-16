using System.Linq;
using Godot;

namespace hamsterbyte.Colors;

[Tool]
public partial class UIPalletes : Control{
    [Export] private Control _paletteList;
    [Export] private FileDialog _createPaletteDialog;
    [Export] private FileDialog _importPaletteDialog;

    private void FilterPalettes(string filter){
        // Convert filter to lower case once to avoid doing it in each iteration
        string lowerCaseFilter = filter.ToLower();

        // Use LINQ to simplify the loop
        foreach (Control control in _paletteList.GetChildren().OfType<Control>()){
            // Check if the control's name contains the filter
            control.Visible = control.Name.ToString()!.ToLower().Contains(lowerCaseFilter);
        }
    }

    private void ShowCreatePaletteDialog(){
        _createPaletteDialog.Show();
    }

    private void ShowImportPaletteDialog(){
        _importPaletteDialog.Show();
    }
}