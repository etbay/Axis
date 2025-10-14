using Godot;
using System;
using System.Threading.Tasks;
using System.Timers;

public partial class Pad : Node2D
{
    [Export] private Color padColor;
    private CollisionShape2D hitboxCollision;
    private Sprite2D sprite;

    public override void _Ready()
    {
        this.hitboxCollision = GetChild<Area2D>(1).GetChild<CollisionShape2D>(0);
        this.sprite = GetChild<Sprite2D>(0);
        this.sprite.Modulate = padColor;
    }

    public async Task Activate()
    {
        this.hitboxCollision.Disabled = false;
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        this.hitboxCollision.Disabled = true;
    }

    public async Task ChangeColor(Color color)
    {
        this.sprite.Modulate = color;
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        this.sprite.Modulate = Color.Color8(255, 255, 255, 255);
    }
}