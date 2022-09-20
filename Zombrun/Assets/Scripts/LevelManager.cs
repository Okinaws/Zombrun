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
    private int maxLevels;

    private void Start()
    {
        maxLevels = PlayerController.Instance.prefabs.Length;
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

    public static int ExpNeedToLvlUp(int currentLevel)
    {
        if (currentLevel == 0) return 0;
        return (currentLevel * currentLevel + currentLevel) * 5;
    }

    public void SetExperience(float exp)
    {
        if (level == maxLevels) return;
        experience += exp;
        float expNeeded = ExpNeedToLvlUp(level);

        if(experience > expNeeded)
        {
            experience = expNeeded;
        }

        float previousExperience = ExpNeedToLvlUp(level - 1);

        if(experience == expNeeded && zombieKilled >= zombieNeed)
        {
            expBarImage.fillAmount = 0;
            LevelUp();
            expNeeded = ExpNeedToLvlUp(level);
            previousExperience = ExpNeedToLvlUp(level - 1);
        }
        if (level != maxLevels) expBarImage.fillAmount = (experience - previousExperience) / (expNeeded - previousExperience);
    }

    public void LevelUp()
    {
        if (level == maxLevels) return;
        level++;
        zombieNeed++;
        if (level == maxLevels)
        {
            lvlText.text = "MAX LEVEL";
            expBarImage.fillAmount = 1;
        }
        else
        {
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
        PlayerController.Instance.PlayerUp(level - 1);
    }

    public void ResetLevel()
    {
        experience = 0;
        zombieNeed = 0;
        zombieKilled = 0;
        level = 1;
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
