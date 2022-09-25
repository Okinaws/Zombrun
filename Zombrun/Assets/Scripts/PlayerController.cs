using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public GameObject[] prefabs;
    public GameObject player;
    [NonSerialized]
    public Animator animator;
    [SerializeField]
    private float maxDistance = 5f;
    private Rigidbody rb;
    [NonSerialized]
    public Coroutine movingCoroutine;
    [NonSerialized]
    public float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float lerpDelay;
    [SerializeField]
    private float rotAngle;
    [SerializeField]
    private float rotDelay;
    private Vector3 targetPos;
    private Vector3 nextPos;
    public bool godMode = false;
    [SerializeField]
    private ParticleSystem hit;
    [SerializeField]
    private ParticleSystem shield;
    public ParticleSystem levelUp;


    void Start()
    {
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody>();
        speed = maxSpeed;
    }

    public void PlayerChange(int number)
    {
        Vector3 oldPosition = player.transform.position;
        player.SetActive(false);
        player = prefabs[number];
        player.transform.position = oldPosition;
        hit.transform.position = player.transform.position;
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody>();
        FireManager.Instance.FireUp(number);
        player.SetActive(true);
    }

    private void Awake()
    {
        player = prefabs[0];
    }

    public void Move(float deltaMove)
    {
        if (deltaMove != 0)
        {
            targetPos = player.transform.position + new Vector3(deltaMove * speed, 0, 0);
            nextPos = Vector3.Lerp(player.transform.position, targetPos, lerpDelay);
        }

        if (Math.Abs(nextPos.x) < maxDistance)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPos, lerpDelay);
        }
        else
        {
            player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(Math.Sign(targetPos.x) * maxDistance, 0, 0), 1f);
        }

        if (deltaMove > 0)
        {
            player.transform.DORotate(new Vector3(0, rotAngle, 0), rotDelay);
        }
        else if (deltaMove < 0)
        {
            player.transform.DORotate(new Vector3(0, -rotAngle, 0), rotDelay);
        }
        else
        {
            player.transform.DORotate(new Vector3(0, 0, 0), rotDelay);
        }
    }

    public void StartGame()
    {
        if (prefabs[0] == player)
        {
            animator.SetTrigger("Run");
        }
        RoadGenerator.Instance.StartLevel();
    }

    public void ResetGame()
    {
        PlayerChange(0);
        targetPos = Vector3.zero;
        rb.velocity = Vector3.zero;
        player.transform.position = Vector3.zero;
        speed = maxSpeed;
        RoadGenerator.Instance.ResetLevel();
        PropGenerator.Instance.ResetLevel();
        LevelManager.Instance.ResetLevel();
        FireManager.Instance.ResetLevel();
        Score.Instance.ResetLevel();
        MenuManager.Instance.ResetGame();
    }

    public void Death()
    {
        StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        hit.Play();
        Vibration.Instance.Vibrate(100);
        StartCoroutine(CameraShake.Instance.Shake(1f, 0.5f));
        if (LevelManager.Instance.level == 1)
        {
            animator.SetTrigger("Death");
            speed = 0;
            RoadGenerator.Instance.speed = 0;
            PropGenerator.Instance.speed = 0;
            FireManager.Instance.fireFlag = false;
            Score.Instance.isPaused = true;
        }
        else
        {
            godMode = true;
            shield.Play();
            LevelManager.Instance.LevelDown();
            if (prefabs[0] == player)
            {
                animator.SetTrigger("Run");
            }
        }
        yield return new WaitForSeconds(1.9f);

        if (!godMode)
        {
            ResetGame();
        }
        else
        {
            godMode = false;
            shield.Stop();
        }
    }
}
