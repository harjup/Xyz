using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Pusher : MonoBehaviour
{
    private Player _player;
    private GameObject _mesh;

    public void Start()
    {
        _mesh = transform.FindChild("Mesh").gameObject;
        _player = FindObjectOfType<Player>();

        Vector3 location = PushCoordinator.Instance.GetPushingSpot();
        transform.DOMove(location, 20f).SetSpeedBased().SetEase(Ease.Linear);
    }

    public void Update()
    {
        var position = GetRelativePosition(_player.transform, transform);
        LookToward(position);
    }

    // Duplicated from Chaser.cs, will need to get split into own component
    private Vector3 GetRelativePosition(Transform self, Transform other)
    {
        return other.transform.position - self.position;
    }

    private Vector3 GetRelativePosition(Vector3 self, Vector3 other)
    {
        return other - self;
    }

    // Duplicated from Chaser.cs, will need to get split into own component
    private void LookToward(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction.SetY(0), Vector3.up);
    }

    public Vector3 GetPushForce(Player player)
    {
        var direction = player.transform.position - transform.position;
        const float velocity = 60f;

        return direction.normalized * velocity;
    }

    public void PunchTowards(Vector3 position)
    {
        var targetdirection = GetRelativePosition(transform.position, position).normalized;
        transform.DOPunchPosition(targetdirection, .25f);
    }
}
