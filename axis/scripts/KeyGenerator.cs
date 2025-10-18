using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Markup;

public partial class KeyGenerator : Node2D
{
    public event Action LevelEnd;
    public AudioStreamPlayer2D Song { private get; set; }
    public double SongPlaybackPosition { get; private set; }

    [Export] private Godot.Collections.Array<int> keySpawnTimeMs;
    [Export] private Godot.Collections.Array<Key.KeyDirection> keyDirections;
    [Export] private Godot.Collections.Array<Key.KeyOffset> keyOffsets;
    [Export] private int levelIndex;    // get rid of this
    private PackedScene KeyScene = GD.Load<PackedScene>("res://scenes/key.tscn");
    private int startTime;
    private bool isLevelDone = false;

    /// <summary>
    /// Queue that contains a tuple with Item1 referring to the seconds it should take to spawn
    /// and with Item2 referring to the index of KeyDirections and keyOffsets corresponding to the key.
    /// </summary>
    private Queue<Tuple<int, int>> keySpawnQueue = new Queue<Tuple<int, int>>();
    private List<Key> keySpawnOrder = new List<Key>();

    public override void _Ready()
    {
        startTime = (int)Time.GetTicksMsec();
        for (int i = 0; i < keySpawnTimeMs.Count; i++)
        {
            keySpawnQueue.Enqueue(new Tuple<int, int>(keySpawnTimeMs[i], i));
        }
    }

    public override void _Process(double delta)
    {
        SongPlaybackPosition = Song.GetPlaybackPosition() * 1000.0;

        if (keySpawnQueue.Count > 0 && SongPlaybackPosition >= keySpawnQueue.Peek().Item1 * 100 - 1680)
        {
            SpawnKey();
        }
        else if (keySpawnQueue.Count == 0 && !isLevelDone && !Song.Playing)
        {
            isLevelDone = true;
            this.LevelEnd?.Invoke();
            _ = EndLevel();
        }
    }

    private void SpawnKey()
    {
        Node key = KeyScene.Instantiate();

        if (key is Key k)
        {
            int index = keySpawnQueue.Dequeue().Item2;

            k.SetData(keyDirections[index], keyOffsets[index]);
            k.SpawnTimeMs = SongPlaybackPosition;
            this.keySpawnOrder.Add(k);
            k.KeyDestroyed += OnKeyDestroy;
        }

        this.AddChild(key);
        HighlightClosestKey();
    }

    private void OnKeyDestroy(Key key)
    {
        this.keySpawnOrder.Remove(key);
        HighlightClosestKey();
    }

    private void HighlightClosestKey()
    {
        if (this.keySpawnOrder.Count > 0)
        {
            this.keySpawnOrder[0].Highlight();
        }
    }

    private async Task EndLevel()
    {
        await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
        GetTree().ChangeSceneToPacked(GameData.LevelSummary);
    }
}