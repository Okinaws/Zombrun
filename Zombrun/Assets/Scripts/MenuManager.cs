using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private GameObject startBtn;
    [SerializeField]
    private GameObject pauseBtn;

    public void ResetGame()
    {
        startBtn.SetActive(true);
        pauseBtn.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Score.Instance.isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Score.Instance.isPaused = false;
    }
}
