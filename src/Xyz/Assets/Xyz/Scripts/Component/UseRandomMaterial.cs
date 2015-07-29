using UnityEngine;
using System.Collections.Generic;

public class UseRandomMaterial : MonoBehaviour
{
    public List<Material> Materials;

    // Use this for initialization
    void Start()
    {
         gameObject.GetComponent<Renderer>().material = Materials.GetRandom();
    }
}
