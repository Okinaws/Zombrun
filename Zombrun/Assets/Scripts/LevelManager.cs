using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    public int level = 1;
    public float experience { get; private set; }
    [SerializeField]
    private TextMeshProUGUI lvlText;
    [SerializeField]
    private Image expBarImage;
    [SerializeField]
    private Image[] zombieHeads;
    [SerializeField]
    private Image[] zombieHeadsDone;
    private int zombieNeed = 0;
    public int zombieKilled = 0;
    [NonSerialized]
    public int maxLevels;
    private float expNeeded;

    private void Start()
    {
        maxLevels = PlayerController.Instance.prefabs.Length;
        expNeeded = ExpNeedToLvlUp(level);
    }

    public void ZombieDone()
    {
        zombieKilled++;
        if (zombieKilled <= zombieNeed)
        {
            for (int i = 0; i < zombieKilled; i++)
            {
                zombieHeadsDone[i].gameObject.SetActive(true);
            }
        }
    }

    public static float ExpNeedToLvlUp(int currentLevel)
    {
        if (currentLevel == 0) return 0;
        return currentLevel * 10f;
    }

    public void SetExperience(float exp)
    {
        if (level == maxLevels) return;
        experience += exp;

        if(experience > expNeeded)
        {
            experience = expNeeded;
        }

        if(experience == expNeeded && zombieKilled >= zombieNeed)
        {
            expBarImage.fillAmount = 0;
            LevelUp();
        }
        if (level != maxLevels) expBarImage.fillAmount = experience / expNeeded;
    }

    public void LevelUp()
    {
        if (level == maxLevels) return;
        Vibration.Instance.Vibrate(50);
        StartCoroutine(CameraShake.Instance.Shake(1f, 0.3f));
        level++;
        expNeeded = ExpNeedToLvlUp(level);
        experience = 0;
        if (level == maxLevels)
        {
            lvlText.text = "MAX LEVEL";
            expBarImage.fillAmount = 1;
        }
        else
        {
            zombieNeed++;
            lvlText.text = $"LEVEL {level - 1}";

            for (int i = 0; i < zombieNeed; i++)
            {
                zombieHeads[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < zombieNeed; i++)
            {
                zombieHeadsDone[i].gameObject.SetActive(false);
            }
        }
        zombieKilled = 0;
        PlayerController.Instance.levelUp.Play();
        PlayerController.Instance.PlayerChange(level - 1);
    }

    public void LevelDown()
    {
        if (level == 1) return;
        level--;
        expNeeded = ExpNeedToLvlUp(level);
        experience = 0;
        expBarImage.fillAmount = 0;

        for (int i = 0; i < zombieNeed; i++)
        {
            zombieHeadsDone[i].gameObject.SetActive(false);
        }

        if (level + 1 != maxLevels)
        {
            zombieNeed--;
            zombieHeads[zombieNeed].gameObject.SetActive(false);
        }
        lvlText.text = $"LEVEL {level - 1}";
        zombieKilled = 0;
        PlayerController.Instance.PlayerChange(level - 1);
    }

    public void ResetLevel()
    {
        experience = 0;
        zombieNeed = 0;
        zombieKilled = 0;
        level = 1;
        expNeeded = ExpNeedToLvlUp(level);
        expBarImage.fillAmount = 0;
        lvlText.text = $"LEVEL 0";
        if (level != maxLevels)
        {
            for (int i = 0; i < zombieHeads.Length; i++)
            {
                zombieHeads[i].gameObject.SetActive(false);
                zombieHeadsDone[i].gameObject.SetActive(false);
            }
        }
    }
}
