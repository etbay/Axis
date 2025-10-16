using Godot;
using System;
using System.Threading.Tasks;

public partial class GameUserInterface : Control
{
    private Tween fadeOut;
    private Label hitRating;
    private Label points;

    public override void _Ready()
    {
        this.hitRating = GetChild<Label>(0);
        this.points = GetChild<Label>(1);
        this.points.Text = "0";
        this.hitRating.Modulate = Color.Color8(255, 255, 255, 0);
        Key.KeyHit += OnKeyHit;
    }

    public override void _ExitTree()
    {
        Key.KeyHit -= OnKeyHit;
    }

    private void OnKeyHit(string text, int pointValue)
    {
        if (fadeOut != null && fadeOut.IsRunning())
            fadeOut?.Kill();

        this.hitRating.Text = text;
        this.points.Text = (int.Parse(this.points.Text) + pointValue).ToString();
        PlayerData.TotalScore = int.Parse(this.points.Text);
        this.hitRating.Modulate = Color.Color8(255, 255, 255, 255);

        fadeOut = CreateTween();
        fadeOut.TweenProperty(this.hitRating, "modulate:a", 0f, 1);
        fadeOut.Play();
    }
}