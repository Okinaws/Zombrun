using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsMovement : Singleton<AnimationsMovement>
{
    void Update()
    {
        transform.position = new Vector3(PlayerController.Instance.player.transform.position.x, transform.position.y, transform.position.z);
    }
}
