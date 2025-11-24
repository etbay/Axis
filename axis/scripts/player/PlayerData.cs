using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public static class PlayerData
{
	public static event Action<int> TotalScoreChanged;
	public static int NumPerfects { get; set; } = 0;
	public static int NumGreats { get; set; } = 0;
	public static int NumGoods { get; set; } = 0;
	public static int NumOkays { get; set; } = 0;
	public static int NumMisses { get; set; } = 0;
	private static int totalScore = 0;
	public static int TotalScore
	{
		get
		{
			return totalScore;
		}
		
		set
		{
			totalScore = value;
			TotalScoreChanged?.Invoke(totalScore);
		} 
	}

	public static Dictionary<int, int> LevelScores { get; set; } = new Dictionary<int, int>();
	public static Dictionary<int, int> EndlessScores { get; set; } = new Dictionary<int, int>();

	public static void Reset()
	{
		NumPerfects = 0;
		NumGreats = 0;
		NumGoods = 0;
		NumOkays = 0;
		NumMisses = 0;
		totalScore = 0;
	}

	public static void ResetSaveData()
	{
		LevelScores.Clear();
		EndlessScores.Clear();
	}

	public static void SaveData()
	{
		var data = new PlayerSaveData
		{
			LevelScores = new Dictionary<int, int>(LevelScores),
			EndlessScores = new Dictionary<int, int>(EndlessScores)
		};

		string json = JsonSerializer.Serialize(data);

		var file = FileAccess.Open("user://player_scores.json", FileAccess.ModeFlags.Write);
		if (file != null)
		{
			file.StoreString(json);
			file.Close();
		}
	}

	public static void LoadData()
	{
		if (!FileAccess.FileExists("user://player_scores.json"))
			return;

		var file = FileAccess.Open("user://player_scores.json", FileAccess.ModeFlags.Read);
		if (file != null)
		{
			string json = file.GetAsText();
			file.Close();

			var data = JsonSerializer.Deserialize<PlayerSaveData>(json);
			if (data != null)
			{
				LevelScores = new Dictionary<int, int>(data.LevelScores);
				EndlessScores = new Dictionary<int, int>(data.EndlessScores);
			}
		}
	}

	private class PlayerSaveData
	{
		public Dictionary<int, int> LevelScores { get; set; } = new Dictionary<int, int>();
		public Dictionary<int, int> EndlessScores { get; set; } = new Dictionary<int, int>();
	}
}
