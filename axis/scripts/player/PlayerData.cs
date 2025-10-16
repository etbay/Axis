using Godot;
using System;

public static class PlayerData
{
    public static int NumPerfects { get; set; } = 0;
    public static int NumGreats { get; set; } = 0;
    public static int NumGoods { get; set; } = 0;
    public static int NumOkays { get; set; } = 0;
    public static int NumMisses { get; set; } = 0;
    public static int TotalScore { get; set; } = 0;

    public static void Reset()
    {
        NumPerfects = 0;
        NumGreats = 0;
        NumGoods = 0;
        NumOkays = 0;
        NumMisses = 0;
    }
}