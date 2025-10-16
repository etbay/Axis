using Godot;
using System;

public partial class StatDisplay : Control
{
    [Export] private Label numHitRatings;
    [Export] private Label hitRatingScores;
    [Export] private Label totalScore;

    public override void _Ready()
    {
        SaveGame();

        this.numHitRatings.Text = "x" + PlayerData.NumPerfects + "\nx" + PlayerData.NumGreats + "\nx" + PlayerData.NumGoods
            + "\nx" + PlayerData.NumOkays + "\nx" + PlayerData.NumMisses;
        this.hitRatingScores.Text = (PlayerData.NumPerfects * GameData.PerfectHitValue).ToString() + "\n"
            + (PlayerData.NumGreats * GameData.GreatHitValue).ToString() + "\n"
            + (PlayerData.NumGoods * GameData.GoodHitValue).ToString() + "\n"
            + (PlayerData.NumOkays * GameData.OkayHitValue).ToString() + "\n0";
        this.totalScore.Text = "Total: " + (PlayerData.NumPerfects * GameData.PerfectHitValue 
            + PlayerData.NumGreats * GameData.GreatHitValue + PlayerData.NumGoods * GameData.GoodHitValue
            + PlayerData.NumOkays * GameData.OkayHitValue).ToString();
    }

    private void SaveGame()
    {
        PlayerData.SaveData();
    }

}
