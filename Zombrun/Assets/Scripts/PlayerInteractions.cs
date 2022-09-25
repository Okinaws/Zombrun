using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lose" && !PlayerController.Instance.godMode)
        {
            PlayerController.Instance.Death();
        }
        if (other.gameObject.tag == "LvlItem")
        {
            PropGenerator.Instance.props.Remove(other.gameObject);
            PoolManager.Instance.Despawn(other.gameObject);
            LevelManager.Instance.SetExperience(1f);
        }
        if (other.gameObject.tag == "Zombie" && !PlayerController.Instance.godMode)
        {
            other.gameObject.GetComponent<ZombieController>().AttackPlayer();
            PlayerController.Instance.Death();
        }
        if (other.gameObject.tag == "Coin")
        {
            PropGenerator.Instance.props.Remove(other.gameObject);
            PoolManager.Instance.Despawn(other.gameObject);
            CoinsManager.Instance.AddCoin(other.gameObject.GetComponent<Coin>().coinsCount);
        }
    }
}
