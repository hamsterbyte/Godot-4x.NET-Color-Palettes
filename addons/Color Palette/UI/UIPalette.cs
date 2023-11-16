using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

namespace hamsterbyte.Colors;
[Tool]
public partial class UIPalette : PanelContainer{
    [Export] private PackedScene _swatchScene;
    [Export] private Label _nameLabel;
    [Export] private HFlowContainer _swatchFlow;
    public List<UISwatch> Swatches = new();
    public string Path{ get; private set; }

    public void SetName(string name){
        Name = name;
        _nameLabel.Text = Name;
    }

    public void SetPath(string path){
        Path = path;
    }

    private void MakeGlobalSwatches(){
        UpdateEditorPalette(Swatches, _nameLabel.Text);
    }
    
    /// <summary>
    /// This method is used by the editor plugin. Do not invoke this at runtime
    /// </summary>
    /// <param name="swatches"></param>
    /// <param name="name"></param>
    private void UpdateEditorPalette(List<UISwatch> swatches, string name){
        Palettes.EditorPalette.Clear();
        foreach (UISwatch swatch in swatches){
            Palettes.EditorPalette.Add(swatch.Color);
        }

        GD.Print($"Palette Changed: {name}");
    }

    public void AddSwatch(Color color){
        Control scene = _swatchScene.Instantiate<Control>();
        UISwatch swatch = scene as UISwatch;
        Swatches.Add(swatch);
        swatch!.SetColor(color);
        _swatchFlow.AddChild(scene);
        _swatchFlow.MoveChild(scene, _swatchFlow.GetChildCount() - 2);
    }

    public void SavePalette(){
        string extension = System.IO.Path.GetExtension(Path);
        switch (extension){
            case ".hex":
                SaveHex();
                break;
            case ".gpl":
                SaveGPL();
                break;
        }
    }

    private void SaveHex(){
        string[] lines = new string[Swatches.Count];
        for (int i = 0; i < lines.Length; i++){
            lines[i] = Swatches[i].Color.ToHtml(false);
        }

        File.WriteAllLines(Path, lines);
        GD.Print($"{_nameLabel.Text} saved.");
    }

    private void SaveGPL(){
        // Read all lines and filter
        string[] header = File.ReadAllLines(Path).Where(line => !char.IsNumber(line[0])).ToArray();

        // Initialize lines array with the exact required size
        string[] lines = new string[Swatches.Count + header.Length];

        // Update the "#Colors: " line if it exists
        int colorLineIndex = Array.FindIndex(header, line => line.StartsWith("#Colors: "));
        if (colorLineIndex >= 0) {
            header[colorLineIndex] = $"#Colors: {Swatches.Count}";
        }

        // Copy the header to lines
        Array.Copy(header, lines, header.Length);

        // Populate the rest of the lines array
        for (int i = 0; i < Swatches.Count; i++){
            Color c = Swatches[i].Color;
            lines[i + header.Length] = $"{c.R8}\t{c.G8}\t{c.B8}\t{c.ToHtml(false)}";
        }

        // Write all lines to the file
        File.WriteAllLines(Path, lines);
        GD.Print($"{_nameLabel.Text} saved.");
    }
}