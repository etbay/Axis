using Godot;
using System;
using System.Collections.Generic;

public static class GameData
{
    public static int KeyOffsetDistance { get; } = 50;
    public static Color UpColor { get; } = Color.Color8(0, 255, 255, 255);      // cyan
    public static Color DownColor { get; } = Color.Color8(255, 0, 255, 255);    // magenta
    public static Color LeftColor { get; } = Color.Color8(0, 255, 0, 255);      // lime
    public static Color RightColor { get; } = Color.Color8(255, 165, 0, 255);   // orange
    public static Vector2 WindowSize { get; } = new Vector2(720, 720);
    public static int PerfectHitValue { get; } = 100;
    public static int GreatHitValue { get; } = 50;
    public static int GoodHitValue { get; } = 30;
    public static int OkayHitValue { get; } = 15;
    public static PackedScene MainMenu { get; } = GD.Load<PackedScene>("res://scenes/levels/main_menu.tscn");
    public static PackedScene Settings { get; } = GD.Load<PackedScene>("res://scenes/levels/settings.tscn");
    public static PackedScene EndlessMode { get; } = GD.Load<PackedScene>("res://scenes/levels/endless_mode.tscn");
    public static PackedScene LevelSelection { get; } = GD.Load<PackedScene>("res://scenes/levels/level_select.tscn");
    public static Dictionary<int, PackedScene> Levels { get; } = new Dictionary<int, PackedScene>()
    {
        { 1, GD.Load<PackedScene>("res://scenes/levels/level_one.tscn") },
        { 2, GD.Load<PackedScene>("res://scenes/levels/level_two.tscn") }
    };
    
    public static PackedScene LevelSummary { get; } = GD.Load<PackedScene>("res://scenes/levels/level_summary.tscn");

    public static int KeySpeed { get; set; } = 2;
    public static int KeyTravelDistance { get; } = 260;
    public static int KeyTravelTime { get; } = 1600;
}