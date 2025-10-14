using Godot;
using System;
using System.Threading.Tasks;

public partial class HitFeedback : Label
{
    private Tween fadeOut;

    public override void _Ready()
    {
        this.Modulate = Color.Color8(255, 255, 255, 0);
        Key.KeyHit += UpdateText;
    }

    private void UpdateText(string text)
    {
        if (fadeOut != null && fadeOut.IsRunning())
            fadeOut?.Kill();

        this.Text = text;
        this.Modulate = Color.Color8(255, 255, 255, 255);

        fadeOut = CreateTween();
        fadeOut.TweenProperty(this, "modulate:a", 0f, 1);
        fadeOut.Play();
    }
}