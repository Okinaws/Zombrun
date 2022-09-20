using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 50f;
    public float damage = 3f;
    public int destroyPositionZ = 120;

    void Update()
    {
        transform.position -= new Vector3(0, 0, -speed * Time.deltaTime);

        if (transform.position.z > destroyPositionZ) PoolManager.Instance.Despawn(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            other.gameObject.GetComponent<ZombieController>().Hit(damage);
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}
