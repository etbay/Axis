using Godot;
using System;
using System.Threading.Tasks;

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
    private Sprite2D keyLengthTail;
    private Color color;
    private PointLight2D light;
    private Sprite2D hitEffect;
    private Node2D keyLength;
    private Vector2 keyLengthScale;
    private bool isHit;
    private bool isHeld;
    public KeyType HitType { get; private set; }
    public int KeyLengthMs { get; private set; }
    public double SpawnTimeMs { get; set; } = 0;

    public override void _Ready()
    {
        this.Position = this.spawnPosition;
        this.sprite.Modulate = this.color;
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
        if (!isHit)
            KeyDestroyed?.Invoke(this);
    }

    public void Initialize()
    {
        this.sprite = GetNode<Sprite2D>("Sprite");
        this.light = GetNode<PointLight2D>("PointLight");
        this.hitEffect = GetNode<Sprite2D>("HitEffect");
        this.hitEffect.Visible = false;
        this.keyLength = GetNode<Node2D>("KeyLength");
        this.keyLength.Visible = false;
        this.keyLengthTail = GetNode<Sprite2D>("KeyLength/KeyLengthSprite/KeyLengthTailSprite");
    }

    public void SetData(KeyDirection keyDirection, KeyOffset keyOffset, KeyType keyType, int lengthMs)
    {
        this.Initialize();

        // sets key data for direction key is traveling
        switch (keyDirection)
        {
            case KeyDirection.UP:
                this.direction = Vector2.Up;
                this.spawnPosition = Vector2.Down * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.DownColor;
                this.light.Color = GameData.DownColor;
                this.keyLength.RotationDegrees = 90;
                break;
            case KeyDirection.DOWN:
                this.direction = Vector2.Down;
                this.spawnPosition = Vector2.Up * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.UpColor;
                this.light.Color = GameData.UpColor;
                this.keyLength.RotationDegrees = 270;
                break;
            case KeyDirection.LEFT:
                this.direction = Vector2.Left;
                this.spawnPosition = Vector2.Right * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.RightColor;
                this.light.Color = GameData.RightColor;
                break;
            case KeyDirection.RIGHT:
                this.direction = Vector2.Right;
                this.spawnPosition = Vector2.Left * (int)(GameData.WindowSize.X / 2);
                this.color = GameData.LeftColor;
                this.light.Color = GameData.LeftColor;
                this.keyLength.RotationDegrees = 180;
                break;
        }
        this.color.A = 0.5f;

        Vector2 spawnDirection = this.spawnPosition.Normalized().Abs();
        // offsets key spawn depending on the direction it is coming from
        switch (keyOffset)
        {
            case KeyOffset.NEGATIVE:
                // example: KeyDirection is RIGHT
                // (-360, 0) += (0, 1) * -50 = (-360, -50)
                // key will spawn 50px above centered spawn
                this.spawnPosition += new Vector2(spawnDirection.Y, spawnDirection.X) * -GameData.KeyOffsetDistance;
                break;
            case KeyOffset.POSITIVE:
                this.spawnPosition += new Vector2(spawnDirection.Y, spawnDirection.X) * GameData.KeyOffsetDistance;
                break;
        }

        if (keyType == Key.KeyType.HOLD)
        {
            GD.Print("Setting holding key to be held for " + (float)lengthMs / 1000 + " seconds");
            this.HitType = KeyType.HOLD;
            this.KeyLengthMs = lengthMs;
            this.keyLength.Modulate = this.color;
            this.keyLengthTail.Modulate = this.color;
            this.keyLengthScale = new Vector2((float)KeyLengthMs / 500, 1);
            this.keyLength.Scale = keyLengthScale;
            this.keyLength.Visible = true;
        }
    }

    public void Highlight()
    {
        Color newColor = this.sprite.Modulate;
        newColor.A = 1.0f;
        this.sprite.Modulate = newColor;
        this.light.Energy = 1;
    }

    public void Hit(string hitRating)
    {
        if (this.isHit)
        {
            return;
        }

        this.isHit = true;
        this.hitEffect.Modulate = this.color;

        if (this.direction == Vector2.Up || this.direction == Vector2.Down)
        {
            this.hitEffect.RotationDegrees = 0;
        }

        switch (hitRating)
        {
            case "Perfect!":
                break;
            case "Great!":
                this.hitEffect.Scale = this.hitEffect.Scale * 0.8f;
                break;
            case "Good!":
                this.hitEffect.Scale = this.hitEffect.Scale * 0.6f;
                break;
            case "Okay":
                this.hitEffect.Scale = this.hitEffect.Scale * 0f;
                break;
        }

        this.hitEffect.Visible = true;
        _ = FadeOut();
    }

    public void Hold()
    {
        this.SetProcess(false);
        KeyDestroyed?.Invoke(this);
    }

    public bool ShortenKey(double scaleFactor)
    {
        if (scaleFactor <= 0)
        {
            this.keyLength.Scale = new Vector2((float)scaleFactor, 1);
            return false;
        }
        this.keyLength.Scale = keyLengthScale * new Vector2((float)scaleFactor, 1);
        return true;
    }

    public double GetRemainingKeyLengthPercentage()
    {
        return 1 - (this.keyLength.Scale / this.keyLengthScale).X;
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

    private async Task FadeOut()
    {
        KeyDestroyed?.Invoke(this);

        this.SetProcess(false);
        this.sprite.Visible = false;
        this.light.Visible = false;

        Tween fadeInTween = CreateTween();
        fadeInTween.TweenProperty(this.hitEffect, "modulate:a", 0.35f, 0.1f);
        fadeInTween.Play();
        await ToSignal(fadeInTween, "finished");

        Tween keyLengthFadeOutTween = CreateTween();
        keyLengthFadeOutTween.TweenProperty(this.keyLength, "modulate:a", 0f, 0.3f);
        keyLengthFadeOutTween.Play();

        Tween keyLengthTailFadeOutTween = CreateTween();
        keyLengthTailFadeOutTween.TweenProperty(this.keyLengthTail, "modulate:a", 0f, 0.3f);
        keyLengthTailFadeOutTween.Play();

        Tween fadeOutTween = CreateTween();
        fadeOutTween.TweenProperty(this.hitEffect, "modulate:a", 0f, 0.3f);
        fadeOutTween.Play();
        await ToSignal(fadeOutTween, "finished");
        
        this.QueueFree();
    }
}