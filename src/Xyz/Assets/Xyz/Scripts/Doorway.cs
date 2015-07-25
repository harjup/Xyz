using System;
using UnityEngine;
using System.Collections;

public class Doorway : MonoBehaviour
{
    public GameObject EnterTarget { get; private set; }

    private GameObject _getoutArrow;

    public void Start()
    {
        EnterTarget = transform.FindChild("EnterTarget").gameObject;

        _getoutArrow = transform.FindChild("Getout").gameObject;
        _getoutArrow.SetActive(false);
    }

    public void DisplayGetoutArrow()
    {
        _getoutArrow.SetActive(true);
    }
}
