using Godot;
using System;
using System.Runtime.Serialization;

public partial class RandomKeyGenerator : KeyGenerator
{
    private int lastTime;
    private Random random;
    private int startTime = -1;
    private int intervalMs = 1000;

    public override void _Ready()
    {
        SpeedSelectionButton.SpeedSelected += StartGeneration;
        this.SetProcess(false);
        this.random = new Random();
    }

    public override void _ExitTree()
    {
        SpeedSelectionButton.SpeedSelected -= StartGeneration;
    }


    public override void _Process(double delta)
    {
        if (startTime == -1)
            startTime = (int)Time.GetTicksMsec();
        int currentTime = (int)Time.GetTicksMsec();

        if (currentTime - startTime >= 3000 && currentTime - this.lastTime >= intervalMs)
        {
            this.lastTime = currentTime;
            this.SpawnKey();
        }

        if (PlayerData.NumMisses >= 3 && !isLevelDone)
        {
            this.isLevelDone = true;
            this.EndLevel();
        }
    }

    protected override void SpawnKey()
    {
        var key = KeyScene.Instantiate();

        if (key is Key k)
        {
            var keyDirectionValues = Enum.GetValues(typeof(Key.KeyDirection));
            var keyOffsetValues = Enum.GetValues(typeof(Key.KeyOffset));
            k.SetData((Key.KeyDirection)keyDirectionValues.GetValue(random.Next(keyDirectionValues.Length)),
                (Key.KeyOffset)keyOffsetValues.GetValue(random.Next(keyOffsetValues.Length)));
            this.keySpawnOrder.Add(k);
            k.KeyDestroyed += this.OnKeyDestroy;
        }

        this.AddChild(key);
        this.HighlightClosestKey();
    }

    private new void EndLevel()
    {
        this.EmitLevelEnd();
        GetTree().ChangeSceneToPacked(GameData.LevelSummary);
    }

    private void StartGeneration()
    {
        switch (GameData.KeySpeed)
        {
            case 1:
                intervalMs = 2000;
                break;
            case 2:
                intervalMs = 1500;
                break;
            case 3:
                intervalMs = 1000;
                break;
            case 4:
                intervalMs = 700;
                break;
            case 5:
                intervalMs = 600;
                break;
            case 6:
                intervalMs = 500;
                break;
            default:
                intervalMs = 1000;
                break;
        }
        this.SetProcess(true);
    }
}
