using System.Collections.Generic;
using System.Threading.Tasks;

namespace hamsterbyte.Colors;

// RandomIntEditor.cs
#if TOOLS
using Godot;

[Tool]
public partial class ColorPickerPropertyOverride : EditorProperty{
    // The main control for editing the property.
    private PackedScene _customColorPicker =
        ResourceLoader.Load<PackedScene>("res://addons/Color Palette/Scenes/CustomColorPicker.tscn");

    private PackedScene _swatchScene =
        ResourceLoader.Load<PackedScene>("res://addons/Color Palette/Scenes/PopupSwatch.tscn");

    private Control _control;
    private ColorPickerButton _propertyControl;
    private Button _paletteButton;
    private PopupPanel _palettePanel;
    private HFlowContainer _popupSwatches;

    private Color _currentValue;
    private bool _updating;

    public override void _EnterTree(){
        _propertyControl.Color = (Color)GetEditedObject().Get(GetEditedProperty());
    }

    public ColorPickerPropertyOverride(){
        GatherReferences();
        AddControls();
        SetupCallbacks();
    }

    private void GatherReferences(){
        _control = _customColorPicker.Instantiate<Control>();
        _propertyControl = (ColorPickerButton)_control.GetChild(0);
        _propertyControl.GetPicker().PresetsVisible = false;
        _paletteButton = (Button)_control.GetChild(1);
        _palettePanel = (PopupPanel)_control.GetChild(2);
        _popupSwatches = (HFlowContainer)_palettePanel.GetChild(0);
        PropertyCanRevertChanged += Revert;
    }

    private void Revert(StringName property, bool canRevert){
        if (canRevert) return;
        _currentValue = Colors.White;
        _propertyControl.Color = Colors.White;
        _propertyControl.GetPicker().Color = Colors.White;
    }

    private void AddControls(){
        AddChild(_control);
        AddFocusable(_control);
    }

    private async void PopulateSwatches(List<Color> colors){
        foreach (Node child in _popupSwatches.GetChildren()){
            child.Free();
        }

        ResetSizes(colors.Count);

        await AwaitDeferred();
        foreach (Color color in colors){
            UIPopupSwatch swatch = _swatchScene.Instantiate<UIPopupSwatch>();
            _popupSwatches.AddChild(swatch);
            swatch.SetColor(color);
            Button btn = (Button)swatch.GetChild(0);
            btn.Pressed += () => {
                _palettePanel.Hide();
                _propertyControl.Color = color;
                _currentValue = color;
                EmitChanged(GetEditedProperty(), _currentValue);
            };
        }
    }

    private Task AwaitDeferred(){
        return Task.Run(() => {
            ulong startFrame = Engine.GetProcessFrames();
            while (Engine.GetProcessFrames() < startFrame + 1){
                if (Engine.GetProcessFrames() < startFrame) return;
                Task.Delay(1);
            }
        });
    }

    private void SetupCallbacks(){
        _propertyControl.ColorChanged += OnColorChanged;
        _paletteButton.Pressed += ShowPalette;
    }

    private void ResetSizes(int swatchCount){
        int rows = swatchCount / 10;
        int height = Mathf.Max(30, rows * 30 + 10);
        _popupSwatches.Size = new Vector2(_popupSwatches.Size.X, height);
        _palettePanel.Size = new Vector2I(_palettePanel.Size.X, height);
    }

    private void ShowPalette(){
        if (Palettes.EditorPalette.Count == 0){
            AcceptDialog dialog = new();
            dialog.DialogText = "No palette set. Use the Palettes dock to assign a color palette.";
            dialog.Title = "No Palette Set";
            dialog.Position = (Vector2I)GetScreenPosition();
            dialog.Show();
            return;
        }

        _palettePanel.Visible = true;
        _palettePanel.Position = (Vector2I)GetScreenPosition();
        PopulateSwatches(Palettes.EditorPalette);
    }

    private void OnColorChanged(Color color){
        // Ignore the signal if the property is currently being updated.
        if (_updating){
            return;
        }

        _currentValue = _propertyControl.GetPicker().Color;
        EmitChanged(GetEditedProperty(), _currentValue);
    }

    public override void _UpdateProperty(){
        // Read the current value from the property.
        Color newValue = (Color)GetEditedObject().Get(GetEditedProperty());
        if (newValue == _currentValue){
            return;
        }

        // Update the control with the new value.
        _updating = true;
        _currentValue = newValue;
        _updating = false;
    }
}
#endif