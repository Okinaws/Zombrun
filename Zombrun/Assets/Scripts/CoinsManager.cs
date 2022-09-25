using TMPro;
using UnityEngine;
using DG.Tweening;

public class CoinsManager : Singleton<CoinsManager>
{
    private int coinsCount = 0;
    [SerializeField]
    private TextMeshProUGUI coinsCountText;
    [SerializeField]
    private TextMeshProUGUI coinsAddedText;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            coinsCount = PlayerPrefs.GetInt("Coins");
        }
        coinsCountText.text = $"{coinsCount}";
    }

    public void AddCoin(int coins)
    {
        coinsAddedText.DOFade(1f, 0f);
        coinsCount += coins;
        PlayerPrefs.SetInt("Coins", coinsCount);
        coinsCountText.text = $"{coinsCount}";
        coinsAddedText.text = $"+{coins}";
        coinsAddedText.DOFade(0f, 1f);
    }
}
