using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Pusher : MonoBehaviour
{
    private Player _player;
    private GameObject _mesh;
    private Animator _animator;

    private enum State
    {
        Unknown,
        Idle,
        Walk
    }

    private State _state;

    public void Start()
    {
        _mesh = transform.FindChild("Mesh").gameObject;
        _player = FindObjectOfType<Player>();
        _animator = GetComponentInChildren<Animator>();

        Vector3 location = PushCoordinator.Instance.GetPushingSpot();

        _animator.Play("Walk");
        _state = State.Walk;

        StartCoroutine(MoveToPushingSpot(location));
    }

    public IEnumerator MoveToPushingSpot(Vector3 location)
    {
        while (true)
        {
            _state = State.Walk;
            _animator.Play("Walk");
            var position = GetRelativePosition(location, transform.position);
            LookToward(position);
            yield return transform.DOMove(location, 20f).SetSpeedBased().SetEase(Ease.Linear).WaitForCompletion();
            _animator.Play("Idle");
            _state = State.Idle;
            yield return new WaitForSeconds(Random.Range(.5f, 3f));
            location = PushCoordinator.Instance.GetPushingSpot();
            
        }
    }


    public void Update()
    {
        if (_state == State.Idle)
        {
            var position = GetRelativePosition(_player.transform, transform);
            LookToward(position);
        } 
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
