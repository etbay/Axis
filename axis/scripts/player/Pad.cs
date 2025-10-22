using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public partial class Pad : Area2D
{
    public static Action<string, int> KeyHit;
    private CollisionShape2D hitbox;
    private Sprite2D sprite;
    private PointLight2D light;
    private bool isPadDisabled = false;
    private List<Key> keysInHitbox = new List<Key>();

    public override void _Ready()
    {
        this.AreaEntered += OnHitboxEntered;
        this.hitbox = GetNode<CollisionShape2D>("Hitbox");
        this.sprite = GetNode<Sprite2D>("Sprite");
        this.light = GetNode<PointLight2D>("PointLight");
        this.light.Visible = false;
    }

    public void SetColor(Color color)
    {
        this.sprite.Modulate = color;
        this.light.Modulate = color;

        Color newColor = this.sprite.Modulate;
        newColor.A = 0.6f;
        this.sprite.Modulate = newColor;
    }

    public async Task Activate()
    {
        this.hitbox.Disabled = false;
        this.light.Visible = true;
        await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
        this.hitbox.Disabled = true;
        this.light.Visible = false;
    }

    public void SelectPad()
    {
        Color newColor = this.sprite.Modulate;
        newColor.A = 1.0f;
        this.sprite.Modulate = newColor;
    }

    public void DeselectPad()
    {
        Color newColor = this.sprite.Modulate;
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

            string hitRating;
            Vector2 posDifference = (this.GlobalPosition - area.GlobalPosition).Abs();
            if (posDifference.X < 10 && posDifference.Y < 10)
            {
                hitRating = "Perfect!";
                PlayerData.NumPerfects++;
            }
            else if (posDifference.X < 20 && posDifference.Y < 20)
            {
                hitRating = "Great!";
                PlayerData.NumGreats++;
            }
            else if (posDifference.X < 40 && posDifference.Y < 40)
            {
                hitRating = "Good!";
                PlayerData.NumGoods++;
            }
            else
            {
                hitRating = "Okay";
                PlayerData.NumOkays++;
            }

            if (GameData.HitValues.ContainsKey(hitRating))
                KeyHit?.Invoke(hitRating, GameData.HitValues[hitRating]);

            Key firstKey = keysInHitbox.First();
            foreach (Key k in keysInHitbox)
            {
                if (firstKey.GlobalPosition.Abs().X < k.GlobalPosition.Abs().X || firstKey.GlobalPosition.Abs().Y < k.GlobalPosition.Abs().Y)
                {
                    firstKey = k;
                }
            }
            keysInHitbox.Remove(firstKey);
            firstKey.Hit(hitRating);
        }
    }

    private async Task DisablePad()
    {
        isPadDisabled = true;
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        isPadDisabled = false;
    }
}