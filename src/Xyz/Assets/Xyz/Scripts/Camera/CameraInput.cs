using UnityEngine;
using System.Collections;
using Assets.Scripts.Managers;

public class CameraInput : MonoBehaviour
{
    private InputManager _inputManager;
    private Camera _camera;

    public void Start()
    {
        _inputManager = InputManager.Instance;
        _camera = Camera.main;
    }

    public Vector3 GetInputDirection()
    {
        return GetInputDirection(_camera);
    }

    public Vector3 GetInputDirection(Camera camera)
    {
        var forward = Vector3.forward;
        //Create the reference axis based on the camera rotation, ignoring y rotation
        //We're getting the main camera, which should be the one that's enabled. It's null if disabled so don't do anything if so
        if (camera != null)
        {
            forward = camera.transform.TransformDirection(Vector3.forward).SetY(0).normalized;
        }

        var right = new Vector3(forward.z, 0.0f, -forward.x);

        //Set the player's walk direction
        Vector3 walkVector = (
            _inputManager.HoritzontalAxis * right
            + _inputManager.VerticalAxis * forward
            );

        //prevent the player from moving faster when walking diagonally
        if (walkVector.sqrMagnitude > 1f)
        {
            walkVector = walkVector.normalized;
        }

        return walkVector;
    }

}
