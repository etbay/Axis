using Godot;
using System;

public static class GameData
{
    private static int keyOffsetDistance = 50;
    private static Color upColor = Color.Color8(0, 255, 255, 255);      // cyan
    private static Color downColor = Color.Color8(255, 0, 255, 255);    // magenta
    private static Color leftColor = Color.Color8(0, 255, 0, 255);      // lime
    private static Color rightColor = Color.Color8(255, 165, 0, 255);   // orange
    private static Vector2 windowSize = new Vector2(720, 720);

    public static int KeyOffsetDistance { get { return keyOffsetDistance; } }

    public static Color UpColor { get { return upColor; } }
    public static Color DownColor { get { return downColor; } }
    public static Color LeftColor { get { return leftColor; } }
    public static Color RightColor { get { return rightColor; } }

    public static Vector2 WindowSize { get { return windowSize; } }
}