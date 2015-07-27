using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Xyz.Scripts;

public class ChaserContainer : MonoBehaviour
{
    public void AddChaser(Chaser chaser)
    {
        chaser.transform.SetParent(transform);
    }

    public void RemoveChaser(Chaser chaser)
    {
        chaser.transform.SetParent(null);
    }

    public List<Chaser> GetChasers()
    {
        return transform.GetComponentsInChildren<Chaser>().ToList();
    }

}
