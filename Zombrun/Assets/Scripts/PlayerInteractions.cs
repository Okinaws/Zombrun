using System.Collections;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private IEnumerator Die()
    {
        PlayerController.Instance.animator.SetTrigger("Death");
        PlayerController.Instance.movingCoroutine = null;
        RoadGenerator.Instance.speed = 0;
        PropGenerator.Instance.speed = 0;
        FireManager.Instance.fireFlag = false;
        Score.Instance.isPaused = true;

        yield return new WaitForSeconds(1.9f);
        PlayerController.Instance.ResetGame();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lose")
        {
            StartCoroutine(Die());
        }
        if (other.gameObject.tag == "LvlItem")
        {
            PropGenerator.Instance.props.Remove(other.gameObject);
            PoolManager.Instance.Despawn(other.gameObject);
            LevelManager.Instance.SetExperience(0.5f);
        }
        if (other.gameObject.tag == "Zombie")
        {
            other.gameObject.GetComponent<ZombieController>().AttackPlayer();
            StartCoroutine(Die());
        }
    }
}
