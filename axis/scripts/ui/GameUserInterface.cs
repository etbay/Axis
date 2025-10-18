using Godot;
using System;
using System.Threading.Tasks;

public partial class GameUserInterface : Control
{
    private Tween fadeOutTween;
    private Label hitRatingLabel;
    private Label pointsLabel;

    public override void _Ready()
    {
        this.hitRatingLabel = GetChild<Label>(0);
        this.pointsLabel = GetChild<Label>(1);
        this.pointsLabel.Text = "0";
        this.hitRatingLabel.Modulate = Color.Color8(255, 255, 255, 0);
        PlayerData.TotalScoreChanged += UpdateScore;
    }

    public override void _ExitTree()
    {
        PlayerData.TotalScoreChanged -= UpdateScore;
    }

    public void OnKeyHit(string text, int pointValue)
    {
        if (fadeOutTween != null && fadeOutTween.IsRunning())
            fadeOutTween?.Kill();

        this.hitRatingLabel.Text = text;
        PlayerData.TotalScore += pointValue;
        this.hitRatingLabel.Modulate = Color.Color8(255, 255, 255, 255);

        fadeOutTween = CreateTween();
        fadeOutTween.TweenProperty(this.hitRatingLabel, "modulate:a", 0f, 1);
        fadeOutTween.Play();
    }
    private void UpdateScore(int score)
    {
        this.pointsLabel.Text = score.ToString();
    }
}