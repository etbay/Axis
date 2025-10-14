using Godot;
using System;

public partial class Key : Node2D
{
    public static Action<string> KeyHit;
    public enum KeyDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    public enum KeyOffset
    {
        NEGATIVE = -1, NONE = 0, POSITIVE = 1
    }

    public enum KeyType
    {
        NORMAL, HOLD
    }

    private Vector2 spawnPosition;
    private Vector2 direction;
    private int speed = 3;
    private Sprite2D sprite;
    private Color color;
    private PointLight2D light;
    private Area2D hitbox;

    public Vector2 SpawnPosition
    {
        get { return this.spawnPosition; }
        set { this.spawnPosition = value;  }
    }
    
    public Vector2 Direction
    {
        get { return this.direction; }
        set { this.direction = value; }
    }

    public Color Color
    {
        get { return this.color; }
        set { this.color = value; }
    }

    public void SetData(KeyDirection keyDirection, KeyOffset keyOffset)
    {
        if (this.light == null)
            this.light = GetChild<PointLight2D>(2);

        switch (keyDirection)
        {
            case KeyDirection.UP:
                this.direction = Vector2.Up;
                this.SpawnPosition = Vector2.Down * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.DownColor;
                this.light.Color = GameData.DownColor;
                break;
            case KeyDirection.DOWN:
                this.direction = Vector2.Down;
                this.SpawnPosition = Vector2.Up * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.UpColor;
                this.light.Color = GameData.UpColor;
                break;
            case KeyDirection.LEFT:
                this.direction = Vector2.Left;
                this.SpawnPosition = Vector2.Right * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.RightColor;
                this.light.Color = GameData.RightColor;
                break;
            case KeyDirection.RIGHT:
                this.direction = Vector2.Right;
                this.SpawnPosition = Vector2.Left * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.LeftColor;
                this.light.Color = GameData.LeftColor;
                break;
        }

        Vector2 spawnDirection = this.SpawnPosition.Normalized().Abs();
        // offsets key spawn depending on the direction it is coming from
        switch (keyOffset)
        {
            case KeyOffset.NEGATIVE:
                // example: KeyDirection is RIGHT
                // (-360, 0) += (0, 1) * -50 = (-360, -50)
                // key will spawn 50px above centered spawn
                this.SpawnPosition += new Vector2(spawnDirection.Y, spawnDirection.X) * -GameData.KeyOffsetDistance;
                break;
            case KeyOffset.POSITIVE:
                this.SpawnPosition += new Vector2(spawnDirection.Y, spawnDirection.X) * GameData.KeyOffsetDistance;
                break;
        }
    }

    public override void _Ready()
    {
        this.sprite = GetChild<Sprite2D>(0);
        this.hitbox = GetChild<Area2D>(1);
        this.light = GetChild<PointLight2D>(2);
        this.hitbox.AreaEntered += OnHitboxEntered;
        this.sprite.Modulate = this.color;
        this.Position = this.SpawnPosition;
    }

    public override void _PhysicsProcess(double delta)
    {
        this.Position += this.direction * this.speed;
    }

    private void OnHitboxEntered(Area2D area)
    {
        if (area.Name != "PadHitbox")
        {
            if (area.Name == "KillzoneHitbox")
                KeyHit?.Invoke("Miss");
    
            this.QueueFree();
            return;
        }

        Vector2 posDifference = (this.GlobalPosition - area.GlobalPosition).Abs();
        if (posDifference.X < 10 && posDifference.Y < 10)
        {
            KeyHit?.Invoke("Perfect!");
        }
        else if (posDifference.X < 20 && posDifference.Y < 20)
        {
            KeyHit?.Invoke("Great!");
        }
        else if (posDifference.X < 40 && posDifference.Y < 40)
        {
            KeyHit?.Invoke("Good!");
        }
        else
        {
            KeyHit?.Invoke("Okay");
        }
        this.QueueFree();
    }
}