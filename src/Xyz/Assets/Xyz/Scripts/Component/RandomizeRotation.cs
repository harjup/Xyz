using UnityEngine;
using System.Collections;


public class RandomizeRotation : MonoBehaviour 
{
	void Start ()
	{
	    var eulerAngles = transform.localRotation;
	    var yAmount = Random.Range(20f, 160f);
        transform.localRotation = Quaternion.Euler(eulerAngles.x, yAmount, eulerAngles.z);
	}
}
