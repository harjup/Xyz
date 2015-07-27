using UnityEngine;
using System.Collections;
using DG.Tweening;

public class HoverTween : MonoBehaviour 
{
	void Start()
	{
	    transform
            .DOLocalMoveY(transform.position.y + Random.Range(.25f, .75f), Random.Range(.25f, .75f))
            .SetEase(Ease.InOutElastic)
            .SetLoops(-1, LoopType.Yoyo)
            .Play();
	}
}
