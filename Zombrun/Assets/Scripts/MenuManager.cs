using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private GameObject startBtn;
    [SerializeField]
    private GameObject pauseBtn;
    [SerializeField]
    private GameObject devToolsBtn;
    [SerializeField]
    private Toggle devToolsToggle;

    private void Awake()
    {
        startBtn.transform.DOScale(new Vector3(6f, 6f, 6f), 1f).SetLoops(-1, LoopType.Yoyo);
    }

    public void ResetGame()
    {
        startBtn.SetActive(true);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        Score.Instance.isPaused = true;
        SwipeManager.Instance.isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Score.Instance.isPaused = false;
        SwipeManager.Instance.isPaused = false;
    }

    public void setDevTools()
    {
        devToolsBtn.SetActive(devToolsToggle.isOn);
    }
}
