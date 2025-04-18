using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI HighScoreText;
    public TMP_InputField playerNameInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateHighScoresText();
    }

    public void StartGame()
    {
        string playerName = playerNameInput.text;
        GameDataManager.Instance.PlayerName = playerName;
        SceneManager.LoadScene(1);
    }

    private void UpdateHighScoresText()
    {
        string[] highScoreNames = GameDataManager.Instance.HighScoreNames;
        int[] highScoreValues = GameDataManager.Instance.HighScoreValues;

        HighScoreText.text = "High Scores";
        for (int i = 0; i < highScoreNames.Length; i++)
        {
            HighScoreText.text += "\r\n" + highScoreNames[i] + "\t" + highScoreValues;
        }
    }
}
