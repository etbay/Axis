using Godot;
using System;
using System.Runtime.Serialization;

public partial class RandomKeyGenerator : Node
{
    private PackedScene KeyScene;
    private int lastTime;
    private Random random;
    private int startTime = -1;
    private int intervalMs = 1000;

    public override void _Ready()
    {
        SpeedSelectionButton.SpeedSelected += StartGeneration;
        this.SetProcess(false);
        this.random = new Random();
        this.KeyScene = (PackedScene)ResourceLoader.Load("res://scenes/key.tscn");
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
            GenerateKey();
        }

        if (PlayerData.NumMisses >= 3)
        {
            if (PlayerData.EndlessScores.ContainsKey(GameData.KeySpeed)
                && PlayerData.EndlessScores[GameData.KeySpeed] < PlayerData.TotalScore)
            {
                PlayerData.EndlessScores[GameData.KeySpeed] = PlayerData.TotalScore;
            }
            else if (!PlayerData.EndlessScores.ContainsKey(GameData.KeySpeed))
            {
                PlayerData.EndlessScores[GameData.KeySpeed] = PlayerData.TotalScore;
            }
            GetTree().ChangeSceneToPacked(GameData.LevelSummary);
        }
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

    private void GenerateKey()
    {
        var key = KeyScene.Instantiate();

        if (key is Key k)
        {
            var keyDirectionValues = Enum.GetValues(typeof(Key.KeyDirection));
            var keyOffsetValues = Enum.GetValues(typeof(Key.KeyOffset));
            k.SetData((Key.KeyDirection)keyDirectionValues.GetValue(random.Next(keyDirectionValues.Length)),
                (Key.KeyOffset)keyOffsetValues.GetValue(random.Next(keyOffsetValues.Length)));
        }

        this.AddChild(key);
    }
}
