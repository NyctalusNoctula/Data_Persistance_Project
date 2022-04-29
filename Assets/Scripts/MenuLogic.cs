using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuLogic : MonoBehaviour
{
    public Dropdown DifficultyDropDown;
    public Dropdown ColorDropDown;

    public void SetNameString(InputField PlayerNameInput)
    {
        Menu.Instance.nameInput = PlayerNameInput.text;
    }

    public void SetDifficulty(Dropdown DifficultyDropDown)
    {
        Settings.Instance.DifficultyLevel = DifficultyDropDown.value;
    }

    public Color IntToColor(Dropdown ColorDrownDown)
    {
        Dictionary<int, Color> colorConverter = new Dictionary<int, Color>()
        {
            { 0 , Color.white },
            { 1, Color.black },
            { 2, Color.red },
            { 3, Color.blue }
        };

        return colorConverter[ColorDrownDown.value];
    }

    public int ColorToInt(Color color)
    {
        Dictionary<Color, int> colorConverter = new Dictionary<Color, int>()
        {
            { Color.white, 0 },
            { Color.black, 1 },
            { Color.red, 2 },
            { Color.blue, 3 }
        };

        return colorConverter[color];
    }

    public void SetColor(Dropdown ColorDrownDown)
    {
        Settings.Instance.BallColor = IntToColor(ColorDrownDown);
    }


    public void LoadColorAndDifficulty()
    {
        SaveLoad.SettingsSaveData data = SaveLoad.LoadSettings();
        if (data != null)
        {
            DifficultyDropDown.value = data.Difficulty;
            ColorDropDown.value = ColorToInt(data.BallColor);
        }

    }

    public void SaveColorAndDifficulty()
    {
        SaveLoad.SaveSettings(Settings.Instance.BallColor, Settings.Instance.DifficultyLevel);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadLeaderBoard()
    {
        SceneManager.LoadScene(3);
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
