using Godot;
using System;

public partial class Killzone : Area2D
{
	public static Action<string, int> KeyHit;

	public override void _Ready()
	{
		this.AreaEntered += OnHitboxEntered;
	}

	private void OnHitboxEntered(Area2D area)
	{
		if (area is Key key)
		{
			KeyHit?.Invoke("Miss", 0);
			PlayerData.NumMisses++;
			key.QueueFree();
		}
	}
}
