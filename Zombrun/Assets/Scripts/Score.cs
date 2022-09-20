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

    private void Awake()
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
            bestScoreText.text = $"BEST SCORE: {bestScore}";
        }
    }

    private void FixedUpdate()
    {
        score++;
        scoreText.text = $"SCORE: {score}";
    }

    public void ResetLevel()
    {
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
