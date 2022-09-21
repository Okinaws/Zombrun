using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public GameObject[] prefabs;
    public GameObject player;
    public Animator animator;
    private Vector3 startGamePosition;
    [SerializeField]
    private float laneOffset = 2.5f;
    [SerializeField]
    private float laneChangeSpeed = 15;
    private Rigidbody rb;
    private float pointStart;
    private float pointFinish;
    private bool isMoving = false;
    [NonSerialized]
    public Coroutine movingCoroutine;
    private int sidelinesCount = 2;

    void Start()
    {
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody>();
        startGamePosition = player.transform.position;
        SwipeManager.Instance.MoveEvent += MovePlayer;
    }

    public void PlayerUp(int number)
    {
        Vector3 oldPosition = player.transform.position;
        player.SetActive(false);
        player = prefabs[number];
        player.transform.position = oldPosition;
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody>();
        FireManager.Instance.FireUp(number);
        player.SetActive(true);
    }

    private void Awake()
    {
        player = prefabs[0];
    }

    void MovePlayer(bool[] swipes)
    {
        if (swipes[(int)SwipeManager.Direction.Left] && pointFinish > sidelinesCount * -laneOffset)
        {
            Move(-laneChangeSpeed);
        }
        if (swipes[(int)SwipeManager.Direction.Right] && pointFinish < sidelinesCount * laneOffset)
        {
            Move(laneChangeSpeed);
        }
    }

    void Move(float speed)
    {
        pointStart = pointFinish;
        pointFinish += Mathf.Sign(speed) * laneOffset;
        if (isMoving)
        {
            StopCoroutine(movingCoroutine);
            isMoving = false;
        }
        movingCoroutine = StartCoroutine(MoveCoroutine(speed));
    }

    IEnumerator MoveCoroutine(float vectorX)
    {
        isMoving = true;
        while (Mathf.Abs(pointStart - player.transform.position.x) < laneOffset)
        {
            yield return new WaitForFixedUpdate();

            rb.velocity = new Vector3(vectorX, rb.velocity.y, 0);
            float x = Mathf.Clamp(player.transform.position.x, Mathf.Min(pointStart, pointFinish), Mathf.Max(pointStart, pointFinish));
            player.transform.position = new Vector3(x, player.transform.position.y, player.transform.position.z);
        }
        rb.velocity = Vector3.zero;
        player.transform.position = new Vector3(pointFinish, player.transform.position.y, player.transform.position.z);
        isMoving = false;
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
        PlayerUp(0);
        rb.velocity = Vector3.zero;
        pointStart = 0;
        pointFinish = 0;
        player.transform.position = startGamePosition;
        RoadGenerator.Instance.ResetLevel();
        PropGenerator.Instance.ResetLevel();
        LevelManager.Instance.ResetLevel();
        FireManager.Instance.ResetLevel();
        Score.Instance.ResetLevel();
        MenuManager.Instance.ResetGame();
    }
}
