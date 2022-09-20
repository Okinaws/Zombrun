using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : Singleton<RoadGenerator>
{
    [SerializeField]
    private GameObject RoadPrefab;
    [SerializeField]
    private GameObject FirstRoadPrefab;
    private List<GameObject> roads = new List<GameObject>();
    public float maxSpeed = 10;
    [NonSerialized]
    public float speed = 0;
    [SerializeField]
    private int maxRoadCount = 7;
    [SerializeField]
    private int destroyPositionZ = -100;
    [SerializeField]
    private Vector3 newRoadPosition = new Vector3(0, 0, 49);

    void Start()
    {
        PoolManager.Instance.PreLoad(RoadPrefab, maxRoadCount);
        ResetLevel();
    }

    void Update()
    {
        if (speed == 0) return;

        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (roads[0].transform.position.z < destroyPositionZ)
        {
            PoolManager.Instance.Despawn(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad();
        }
    }

    public void StartLevel()
    {
        speed = maxSpeed;
        SwipeManager.Instance.enabled = true;
        Score.Instance.enabled = true;
    }

    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;
        GameObject go;
        if (roads.Count > 0)
        {
            if (roads.Count == 1)
            {
                pos = roads[roads.Count - 1].transform.position + 2 * newRoadPosition;
            }
            else
            {
                pos = roads[roads.Count - 1].transform.position + newRoadPosition;
            }
        }
        if (roads.Count == 0)
        {
            go = PoolManager.Instance.Spawn(FirstRoadPrefab, pos, Quaternion.identity);
        }
        else
        {
            go = PoolManager.Instance.Spawn(RoadPrefab, pos, Quaternion.identity);
        }
        go.transform.SetParent(transform);
        roads.Add(go);
    }

    public void ResetLevel()
    {
        speed = 0;
        while (roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);
        }
        for (int i = 1; i < maxRoadCount; i++)
        {
            CreateNextRoad();
        }

        SwipeManager.Instance.enabled = false;
        Score.Instance.enabled = false;
    }
}
