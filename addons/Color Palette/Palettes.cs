using System.Collections.Generic;
using System.IO;
using Godot;
using FileAccess = Godot.FileAccess;

namespace hamsterbyte.Colors;

public static class Palettes{
    #region EDITOR ONLY

    public static readonly List<Color> EditorPalette = new();

    #endregion

    private static readonly Dictionary<string, List<Color>> _palettes = new();

    private static Dictionary<string, List<Color>> _initialized{
        get{
            if (_palettes.Count == 0){
                Initialize();
            }

            return _palettes;
        }
    }

    public static List<Color> Get(string paletteName){
        return _initialized[paletteName];
    }

    public static void Initialize(){
        ParseFiles();
    }

    private static void ParseFiles(){
        string[] filePaths = DirAccess.GetFilesAt("res://addons/Color Palette/Palettes");
        foreach (string f in filePaths){
            string path = $"res://addons/Color Palette/Palettes/{f}";
            string extension = Path.GetExtension(path).ToLower();
            switch (extension){
                case ".hex":
                    ParseHex(path);
                    break;
                case ".gpl":
                    ParseGPL(path);
                    break;
                default:
                    GD.Print($"Unsupported palette file type. ({extension})");
                    break;
            }
        }
    }

    private static bool ValidatePaletteName(string path, out string name){
        string parsedName = path.Split('/')[^1];
        parsedName = parsedName.TrimEnd(Path.GetExtension(path).ToCharArray());
        name = parsedName;
        return _palettes.ContainsKey(parsedName) || _palettes.TryAdd(parsedName, new List<Color>());
    }

    private static void ParseHex(string path){
        if (!ValidatePaletteName(path, out string name)) return;
        FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        while (!file.EofReached()){
            string line = file.GetLine().Trim();
            if (line != string.Empty){
                _palettes[name].Add(new Color(line));
            }
        }
    }

    private static void ParseGPL(string path){
        if (!ValidatePaletteName(path, out string name)) return;
        FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        while (!file.EofReached()){
            string line = file.GetLine().Trim();
            if (line == string.Empty) continue;
            if (!char.IsNumber(line[0])) continue;
            string[] split = line.Split('\t');
            _palettes[name].Add(new Color(split[^1]));
        }
    }
}