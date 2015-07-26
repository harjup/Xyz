using System;
using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour
{
    public GameObject EnterTarget { get; private set; }

    private GameObject _collider;
    private GameObject _getoutArrow;

    public void Start()
    {
        EnterTarget = transform.FindChild("EnterTarget").gameObject;

        _getoutArrow = transform.FindChild("Getout").gameObject;
        _collider = transform.FindChild("Collider").gameObject;

        _getoutArrow.SetActive(false);
        _collider.SetActive(false);
    }

    public void EnableExit()
    {
        _collider.SetActive(true);
        _getoutArrow.SetActive(true);
    }
}
