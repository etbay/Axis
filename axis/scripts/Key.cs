using Godot;
using System;

public partial class Key : Area2D
{
    public event Action<Key> KeyDestroyed;
    public static bool IsInLevel;
    public enum KeyDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    public enum KeyOffset
    {
        NEGATIVE, NONE, POSITIVE
    }

    public enum KeyType
    {
        NORMAL, HOLD
    }

    private Vector2 spawnPosition;
    private Vector2 direction;
    private Sprite2D sprite;
    private Color color;
    private PointLight2D light;
    private Area2D hitbox;
    public double SpawnTimeMs { get; set; } = 0;

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
            this.light = GetChild<PointLight2D>(1);

        // sets key data for direction key is traveling
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
        this.color.A = 0.5f;

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
        this.sprite = GetNode<Sprite2D>("Sprite");
        this.light = GetNode<PointLight2D>("PointLight");
        this.sprite.Modulate = this.color;
        this.Position = this.SpawnPosition;
    }

    public override void _Process(double delta)
    {
        if (!IsInLevel)
        {
            this.Position += this.direction * GameData.KeySpeed;
        }
        else
        {
            MoveLevelKey();
        }
    }

    public override void _ExitTree()
    {
        KeyDestroyed?.Invoke(this);
    }

    public void Highlight()
    {
        Color newColor = this.sprite.Modulate;
        newColor.A = 1.0f;
        this.sprite.Modulate = newColor;
    }

    private void MoveLevelKey()
    {
        if (GetParent() is KeyGenerator generator)
        {
            double songTimeMs = generator.SongPlaybackPosition;
            double t = (songTimeMs - SpawnTimeMs) / GameData.KeyTravelTime;
            this.Position = this.spawnPosition + (this.direction * (GameData.KeyTravelDistance + 5) * (float)t);
        }
    }
}