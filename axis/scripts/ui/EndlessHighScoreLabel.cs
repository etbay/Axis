using Godot;
using System;

public partial class EndlessHighScoreLabel : Label
{
    [Export] private int speed;

    public override void _Ready()
    {
        if (PlayerData.EndlessScores.ContainsKey(speed))
        {
            this.Text = "High Score: " + PlayerData.EndlessScores[speed];
        }
        else
        {
            this.Text = "No High Score";
        }
    }
}
