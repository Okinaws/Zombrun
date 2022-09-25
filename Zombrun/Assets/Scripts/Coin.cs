using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 1.5f;
    public int coinsCount = 1;


    void Update()
    {
        transform.Rotate(new Vector3(0, rotateSpeed, 0));
    }
}
