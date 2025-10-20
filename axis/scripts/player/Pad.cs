using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class Pad : Area2D
{
    public static Action<string, int> KeyHit;
    [Export] private Color padColor;
    private CollisionShape2D hitbox;
    private Sprite2D sprite;
    private bool isPadDisabled = false;
    private List<Key> keysInHitbox = new List<Key>();

    public override void _Ready()
    {
        this.AreaEntered += OnHitboxEntered;
        this.hitbox = GetNode<CollisionShape2D>("Hitbox");
        this.sprite = GetNode<Sprite2D>("Sprite");
        this.sprite.Modulate = padColor;
    }

    public async Task Activate()
    {
        this.hitbox.Disabled = false;
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        this.hitbox.Disabled = true;
    }

    public async Task HighlightPad()
    {
        Color newColor = this.sprite.Modulate;
        newColor.A = 1.0f;
        this.sprite.Modulate = newColor;
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        newColor.A = 0.6f;
        this.sprite.Modulate = newColor;
    }

    private void OnHitboxEntered(Area2D area)
    {
        if (area is Key key)
        {
            if (isPadDisabled)
            {
                return;
            }
            else
            {
                _ = DisablePad();
            }

            keysInHitbox.Add(key);

            Vector2 posDifference = (this.GlobalPosition - area.GlobalPosition).Abs();
            if (posDifference.X < 10 && posDifference.Y < 10)
            {
                KeyHit?.Invoke("Perfect!", GameData.PerfectHitValue);
                PlayerData.NumPerfects++;
            }
            else if (posDifference.X < 20 && posDifference.Y < 20)
            {
                KeyHit?.Invoke("Great!", GameData.GreatHitValue);
                PlayerData.NumGreats++;
            }
            else if (posDifference.X < 40 && posDifference.Y < 40)
            {
                KeyHit?.Invoke("Good!", GameData.GoodHitValue);
                PlayerData.NumGoods++;
            }
            else
            {
                KeyHit?.Invoke("Okay", GameData.OkayHitValue);
                PlayerData.NumOkays++;
            }

            Key firstKey = keysInHitbox.First();
            foreach (Key k in keysInHitbox)
            {
                if (firstKey.GlobalPosition.Abs().X < k.GlobalPosition.Abs().X || firstKey.GlobalPosition.Abs().Y < k.GlobalPosition.Abs().Y)
                {
                    firstKey = k;
                }
            }
            keysInHitbox.Remove(firstKey);
            firstKey.QueueFree();
        }
    }

    private async Task DisablePad()
    {
        isPadDisabled = true;
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        isPadDisabled = false;
    }
}