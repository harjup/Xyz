using UnityEngine;
using System.Collections;

public class OffsetAnimationRandomAmount : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        StartCoroutine(StartWithRandomOffset());
    }

    private IEnumerator StartWithRandomOffset()
    {
        var animator = GetComponent<Animator>();
        animator.speed = 0f;
        yield return new WaitForSeconds(Random.Range(0.1f, 1f));
        animator.speed = Random.Range(.5f, 1f);
    }

}
