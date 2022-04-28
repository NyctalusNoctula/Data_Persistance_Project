using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


#region Scores

[Serializable]
public class ScoreTable
{
    public List<Score> Scores = new List<Score>();

    public ScoreTable()
    {
        // for serializer
    }

    public ScoreTable([NotNull] List<Score> scores)
    {
        Scores = scores ?? throw new ArgumentNullException(nameof(scores));
    }
}

[Serializable]
public class Score
{
    public string Name;
    public int Points;

    public Score()
    {
        // for serializer
    }

    public Score(string name, int points)
    {
        Name = name;
        Points = points;
    }
}

public class SaveLoad
{
    public static void SaveScore(string name, int points)
    {
        ScoreTable data = LoadScore();

        Score newScore = new Score(name, points);

        data.Scores.Add(newScore);

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public static ScoreTable LoadScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            ScoreTable data = JsonUtility.FromJson<ScoreTable>(json);
            return data;
        }

        return null;
    }

    public static Score GetHighestScore()
    {
        var scores = LoadScore().Scores.OrderByDescending(s => s.Points).Take(1);
        Score s = scores.Count() > 0 ? scores.First() : null;
        return s;
    }


    public static IEnumerable<Score> GetHighestScores() => LoadScore().Scores.OrderByDescending(s => s.Points).Take(10);


    #endregion

    #region Settings

    [System.Serializable]
    public class SettingsSaveData
    {
        public Color BallColor;
        public int Difficulty;
    }

    public static void SaveSettings(Color colorToSave, int difficulty)
    {
        SettingsSaveData data = new SettingsSaveData();
        data.BallColor = colorToSave;
        data.Difficulty = difficulty;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/settings.json", json);
    }

    public static SettingsSaveData LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SettingsSaveData data = JsonUtility.FromJson<SettingsSaveData>(json);

            return data;
        }

        return null;
    }

    #endregion

}
