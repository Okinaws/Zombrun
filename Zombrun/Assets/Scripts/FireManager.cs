using UnityEngine;

public class FireManager : Singleton<FireManager>
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject fireBullet;
    [SerializeField]
    private ParticleSystem fire;
    [SerializeField]
    private float startTimeFire = 0;
    private float timeFire = 0;
    public bool fireFlag = false;
    private Quaternion spawnBulletRotation = Quaternion.Euler(90, 0, 0);

    void Update()
    {
        transform.position = new Vector3(PlayerController.Instance.player.transform.position.x, transform.position.y, transform.position.z);

        if (startTimeFire > 0) timeFire -= Time.deltaTime;

        if (fireFlag && timeFire < 0)
        {
            if (LevelManager.Instance.level == LevelManager.Instance.maxLevels)
            {
                PoolManager.Instance.Spawn(fireBullet, new Vector3(transform.position.x, 1f, 0), spawnBulletRotation);
            } 
            else
            {
                PoolManager.Instance.Spawn(bullet, new Vector3(transform.position.x, 1f, 0), spawnBulletRotation);
            }
            timeFire = startTimeFire;
        }
        
        if (LevelManager.Instance.level == LevelManager.Instance.maxLevels && fireFlag)
        {
            fire.Play();
        }
        else
        {
            fire.Stop();
        }
    }

    public void FireUp(int level)
    {
        if (level != 0) startTimeFire = 1f / (float)level * 0.75f;
        else
        {
            startTimeFire = 0;
            timeFire = 0;
        }
    }

    public void ResetLevel()
    {
        startTimeFire = 0;
        fireFlag = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            fireFlag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            fireFlag = false;
        }
    }
}
