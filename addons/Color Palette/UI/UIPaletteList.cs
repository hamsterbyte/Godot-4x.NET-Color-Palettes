using Godot;
using System.Collections.Generic;
using System.IO;

namespace hamsterbyte.Colors;
[Tool]
public partial class UIPaletteList : VBoxContainer{
    [Export] private PackedScene _paletteScene;
    [Export] private LineEdit _filter;

    public override void _EnterTree(){
        ParseFiles();
    }

    private void ParseFiles(){
        string globalPath = ProjectSettings.GlobalizePath("res://addons/Color Palette/Palettes");
        string[] filePaths = Directory.GetFiles(globalPath);
        foreach (string path in filePaths){
            string correctedPath = path.Replace('\\', '/');
            string extension = Path.GetExtension(correctedPath).ToLower();
            switch (extension){
                case ".hex":
                    ParseHex(correctedPath);
                    break;
                case ".gpl":
                    ParseGPL(correctedPath);
                    break;
                default:
                    GD.Print($"Unsupported palette file type. ({extension})");
                    break;
            }
        }
    }

    private void Refresh(){
        foreach (Node child in GetChildren()){
            child.Free();
        }

        _filter.Clear();
        CallDeferred(nameof(ParseFiles));
    }

    private void ParseHex(string path){
        UIPalette palette = CreatePalette(path);
        IEnumerable<string> lines = File.ReadLines(path);
        foreach (string line in lines){
            palette?.AddSwatch(new Color(line));
        }
    }

    private void ParseGPL(string path){
        UIPalette palette = CreatePalette(path);
        IEnumerable<string> lines = File.ReadLines(path);
        foreach (string line in lines){
            if (!char.IsNumber(line[0])) continue;
            string[] split = line.Split('\t');
            palette?.AddSwatch(new Color(split[^1]));
        }
    }

    private UIPalette CreatePalette(string path){
        string globalPath = ProjectSettings.GlobalizePath(path);
        if (!File.Exists(globalPath)){
            File.Create(globalPath!).Dispose();
            GD.Print($"Palette Created: {globalPath}");
            Refresh();
            return null;
        }

        string name = path.Split('/')[^1];
        name = name.TrimEnd(Path.GetExtension(path).ToCharArray());
        UIPalette palette = _paletteScene.Instantiate<UIPalette>();
        AddChild(palette);
        palette!.SetName(name);
        palette!.SetPath(path);
        return palette;
    }

    private void ImportPalette(string path){
        string globalPath = ProjectSettings.GlobalizePath("res://addons/Color Palette/Palettes");
        string fileName = path.Split('/')[^1];
        File.Copy(path, Path.Combine(globalPath, fileName), true);
        Refresh();
    }
}