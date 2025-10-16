using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

public partial class KeyGenerator : Node2D
{
    [Export] private PackedScene KeyScene;
    [Export] private Godot.Collections.Array<int> keySpawnTimeSeconds;
    [Export] private Godot.Collections.Array<Key.KeyDirection> keyDirections;
    [Export] private Godot.Collections.Array<Key.KeyOffset> keyOffsets;
    private int startTime;
    private bool isLevelDone = false;

    /// <summary>
    /// Queue that contains a tuple with Item1 referring to the seconds it should take to spawn
    /// and with Item2 referring to the index of KeyDirections and keyOffsets corresponding to the key.
    /// </summary>
    private Queue<Tuple<int, int>> keySpawnQueue = new Queue<Tuple<int, int>>();

    public override void _Ready()
    {
        startTime = (int)(Time.GetTicksMsec() / 1000);
        for (int i = 0; i < keySpawnTimeSeconds.Count; i++)
        {
            keySpawnQueue.Enqueue(new Tuple<int, int>(keySpawnTimeSeconds[i], i));
        }
    }

    public override void _Process(double delta)
    {
        if (keySpawnQueue.Count > 0 && keySpawnQueue.Peek().Item1 == (int)(Time.GetTicksMsec() / 1000) - startTime)
        {
            var key = KeyScene.Instantiate();

            if (key is Key k)
            {
                int index = keySpawnQueue.Dequeue().Item2;

                k.SetData(keyDirections[index], keyOffsets[index]);
            }

            this.AddChild(key);
        }
        else if (keySpawnQueue.Count == 0 && !isLevelDone)
        {
            isLevelDone = true;
            _ = LoadLevelSummary();
        }
    }

    private async Task LoadLevelSummary()
    {
        await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
        GetTree().ChangeSceneToPacked(GameData.LevelSummary);
    }
}