using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string _bestScoreName;
    private int _bestScorePoints;
    
    [SerializeField]
    private Text _bestScoreText;

    [SerializeField]
    private int _difficultyLevel;
    private Color _ballColor;
    
    [SerializeField]
    private Text _difficultytext;

    [SerializeField]
    private MeshRenderer Renderer;

    // Start is called before the first frame update
    void Start()
    {
        LoadScore();
        LoadColorAndDifficulty();
        UpdateBestScoreText();
        UpdateDifficultyText();

        Material material = Renderer.material;

        material.color = _ballColor;
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                if (_difficultyLevel == 0)
                {
                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
                }
                else if (_difficultyLevel == 1)
                {
                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 4.0f, ForceMode.VelocityChange);
                }
                else if (_difficultyLevel == 2)
                {
                    Ball.transform.SetParent(null);
                    Ball.AddForce(forceDir * 6.0f, ForceMode.VelocityChange);
                }
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        SaveLoad.SaveScore(Menu.Instance.nameInput, m_Points);

        SceneManager.LoadScene(3);
    }

    private void LoadScore()
    {
        Score data = SaveLoad.GetHighestScore();
        if (data != null)
        {
            _bestScoreName = data.Name;
            _bestScorePoints = data.Points;
        }

    }

    public void LoadColorAndDifficulty()
    {
        if (Settings.Instance != null)
        {
            _difficultyLevel = Settings.Instance.DifficultyLevel;
            _ballColor = Settings.Instance.BallColor;
        }
        else
        {
            _difficultyLevel = 1;
            _ballColor = Color.white;
        }
    }

    private void UpdateBestScoreText()
    {
        _bestScoreText.text = $"Best Score : {_bestScoreName} : {_bestScorePoints}";
    }

    private void UpdateDifficultyText()
    {
        Dictionary<int, string> difficultyConverter = new Dictionary<int, string>()
        {
            { 0, "Difficulty: Easy" },
            { 1, "Difficulty: Medium" },
            { 2, "Difficulty: Hard" },
        };

        _difficultytext.text = $"{difficultyConverter[_difficultyLevel]}";
    }
}
