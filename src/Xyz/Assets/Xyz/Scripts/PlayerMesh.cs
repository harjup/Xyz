using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PlayerMesh : MonoBehaviour
{
    private Quaternion _initialRotation;

    public void ResetRotation()
    {
        transform.localRotation = _initialRotation;
    }

	// Use this for initialization
	void Start ()
	{
        _initialRotation = transform.localRotation;
	}

    private Tweener _danceTweener;
    public void DanceTween()
    {
        if (_danceTweener == null)
        {
            ResetRotation();
            var endVal = _initialRotation.eulerAngles.SetZ(30f);
            _danceTweener = transform.DOLocalRotate(endVal, .5f)
                    .ChangeStartValue(_initialRotation.eulerAngles.SetZ(-30f))
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.Linear)
                    .Play();
        }
    }

    public void StopDancing()
    {
        if (_danceTweener != null)
        {
            _danceTweener.Kill();
            ResetRotation();
            _danceTweener = null;
        }
    }
}
