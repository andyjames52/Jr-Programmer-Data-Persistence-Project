using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    private string SaveDataFilePath;

    public string[] HighScoreNames;
    public int[] HighScoreValues;

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
    class SaveData
    {
        public string[] HighScoreNames;
        public int[] HighScoreValues;
    }

    public void SaveHighScores()
    {
        Debug.Log("SaveHighScores starting");
        SaveData data = new SaveData();
        data.HighScoreNames = new string[HighScoreNames.Length];
        Array.Copy(HighScoreNames, data.HighScoreNames, HighScoreNames.Length);

        data.HighScoreValues = new int[HighScoreValues.Length];
        Array.Copy(HighScoreValues, data.HighScoreValues, HighScoreValues.Length);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(SaveDataFilePath, json);
        Debug.Log("SaveHighScores finished");
    }

    public void LoadHighScores()
    {
        Debug.Log("LoadHighScores starting");
        if (File.Exists(SaveDataFilePath))
        {
            Debug.Log("reading file data");
            string json = File.ReadAllText(SaveDataFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            HighScoreNames = new string[data.HighScoreNames.Length];
            Array.Copy(data.HighScoreNames, HighScoreNames, data.HighScoreNames.Length);

            HighScoreValues = new int[data.HighScoreValues.Length];
            Array.Copy(data.HighScoreValues, HighScoreValues, data.HighScoreValues.Length);
        }
        else
        {
            // Create empty score array
        }
            Debug.Log("LoadHighScores finished");
    }

    public void CheckAndAddHighScore(int scoreValue)
    {
        Debug.Log("CheckAndAddHighScore starting");

        // Add an empty element at the end if it's less than max size
        if (HighScoreNames.Length < MaxNumHighScores)
        {
            Debug.Log("appending array element");
            HighScoreNames.Append("");
            HighScoreValues.Append(-1);
        }

        int prevLowestScore = HighScoreValues[HighScoreValues.Length - 1];
        if (scoreValue <= prevLowestScore)
        {
            Debug.Log("score doesn't make the board.  returning");
            return;
        }

        // Insert new score into the array following the last >= score
        Debug.Log("determining insertIdx");
        int insertIdx = -1;
        for (int i = 0; insertIdx < 0 && i < HighScoreValues.Length; i++)
        {
            if (HighScoreValues[i] < scoreValue)
            {
                insertIdx = i;
            }
        }
        Debug.Log("insertIdx=" + insertIdx);

        // Start with last element and work back.  Insert new score along the way
        Debug.Log("processing arrays");
        int currentArrayLength = HighScoreNames.Length;
        for (int i = MaxNumHighScores-1; i >= 0; i--)
        {
            if (i < insertIdx)
            {
                // Do nothing as high score is better than new score and no change needs to be made
                Debug.Log("do nothing case");
            }
            else if (i == insertIdx)
            {
                // Insert new score
                Debug.Log("insert case");
                HighScoreNames[i] = PlayerName;
                HighScoreValues[i] = scoreValue;
            }
            else
            {
                // Copy the next highest score into this index (i.e., shift down)
                Debug.Log("shift down case");
                HighScoreNames[i] = HighScoreNames[i - 1];
                HighScoreValues[i] = HighScoreValues[i - 1];
            }
        }

        // Save high scores since they were updated
        Debug.Log("calling SaveHighScores");
        SaveHighScores();
        Debug.Log("CheckAndAddHighScore finished");
    }
}
