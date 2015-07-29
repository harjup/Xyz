using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class AudienceGenerator : MonoBehaviour
{
    
    // Use this for initialization
    void Start()
    {
        var amount = 5;

        var difficultyManager = FindObjectOfType<DifficultyManager>();
        if (difficultyManager != null)
        {
            amount = difficultyManager.GetAudienceMemberCount();
        }

        var audiencePrefabs = new List<GameObject>
        {
            Resources.Load<GameObject>("Prefabs/Audience_Male"),
            Resources.Load<GameObject>("Prefabs/Audience_Female")
        };

        SpawnAudienceMembers(amount, audiencePrefabs);
    }


    public void SpawnAudienceMembers(int spawnAmount, List<GameObject> prefabs)
    {
        var render = gameObject.GetComponent<Renderer>();
        if (render != null)
        {
            render.enabled = false;
        }

        var position = transform.position;


        var deviation = Random.Range(-2, 3);
        spawnAmount += deviation;

        var isDeadZone = Random.Range(0, 5) == 0;
        if (isDeadZone)
        {
            spawnAmount = 2;
        }


        for (int i = 0; i < spawnAmount; i++)
        {
            var xBound = ((Random.Range(-1f, 1f) * transform.localScale.x / 2f));
            var zBound = (Random.Range(-1f, 1f) * transform.localScale.z / 2f);
            var targetVector = new Vector3(xBound, 0, zBound);

            Vector3 rotatedPosition = Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) * targetVector;
            Quaternion targetRotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.y + Random.Range(-45, 45f), Vector3.up);

            var prefab = prefabs.GetRandom();

            Instantiate(prefab, position + rotatedPosition, targetRotation);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
