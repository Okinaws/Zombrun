using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem blood;
    [SerializeField]
    private ParticleSystem death;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private Image healthBar;
    private Animator animator;
    private float HP;
    private float MaxHP;
    [SerializeField]
    private int HPForLevel = 10;
    private int level;
    private float dieTime = 1.9f;


    void Start()
    {
        ResetZombie();
    }

    public void ResetZombie()
    {
        animator = GetComponent<Animator>();
        gameObject.tag = "Zombie";
        int playerLevel = LevelManager.Instance.level - 1;
        level = Random.Range(playerLevel, playerLevel + 2);
        if (level == 0) level = 1;
        MaxHP = HPForLevel * level;
        HP = MaxHP;
        levelText.text = $"LEVEL {level}";
        healthBar.fillAmount = 1;
    }

    public void AttackPlayer()
    {
        animator.SetTrigger("Attack");
    }

    private IEnumerator Die()
    {
        healthBar.fillAmount = 0;
        gameObject.tag = "Untagged";
        death.Play();
        animator.SetTrigger("Death");
        FireManager.Instance.fireFlag = false;
        LevelManager.Instance.ZombieDone();
        LevelManager.Instance.SetExperience((float)level * 2);
        PropGenerator.Instance.CreateCoin(transform.position, level);
        yield return new WaitForSeconds(dieTime);
        PoolManager.Instance.Despawn(gameObject);
        PropGenerator.Instance.props.Remove(gameObject);
    }

    public void Hit(float damage)
    {
        HP -= damage;
        blood.Play();        
        if (HP > 0)
        {
            healthBar.fillAmount = HP / MaxHP;
        }
        else
        {
            StartCoroutine(Die());
        }
    }
}