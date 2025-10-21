using Godot;
using System;

public partial class StatDisplay : Control
{
    private Label numHitRatingsLabel;
    private Label hitRatingScoresLabel;
    private Label totalScoreLabel;
    private Button mainMenuButton;

    public override void _Ready()
    {
        this.numHitRatingsLabel = GetNode<Label>("Stats/NumHitRatingsLabel");
        this.hitRatingScoresLabel = GetNode<Label>("Stats/HitRatingScoresLabel");
        this.totalScoreLabel = GetNode<Label>("TotalScoreLabel");
        this.mainMenuButton = GetNode<Button>("MainMenuButton");
        this.mainMenuButton.Pressed += LoadMainMenu;

        SaveGame();

        this.numHitRatingsLabel.Text = "x" + PlayerData.NumPerfects + "\nx" + PlayerData.NumGreats + "\nx" + PlayerData.NumGoods
            + "\nx" + PlayerData.NumOkays + "\nx" + PlayerData.NumMisses;
        this.hitRatingScoresLabel.Text = (PlayerData.NumPerfects * GameData.PerfectHitValue).ToString() + "\n"
            + (PlayerData.NumGreats * GameData.GreatHitValue).ToString() + "\n"
            + (PlayerData.NumGoods * GameData.GoodHitValue).ToString() + "\n"
            + (PlayerData.NumOkays * GameData.OkayHitValue).ToString() + "\n0";
        this.totalScoreLabel.Text = "Total: " + (PlayerData.NumPerfects * GameData.PerfectHitValue 
            + PlayerData.NumGreats * GameData.GreatHitValue + PlayerData.NumGoods * GameData.GoodHitValue
            + PlayerData.NumOkays * GameData.OkayHitValue).ToString();
    }

    private void SaveGame()
    {
        PlayerData.SaveData();
    }

    private void LoadMainMenu()
    {
        this.GetTree().ChangeSceneToPacked(GameData.MainMenu);
    }
}
