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
    protected PackedScene KeyScene = GD.Load<PackedScene>("res://scenes/key.tscn");
    protected bool isLevelDone = false;

    /// <summary>
    /// Queue that contains a tuple with Item1 referring to the seconds it should take to spawn
    /// and with Item2 referring to the index of KeyDirections and keyOffsets corresponding to the key.
    /// </summary>
    private Queue<Tuple<int, int>> keySpawnQueue = new Queue<Tuple<int, int>>();
    protected List<Key> keySpawnOrder = new List<Key>();

    public override void _Ready()
    {
        for (int i = 0; i < keySpawnTimeMs.Count; i++)
        {
            keySpawnQueue.Enqueue(new Tuple<int, int>(keySpawnTimeMs[i], i));
        }
    }

    public override void _Process(double delta)
    {
        this.SongPlaybackPosition = this.Song.GetPlaybackPosition() * 1000.0;

        if (this.keySpawnQueue.Count > 0 && this.SongPlaybackPosition >= this.keySpawnQueue.Peek().Item1 * 100 - 1680)
        {
            this.SpawnKey();
        }
        else if (this.keySpawnQueue.Count == 0 && !this.isLevelDone && !this.Song.Playing)
        {
            this.isLevelDone = true;
            _ = this.EndLevel();
        }
    }

    protected virtual void SpawnKey()
    {
        Node keyScene = this.KeyScene.Instantiate();

        if (keyScene is Key key)
        {
            int index = this.keySpawnQueue.Dequeue().Item2;

            key.SetData(this.keyDirections[index], this.keyOffsets[index]);
            key.SpawnTimeMs = this.SongPlaybackPosition;
            this.keySpawnOrder.Add(key);
            key.KeyDestroyed += this.OnKeyDestroy;
        }

        this.AddChild(keyScene);
        this.HighlightClosestKey();
    }

    protected void OnKeyDestroy(Key key)
    {
        this.keySpawnOrder.Remove(key);
        this.HighlightClosestKey();
    }

    protected void HighlightClosestKey()
    {
        if (this.keySpawnOrder.Count > 0)
        {
            this.keySpawnOrder[0].Highlight();
        }
    }

    protected async Task EndLevel()
    {
        EmitLevelEnd();
        await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
        GetTree().ChangeSceneToPacked(GameData.LevelSummary);
    }

    protected void EmitLevelEnd()
    {
        this.LevelEnd?.Invoke();
    }
}