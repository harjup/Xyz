using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    // How quickly the camera accelerates to keep up with the player.
    // Keep it under 1f.
    public float cameraSnapX = .1f;
    public float cameraSnapZ = .4f;

    // Distance the player can move away from the camera center before it starts trying to recenter.
    public float cameraFreeX = 2.5f;
    public float cameraFreeZ = 1.25f;

    //TODO: These should be acquired in start eventually. Bad.
    public GameObject player;
    public GameObject cameraCenter;

	private void FixedUpdate () {
        MoveCamera(cameraCenter.transform, player.transform);
	}

    public void MoveCamera(Transform cameraTransform, Transform targetTransform)
    {
        // TODO: Maybe let's use DoTween and have it be reasonably smooth instead of falling on our old iTween business
        var positionDifference = targetTransform.position - cameraTransform.position;
        float xSpeed = Mathf.Abs(positionDifference.x) * cameraSnapX;
        float zSpeed = Mathf.Abs(positionDifference.z) * cameraSnapZ;

        //Cap the camera's speed so it doesn't go fucking nuts and start overshooting the player
        if (xSpeed > 60) { xSpeed = 60; }
        if (zSpeed > 60) { zSpeed = 60; }

        if (Mathf.Abs(positionDifference.x) >= cameraFreeX)
        {
            cameraTransform.position = cameraTransform.position.SetX(iTween.FloatUpdate(cameraTransform.position.x, targetTransform.position.x, xSpeed));
        }
        if (Mathf.Abs(positionDifference.z) >= cameraFreeZ)
        {
            cameraTransform.position = cameraTransform.position.SetZ(iTween.FloatUpdate(cameraTransform.position.z, targetTransform.position.z, zSpeed));
        }
    }
}
