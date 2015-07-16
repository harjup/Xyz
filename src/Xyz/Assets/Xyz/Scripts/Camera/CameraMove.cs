using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    public GameObject cameraCenter;

    private Vector3 velocity = Vector3.zero;

	void Start () 
    {
	
	}
	
	private void FixedUpdate () {
        MoveCamera(cameraCenter.transform, player.transform);
	}

    public void MoveCamera(Transform cameraTransform, Transform targetTransform)
    {
        // TODO: Maybe let's use DoTween and have it be reasonably smooth instead of falling on our old iTween business
        var positionDifference = targetTransform.position - cameraTransform.position;
        float xSpeed = Mathf.Abs(positionDifference.x) * .1f;
        float zSpeed = Mathf.Abs(positionDifference.z) * .1f;

        //Cap the camera's speed so it doesn't go fucking nuts and start overshooting the player
        if (xSpeed > 40) { xSpeed = 40; }
        if (zSpeed > 40) { zSpeed = 40; }

        if (Mathf.Abs(positionDifference.x) >= 10)
        {
            cameraTransform.position = cameraTransform.position.SetX(iTween.FloatUpdate(cameraTransform.position.x, targetTransform.position.x, xSpeed));
        }
        if (Mathf.Abs(positionDifference.z) >= 10f)
        {
            cameraTransform.position = cameraTransform.position.SetZ(iTween.FloatUpdate(cameraTransform.position.z, targetTransform.position.z, zSpeed));
        }
    }
}
