using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScorePanel : MonoBehaviour
{
    [SerializeField]
    private Text _scoresText;

    private void Awake()
    {
        PrintScores();
    }

    private void PrintScores()
    {
        string scoresString = string.Empty;

        var scores = SaveLoad.GetHighestScores();
        foreach (var score in scores)
            scoresString += $"{score.Name}: {score.Points}\n";

        _scoresText.text = scoresString;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
