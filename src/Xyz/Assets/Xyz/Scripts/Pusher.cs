using UnityEngine;
using System.Collections;

public class Pusher : MonoBehaviour
{
    public Vector3 GetPushForce(Player player)
    {
        var direction = player.transform.position - transform.position;
        const float velocity = 60f;

        return direction.normalized * velocity;
    }


}
