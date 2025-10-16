using Godot;
using System;

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
    public static PackedScene LevelSummary { get; private set; }

    public static void Initialize()
    {
        LevelSummary = (PackedScene)ResourceLoader.Load("res://scenes/levels/level_summary.tscn");
    }
}