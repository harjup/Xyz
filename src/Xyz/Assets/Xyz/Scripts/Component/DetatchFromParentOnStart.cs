﻿using UnityEngine;
using System.Collections;

public class DetatchFromParentOnStart : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null, false);
    }
}
