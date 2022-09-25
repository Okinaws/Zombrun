using System;
using TMPro;
using UnityEngine;

public class Score : Singleton<Score>
{
    public int score = 0;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    private int bestScore = 0;
    [NonSerialized]
    public bool isPaused = false;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }

        bestScoreText.text = $"BEST SCORE: {bestScore}";
    }

    private void Update()
    {
        if (!isPaused)
        {
            score++;
            scoreText.text = $"SCORE: {score}";
        }
    }

    public void ResetLevel()
    {
        isPaused = false;
        bestScoreText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        if (score > bestScore) 
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
            bestScoreText.text = $"BEST SCORE: {bestScore}";
        }
        score = 0;
        scoreText.text = $"SCORE: 0";
    }
}
