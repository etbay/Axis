using Godot;
using System;

public static class PlayerData
{
    public static int NumPerfects { get; set; } = 10;
    public static int NumGreats { get; set; } = 5;
    public static int NumGoods { get; set; } = 3;
    public static int NumOkays { get; set; } = 5;
    public static int NumMisses { get; set; } = 4;
    public static int TotalScore { get; set; } = 0;
}