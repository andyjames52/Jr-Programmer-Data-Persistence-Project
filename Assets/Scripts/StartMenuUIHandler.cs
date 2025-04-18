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
        HighScoreText.text = "";
        for (int i = 0; i < GameDataManager.Instance.HighScoreEntries.Length; i++)
        {
            HighScoreText.text += "\r\n" + GameDataManager.Instance.HighScoreEntries[i].name + "\t" + GameDataManager.Instance.HighScoreEntries[i].score;
        }
    }
}
