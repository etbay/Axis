using Godot;
using System;

public partial class LevelSelect : Control
{
    public override void _Ready()
    {
        Control levelContainer = GetNode<Control>("Levels");

        for (int i = 0; i < levelContainer.GetChildCount(); i++)
        {
            if (levelContainer.GetChild(i) is Button levelSelectButton)
            {
                int levelIndex = i + 1;

                Label highScoreLabel = levelSelectButton.GetNode<Label>("HighScoreLabel");
                if (PlayerData.LevelScores.ContainsKey(levelIndex))
                    highScoreLabel.Text = "High Score: " + PlayerData.LevelScores[levelIndex];
                else
                    highScoreLabel.Text = "No High Score";

                levelSelectButton.Pressed += () => { this.GetTree().ChangeSceneToPacked(GameData.Levels[levelIndex]); };
            }
        }
    }

    private void LoadLevelOne()
    {
        this.GetTree().ChangeSceneToPacked(GameData.Levels[1]);
    }
}
