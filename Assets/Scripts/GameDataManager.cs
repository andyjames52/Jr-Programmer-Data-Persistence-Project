using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    private string SaveDataFilePath;

    public HighScoreEntry[] HighScoreEntries;

    public string PlayerName;
    private int MaxNumHighScores = 5;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SaveDataFilePath = Application.persistentDataPath + "/savefile.json";

        LoadHighScores();
    }

    [System.Serializable]
    public class HighScoreEntry
    {
        public string name;
        public int score;

        public HighScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }
    }

    [System.Serializable]
    class SaveData
    {
        public HighScoreEntry[] HighScoreEntries;
    }

    public void SaveHighScores()
    {
        SaveData data = new SaveData();

        data.HighScoreEntries = new HighScoreEntry[HighScoreEntries.Length];
        for (int i = 0; i < HighScoreEntries.Length; i++)
        {
            data.HighScoreEntries[i] = new HighScoreEntry(HighScoreEntries[i].name, HighScoreEntries[i].score);
        }

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(SaveDataFilePath, json);
    }

    public void LoadHighScores()
    {
        HighScoreEntries = new HighScoreEntry[MaxNumHighScores];

        if (File.Exists(SaveDataFilePath))
        {
            string json = File.ReadAllText(SaveDataFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // Load high score entries
            int numDataScores = data.HighScoreEntries.Length;
            for (int i = 0; i < MaxNumHighScores; i++)
            {
                if (i < numDataScores) {
                    // Load data entry
                    HighScoreEntries[i] = new HighScoreEntry(data.HighScoreEntries[i].name, data.HighScoreEntries[i].score);
                }
                else
                {
                    // Load empty score array
                    HighScoreEntries[i] = new HighScoreEntry("------", 0);
                }
            }
        }
        else
        {
            // Create empty score array
            for (int i = 0; i  < MaxNumHighScores; i++)
            {
                HighScoreEntries[i] = new HighScoreEntry("------", 0);
            }
        }
    }

    public void CheckAndAddHighScore(int scoreValue)
    {
        int prevLowestScore = HighScoreEntries[HighScoreEntries.Length - 1].score;
        if (scoreValue <= prevLowestScore)
        {
            return;
        }

        // Insert new score into the array following the last > score
        int insertIdx = MaxNumHighScores;
        for (int i = 0; i < MaxNumHighScores; i++)
        {
            if (scoreValue > HighScoreEntries[i].score)
            {
                insertIdx = i;
                break;
            }
        }

        if (insertIdx ==  MaxNumHighScores)
        {
            return;
        }

        // Start with last element and work back.  Insert new score along the way
        for (int i = MaxNumHighScores - 1; i >= 0; i--)
        {
            if (i < insertIdx)
            {
                // Do nothing as high score is better than new score and no change needs to be made
            }
            else if (i == insertIdx)
            {
                // Insert new score
                HighScoreEntries[i] = new HighScoreEntry(PlayerName, scoreValue);
            }
            else
            {
                // Copy the next highest score into this index (i.e., shift down)
                HighScoreEntries[i] = new HighScoreEntry(HighScoreEntries[i - 1].name, HighScoreEntries[i - 1].score);
            }
        }

        // Save high scores since they were updated
        SaveHighScores();
    }
}
