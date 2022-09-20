using UnityEngine;

public class LvlItem : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 1.5f;


    void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }
}
