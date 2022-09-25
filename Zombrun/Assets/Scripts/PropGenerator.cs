using System.Collections.Generic;
using UnityEngine;

public class PropGenerator : Singleton<PropGenerator>
{
    [SerializeField]
    private GameObject[] propPrefabs;
    [SerializeField]
    private GameObject[] zombiePrefabs;
    [SerializeField]
    private GameObject[] lvlItemPrefabs;
    [SerializeField]
    private GameObject coinPrefab;
    public List<GameObject> props = new List<GameObject>();
    private float maxSpeed;
    [SerializeField]
    private float zombieSpeed = 2f;
    public float speed = 0;
    private float[] positions = { -5f, -2.5f, 0, 2.5f, 5f };
    private float itemsHeight = 0.5f;
    private int lvlItemsCount = 4;
    private int spawnOrder = 1;
    private float propTimePassed = 0f;
    [SerializeField]
    private int destroyPositionZ = -50;
    [SerializeField]
    private float spawnTime = 1.5f;

    void Start()
    {
        ResetLevel();
    }


    void Update()
    {
        if (speed == 0) return;

        foreach (GameObject prop in props)
        {
            if (prop.tag == "Zombie")
            {
                prop.transform.position -= new Vector3(0, 0, (speed + zombieSpeed) * Time.deltaTime);
            }
            else
            {
                prop.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            }
        }

        if (props.Count > 0 && props[0].transform.position.z < destroyPositionZ)
        {
            PoolManager.Instance.Despawn(props[0]);
            props.RemoveAt(0);
        }

        propTimePassed += Time.deltaTime;
        if (propTimePassed > spawnTime)
        {
            CreateProp();
            propTimePassed = 0;
        }
    }

    public void StartLevel()
    {
        maxSpeed = RoadGenerator.Instance.maxSpeed;
        speed = maxSpeed;
    }

    public void ResetLevel()
    {
        speed = 0;
        spawnOrder = 1;
        while (props.Count > 0)
        {
            PoolManager.Instance.Despawn(props[0]);
            props.RemoveAt(0);
        }
    }

    private void CreateProp ()
    {
        switch (spawnOrder)
        {
            case 1:
                CreateObstacle();
                break;
            case 2:
                CreateObstacle();
                break;
            case 3:
                CreateObstacle();
                break;
            case 4:
                CreateLvlItems();
                break;
            case 5:
                CreateObstacle();
                break;
            case 6:
                CreateObstacle();
                break;
            case 7:
                CreateObstacle();
                break;
            case 8:
                CreateLvlItems();
                break;
            case 9:
                CreateZombie();
                spawnOrder = 0;
                break;
        }
        spawnOrder++;
    }

    private void CreateObstacle()
    {
        Vector3 pos = Vector3.zero;
        pos = new Vector3(positions[Random.Range(0, positions.Length)], 0, transform.position.z);
        GameObject obstacle = PoolManager.Instance.Spawn(propPrefabs[Random.Range(0, propPrefabs.Length)], pos, Quaternion.identity);
        obstacle.transform.SetParent(transform);
        props.Add(obstacle);
    }

    private void CreateLvlItems()
    {
        Vector3 itemPos = Vector3.zero;
        Quaternion itemRot = Quaternion.Euler(90, 0, 180);
        float randLine = positions[Random.Range(0, positions.Length)];
        GameObject randLvlItem = lvlItemPrefabs[Random.Range(0, lvlItemPrefabs.Length)];

        for (int i = -lvlItemsCount / 2; i < lvlItemsCount / 2; i++)
        {
            Vector3 pos = Vector3.zero;
            pos = new Vector3(randLine, 0, transform.position.z);
            itemPos.y = itemsHeight;
            itemPos.z = i * (15f / lvlItemsCount);
            GameObject lvlItem = PoolManager.Instance.Spawn(randLvlItem, itemPos + pos, itemRot);
            lvlItem.transform.SetParent(transform);
            props.Add(lvlItem);
        }
    }

    public void CreateCoin(Vector3 pos, int coins)
    {
        Vector3 itemPos = Vector3.zero;
        Quaternion itemRot = Quaternion.Euler(0,0,0);
        itemPos.y = itemsHeight;
        GameObject coin = PoolManager.Instance.Spawn(coinPrefab, itemPos + pos, itemRot);
        coin.transform.SetParent(transform);
        coin.gameObject.GetComponent<Coin>().coinsCount = coins;
        props.Add(coin);
    }

    private void CreateZombie()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.Euler(0, 180, 0);
        pos = new Vector3(positions[Random.Range(0, positions.Length)], 0, transform.position.z);
        GameObject zombie = PoolManager.Instance.Spawn(zombiePrefabs[Random.Range(0, zombiePrefabs.Length)], pos, rot);
        zombie.gameObject.GetComponent<ZombieController>().ResetZombie();
        zombie.transform.SetParent(transform);
        props.Add(zombie);
    }
}

